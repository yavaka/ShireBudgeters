namespace ShireBudgeters.Common.DTOs;

public class CommentDTO
{
    public int Id { get; set; }
    public int PostId { get; set; }
    public required string AuthorName { get; set; }
    public required string AuthorEmail { get; set; }
    public required string ContentBody { get; set; }
    public DateTime CreatedDate { get; set; }
}
