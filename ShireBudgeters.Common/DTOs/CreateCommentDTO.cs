namespace ShireBudgeters.Common.DTOs;

public class CreateCommentDTO
{
    public int PostId { get; set; }
    public required string AuthorName { get; set; }
    public required string AuthorEmail { get; set; }
    public required string ContentBody { get; set; }
}
