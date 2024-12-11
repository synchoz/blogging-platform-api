using BloggingApp.Data;
using BloggingApp.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace blogging_platform_api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly PasswordHasher<UsersD> _hasher;

        public AuthController(ApplicationDbContext context, PasswordHasher<UsersD> hasher)
        {
            _context = context;
            _hasher = hasher;
        }

        [HttpPost("register")]
        public async Task<IResult> RegisterUser(UsersD user) 
        {
            if(user == null || string.IsNullOrEmpty(user.Email) || string.IsNullOrEmpty(user.Password)) 
            {
                return TypedResults.BadRequest("Invalid user data");
            }
            
            var userExists = await FindUserByEmail(user.Email);

            if(userExists != null)
            {
                return TypedResults.BadRequest("email already exists");
            }
            
            var newUser = new Users
            {
                CreatedDateAt = DateTime.Now,
                Email = user.Email,
                PasswordHash = _hasher.HashPassword(user, user.Password)
            };
          
            _context.Users.Add(newUser);
            await _context.SaveChangesAsync();

            return TypedResults.Ok("For now this would suffice");
        }

        [HttpPost("login")]
        public async Task<IResult> Login(UsersD user)
        {
            if(user == null || string.IsNullOrEmpty(user.Email) || string.IsNullOrEmpty(user.Password)) 
            {
                return TypedResults.BadRequest("Invalid user data");
            }

            var userExists = await FindUserByEmail(user.Email);

            if(userExists == null)
            {
                return TypedResults.BadRequest("email doesn't exist in the DB");
            }

            if(_hasher.VerifyHashedPassword(user, userExists.PasswordHash, user.Password) > 0)
            {
                return TypedResults.Ok("Perfectly safe to login!");
            }
            else 
            {
                return TypedResults.Ok("Wrong password please try again!");
            }
        }

        private async Task<Users> FindUserByEmail(string email)
        {
            var user = await _context.Users.FirstOrDefaultAsync(e => e.Email == email);

            return user;
        }

    }
}