namespace To_Do.DataAccess.Models;

public class Category
{
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public User? User { get; set; }
    public Guid? UserId { get; set; }
    public List<ToDo> ToDos { get; set; } = [];
}