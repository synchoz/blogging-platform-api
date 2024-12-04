using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace BloggingApp.Models;

public class BlogTag
{
    public int Id { get; set; }
    public int BlogId { get; set; }

    [StringLength(50)]
    public required string Tag { get; set; } = string.Empty;

    // Navigation property to the related Blog
    [JsonIgnore]
    public Blog? Blog { get; set; }
}