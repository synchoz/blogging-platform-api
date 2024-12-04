using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BloggingApp.Models;

public class Blog
{
    public int Id { get; set; }
    [StringLength(100)]
    public required string Title { get; set; }
    [StringLength(2000)]
    public required string Content { get; set; }
    [StringLength(50)]
    public required string Category { get; set; }
    public List<BlogTag> Tags { get; set; } = new List<BlogTag>();
    [DataType(DataType.Date)]
    public DateTime CreatedDateAt { get; set; } = DateTime.UtcNow;
    [DataType(DataType.Date)]
    public DateTime UpdatedDateAt { get; set; } = DateTime.UtcNow;
}

public class BlogD
{

    public required string Title { get; set; }

    public required string Content { get; set; }

    public required string Category { get; set; }
    public List<string> Tags { get; set; } = new List<string>();

    public DateTime CreatedDateAt { get; set; }
    public DateTime UpdatedDateAt { get; set; }
}
