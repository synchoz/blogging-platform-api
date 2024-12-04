using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using BloggingApp.Data;
using BloggingApp.Models;
using Microsoft.CodeAnalysis.Elfie.Serialization;

namespace blogging_platform_api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BlogController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public BlogController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: api/Blog
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Blog>>> GetBlog(string term = "")
        {
            //title, content or category
            //students = students.OrderBy(s => s.LastName);
            Console.WriteLine(term);
            var blogs = await _context.Blog.Include(b => b.Tags).ToListAsync(); // Eagerly load related tags
            /* List <Blog> filteredBlogs = new List<Blog>(); */
            if(term != "") 
            {
                return blogs.Where(b => 
                                    b.Title.Contains(term, StringComparison.OrdinalIgnoreCase) || 
                                    b.Content.Contains(term, StringComparison.OrdinalIgnoreCase) || 
                                    b.Category.Contains(term, StringComparison.OrdinalIgnoreCase)).ToList();
            } 
            else 
            {
                return blogs;
            }
        }

        // GET: api/Blog/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Blog>> GetBlog(int id)
        {
            var blog = await _context.Blog
            .Include(b => b.Tags) // Eagerly load related tags
            .FirstOrDefaultAsync(b => b.Id == id);

            if (blog == null)
            {
                return NotFound();
            }

            return blog;
        }

        // PUT: api/Blog/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<ActionResult<BlogD>> PutBlog(int id, Blog blog)
        {
            var blogItem = await _context.Blog.Include(b => b.Tags).FirstOrDefaultAsync(b => b.Id == id);
            if (blogItem == null)
            {
                return NotFound();
            }
            blogItem.Title = blog.Title;
            blogItem.Category = blog.Content;
            blogItem.Category = blog.Category;
            blogItem.UpdatedDateAt = DateTime.Now;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException) when (!BlogExists(id))
            {
                return NotFound();
            }

            return BlogItemD(blogItem); 
        }


        // POST: api/Blog
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Blog>> PostBlog(Blog blog)
        {
            Console.WriteLine($"blog: {blog}");
            if(blog == null || string.IsNullOrEmpty(blog.Title) || string.IsNullOrEmpty(blog.Content)) 
            {
                return BadRequest("Invalid blog data");
            }
            _context.Blog.Add(blog);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetBlog", new { id = blog.Id }, blog);
        }

        // DELETE: api/Blog/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteBlog(int id)
        {
            var blog = await _context.Blog.FindAsync(id);
            if (blog == null)
            {
                return NotFound();
            }

            _context.Blog.Remove(blog);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool BlogExists(int id)
        {
            return _context.Blog.Any(e => e.Id == id);
        }

         private static BlogD BlogItemD(Blog blog) =>
            new BlogD
            {
                Title = blog.Title,
                Content = blog.Content,
                Category = blog.Category,
                Tags = blog.Tags.Select(tag => tag.Tag).ToList(),
                CreatedDateAt = blog.CreatedDateAt,
                UpdatedDateAt = blog.UpdatedDateAt
            };
    }
}
