using System;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using DailyHelper.Models.ViewModels;
using DailyHelper.Models.ViewModels.Requests;
using DailyHelper.Settings;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;

namespace DailyHelper.Services
{
    public class IdentityService : IIdentityService
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly JwtSettings _jwtSettings;
        public IdentityService(UserManager<IdentityUser> userManager, JwtSettings settings)
        {
            _userManager = userManager;
            _jwtSettings = settings;
        }
        
        public async Task<AuthenticationResult> RegisterAsync(UserRegistrationRequest request)
        {
            var existingUser = await _userManager.FindByEmailAsync(request.Email);

            if (existingUser != null)
            {
                return new AuthenticationResult()
                {
                    Errors = new[] { "User with this Email already exist" }
                };
            }

            var newUser = new IdentityUser()
            {
                Email = request.Email,
                UserName = request.Name
            };

            var createdUser = await _userManager
                .CreateAsync(newUser,request.Password);

            if (!createdUser.Succeeded)
            {
                new AuthenticationResult()
                {
                    Errors = createdUser.Errors.Select(x => x.Description)
                };
            }

            return GetAuthenticationResult(newUser);
        }

        public async Task<AuthenticationResult> LoginAsync(UserLoginRequest request)
        {
            var user = await _userManager.FindByEmailAsync(request.Email);

            if (user == null)
            {
                return new AuthenticationResult()
                {
                    Errors = new[] { "That user doesn't exist" }
                };
            }

            var hasValidPassword = await _userManager.CheckPasswordAsync(user, request.Password);

            if (!hasValidPassword)
            {
                return new AuthenticationResult()
                {
                    Errors = new[] { "Wrong email/password" }
                };
            }
            
            return GetAuthenticationResult(user);
        }

        private AuthenticationResult GetAuthenticationResult(IdentityUser user)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_jwtSettings.Secret);
            var tokenDescriptor = new SecurityTokenDescriptor()
            {
                Subject = new ClaimsIdentity(new[]
                {
                    new Claim(JwtRegisteredClaimNames.Sub, user.UserName),
                    new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                    new Claim(JwtRegisteredClaimNames.Email, user.Email),
                    new Claim("Id", user.Id)
                }),
                Expires = DateTime.Now.AddHours(2),
                SigningCredentials = new SigningCredentials(
                    new SymmetricSecurityKey(key),
                    SecurityAlgorithms.HmacSha256Signature),
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);

            return new AuthenticationResult()
            {
                Success = true,
                Token = tokenHandler.WriteToken(token)
            };
        }
    }
}