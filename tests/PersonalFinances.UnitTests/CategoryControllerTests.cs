using Microsoft.AspNetCore.Mvc;
using PersonalFinances.Controllers;
using PersonalFinances.Models;
using PersonalFinances.Services;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit;

namespace PersonalFinances.UnitTests
{
    public class CategoriesControllerTests
    {
        private readonly ICategoryService _categoryService;

        public CategoriesControllerTests()
        {
            _categoryService = new MockCategoryService(); // Substitua pelo mock adequado
        }

        [Fact]
        public async Task ListAllCategories_ReturnsOk()
        {
            // Arrange
            var controller = new CategoriesController(_categoryService);

            // Act
            var result = await controller.ListAllCategories();

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            var categories = Assert.IsAssignableFrom<IEnumerable<CategoryModel>>(okResult.Value);
            Assert.NotEmpty(categories);
        }

        [Fact]
        public async Task ListAllIncomeCategories_ReturnsOk()
        {
            // Arrange
            var controller = new CategoriesController(_categoryService);

            // Act
            var result = await controller.ListAllIncomeCategories();

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            var incomeCategories = Assert.IsAssignableFrom<IEnumerable<CategoryModel>>(okResult.Value);
            Assert.NotEmpty(incomeCategories);
        }

        [Fact]
        public async Task ListAllExpensesCategories_ReturnsOk()
        {
            // Arrange
            var controller = new CategoriesController(_categoryService);

            // Act
            var result = await controller.ListAllExpensesCategories();

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            var expensesCategories = Assert.IsAssignableFrom<IEnumerable<CategoryModel>>(okResult.Value);
            Assert.NotEmpty(expensesCategories);
        }

        // Classe mock para simular o serviço ICategoryService
        public class MockCategoryService : ICategoryService
        {
            public Task<List<CategoryModel>> GetAllCategories()
            {
                // Simula a recuperação de categorias
                return Task.FromResult(new List<CategoryModel>
                {
                    new CategoryModel { CategoryID = 1, Name = "Category 1", Type = "Expense" },
                    new CategoryModel { CategoryID = 2, Name = "Category 2", Type = "Income" }
                });
            }

            public Task<List<CategoryModel>> GetExpensesCategories()
            {
                // Simula a recuperação de categorias de despesas
                return Task.FromResult(new List<CategoryModel>
                {
                    new CategoryModel { CategoryID = 1, Name = "Expense Category 1", Type = "Expense" },
                    new CategoryModel { CategoryID = 2, Name = "Expense Category 2", Type = "Expense" }
                });
            }

            public Task<List<CategoryModel>> GetIncomeCategories()
            {
                // Simula a recuperação de categorias de renda
                return Task.FromResult(new List<CategoryModel>
                {
                    new CategoryModel { CategoryID = 1, Name = "Income Category 1", Type = "Income" },
                    new CategoryModel { CategoryID = 2, Name = "Income Category 2", Type = "Income" }
                });
            }
        }
    }
}
