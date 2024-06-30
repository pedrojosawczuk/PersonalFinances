using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using Moq;
using PersonalFinances.Controllers;
using PersonalFinances.Models;
using PersonalFinances.Services;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace UnitTests.Controllers;

public class UserControllerTests
{
	private readonly UserController _controller;
	private readonly Mock<IUserService> _userService;

	public UserControllerTests()
	{
		_userService = new Mock<IUserService>();
		_controller = new UserController(_userService.Object);

		_controller.ControllerContext = new ControllerContext
		{
			HttpContext = new DefaultHttpContext()
		};
	}

	private void SetAuthorizationHeader(string token)
	{
		_controller.ControllerContext.HttpContext.Request.Headers["Authorization"] = $"{token}";
	}

	private string GenerateToken(long userId, string email)
	{
		var tokenHandler = new JwtSecurityTokenHandler();
		var key = Encoding.ASCII.GetBytes("cafecafecafecafecafecafecafecafecafecafecafecafecafecafecafecafecafecafecafecafecafecafecafecafe");

		var tokenDescriptor = new SecurityTokenDescriptor
		{
			Subject = new ClaimsIdentity(new[]
			 {
						  new Claim("id", userId.ToString()),
						  new Claim("email", email)
					 }),
			Expires = DateTime.UtcNow.AddDays(30),
			SigningCredentials = new SigningCredentials(
				  new SymmetricSecurityKey(key),
				  SecurityAlgorithms.HmacSha256Signature)
		};

		var token = tokenHandler.CreateToken(tokenDescriptor);
		return tokenHandler.WriteToken(token);
	}

	[Fact]
	public async Task Verify_ValidToken_ReturnsOk()
	{
		// Arrange
		var token = GenerateToken(1, "john.doe@example.com");
		SetAuthorizationHeader(token);

		var user = new UserModel
		{
			UserID = 1,
			Name = "John",
			LastName = "Doe",
			Email = "john.doe@example.com",
			Photo = null,
		};

		_userService.Setup(s => s.GetUserById(It.IsAny<long>())).ReturnsAsync(user);

		// Act
		var result = await _controller.Verify();

		// Assert
		Assert.IsType<OkObjectResult>(result);
	}

	[Fact]
	public async Task Login_ValidCredentials_ReturnsOk()
	{
		// Arrange
		var user = new UserModel
		{
			Email = "john.doe@example.com",
			Password = "password123"
		};

		var dbUser = new UserModel
		{
			UserID = 1,
			Email = user.Email,
			Password = PasswordUtility.HashPassword(user.Password)
		};

		var tokenString = "generated-jwt-token";

		_userService.Setup(s => s.IsEmailValid(user.Email)).Returns(true);
		_userService.Setup(s => s.AuthenticateUser(user.Email, user.Password)).ReturnsAsync(dbUser);
		_userService.Setup(s => s.GenerateJwtToken(dbUser)).Returns(tokenString);

		// Act
		var result = await _controller.Login(user);

		// Assert
		var okResult = Assert.IsType<OkObjectResult>(result);
		var returnValue = Assert.IsType<String>(okResult.Value);
		Assert.Equal(tokenString, returnValue);
	}
}