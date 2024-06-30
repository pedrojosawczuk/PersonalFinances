using System.Text;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using PersonalFinances.Models;
using PersonalFinances.DataContext;
using Microsoft.AspNetCore.Hosting;
using System.Net.Http.Json;
using PersonalFinances.Services;

namespace IntegrationTests;

public class UserControllerIntegrationTests : IClassFixture<CustomWebApplicationFactory<Startup>>
{
	private readonly HttpClient _client;
	private readonly CustomWebApplicationFactory<Startup> _factory;

	public UserControllerIntegrationTests(CustomWebApplicationFactory<Startup> factory)
	{
		_factory = factory;
		_client = factory.CreateClient();
	}

	[Fact]
	public async Task Login_ReturnsOk_WithValidCredentials()
	{
		// Arrange
		var user = new UserModel
		{
			Email = "john.doe@example.com",
			Password = "password"
		};
		var json = JsonConvert.SerializeObject(user);
		var stringContent = new StringContent(json, Encoding.UTF8, "application/json");

		using (var scope = _factory.Server.Services.CreateScope())
		{
			var context = scope.ServiceProvider.GetRequiredService<EFDataContext>();
			context.Users.RemoveRange(context.Users);
			context.Users.Add(new UserModel { Email = "john.doe@example.com", Password = PasswordUtility.HashPassword(user.Password) });
			context.SaveChanges();
		}

		// Act
		var response = await _client.PostAsync("/api/user/login", stringContent);

		// Assert
		response.EnsureSuccessStatusCode();
		var responseString = await response.Content.ReadAsStringAsync();
		Assert.NotEmpty(responseString);
	}
}