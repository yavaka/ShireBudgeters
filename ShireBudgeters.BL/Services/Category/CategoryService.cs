using ShireBudgeters.BL.Common.Helpers;
using ShireBudgeters.BL.Common.Mappings;
using ShireBudgeters.Common.DTOs;
using ShireBudgeters.DA.Repositories.Category;

namespace ShireBudgeters.BL.Services.Category;

/// <summary>
/// Service for managing category operations.
/// </summary>
internal class CategoryService(ICategoryRepository categoryRepository) : ICategoryService
{
    private readonly ICategoryRepository _categoryRepository = categoryRepository;

    /// <inheritdoc/>
    public async Task<CategoryDTO?> GetByIdAsync(int id, CancellationToken cancellationToken = default)
    {
        var category = await _categoryRepository.GetByIdAsync(id, cancellationToken);
        return category?.ToCategoryDTO();
    }

    /// <inheritdoc/>
    public async Task<IEnumerable<CategoryDTO>> GetByUserIdAsync(string userId, CancellationToken cancellationToken = default)
    {
        var categories = await _categoryRepository.GetByUserIdAsync(userId, cancellationToken);
        return categories.Select(c => c.ToCategoryDTO());
    }

    /// <inheritdoc/>
    public async Task<IEnumerable<CategoryDTO>> GetActiveByUserIdAsync(string userId, CancellationToken cancellationToken = default)
    {
        var categories = await _categoryRepository.GetActiveByUserIdAsync(userId, cancellationToken);
        return categories.Select(c => c.ToCategoryDTO());
    }

    /// <inheritdoc/>
    public async Task<IEnumerable<CategoryDTO>> GetRootCategoriesByUserIdAsync(string userId, CancellationToken cancellationToken = default)
    {
        var categories = await _categoryRepository.GetRootCategoriesByUserIdAsync(userId, cancellationToken);
        return categories.Select(c => c.ToCategoryDTO());
    }

    /// <inheritdoc/>
    public async Task<IEnumerable<CategoryDTO>> GetChildCategoriesAsync(int parentCategoryId, CancellationToken cancellationToken = default)
    {
        var categories = await _categoryRepository.GetChildCategoriesAsync(parentCategoryId, cancellationToken);
        return categories.Select(c => c.ToCategoryDTO());
    }

    /// <inheritdoc/>
    public async Task<CategoryDTO> CreateAsync(CategoryDTO categoryDto, string? userId, CancellationToken cancellationToken = default)
    {
        // Validate and sanitize name
        CategoryValidationHelper.ValidateName(categoryDto.Name);
        categoryDto.Name = CategoryValidationHelper.SanitizeName(categoryDto.Name);

        // Validate description
        CategoryValidationHelper.ValidateDescription(categoryDto.Description);
        if (!string.IsNullOrWhiteSpace(categoryDto.Description))
        {
            categoryDto.Description = CategoryValidationHelper.SanitizeDescription(categoryDto.Description);
        }

        // Validate color
        CategoryValidationHelper.ValidateColorLength(categoryDto.Color);
        if (!string.IsNullOrWhiteSpace(categoryDto.Color))
        {
            if (!CategoryValidationHelper.IsValidColorFormat(categoryDto.Color))
            {
                throw new ArgumentException("Category color must be a valid hex color (#FF0000, #fff) or CSS color name.", nameof(categoryDto));
            }
        }

        // Validate UserId
        CategoryValidationHelper.ValidateUserId(categoryDto.UserId);

        // Security check: UserId must match userId
        if (categoryDto.UserId != userId)
        {
            throw new UnauthorizedAccessException("UserId must match the authenticated user.");
        }

        // Validate parent category if specified
        if (categoryDto.ParentCategoryId.HasValue)
        {
            // Note: For new categories (Id = 0), self-reference check is not needed
            // But we still validate parent category exists and belongs to the user

            var parentCategory = await _categoryRepository.GetByIdAsync(categoryDto.ParentCategoryId.Value, cancellationToken);
            if (parentCategory == null)
            {
                throw new ArgumentException("Parent category not found.", nameof(categoryDto));
            }

            // Validate parent category ownership
            CategoryValidationHelper.ValidateOwnershipOrThrow(parentCategory.UserId, categoryDto.UserId);
        }

        var category = categoryDto.ToCategoryModel();
        
        // Set audit properties
        category.CreatedBy = userId;
        category.CreatedDate = DateTime.UtcNow;
        category.ModifiedBy = null;
        category.ModifiedDate = null;

        var createdCategory = await _categoryRepository.AddAsync(category, cancellationToken);
        return createdCategory.ToCategoryDTO();
    }

    /// <inheritdoc/>
    public async Task<CategoryDTO> UpdateAsync(CategoryDTO categoryDto, string? userId, CancellationToken cancellationToken = default)
    {
        // Validate ID
        if (categoryDto.Id <= 0)
        {
            throw new ArgumentException("Category ID is required for update.", nameof(categoryDto));
        }

        // Validate and sanitize name
        CategoryValidationHelper.ValidateName(categoryDto.Name);
        var sanitizedName = CategoryValidationHelper.SanitizeName(categoryDto.Name);

        // Validate description
        CategoryValidationHelper.ValidateDescription(categoryDto.Description);
        string? sanitizedDescription = null;
        if (!string.IsNullOrWhiteSpace(categoryDto.Description))
        {
            sanitizedDescription = CategoryValidationHelper.SanitizeDescription(categoryDto.Description);
        }

        // Validate color
        CategoryValidationHelper.ValidateColorLength(categoryDto.Color);
        if (!string.IsNullOrWhiteSpace(categoryDto.Color))
        {
            if (!CategoryValidationHelper.IsValidColorFormat(categoryDto.Color))
            {
                throw new ArgumentException("Category color must be a valid hex color (#FF0000, #fff) or CSS color name.", nameof(categoryDto));
            }
        }

        // Validate UserId
        CategoryValidationHelper.ValidateUserId(categoryDto.UserId);

        var existingCategory = await _categoryRepository.GetByIdAsync(categoryDto.Id, cancellationToken);
        if (existingCategory == null)
        {
            throw new KeyNotFoundException($"Category with ID {categoryDto.Id} not found.");
        }

        // Verify ownership
        CategoryValidationHelper.ValidateOwnershipOrThrow(existingCategory.UserId, categoryDto.UserId);

        // Security check: UserId must match userId
        if (existingCategory.UserId != userId)
        {
            throw new UnauthorizedAccessException("Category does not belong to the authenticated user.");
        }

        // Validate parent category if specified
        if (categoryDto.ParentCategoryId.HasValue)
        {
            // Prevent self-reference
            CategoryValidationHelper.ValidateParentCategory(categoryDto.Id, categoryDto.ParentCategoryId);

            var parentCategory = await _categoryRepository.GetByIdAsync(categoryDto.ParentCategoryId.Value, cancellationToken);
            if (parentCategory == null)
            {
                throw new ArgumentException("Parent category not found.", nameof(categoryDto));
            }

            // Validate parent category ownership
            CategoryValidationHelper.ValidateOwnershipOrThrow(parentCategory.UserId, categoryDto.UserId);

            // Check for circular reference by checking if any child is the parent
            var childCategories = await _categoryRepository.GetChildCategoriesAsync(categoryDto.Id, cancellationToken);
            if (childCategories.Any(c => c.Id == categoryDto.ParentCategoryId.Value))
            {
                throw new ArgumentException("Cannot set parent category as it would create a circular reference.", nameof(categoryDto));
            }
        }

        // Update properties
        existingCategory.Name = sanitizedName;
        existingCategory.Description = sanitizedDescription;
        existingCategory.Color = categoryDto.Color;
        existingCategory.ParentCategoryId = categoryDto.ParentCategoryId;
        existingCategory.IsActive = categoryDto.IsActive;

        // Set audit properties
        existingCategory.ModifiedBy = userId;
        existingCategory.ModifiedDate = DateTime.UtcNow;

        // Clear navigation properties to avoid tracking conflicts before updating
        existingCategory.User = null;
        existingCategory.ParentCategory = null;
        existingCategory.ChildCategories = null;

        await _categoryRepository.UpdateAsync(existingCategory, cancellationToken);
        return existingCategory.ToCategoryDTO();
    }

    /// <inheritdoc/>
    public async Task DeleteAsync(int id, string? userId = null, CancellationToken cancellationToken = default)
    {
        var category = await _categoryRepository.GetByIdAsync(id, cancellationToken);
        if (category == null)
        {
            throw new KeyNotFoundException($"Category with ID {id} not found.");
        }

        // Verify ownership if userId is provided
        if (!string.IsNullOrWhiteSpace(userId))
        {
            CategoryValidationHelper.ValidateOwnershipOrThrow(category.UserId, userId);
        }

        // Check if category has children
        var childCategories = await _categoryRepository.GetChildCategoriesAsync(id, cancellationToken);
        if (childCategories.Any())
        {
            throw new InvalidOperationException("Cannot delete category that has child categories. Please delete or reassign child categories first.");
        }

        await _categoryRepository.DeleteAsync(category, cancellationToken);
    }

    /// <inheritdoc/>
    public async Task SoftDeleteAsync(int id, string? userId, CancellationToken cancellationToken = default)
    {
        var category = await _categoryRepository.GetByIdAsync(id, cancellationToken);
        if (category == null)
        {
            throw new KeyNotFoundException($"Category with ID {id} not found.");
        }

        category.IsActive = false;
        category.ModifiedBy = userId;
        category.ModifiedDate = DateTime.UtcNow;

        await _categoryRepository.UpdateAsync(category, cancellationToken);
    }
}
