using System.ComponentModel.DataAnnotations;

namespace ShireBudgeters.Common.DTOs;

public class CommentDTO
{
    public int Id { get; set; }
    public int PostId { get; set; }

    [Required(ErrorMessage = "Name is required.")]
    [StringLength(100, MinimumLength = 1, ErrorMessage = "Name must be 1–100 characters.")]
    public required string AuthorName { get; set; }

    [Required(ErrorMessage = "Email is required.")]
    [EmailAddress(ErrorMessage = "Please enter a valid email address.")]
    [StringLength(255, ErrorMessage = "Email cannot exceed 255 characters.")]
    public required string AuthorEmail { get; set; }

    [Required(ErrorMessage = "Comment is required.")]
    [StringLength(4000, MinimumLength = 1, ErrorMessage = "Comment must be 1–4000 characters.")]
    public required string ContentBody { get; set; }

    public DateTime CreatedDate { get; set; }
}
