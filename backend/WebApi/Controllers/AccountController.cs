using Application.Interfaces;
using Domain.DTOs.Models.Account;
using Domain.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly UserManager<User> _userManager;
        private readonly ITokenService _tokenService;
        
        private readonly SignInManager<User> _signInManager;
        public AccountController(UserManager<User> userManager, ITokenService tokenService, SignInManager<User> signInManager)
        {
            _userManager = userManager;
            _tokenService = tokenService;
            _signInManager = signInManager;
        }

        [HttpPost]
        [Route("register")]
        public async Task<IActionResult> Register([FromBody] Register user)
        {
            if (user is null)
            {
                return BadRequest();
            }

            try
            {
                var newUser = new User
                {
                    UserName = user.Username
                };

                var result = await _userManager.CreateAsync(newUser, user.Password);

                if (result.Succeeded)
                {
                    var rolesResult = await _userManager.AddToRoleAsync(newUser, "User");

                    if (rolesResult.Succeeded)
                    {

                        return Ok(
                            new NewUser
                            {
                                UserName = newUser.UserName,
                                Token = _tokenService.CreateToken(newUser)
                            }
                        );
                    }

                    return BadRequest(rolesResult.Errors);
                }

                return BadRequest(result.Errors);
            }
            catch (System.Exception e)
            {
                return BadRequest(e);
            }
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] Register user)
        {
            if (user is null)
            {
                return BadRequest();
            }

            try
            {
                var existingUser = await _userManager.Users.FirstOrDefaultAsync(u => u.UserName == user.Username.ToLower());

                if (existingUser is null)
                {
                    return Unauthorized("Username not found and/or invalid password");
                }

                var result = await _signInManager.CheckPasswordSignInAsync(existingUser, user.Password, false);

                if (!result.Succeeded)
                {
                    return Unauthorized("Username not found and/or invalid password");
                }

                return Ok(
                    new NewUser
                    {
                        UserName = existingUser.UserName,
                        Token = _tokenService.CreateToken(existingUser)
                    }
                );
                
            }
            catch (System.Exception e)
            {
                return BadRequest(e);
            }
        }
    }
}