namespace To_Do.DataAccess.Models;

public class User
{
    public Guid Id { get; set; }
    public string Login { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string PasswordHash { get; set; } = string.Empty;
    public List<ToDo> ToDos { get; set; } = [];
    public List<Category> Categories { get; set; } = [];


}