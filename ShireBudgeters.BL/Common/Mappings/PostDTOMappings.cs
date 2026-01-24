using ShireBudgeters.Common.DTOs;
using ShireBudgeters.DA.Models;

namespace ShireBudgeters.BL.Common.Mappings;

internal static class PostDTOMappings
{
    /// <summary>
    /// Maps a PostModel to a PostDTO.
    /// </summary>
    public static PostDTO ToPostDTO(this PostModel post)
    {
        return new PostDTO
        {
            Id = post.Id,
            AuthorId = post.AuthorId,
            CategoryId = post.CategoryId,
            Title = post.Title,
            Slug = post.Slug,
            ContentBody = post.ContentBody,
            FeaturedImageUrl = post.FeaturedImageUrl,
            MetaDescription = post.MetaDescription,
            PublicationDate = post.PublicationDate,
            IsPublished = post.IsPublished,
            CreatedBy = post.CreatedBy,
            CreatedDate = post.CreatedDate,
            ModifiedBy = post.ModifiedBy,
            ModifiedDate = post.ModifiedDate
        };
    }

    /// <summary>
    /// Maps a PostDTO to a PostModel for create/update operations.
    /// </summary>
    public static PostModel ToPostModel(this PostDTO postDto)
    {
        return new PostModel
        {
            Id = postDto.Id,
            AuthorId = postDto.AuthorId,
            CategoryId = postDto.CategoryId,
            Title = postDto.Title,
            Slug = postDto.Slug,
            ContentBody = postDto.ContentBody,
            FeaturedImageUrl = postDto.FeaturedImageUrl,
            MetaDescription = postDto.MetaDescription,
            PublicationDate = postDto.PublicationDate,
            IsPublished = postDto.IsPublished,
            CreatedBy = postDto.CreatedBy,
            CreatedDate = postDto.CreatedDate,
            ModifiedBy = postDto.ModifiedBy,
            ModifiedDate = postDto.ModifiedDate
        };
    }
}

