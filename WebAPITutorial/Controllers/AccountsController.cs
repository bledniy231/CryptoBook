using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MimeKit;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using WebAPITutorial.DBContexts;
using WebAPITutorial.Models.Identity;
using WebAPITutorial.TokenService;

namespace WebAPITutorial.Controllers
{
    [ApiController]
	[Route("api/[controller]")]
	public class AccountsController : ControllerBase
	{
		private readonly ITokenService _tokenService;
		private readonly UserManager<User> _userManager;
		private readonly UserContext _userContext;
		private readonly IConfiguration _configuration;

		public AccountsController(ITokenService tokenService, UserManager<User> userManager, UserContext userContext, IConfiguration configuration)
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
		[ProducesResponseType(StatusCodes.Status404NotFound)]
		public async Task<ActionResult<AuthResponse>> Authenticate([FromBody] AuthRequest authRequest, bool isRegister = false)
		{
			if (!ModelState.IsValid) return BadRequest("Invalid model");

			User? managedUser = await _userManager.FindByEmailAsync(authRequest.Email);
			if (managedUser == null) return NotFound("Invalid e-mail");

			if (!await _userManager.CheckPasswordAsync(managedUser, authRequest.Password)) return BadRequest("Invalid password");

			User? user = _userContext.Users.FirstOrDefault(user => user.Email == authRequest.Email);
			if (user == null) return Unauthorized();

			List<long> roleIds = await _userContext.UserRoles.Where(role => role.UserId == user.Id).Select(role => role.RoleId).ToListAsync();
			var roles = await _userContext.Roles.Where(role => roleIds.Contains(role.Id)).ToListAsync();

			string accessToken = _tokenService.CreateAccessToken(user, roles);
			user.RefreshToken = _tokenService.CreateRefreshToken();
			user.RefreshTokenExpiryTime = DateTime.UtcNow.AddDays(_configuration.GetSection("Jwt:RefreshTokenValidityInDays").Get<int>());

			await _userContext.SaveChangesAsync();

			if (!isRegister)
				return Ok(new AuthResponse
				{
					Username = user.UserName!,
					Email = user.Email!,
					Token = accessToken,
					RefreshToken = user.RefreshToken
				});
			else
				return CreatedAtAction(nameof(Authenticate), new AuthResponse
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

			User user = new User
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

			User? userFromDB = _userContext.Users.FirstOrDefault(user => user.Email == registerRequest.Email);
			if (userFromDB == null) return BadRequest("Register failed after trying get user from DB");
			await _userManager.AddToRoleAsync(userFromDB, UserRoles.Student);

			return await Authenticate(new AuthRequest
			{
				Email = registerRequest.Email,
				Password = registerRequest.Password
			}, true);
		}

		[HttpPost("refresh-token")]
		[ProducesResponseType(StatusCodes.Status400BadRequest)]
		[ProducesResponseType(StatusCodes.Status200OK)]
		public async Task<ActionResult> RefreshUserTokens([FromBody] TokenModel? tokenModel)
		{
			if (tokenModel == null) return BadRequest("Invalid token model");

			string? accessToken = tokenModel.AccessToken;
			string? refreshToken = tokenModel.RefreshToken;
			ClaimsPrincipal? principal = _tokenService.GetPrincipalFromExpiredToken(accessToken);
			if (principal == null) return BadRequest("Invalid access token or refresh token");

			string? username = principal.Identity!.Name;
			User user = await _userManager.FindByNameAsync(username);

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
		[ProducesResponseType(StatusCodes.Status404NotFound)]
		[ProducesResponseType(StatusCodes.Status200OK)]
		[ProducesResponseType(StatusCodes.Status401Unauthorized)]
		public async Task<IActionResult> RevokeUser(string username)
		{
			User? user = await _userManager.FindByNameAsync(username);
			if (user == null) return NotFound("Invalid username");

			user.RefreshToken = null;
			await _userManager.UpdateAsync(user);

			return Ok();
		}

		[Authorize(Roles = UserRoles.Admin)]
		[HttpPost]
		[Route("revoke-all-users")]
		[ProducesResponseType(StatusCodes.Status200OK)]
		[ProducesResponseType(StatusCodes.Status401Unauthorized)]
		public async Task<IActionResult> RevokeAllUsers()
		{
			List<User> users = _userManager.Users.ToList();
			foreach (var user in users)
			{
				user.RefreshToken = null;
				await _userManager.UpdateAsync(user);
			}

			return Ok();
		}

		[Authorize]
		[HttpGet("get-user-info/{username}")]
		[ProducesResponseType(StatusCodes.Status200OK)]
		[ProducesResponseType(StatusCodes.Status404NotFound)]
		[ProducesResponseType(StatusCodes.Status401Unauthorized)]
		public async Task<ActionResult<User>> GetUserInfo(string username)
		{
			User? user = await _userContext.Users.FirstOrDefaultAsync(u => u.UserName == username);
			if (user == null) return NotFound("Invalid username");

			return Ok(user);
		}

		[Authorize]
		[HttpGet("get-user-image/{username}", Name = "get-user-image")]
		[ProducesResponseType(StatusCodes.Status200OK)]
		[ProducesResponseType(StatusCodes.Status404NotFound)]
		[ProducesResponseType(StatusCodes.Status401Unauthorized)]
		public async Task<IActionResult> GetUserImage(string username)
		{
			User? user = await _userContext.Users.FirstOrDefaultAsync(u => u.UserName == username);
			if (user == null) return NotFound("Invalid username");

			if (!System.IO.File.Exists(user.ImageLink)) return NotFound("Image not found");

			byte[] fileBites = System.IO.File.ReadAllBytes(user.ImageLink);

			string mimeType = MimeTypes.GetMimeType(Path.GetFileName(user.ImageLink));
			
			return Ok(File(fileBites, mimeType, Path.GetFileName(user.ImageLink)));
		}

		[Authorize]
		[HttpPost("post-description/{username}")]
		[ProducesResponseType(StatusCodes.Status200OK)]
		[ProducesResponseType(StatusCodes.Status404NotFound)]
		[ProducesResponseType(StatusCodes.Status401Unauthorized)]
		public async Task<ActionResult> PostDescription(string username, [FromBody] string description)
		{
			User? user = await _userContext.Users.FirstOrDefaultAsync(u => u.UserName == username);
			if (user == null) return NotFound("Invalid username");

			user.Description = description;
			await _userContext.SaveChangesAsync();

			return Ok();
		}

		[Authorize]
		[HttpPost("upload-profile-image/{username}")]
		[Consumes("multipart/form-data")]
		[ProducesResponseType(StatusCodes.Status200OK)]
		[ProducesResponseType(StatusCodes.Status404NotFound)]
		[ProducesResponseType(StatusCodes.Status400BadRequest)]
		[ProducesResponseType(StatusCodes.Status401Unauthorized)]
		public async Task<ActionResult> UploadProfileImage(string username, IFormFile file)
		{
			if (file == null || file.Length == 0) return BadRequest("Invalid file");

			User? user = await _userContext.Users.FirstOrDefaultAsync(u => u.UserName == username);
			if (user == null) return NotFound("Invalid username");

			string uniqueFileName = Guid.NewGuid().ToString() + "_" + file.FileName;
			string imagesFolderPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/profile_images");
			string filePath = Path.Combine(imagesFolderPath, uniqueFileName);
			Directory.CreateDirectory(imagesFolderPath);

			using (var stream = new FileStream(filePath, FileMode.Create))
			{
				await file.CopyToAsync(stream);
			}

			user.ImageLink = filePath;
			await _userContext.SaveChangesAsync();

			return Ok();
		}
	}
}
