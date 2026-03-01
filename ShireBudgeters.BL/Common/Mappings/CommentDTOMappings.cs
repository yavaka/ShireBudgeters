using ShireBudgeters.Common.DTOs;
using ShireBudgeters.DA.Models;

namespace ShireBudgeters.BL.Common.Mappings;

internal static class CommentDTOMappings
{
    /// <summary>
    /// Maps a CommentModel to a CommentDTO.
    /// </summary>
    public static CommentDTO ToCommentDTO(this CommentModel model)
    {
        return new CommentDTO
        {
            Id = model.Id,
            PostId = model.PostId,
            AuthorName = model.AuthorName,
            AuthorEmail = model.AuthorEmail,
            ContentBody = model.ContentBody,
            CreatedDate = model.CreatedDate
        };
    }

    /// <summary>
    /// Maps a CreateCommentDTO and post Id to a new CommentModel.
    /// </summary>
    public static CommentModel ToCommentModel(this CreateCommentDTO dto)
    {
        return new CommentModel
        {
            PostId = dto.PostId,
            AuthorName = dto.AuthorName,
            AuthorEmail = dto.AuthorEmail,
            ContentBody = dto.ContentBody,
            CreatedDate = DateTime.UtcNow
        };
    }

    /// <summary>
    /// Applies CommentDTO values to an existing CommentModel (for update).
    /// </summary>
    public static void ApplyTo(this CommentDTO dto, CommentModel model)
    {
        model.AuthorName = dto.AuthorName;
        model.AuthorEmail = dto.AuthorEmail;
        model.ContentBody = dto.ContentBody;
    }
}
