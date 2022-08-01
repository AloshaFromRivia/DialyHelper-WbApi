using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Text.Unicode;
using System.Threading.Tasks;
using DailyHelper.Entity;
using DailyHelper.Models;
using DailyHelper.Models.ViewModels;
using DailyHelper.Models.ViewModels.Requests;
using DailyHelper.Models.ViewModels.Responses;
using DailyHelper.Services;
using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using Microsoft.AspNetCore.Identity;


namespace DailyHelper.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private ApplicationDbContext _context;
        private IIdentityService _identityService;

        public AccountController(ApplicationDbContext context, IIdentityService service)
        {
            _context = context;
            _identityService = service;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] UserRegistrationRequest request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(new AuthFailedResponse()
                {
                    Errors = ModelState.Values
                        .SelectMany(x => x.Errors.Select(xx => xx.ErrorMessage))
                });
            }
            
            var authResponse = await _identityService.RegisterAsync(request);

            if (!authResponse.Success)
            {
                return BadRequest(new AuthFailedResponse()
                {
                    Errors = authResponse.Errors
                });
            }
            
            return Ok(new AuthSuccessResponse()
            {
                Token = authResponse.Token
            });
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] UserLoginRequest request)
        {
            var authResponse = await _identityService.LoginAsync(request);

            if (!authResponse.Success)
            {
                return BadRequest(new AuthFailedResponse()
                {
                    Errors = authResponse.Errors
                });
            }
            
            return Ok(new AuthSuccessResponse()
            {
                Token = authResponse.Token
            });
        }
        
    }
}