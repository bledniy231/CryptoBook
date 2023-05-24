using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using WebAPITutorial.DBContexts;
using WebAPITutorial.Models;
using WebAPITutorial.Models.Identity;
using WebAPITutorial.TokenService;

namespace WebAPITutorial.Controllers
{
	[ApiController]
	[Route("[controller]")]
	public class AccountsController : ControllerBase
	{
		private readonly ITokenService _tokenService;
		private readonly UserManager<UserModel> _userManager;
		private readonly UserContext _userContext;
		private readonly IConfiguration _configuration;

		public AccountsController(ITokenService tokenService, UserManager<UserModel> userManager, UserContext userContext, IConfiguration configuration)
		{
			_tokenService = tokenService;
			_userManager = userManager;
			_userContext = userContext;
			_configuration = configuration;
		}

		[HttpPost("login")]
		[ProducesResponseType(StatusCodes.Status400BadRequest)]
		[ProducesResponseType(StatusCodes.Status200OK)]
		[ProducesResponseType(StatusCodes.Status401Unauthorized)]
		public async Task<ActionResult<AuthResponse>> Authenticate([FromBody] AuthRequest authRequest)
		{
			if (!ModelState.IsValid) return BadRequest("Invalid model");

			UserModel? managedUser = await _userManager.FindByEmailAsync(authRequest.Email);
			if (managedUser == null) return BadRequest("Invalid e-mail");

			if (!await _userManager.CheckPasswordAsync(managedUser, authRequest.Password)) return BadRequest("Invalid password");

			UserModel? user = _userContext.Users.FirstOrDefault(user => user.Email == authRequest.Email);
			if (user == null) return Unauthorized();

			List<long> roleIds = await _userContext.UserRoles.Where(role => role.UserId == user.Id).Select(role => role.RoleId).ToListAsync();
			var roles = await _userContext.Roles.Where(role => roleIds.Contains(role.Id)).ToListAsync();

			string accessToken = _tokenService.CreateAccessToken(user, roles);
			user.RefreshToken = _tokenService.CreateRefreshToken();
			user.RefreshTokenExpiryTime = DateTime.UtcNow.AddDays(_configuration.GetSection("Jwt:RefreshTokenValidityInDays").Get<int>());

			await _userContext.SaveChangesAsync();

			return Ok(new AuthResponse
			{
				Username = user.UserName!,
				Email = user.Email!,
				Token = accessToken,
				RefreshToken = user.RefreshToken
			});
		}

		[HttpPost("register")]
		[ProducesResponseType(StatusCodes.Status400BadRequest)]
		[ProducesResponseType(StatusCodes.Status200OK)]
		public async Task<ActionResult<AuthResponse>> Register([FromBody] RegisterRequest registerRequest)
		{
			if (!ModelState.IsValid) return BadRequest("Invalid model");

			UserModel user = new UserModel
			{
				FirstName = registerRequest.FirstName,
				SecondName = registerRequest.SecondName,
				Email = registerRequest.Email,
				UserName = registerRequest.Username
			};

			IdentityResult result = await _userManager.CreateAsync(user, registerRequest.Password);

			if (!result.Succeeded)
			{
				List<string> errors = new List<string>();
				result.Errors.ToList().ForEach(err => errors.Add(err.Description));
				return BadRequest(errors);
			}

			UserModel? userFromDB = _userContext.Users.FirstOrDefault(user => user.Email == registerRequest.Email);
			if (userFromDB == null) return BadRequest("Register failed after trying get user from DB");
			await _userManager.AddToRoleAsync(userFromDB, UserRoles.Student);

			return await Authenticate(new AuthRequest
			{
				Email = registerRequest.Email,
				Password = registerRequest.Password
			});
		}

		[HttpPost("refresh-token")]
		[ProducesResponseType(StatusCodes.Status400BadRequest)]
		[ProducesResponseType(StatusCodes.Status200OK)]
		public async Task<ActionResult> RefreshUserTokens(TokenModel? tokenModel)
		{
			if (tokenModel == null) return BadRequest("Invalid token model");

			string? accessToken = tokenModel.AccessToken;
			string? refreshToken = tokenModel.RefreshToken;
			ClaimsPrincipal? principal = _tokenService.GetPrincipalFromExpiredToken(accessToken);
			if (principal == null) return BadRequest("Invalid access token or refresh token");

			string? username = principal.Identity!.Name;
			UserModel user = await _userManager.FindByNameAsync(username);

			if (user == null || user.RefreshToken != refreshToken || user.RefreshTokenExpiryTime <= DateTime.UtcNow)
				return BadRequest("Invalid access token or refresh token");

			JwtSecurityToken newAccessToken = _tokenService.CreateJwtToken(principal.Claims.ToList());
			string newRefreshToken = _tokenService.CreateRefreshToken();
			user.RefreshToken = newRefreshToken;

			await _userManager.UpdateAsync(user);

			return new ObjectResult(new
			{
				accessToken = new JwtSecurityTokenHandler().WriteToken(newAccessToken),
				refreshToken = newRefreshToken
			});
		}

		[Authorize]
		[HttpPost]
		[Route("revoke/{username}")]
		[ProducesResponseType(StatusCodes.Status400BadRequest)]
		[ProducesResponseType(StatusCodes.Status200OK)]
		public async Task<IActionResult> RevokeUser(string username)
		{
			UserModel? user = await _userManager.FindByNameAsync(username);
			if (user == null) return BadRequest("Invalid username");

			user.RefreshToken = null;
			await _userManager.UpdateAsync(user);

			return Ok();
		}

		[Authorize]
		[HttpPost]
		[Route("revoke-all-users")]
		[ProducesResponseType(StatusCodes.Status400BadRequest)]
		[ProducesResponseType(StatusCodes.Status200OK)]
		public async Task<IActionResult> RevokeAllUsers()
		{
			List<UserModel> users = _userManager.Users.ToList();
			foreach (var user in users)
			{
				user.RefreshToken = null;
				await _userManager.UpdateAsync(user);
			}

			return Ok();
		}
	}
}
