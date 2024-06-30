using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using PersonalFinances.Controllers;
using PersonalFinances.Models;
using PersonalFinances.Services;

namespace UnitTests.Controllers;

public class CategoriesControllerTests
{
    private readonly CategoriesController _controller;
    private readonly Mock<ICategoryService> _categoryService;

    public CategoriesControllerTests()
    {
        _categoryService = new Mock<ICategoryService>();
        _controller = new CategoriesController(_categoryService.Object);

        _controller.ControllerContext = new ControllerContext
        {
            HttpContext = new DefaultHttpContext()
        };
    }

    [Fact]
    public async Task ListAllCategories_ReturnsOk()
    {
        // Arrange
        CategoryModel expected1 = new CategoryModel { CategoryID = 1, Name = "Category 1", Type = "Expense" };
        CategoryModel expected2 = new CategoryModel { CategoryID = 2, Name = "Category 2", Type = "Income" };
        var categories = new List<CategoryModel>();
        categories.Add(expected1);
        categories.Add(expected2);

        _categoryService.Setup(service => service.GetAllCategories()).ReturnsAsync(categories);

        // Act
        var result = await _controller.ListAllCategories();

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result.Result);
        var returnValue = Assert.IsAssignableFrom<List<CategoryModel>>(okResult.Value);
        Assert.NotEmpty(returnValue);
        Assert.IsType<List<CategoryModel>>(returnValue);
        Assert.Contains<CategoryModel>(expected1, returnValue);
        Assert.Contains<CategoryModel>(expected2, returnValue);
    }

    [Fact]
    public async Task ListAllIncomeCategories_ReturnsNoContent()
    {
        // Arrange
        List<CategoryModel> incomeCategories = new List<CategoryModel>();

        _categoryService.Setup(service => service.GetIncomeCategories()).ReturnsAsync(incomeCategories);

        // Act
        var result = await _controller.ListAllIncomeCategories();

        // Assert
        Assert.IsType<NoContentResult>(result.Result);
    }

    [Fact]
    public async Task ListAllExpensesCategories_ReturnsOk()
    {
        // Arrange
        CategoryModel expected1 = new CategoryModel { CategoryID = 1, Name = "Expense Category 1", Type = "Expense" };
        CategoryModel expected2 = new CategoryModel { CategoryID = 2, Name = "Expense Category 2", Type = "Expense" };
        var expensesCategories = new List<CategoryModel>();
        expensesCategories.Add(expected1);
        expensesCategories.Add(expected2);

        _categoryService.Setup(service => service.GetExpensesCategories()).ReturnsAsync(expensesCategories);

        // Act
        var result = await _controller.ListAllExpensesCategories();

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result.Result);
        var returnValue = Assert.IsAssignableFrom<List<CategoryModel>>(okResult.Value);
        Assert.NotEmpty(returnValue);
        Assert.IsType<List<CategoryModel>>(returnValue.ToList());
        Assert.Contains<CategoryModel>(expected1, returnValue);
        Assert.Contains<CategoryModel>(expected2, returnValue);
    }
}