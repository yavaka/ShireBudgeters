using ShireBudgeters.BL.Common.Helpers;
using ShireBudgeters.BL.Common.Mappings;
using ShireBudgeters.Common.DTOs;
using ShireBudgeters.DA.Repositories.Category;
using ShireBudgeters.DA.Repositories.LeadMagnet;

namespace ShireBudgeters.BL.Services.LeadMagnet;

/// <summary>
/// Service for managing lead magnet operations.
/// </summary>
internal class LeadMagnetService(ILeadMagnetRepository leadMagnetRepository, ICategoryRepository categoryRepository) : ILeadMagnetService
{
    private readonly ILeadMagnetRepository _leadMagnetRepository = leadMagnetRepository;
    private readonly ICategoryRepository _categoryRepository = categoryRepository;

    /// <inheritdoc/>
    public async Task<LeadMagnetDTO?> GetByIdAsync(int id, CancellationToken cancellationToken = default)
    {
        var leadMagnet = await _leadMagnetRepository.GetByIdAsync(id, cancellationToken);
        return leadMagnet?.ToLeadMagnetDTO();
    }

    /// <inheritdoc/>
    public async Task<IEnumerable<LeadMagnetDTO>> GetByCategoryIdAsync(int categoryId, CancellationToken cancellationToken = default)
    {
        LeadMagnetValidationHelper.ValidateCategoryId(categoryId);
        
        var leadMagnets = await _leadMagnetRepository.GetByCategoryIdAsync(categoryId, cancellationToken);
        return leadMagnets.Select(lm => lm.ToLeadMagnetDTO());
    }

    /// <inheritdoc/>
    public async Task<IEnumerable<LeadMagnetDTO>> GetActiveByCategoryIdAsync(int categoryId, CancellationToken cancellationToken = default)
    {
        LeadMagnetValidationHelper.ValidateCategoryId(categoryId);
        
        var leadMagnets = await _leadMagnetRepository.GetActiveByCategoryIdAsync(categoryId, cancellationToken);
         return leadMagnets.Select(lm => lm.ToLeadMagnetDTO());
    }

    /// <inheritdoc/>
    public async Task<LeadMagnetDTO> CreateAsync(LeadMagnetDTO leadMagnetDto, string? userId, CancellationToken cancellationToken = default)
    {
        // Validate and sanitize title
        LeadMagnetValidationHelper.ValidateTitle(leadMagnetDto.Title);
        leadMagnetDto.Title = LeadMagnetValidationHelper.SanitizeTitle(leadMagnetDto.Title);

        // Validate form action URL
        LeadMagnetValidationHelper.ValidateFormActionUrl(leadMagnetDto.FormActionUrl);

        // Validate download file URL
        LeadMagnetValidationHelper.ValidateDownloadFileUrl(leadMagnetDto.DownloadFileUrl);

        // Validate category ID
        LeadMagnetValidationHelper.ValidateCategoryId(leadMagnetDto.CategoryId);

        // Validate category exists
        var category = await _categoryRepository.GetByIdAsync(leadMagnetDto.CategoryId, cancellationToken);
        if (category == null)
        {
            throw new ArgumentException("Category not found.", nameof(leadMagnetDto));
        }

        // Validate category ownership if userId is provided
        if (!string.IsNullOrWhiteSpace(userId))
        {
            if (category.UserId != userId)
            {
                throw new UnauthorizedAccessException("Category does not belong to the authenticated user.");
            }
        }

        var leadMagnet = leadMagnetDto.ToLeadMagnetModel();
        
        // Set audit properties
        leadMagnet.CreatedBy = userId;
        leadMagnet.CreatedDate = DateTime.UtcNow;
        leadMagnet.ModifiedBy = null;
        leadMagnet.ModifiedDate = null;

        var createdLeadMagnet = await _leadMagnetRepository.AddAsync(leadMagnet, cancellationToken);
        return createdLeadMagnet.ToLeadMagnetDTO();
    }

    /// <inheritdoc/>
    public async Task<LeadMagnetDTO> UpdateAsync(LeadMagnetDTO leadMagnetDto, string? userId, CancellationToken cancellationToken = default)
    {
        // Validate ID
        if (leadMagnetDto.Id <= 0)
        {
            throw new ArgumentException("Lead magnet ID is required for update.", nameof(leadMagnetDto));
        }

        // Validate and sanitize title
        LeadMagnetValidationHelper.ValidateTitle(leadMagnetDto.Title);
        var sanitizedTitle = LeadMagnetValidationHelper.SanitizeTitle(leadMagnetDto.Title);

        // Validate form action URL
        LeadMagnetValidationHelper.ValidateFormActionUrl(leadMagnetDto.FormActionUrl);

        // Validate download file URL
        LeadMagnetValidationHelper.ValidateDownloadFileUrl(leadMagnetDto.DownloadFileUrl);

        // Validate category ID
        LeadMagnetValidationHelper.ValidateCategoryId(leadMagnetDto.CategoryId);

        // Get existing lead magnet
        var existingLeadMagnet = await _leadMagnetRepository.GetByIdAsync(leadMagnetDto.Id, cancellationToken);
        if (existingLeadMagnet == null)
        {
            throw new KeyNotFoundException($"Lead magnet with ID {leadMagnetDto.Id} not found.");
        }

        // Validate category exists
        var category = await _categoryRepository.GetByIdAsync(leadMagnetDto.CategoryId, cancellationToken);
        if (category == null)
        {
            throw new ArgumentException("Category not found.", nameof(leadMagnetDto));
        }

        // Validate category ownership if userId is provided
        if (!string.IsNullOrWhiteSpace(userId))
        {
            if (category.UserId != userId)
            {
                throw new UnauthorizedAccessException("Category does not belong to the authenticated user.");
            }
        }

        // Update properties
        existingLeadMagnet.Title = sanitizedTitle;
        existingLeadMagnet.FormActionUrl = leadMagnetDto.FormActionUrl;
        existingLeadMagnet.DownloadFileUrl = leadMagnetDto.DownloadFileUrl;
        existingLeadMagnet.CategoryId = leadMagnetDto.CategoryId;
        existingLeadMagnet.IsActive = leadMagnetDto.IsActive;

        // Set audit properties
        existingLeadMagnet.ModifiedBy = userId;
        existingLeadMagnet.ModifiedDate = DateTime.UtcNow;

        await _leadMagnetRepository.UpdateAsync(existingLeadMagnet, cancellationToken);
        return existingLeadMagnet.ToLeadMagnetDTO();
    }

    /// <inheritdoc/>
    public async Task DeleteAsync(int id, string? userId = null, CancellationToken cancellationToken = default)
    {
        if (id <= 0)
        {
            throw new ArgumentException("Lead magnet ID must be greater than zero.", nameof(id));
        }

        var leadMagnet = await _leadMagnetRepository.GetByIdAsync(id, cancellationToken);
        if (leadMagnet == null)
        {
            throw new KeyNotFoundException($"Lead magnet with ID {id} not found.");
        }

        // Validate ownership if userId is provided
        if (!string.IsNullOrWhiteSpace(userId))
        {
            // Get category to check ownership
            var category = await _categoryRepository.GetByIdAsync(leadMagnet.CategoryId, cancellationToken);
            if (category != null && category.UserId != userId)
            {
                throw new UnauthorizedAccessException("Lead magnet does not belong to the authenticated user.");
            }
        }

        await _leadMagnetRepository.DeleteAsync(leadMagnet, cancellationToken);
    }

    /// <inheritdoc/>
    public async Task SoftDeleteAsync(int id, string? userId, CancellationToken cancellationToken = default)
    {
        var leadMagnet = await _leadMagnetRepository.GetByIdAsync(id, cancellationToken);
        if (leadMagnet == null)
        {
            throw new KeyNotFoundException($"Lead magnet with ID {id} not found.");
        }

        // Validate ownership if userId is provided
        if (!string.IsNullOrWhiteSpace(userId))
        {
            // Get category to check ownership
            var category = await _categoryRepository.GetByIdAsync(leadMagnet.CategoryId, cancellationToken);
            if (category != null && category.UserId != userId)
            {
                throw new UnauthorizedAccessException("Lead magnet does not belong to the authenticated user.");
            }
        }

        leadMagnet.IsActive = false;
        leadMagnet.ModifiedBy = userId;
        leadMagnet.ModifiedDate = DateTime.UtcNow;

        await _leadMagnetRepository.UpdateAsync(leadMagnet, cancellationToken);
    }
}
