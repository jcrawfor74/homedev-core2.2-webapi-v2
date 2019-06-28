using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using HomeDev.Core.WebApi.Extensions;
using HomeDev.Core.WebApi.Interfaces;
using HomeDev.Core.WebApi.Models;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace HomeDev.Core.WebApi.Services
{
    public class TokenAuthenticationService : IAuthenticationService
    {
        private readonly IUserManagementService _userManagementService;
        private readonly TokenSettings _tokenSettings;

        public TokenAuthenticationService(
            IUserManagementService userManagementService,
            IOptions<TokenSettings> tokenOptions
        )
        {
            _userManagementService = userManagementService;
            _tokenSettings = tokenOptions.Value;
        }
        public bool IsAuthenticated(AuthTokenRequest request, out string token)
        {
            token = string.Empty;
            if (!_userManagementService.IsValidUser(request.Username, request.Password).AwaitResult<bool>())
            {
                return false;
            }
            var claims = new []
            {
                new Claim(ClaimTypes.Name, request.Username)
            };
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_tokenSettings.Secret));
            var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var jwtToken = new JwtSecurityToken(
                _tokenSettings.Issuer,
                _tokenSettings.Audience,
                claims,
                expires : DateTime.Now.AddMinutes(_tokenSettings.AccessExpiration),
                signingCredentials : credentials
            );
            token = new JwtSecurityTokenHandler().WriteToken(jwtToken);
            return true;
        }
    }
}