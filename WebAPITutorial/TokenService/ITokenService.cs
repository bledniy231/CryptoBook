using Microsoft.AspNetCore.Identity;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using WebAPITutorial.Models;

namespace WebAPITutorial.TokenService
{
	public interface ITokenService
	{
		public string CreateAccessToken(UserModel user, List<IdentityRole<long>> roles);
		public string CreateRefreshToken();
		public ClaimsPrincipal? GetPrincipalFromExpiredToken(string? accessToken);
		public JwtSecurityToken CreateJwtToken(List<Claim> claims);
	}
}
