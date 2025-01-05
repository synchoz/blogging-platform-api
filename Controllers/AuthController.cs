using BloggingApp.Data;
using BloggingApp.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;

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
                //Lets use Cookie authentication
                var claims = new List<Claim>
                {
                    new Claim(ClaimTypes.Name, user.Email),
                    new Claim(ClaimTypes.Role, "Administrator"),
                }; 

                var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
                var authProperties = new AuthenticationProperties
                {
                    //AllowRefresh = <bool>,
                    // Refreshing the authentication session should be allowed.

                    //ExpiresUtc = DateTimeOffset.UtcNow.AddMinutes(10),
                    // The time at which the authentication ticket expires. A 
                    // value set here overrides the ExpireTimeSpan option of 
                    // CookieAuthenticationOptions set with AddCookie.

                    //IsPersistent = true,
                    // Whether the authentication session is persisted across 
                    // multiple requests. When used with cookies, controls
                    // whether the cookie's lifetime is absolute (matching the
                    // lifetime of the authentication ticket) or session-based.

                    //IssuedUtc = <DateTimeOffset>,
                    // The time at which the authentication ticket was issued.

                    //RedirectUri = <string>
                    // The full path or absolute URI to be used as an http 
                    // redirect response value.
                };

                await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(claimsIdentity), authProperties);

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