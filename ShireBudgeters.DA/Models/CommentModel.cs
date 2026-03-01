namespace ShireBudgeters.DA.Models;

public class CommentModel
{
    public int Id { get; set; }
    public int PostId { get; set; }
    public required string AuthorName { get; set; }
    public required string AuthorEmail { get; set; }
    public required string ContentBody { get; set; }
    public DateTime CreatedDate { get; set; }

    public PostModel Post { get; set; } = default!;
}
