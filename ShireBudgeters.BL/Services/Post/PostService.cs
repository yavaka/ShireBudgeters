using ShireBudgeters.BL.Common.Helpers;
using ShireBudgeters.BL.Common.Mappings;
using ShireBudgeters.Common.Common.Constants;
using ShireBudgeters.Common.DTOs;
using ShireBudgeters.DA.Repositories.Category;
using ShireBudgeters.DA.Repositories.Post;
using PostConstants = ShireBudgeters.Common.Common.Constants.Post;

namespace ShireBudgeters.BL.Services.Post;

/// <summary>
/// Service for managing blog post operations.
/// </summary>
internal class PostService(IPostRepository postRepository, ICategoryRepository categoryRepository) : IPostService
{
    private readonly IPostRepository _postRepository = postRepository;
    private readonly ICategoryRepository _categoryRepository = categoryRepository;

    /// <inheritdoc/>
    public async Task<PostDTO?> GetByIdAsync(int id, string? userId = null, CancellationToken cancellationToken = default)
    {
        var post = await _postRepository.GetByIdAsync(id, cancellationToken);
        
        if (post == null)
        {
            return null;
        }

        // Data Protection: Never expose draft/unpublished posts to unauthorized users
        // Only the author can see their own drafts
        if (!post.IsPublished && post.AuthorId != userId)
        {
            return null; // Return null instead of throwing to prevent information disclosure
        }

        return post.ToPostDTO();
    }

    /// <inheritdoc/>
    public async Task<PostDTO?> GetBySlugAsync(string slug, CancellationToken cancellationToken = default)
    {
        var post = await _postRepository.GetBySlugAsync(slug, cancellationToken);
        
        // Only return if published and currently visible
        if (post != null && post.IsPublished && post.PublicationDate <= DateTime.UtcNow)
        {
            return post.ToPostDTO();
        }

        return null;
    }

    /// <inheritdoc/>
    public async Task<IEnumerable<PostDTO>> GetPublishedPostsAsync(CancellationToken cancellationToken = default)
    {
        var posts = await _postRepository.GetPublishedPostsAsync(cancellationToken);
        return posts.Select(p => p.ToPostDTO());
    }

    /// <inheritdoc/>
    public async Task<IEnumerable<PostDTO>> GetPublishedPostsByCategoryAsync(int categoryId, CancellationToken cancellationToken = default)
    {
        // Validate that the category exists
        var category = await _categoryRepository.GetByIdAsync(categoryId, cancellationToken);
        if (category == null)
        {
            throw new KeyNotFoundException($"Category with ID {categoryId} not found.");
        }

        var posts = await _postRepository.GetPublishedPostsByCategoryAsync(categoryId, cancellationToken);
        return posts.Select(p => p.ToPostDTO());
    }

    /// <inheritdoc/>
    public async Task<IEnumerable<PostDTO>> GetRecentPublishedPostsByCategoryAsync(int categoryId, int count, CancellationToken cancellationToken = default)
    {
        if (count < PostConstants.MinRecentPostsCount)
        {
            throw new ArgumentException($"Count must be at least {PostConstants.MinRecentPostsCount}.", nameof(count));
        }

        if (count > PostConstants.MaxRecentPostsCount)
        {
            throw new ArgumentException($"Count cannot exceed {PostConstants.MaxRecentPostsCount}.", nameof(count));
        }

        var category = await _categoryRepository.GetByIdAsync(categoryId, cancellationToken);
        if (category == null)
        {
            throw new KeyNotFoundException($"Category with ID {categoryId} not found.");
        }

        var posts = await _postRepository.GetRecentPublishedPostsByCategoryAsync(categoryId, count, cancellationToken);
        return posts.Select(p => p.ToPostDTO());
    }

    /// <inheritdoc/>
    public async Task<IEnumerable<PostDTO>> GetPublishedPostsByCategoryAndDescendantsAsync(int parentCategoryId, CancellationToken cancellationToken = default)
    {
        var parent = await _categoryRepository.GetByIdAsync(parentCategoryId, cancellationToken);
        if (parent == null)
        {
            throw new KeyNotFoundException($"Category with ID {parentCategoryId} not found.");
        }

        var childCategories = await _categoryRepository.GetChildCategoriesAsync(parentCategoryId, cancellationToken);
        var categoryIds = new List<int> { parentCategoryId };
        foreach (var child in childCategories)
        {
            categoryIds.Add(child.Id);
        }

        var posts = await _postRepository.GetPublishedPostsByCategoryIdsAsync(categoryIds, cancellationToken);
        return posts.Select(p => p.ToPostDTO());
    }

    /// <inheritdoc/>
    public async Task<IEnumerable<PostDTO>> GetByAuthorIdAsync(string authorId, CancellationToken cancellationToken = default)
    {
        var posts = await _postRepository.GetByAuthorIdAsync(authorId, cancellationToken);
        return posts.Select(p => p.ToPostDTO());
    }

    /// <inheritdoc/>
    public async Task<IEnumerable<PostDTO>> GetDraftsByAuthorIdAsync(string authorId, CancellationToken cancellationToken = default)
    {
        var posts = await _postRepository.GetDraftsByAuthorIdAsync(authorId, cancellationToken);
        return posts.Select(p => p.ToPostDTO());
    }

    /// <inheritdoc/>
    public async Task<IEnumerable<PostDTO>> GetRecentPublishedPostsAsync(int count, CancellationToken cancellationToken = default)
    {
        if (count < PostConstants.MinRecentPostsCount)
        {
            throw new ArgumentException($"Count must be at least {PostConstants.MinRecentPostsCount}.", nameof(count));
        }

        if (count > PostConstants.MaxRecentPostsCount)
        {
            throw new ArgumentException($"Count cannot exceed {PostConstants.MaxRecentPostsCount}.", nameof(count));
        }

        var posts = await _postRepository.GetRecentPublishedPostsAsync(count, cancellationToken);
        return posts.Select(p => p.ToPostDTO());
    }

    /// <inheritdoc/>
    public async Task<PostDTO> CreateAsync(PostDTO postDto, string? userId, CancellationToken cancellationToken = default)
    {
        // Validate required fields
        if (string.IsNullOrWhiteSpace(postDto.Title))
        {
            throw new ArgumentException("Title is required.", nameof(postDto));
        }

        if (string.IsNullOrWhiteSpace(postDto.Slug))
        {
            throw new ArgumentException("Slug is required.", nameof(postDto));
        }

        // Validate slug format (URL-friendly, lowercase, hyphens instead of spaces)
        // Also prevents URL injection attacks
        if (!PostValidationHelper.ValidateSlug(postDto.Slug))
        {
            if (!PostValidationHelper.IsValidSlug(postDto.Slug))
            {
                throw new ArgumentException("Slug must be URL-friendly (lowercase, alphanumeric characters and hyphens only).", nameof(postDto));
            }
            
            if (PostValidationHelper.ContainsUrlInjectionPatterns(postDto.Slug))
            {
                throw new ArgumentException("Slug contains invalid characters that could be used for URL injection.", nameof(postDto));
            }
            
            // Fallback: generic error if validation fails for unknown reason
            throw new ArgumentException("Slug is invalid.", nameof(postDto));
        }

        // Validate slug uniqueness
        if (await _postRepository.CheckSlugExistsAsync(postDto.Slug, null, cancellationToken))
        {
            throw new ArgumentException("A post with this slug already exists.", nameof(postDto));
        }

        // Security check: AuthorId must match userId
        if (string.IsNullOrWhiteSpace(postDto.AuthorId))
        {
            throw new ArgumentException("AuthorId is required.", nameof(postDto));
        }

        if (postDto.AuthorId != userId)
        {
            throw new UnauthorizedAccessException("AuthorId must match the authenticated user.");
        }

        // Validate category if provided
        if (postDto.CategoryId.HasValue)
        {
            var category = await _categoryRepository.GetByIdAsync(postDto.CategoryId.Value, cancellationToken);
            if (category == null)
            {
                throw new ArgumentException("Category not found.", nameof(postDto));
            }

            // Validate category ownership (category must belong to the same user)
            if (category.UserId != postDto.AuthorId)
            {
                throw new UnauthorizedAccessException("Category does not belong to the same user.");
            }
        }

        // Validate MetaDescription length
        PostValidationHelper.ValidateMetaDescription(postDto.MetaDescription);

        // Validate and sanitize ContentBody to prevent XSS attacks
        if (!string.IsNullOrWhiteSpace(postDto.ContentBody))
        {
            postDto.ContentBody = PostValidationHelper.SanitizeContentBody(postDto.ContentBody);
        }

        // Validate FeaturedImageUrl
        if (!string.IsNullOrWhiteSpace(postDto.FeaturedImageUrl))
        {
            if (!PostValidationHelper.IsValidImageUrl(postDto.FeaturedImageUrl))
            {
                throw new ArgumentException("FeaturedImageUrl must point to an allowed domain or relative path.", nameof(postDto));
            }
        }

        // Set default values
        if (!postDto.IsPublished)
        {
            postDto.IsPublished = false;
        }

        var post = postDto.ToPostModel();

        // Set audit properties
        post.CreatedBy = userId;
        post.CreatedDate = DateTime.UtcNow;
        post.ModifiedBy = null;
        post.ModifiedDate = null;

        var createdPost = await _postRepository.AddAsync(post, cancellationToken);
        return createdPost.ToPostDTO();
    }

    /// <inheritdoc/>
    public async Task<PostDTO> UpdateAsync(PostDTO postDto, string? userId, CancellationToken cancellationToken = default)
    {
        // Validate ID
        if (postDto.Id <= 0)
        {
            throw new ArgumentException("Post ID is required for update.", nameof(postDto));
        }

        // Validate required fields
        if (string.IsNullOrWhiteSpace(postDto.Title))
        {
            throw new ArgumentException("Title is required.", nameof(postDto));
        }

        if (string.IsNullOrWhiteSpace(postDto.Slug))
        {
            throw new ArgumentException("Slug is required.", nameof(postDto));
        }

        // Validate slug format
        if (!PostValidationHelper.ValidateSlug(postDto.Slug))
        {
            if (!PostValidationHelper.IsValidSlug(postDto.Slug))
            {
                throw new ArgumentException("Slug must be URL-friendly (lowercase, alphanumeric characters and hyphens only).", nameof(postDto));
            }
            
            if (PostValidationHelper.ContainsUrlInjectionPatterns(postDto.Slug))
            {
                throw new ArgumentException("Slug contains invalid characters that could be used for URL injection.", nameof(postDto));
            }
            
            // Fallback: generic error if validation fails for unknown reason
            throw new ArgumentException("Slug is invalid.", nameof(postDto));
        }

        // Validate slug uniqueness (excluding current post)
        if (await _postRepository.CheckSlugExistsAsync(postDto.Slug, postDto.Id, cancellationToken))
        {
            throw new ArgumentException("A post with this slug already exists.", nameof(postDto));
        }

        // Get existing post
        var existingPost = await _postRepository.GetByIdAsync(postDto.Id, cancellationToken);
        if (existingPost == null)
        {
            throw new KeyNotFoundException($"Post with ID {postDto.Id} not found.");
        }

        // Verify ownership: AuthorId must match existing post's AuthorId and provided userId
        if (existingPost.AuthorId != postDto.AuthorId)
        {
            throw new UnauthorizedAccessException("Post AuthorId cannot be changed.");
        }

        if (existingPost.AuthorId != userId)
        {
            throw new UnauthorizedAccessException("Post does not belong to the authenticated user.");
        }

        // Validate category if provided
        if (postDto.CategoryId.HasValue)
        {
            var category = await _categoryRepository.GetByIdAsync(postDto.CategoryId.Value, cancellationToken);
            if (category == null)
            {
                throw new ArgumentException("Category not found.", nameof(postDto));
            }

            // Validate category ownership
            if (category.UserId != postDto.AuthorId)
            {
                throw new UnauthorizedAccessException("Category does not belong to the same user.");
            }
        }

        // Validate MetaDescription length
        PostValidationHelper.ValidateMetaDescription(postDto.MetaDescription);

        // Validate and sanitize ContentBody to prevent XSS attacks
        string? sanitizedContentBody = null;
        if (!string.IsNullOrWhiteSpace(postDto.ContentBody))
        {
            sanitizedContentBody = PostValidationHelper.SanitizeContentBody(postDto.ContentBody);
        }

        // Validate FeaturedImageUrl
        if (!string.IsNullOrWhiteSpace(postDto.FeaturedImageUrl))
        {
            if (!PostValidationHelper.IsValidImageUrl(postDto.FeaturedImageUrl))
            {
                throw new ArgumentException("FeaturedImageUrl must point to an allowed domain or relative path.", nameof(postDto));
            }
        }

        // Update properties
        existingPost.Title = postDto.Title;
        existingPost.Slug = postDto.Slug;
        existingPost.ContentBody = sanitizedContentBody;
        existingPost.FeaturedImageUrl = postDto.FeaturedImageUrl;
        existingPost.MetaDescription = postDto.MetaDescription;
        existingPost.PublicationDate = postDto.PublicationDate;
        existingPost.IsPublished = postDto.IsPublished;
        existingPost.CategoryId = postDto.CategoryId;

        // Set audit properties
        existingPost.ModifiedBy = userId;
        existingPost.ModifiedDate = DateTime.UtcNow;

        await _postRepository.UpdateAsync(existingPost, cancellationToken);
        return existingPost.ToPostDTO();
    }

    /// <inheritdoc/>
    public async Task DeleteAsync(int id, string? userId, CancellationToken cancellationToken = default)
    {
        var post = await _postRepository.GetByIdAsync(id, cancellationToken);
        if (post == null)
        {
            throw new KeyNotFoundException($"Post with ID {id} not found.");
        }

        // Verify ownership
        if (post.AuthorId != userId)
        {
            throw new UnauthorizedAccessException("Post does not belong to the authenticated user.");
        }

        await _postRepository.DeleteAsync(post, cancellationToken);
    }

    /// <inheritdoc/>
    public async Task<PostDTO> PublishAsync(int id, string? userId, CancellationToken cancellationToken = default)
    {
        var post = await _postRepository.GetByIdAsync(id, cancellationToken);
        if (post == null)
        {
            throw new KeyNotFoundException($"Post with ID {id} not found.");
        }

        // Verify ownership
        if (post.AuthorId != userId)
        {
            throw new UnauthorizedAccessException("Post does not belong to the authenticated user.");
        }

        post.IsPublished = true;

        // If PublicationDate is in the past or not set, set it to now
        if (post.PublicationDate == default || post.PublicationDate < DateTime.UtcNow)
        {
            post.PublicationDate = DateTime.UtcNow;
        }

        // Update audit properties
        post.ModifiedBy = userId;
        post.ModifiedDate = DateTime.UtcNow;

        await _postRepository.UpdateAsync(post, cancellationToken);
        return post.ToPostDTO();
    }

    /// <inheritdoc/>
    public async Task<PostDTO> UnpublishAsync(int id, string? userId, CancellationToken cancellationToken = default)
    {
        var post = await _postRepository.GetByIdAsync(id, cancellationToken);
        if (post == null)
        {
            throw new KeyNotFoundException($"Post with ID {id} not found.");
        }

        // Verify ownership
        if (post.AuthorId != userId)
        {
            throw new UnauthorizedAccessException("Post does not belong to the authenticated user.");
        }

        post.IsPublished = false;

        // Update audit properties
        post.ModifiedBy = userId;
        post.ModifiedDate = DateTime.UtcNow;

        await _postRepository.UpdateAsync(post, cancellationToken);
        return post.ToPostDTO();
    }
}
