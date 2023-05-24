using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using WebAPITutorial.Models;

namespace WebAPITutorial.TokenService
{
	public class TokenService : ITokenService
	{
		private readonly IConfiguration _config;
		public TokenService(IConfiguration config)
		{
			_config = config;
		}

		public string CreateAccessToken(UserModel user, List<IdentityRole<long>> roles)
		{
			JwtSecurityToken token = CreateJwtToken(CreateClaims(user, roles));
			JwtSecurityTokenHandler tokenHandler = new JwtSecurityTokenHandler();
			return tokenHandler.WriteToken(token);
		}

		public string CreateRefreshToken()
		{
			byte[] randomNumber = new byte[64];

			using RandomNumberGenerator rng = RandomNumberGenerator.Create();
				rng.GetBytes(randomNumber);

			return Convert.ToBase64String(randomNumber);
		}

		public ClaimsPrincipal? GetPrincipalFromExpiredToken(string? accessToken)
		{
			TokenValidationParameters tokenValidationParameters = new TokenValidationParameters
			{
				ValidateAudience = false,
				ValidateIssuer = false,
				ValidateIssuerSigningKey = true,
				ValidateLifetime = false,
				IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:Secret"]!))
			};

			JwtSecurityTokenHandler tokenHandler = new JwtSecurityTokenHandler();
			ClaimsPrincipal principal = tokenHandler.ValidateToken(accessToken, tokenValidationParameters, out var securityToken);

			if (securityToken is not JwtSecurityToken jwtSecurityToken 
				|| !jwtSecurityToken.Header.Alg.Equals(SecurityAlgorithms.HmacSha256, StringComparison.InvariantCultureIgnoreCase))
				return null;

			return principal;
		}

		public JwtSecurityToken CreateJwtToken(List<Claim> claims)
		{
			var authSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:Secret"]!));
			int expirationTimeInMin = _config.GetSection("Jwt:Expire").Get<int>();

			return new JwtSecurityToken(
				_config["Jwt:Issuer"],
				_config["Jwt:Audience"],
				claims: claims,
				expires: DateTime.UtcNow.AddMinutes(expirationTimeInMin),
				signingCredentials: new SigningCredentials(authSigningKey, SecurityAlgorithms.HmacSha256));
		}

		private List<Claim> CreateClaims(UserModel user, List<IdentityRole<long>> roles)
		{
			return new List<Claim>
			{
				//new(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
				//new(JwtRegisteredClaimNames.Iat, DateTime.UtcNow.ToString(CultureInfo.InvariantCulture)),
				new(ClaimTypes.NameIdentifier, user.Id.ToString()),
				new(ClaimTypes.Name, user.UserName!),
				new(ClaimTypes.Email, user.Email!),
				new(ClaimTypes.Role, string.Join(" ", roles.Select(role => role.Name))),
			};
		}
	}
}
