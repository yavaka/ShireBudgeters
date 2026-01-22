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
        if (string.IsNullOrWhiteSpace(categoryDto.Name))
        {
            throw new ArgumentException("Category name is required.", nameof(categoryDto));
        }

        if (string.IsNullOrWhiteSpace(categoryDto.UserId))
        {
            throw new ArgumentException("UserId is required.", nameof(categoryDto));
        }

        // Validate parent category if specified
        if (categoryDto.ParentCategoryId.HasValue)
        {
            var parentCategory = await _categoryRepository.GetByIdAsync(categoryDto.ParentCategoryId.Value, cancellationToken);
            if (parentCategory == null)
            {
                throw new ArgumentException("Parent category not found.", nameof(categoryDto));
            }

            if (parentCategory.UserId != categoryDto.UserId)
            {
                throw new UnauthorizedAccessException("Parent category does not belong to the same user.");
            }
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
        if (categoryDto.Id <= 0)
        {
            throw new ArgumentException("Category ID is required for update.", nameof(categoryDto));
        }

        if (string.IsNullOrWhiteSpace(categoryDto.Name))
        {
            throw new ArgumentException("Category name is required.", nameof(categoryDto));
        }

        if (string.IsNullOrWhiteSpace(categoryDto.UserId))
        {
            throw new ArgumentException("UserId is required.", nameof(categoryDto));
        }

        var existingCategory = await _categoryRepository.GetByIdAsync(categoryDto.Id, cancellationToken);
        if (existingCategory == null)
        {
            throw new KeyNotFoundException($"Category with ID {categoryDto.Id} not found.");
        }

        // Verify ownership
        if (existingCategory.UserId != categoryDto.UserId)
        {
            throw new UnauthorizedAccessException("Category does not belong to the specified user.");
        }

        // Validate parent category if specified
        if (categoryDto.ParentCategoryId.HasValue)
        {
            // Prevent circular reference - check if the parent is a child of this category
            if (categoryDto.ParentCategoryId.Value == categoryDto.Id)
            {
                throw new ArgumentException("A category cannot be its own parent.", nameof(categoryDto));
            }

            var parentCategory = await _categoryRepository.GetByIdAsync(categoryDto.ParentCategoryId.Value, cancellationToken);
            if (parentCategory == null)
            {
                throw new ArgumentException("Parent category not found.", nameof(categoryDto));
            }

            if (parentCategory.UserId != categoryDto.UserId)
            {
                throw new UnauthorizedAccessException("Parent category does not belong to the same user.");
            }

            // Check for circular reference by checking if any child is the parent
            var childCategories = await _categoryRepository.GetChildCategoriesAsync(categoryDto.Id, cancellationToken);
            if (childCategories.Any(c => c.Id == categoryDto.ParentCategoryId.Value))
            {
                throw new ArgumentException("Cannot set parent category as it would create a circular reference.", nameof(categoryDto));
            }
        }

        // Update properties
        existingCategory.Name = categoryDto.Name;
        existingCategory.Description = categoryDto.Description;
        existingCategory.Color = categoryDto.Color;
        existingCategory.ParentCategoryId = categoryDto.ParentCategoryId;
        existingCategory.IsActive = categoryDto.IsActive;

        // Set audit properties
        existingCategory.ModifiedBy = userId;
        existingCategory.ModifiedDate = DateTime.UtcNow;

        await _categoryRepository.UpdateAsync(existingCategory, cancellationToken);
        return existingCategory.ToCategoryDTO();
    }

    /// <inheritdoc/>
    public async Task DeleteAsync(int id, CancellationToken cancellationToken = default)
    {
        var category = await _categoryRepository.GetByIdAsync(id, cancellationToken);
        if (category == null)
        {
            throw new KeyNotFoundException($"Category with ID {id} not found.");
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
