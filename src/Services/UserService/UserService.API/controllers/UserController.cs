using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using UserService.API.dtos;
using UserService.Core.interfaces;
using UserService.Core.models;
using UserService.Infrastructure.repositories;

namespace UserService.API.controllers
{
    [Route("api/user")]
    [ApiController]
    public class UserController : ControllerBase
    {

        private readonly UserManager<UserModel> _userManager;
        
        private readonly ITokenService _tokenService;

        private readonly SignInManager<UserModel> _signInManager;
        private readonly IUserRepository _userRepository;

        public UserController(UserManager<UserModel> userManager, ITokenService tokenService, SignInManager<UserModel> signInManager, IUserRepository userRepository)
        {
            _userManager = userManager;
            _tokenService = tokenService;
            _signInManager = signInManager;
            _userRepository = userRepository;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] CreateUserDto createUserDto){
            try{
                if(!ModelState.IsValid){
                    return BadRequest(ModelState);
                }

                var userModel = new UserModel{
                    UserName = createUserDto.Username,
                    Email = createUserDto.Email
                };

                var createdUser = await _userManager.CreateAsync(userModel, createUserDto.Password);

                if(createdUser.Succeeded){
                    var roleResult = await _userManager.AddToRoleAsync(userModel, "User");
                    if(roleResult.Succeeded){
                        return Ok(new NewUserDto{
                            UserName = userModel.UserName,
                            Email = userModel.Email,
                            Token = _tokenService.CreateToken(userModel)
                        });
                    }
                    else{
                        return StatusCode(500, roleResult.Errors);
                    }
                }
                else{
                    return StatusCode(500, createdUser.Errors);
                }

            }
            catch(Exception e){
                return StatusCode(500, e);
            }
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login(LoginDto loginDto){
            if(!ModelState.IsValid){
                return BadRequest(ModelState);
            }

            var user = await _userManager.Users.FirstOrDefaultAsync(x => x.UserName == loginDto.Username.ToLower());

            if(user == null){
                return Unauthorized("Invalid Username");
            }

            var result = await _signInManager.CheckPasswordSignInAsync(user, loginDto.Password, false);

            if(!result.Succeeded){
                return Unauthorized("Username not found and/or password incorrect");
            }

            return Ok(new NewUserDto{
                UserName = user.UserName,
                Email = user.Email,
                Token = _tokenService.CreateToken(user)
            });
        }

        [Authorize]
        [HttpGet]
        public async Task<IActionResult> GetAllUsers(){
            var users = await _userRepository.GetAllUsersAsync();
            var userDtos = users.Select(c => new UserDto{
                UserName = c.UserName,
                Email = c.Email
            });

            return Ok(userDtos);
        }

        [Authorize]
        [HttpPut("email")]
        public async Task<IActionResult> UpdateUserEmail(UpdateUserDto updateEmailDto)
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
                return NotFound("User not found.");

            var result = await _userRepository.UpdateUserEmailAsync(user.Id, updateEmailDto.Email);
            if (result)
                return NoContent();

            return BadRequest("Failed to update email.");
        }



        
    }
}