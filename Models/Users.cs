using System.ComponentModel.DataAnnotations;

namespace BloggingApp.Models;
public class Users
{
    public int Id { get; set; }
    [StringLength(60, MinimumLength = 0)]
    public string PasswordHash { get; set; } = string.Empty;
    [StringLength(100, MinimumLength = 0)]
    public required string Email { get; set; } = string.Empty;
    public DateTime CreatedDateAt { get; set; } = DateTime.UtcNow;
    [DataType(DataType.Date)]
    public DateTime LastLoginDateAt { get; set; } = DateTime.UtcNow;
}

public class UsersD
{
    public int Id { get; set; }
    public string Password { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public DateTime CreatedDateAt { get; set; }
    public DateTime LastLoginDateAt { get; set; }
}