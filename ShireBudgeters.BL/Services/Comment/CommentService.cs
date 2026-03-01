using ShireBudgeters.BL.Common.Helpers;
using ShireBudgeters.BL.Common.Mappings;
using ShireBudgeters.Common.DTOs;
using ShireBudgeters.DA.Models;
using ShireBudgeters.DA.Repositories.Post;
using ShireBudgeters.DA.Repositories.PostComment;

namespace ShireBudgeters.BL.Services.Comment;

/// <summary>
/// Service for managing comment operations.
/// </summary>
internal class CommentService(IPostCommentRepository commentRepository, IPostRepository postRepository) : ICommentService
{
    private readonly IPostCommentRepository _commentRepository = commentRepository;
    private readonly IPostRepository _postRepository = postRepository;

    /// <inheritdoc/>
    public async Task<IEnumerable<CommentDTO>> GetByPostIdAsync(int postId, CancellationToken cancellationToken = default)
    {
        var comments = await _commentRepository.GetByPostIdAsync(postId, cancellationToken);
        return comments.Select(c => c.ToCommentDTO());
    }

    /// <inheritdoc/>
    public async Task<CommentDTO?> GetByIdAsync(int id, CancellationToken cancellationToken = default)
    {
        var comment = await _commentRepository.GetByIdAsync(id, cancellationToken);
        return comment?.ToCommentDTO();
    }

    /// <inheritdoc/>
    public async Task<CommentDTO> CreateAsync(CreateCommentDTO dto, CancellationToken cancellationToken = default)
    {
        // Validate post exists and is published
        var post = await _postRepository.GetByIdAsync(dto.PostId, cancellationToken);
        if (post == null)
        {
            throw new KeyNotFoundException($"Post with ID {dto.PostId} not found.");
        }

        if (!post.IsPublished || post.PublicationDate > DateTime.UtcNow)
        {
            throw new InvalidOperationException("Comments can only be added to published posts.");
        }

        // Server-side validation
        CommentValidationHelper.ValidateAuthorName(dto.AuthorName);
        CommentValidationHelper.ValidateAuthorEmail(dto.AuthorEmail);
        CommentValidationHelper.ValidateContentBody(dto.ContentBody);

        // Sanitize before save
        var sanitizedName = CommentValidationHelper.SanitizeAuthorName(dto.AuthorName);
        var sanitizedEmail = CommentValidationHelper.SanitizeAuthorEmail(dto.AuthorEmail);
        var sanitizedContent = CommentValidationHelper.SanitizeContentBody(dto.ContentBody);

        var model = new CommentModel
        {
            PostId = dto.PostId,
            AuthorName = sanitizedName,
            AuthorEmail = sanitizedEmail,
            ContentBody = sanitizedContent,
            CreatedDate = DateTime.UtcNow
        };

        var created = await _commentRepository.AddAsync(model, cancellationToken);
        return created.ToCommentDTO();
    }

    /// <inheritdoc/>
    public async Task<CommentDTO> UpdateAsync(CommentDTO dto, CancellationToken cancellationToken = default)
    {
        // Validate comment exists
        var existing = await _commentRepository.GetByIdAsync(dto.Id, cancellationToken);
        if (existing == null)
        {
            throw new KeyNotFoundException($"Comment with ID {dto.Id} not found.");
        }

        // Server-side validation
        CommentValidationHelper.ValidateAuthorName(dto.AuthorName);
        CommentValidationHelper.ValidateAuthorEmail(dto.AuthorEmail);
        CommentValidationHelper.ValidateContentBody(dto.ContentBody);

        // Sanitize before save
        existing.AuthorName = CommentValidationHelper.SanitizeAuthorName(dto.AuthorName);
        existing.AuthorEmail = CommentValidationHelper.SanitizeAuthorEmail(dto.AuthorEmail);
        existing.ContentBody = CommentValidationHelper.SanitizeContentBody(dto.ContentBody);

        await _commentRepository.UpdateAsync(existing, cancellationToken);
        return existing.ToCommentDTO();
    }

    /// <inheritdoc/>
    public async Task DeleteAsync(int id, CancellationToken cancellationToken = default)
    {
        var comment = await _commentRepository.GetByIdAsync(id, cancellationToken);
        if (comment == null)
        {
            throw new KeyNotFoundException($"Comment with ID {id} not found.");
        }

        await _commentRepository.DeleteAsync(comment, cancellationToken);
    }
}
