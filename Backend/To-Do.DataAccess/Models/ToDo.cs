namespace To_Do.DataAccess.Models;

public class ToDo
{
    public Guid Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public string? Description { get; set; }
    public Guid UserId { get; set; }
    public User? User { get; set; }
    public Guid CategoryId { get; set; }
    public Category? Category { get; set; }
    public DateTimeOffset CreatedAtUtc { get; set; }
    public DateTimeOffset UpdatedAtUtc { get; set; }
    public bool IsCompleted { get; set; }
    
}