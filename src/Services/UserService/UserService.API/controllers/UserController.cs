using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using UserService.API.dtos;
using UserService.Core.interfaces;
using UserService.Core.models;

namespace UserService.API.controllers
{
    [Route("api/user")]
    [ApiController]
    public class UserController : ControllerBase
    {

        private readonly UserManager<UserModel> _userManager;
        
        private readonly ITokenService _tokenService;

        private readonly SignInManager<UserModel> _signInManager;

        public UserController(UserManager<UserModel> userManager, ITokenService tokenService, SignInManager<UserModel> signInManager)
        {
            _userManager = userManager;
            _tokenService = tokenService;
            _signInManager = signInManager;
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

        
    }
}