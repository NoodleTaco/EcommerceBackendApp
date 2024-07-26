using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
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

        
    }
}