using System.Collections.Generic;
using System;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using refactor_this.Controllers;
using refactor_this.Models;
using refactor_this.Services;
using Moq;
using AutoFixture;

namespace RefactorThis.Tests.ProductsTests
{
    [TestClass]
    public class ProductsControllerTests
    {
        private Mock<IProductService> _mockProductService;
        private Mock<IProductOptionService> _mockProductOptionService;
        private ProductsController _controller;
        private IFixture _fixture;

        [TestInitialize]
        public void TestInitialize()
        {
            // Mock the IProductService
            _mockProductService = new Mock<IProductService>();
            _mockProductOptionService = new Mock<IProductOptionService>();
            _fixture = new Fixture();

            // Instantiate the controller with the mocked service
            _controller = new ProductsController(_mockProductService.Object, _mockProductOptionService.Object); // Assuming you're only testing the GetAll method and don't need the ProductOptionService
        }

        [TestMethod]
        public async Task GetAll_CallsGetAllProducts_Once()
        {
            // Arrange
            var mockProducts = new List<Product>
            {
                new Product { Id = Guid.NewGuid(), Name = "Test Product 1" },
                new Product { Id = Guid.NewGuid(), Name = "Test Product 2" }
            };

            _mockProductService.Setup(service => service.GetAllProductsAsync()).ReturnsAsync(mockProducts);

            // Act
            var result = await _controller.GetAllAsync();

            // Assert
            Assert.IsNotNull(result);
            _mockProductService.Verify(service => service.GetAllProductsAsync(), Times.Once);
        }

        [TestMethod]
        public async Task SearchByName_CallsSearchByName_Once()
        {
            var name = "test";
            // Arrange
            var mockProducts = new List<Product>
            {
                new Product { Id = Guid.NewGuid(), Name = name },
                new Product { Id = Guid.NewGuid(), Name = name }
            };

            _mockProductService.Setup(service => service.GetProductsByNameAsync(It.Is<string>(p => string.Equals(p, name)))).ReturnsAsync(mockProducts);

            // Act
            var result = await _controller.SearchByNameAsync(name);

            // Assert
            Assert.IsNotNull(result);
            _mockProductService.Verify(service => service.GetProductsByNameAsync(name), Times.Once);
        }

        [TestMethod]
        public async Task GetProduct_CallsGetProductById_Once()
        {
            var id = Guid.NewGuid();

            // Arrange
            var mockProducts = new List<Product>
            {
                new Product { Id = id, Name = "return_product", IsNew = false},
                new Product { Id = Guid.NewGuid(), Name = "ignore_product", IsNew = false}
            };

            _mockProductService.Setup(service => service.GetProductByIdAsync(It.Is<Guid>(p => Guid.Equals(p, id)))).ReturnsAsync(mockProducts[0]);

            // Act
            var result = await _controller.GetProductAsync(id);

            // Assert
            Assert.IsNotNull(result);
            _mockProductService.Verify(service => service.GetProductByIdAsync(id), Times.Once);
        }

        [TestMethod]
        public async Task Create_CallsSaveProduct_Once()
        {
            var product = _fixture.Create<Product>();

            _mockProductService.Setup(service => service.SaveProductAsync(It.Is<Product>(p => p == product)));

            // Act
            await _controller.CreateAsync(product);

            // Assert
            _mockProductService.Verify(service => service.SaveProductAsync(product), Times.Once);
        }

        [TestMethod]
        public async Task Update_CallsUpsertProduct_Once()
        {
            var product = _fixture.Create<Product>();

            _mockProductService.Setup(service => service.UpsertProductAsync(It.Is<Guid>(g => g == product.Id),It.Is<Product>(p => p == product)));

            // Act
            await _controller.UpdateAsync(product.Id, product);

            // Assert
            _mockProductService.Verify(service => service.UpsertProductAsync(product.Id, product), Times.Once);
        }

        [TestMethod]
        public async Task Delete_CallsDeleteProduct_Once()
        {
            var product = _fixture.Create<Product>();

            _mockProductService.Setup(service => service.DeleteProductAsync(It.Is<Guid>(g => g == product.Id)));

            // Act
            await _controller.DeleteAsync(product.Id);

            // Assert
            _mockProductService.Verify(service => service.DeleteProductAsync(product.Id), Times.Once);
        }
    }
}
