using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using PersonalFinances.Controllers;
using PersonalFinances.DataContext;
using PersonalFinances.DataContextTest;
using PersonalFinances.Models;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Xunit;

namespace PersonalFinances.UnitTests;

public class CategoriesControllerTests
{
    private EFDataContext _context;

    public CategoriesControllerTests()
    {
        _context = EFDataContextMock.GetMockedDbContext();
    }

    [Fact]
    public async Task ListAllCategories_ReturnsOkResult()
    {
        // Arrange
        var mockHttpContext = new Mock<HttpContext>();
        var token = GenerateJwtToken(1); // Suponha que você tenha um método para gerar o token JWT
        mockHttpContext.SetupGet(x => x.Request.Headers["Authorization"]).Returns(token);

        var controller = new CategoriesController(_context)
        {
            ControllerContext = new ControllerContext
            {
                HttpContext = mockHttpContext.Object
            }
        };

        // Act
        var result = await controller.ListAllCategories();

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result);
        var categories = Assert.IsAssignableFrom<IEnumerable<CategoryModel>>(okResult.Value);
        Assert.Equal(2, categories.Count());
    }

    private string GenerateJwtToken(long userId)
    {
        // Implementar a geração de token JWT com base no userId
        return new JwtSecurityTokenHandler().WriteToken(new JwtSecurityToken(
            claims: new[] { new Claim("id", userId.ToString()) }
        ));
    }
}
