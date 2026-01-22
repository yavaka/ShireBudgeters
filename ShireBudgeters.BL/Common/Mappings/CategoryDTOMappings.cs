using ShireBudgeters.Common.DTOs;
using ShireBudgeters.DA.Models;

namespace ShireBudgeters.BL.Common.Mappings;

internal static class CategoryDTOMappings
{
    /// <summary>
    /// Maps a CategoryModel to a CategoryDTO.
    /// </summary>
    public static CategoryDTO ToCategoryDTO(this CategoryModel category)
    {
        return new CategoryDTO
        {
            Id = category.Id,
            Name = category.Name,
            Description = category.Description,
            Color = category.Color,
            UserId = category.UserId,
            ParentCategoryId = category.ParentCategoryId,
            IsActive = category.IsActive,
            CreatedBy = category.CreatedBy,
            CreatedDate = category.CreatedDate,
            ModifiedBy = category.ModifiedBy,
            ModifiedDate = category.ModifiedDate
        };
    }

    /// <summary>
    /// Maps a CategoryDTO to a CategoryModel for creation.
    /// </summary>
    public static CategoryModel ToCategoryModel(this CategoryDTO categoryDto)
    {
        return new CategoryModel
        {
            Id = categoryDto.Id,
            Name = categoryDto.Name,
            Description = categoryDto.Description,
            Color = categoryDto.Color,
            UserId = categoryDto.UserId,
            ParentCategoryId = categoryDto.ParentCategoryId,
            IsActive = categoryDto.IsActive,
            CreatedBy = categoryDto.CreatedBy,
            CreatedDate = categoryDto.CreatedDate,
            ModifiedBy = categoryDto.ModifiedBy,
            ModifiedDate = categoryDto.ModifiedDate
        };
    }
}

