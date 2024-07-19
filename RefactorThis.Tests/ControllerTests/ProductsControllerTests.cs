using System.Collections.Generic;
using System;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using refactor_this.Controllers;
using refactor_this.Models;
using refactor_this.Services;
using Moq;
using AutoFixture;

namespace RefactorThis.Tests.ControllerTests
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
            // Arrange
            var name = "test";

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
            // Arrange
            var id = Guid.NewGuid();

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
            // Arrange
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
            // Arrange
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
            // Arrange
            var product = _fixture.Create<Product>();

            _mockProductService.Setup(service => service.DeleteProductAsync(It.Is<Guid>(g => g == product.Id)));

            // Act
            await _controller.DeleteAsync(product.Id);

            // Assert
            _mockProductService.Verify(service => service.DeleteProductAsync(product.Id), Times.Once);
        }

        [TestMethod]
        public async Task GetOptions_CallsGetAllProductOptionsById_Once()
        {
            // Arrange
            var productOption1 = _fixture.Create<ProductOption>();
            var productOption2 = _fixture.Create<ProductOption>();

            var productGuid = Guid.NewGuid();

            var mockProductOptions = new List<ProductOption>
            {
                productOption1,
                productOption2
            };

            _mockProductOptionService.Setup(service => service.GetAllProductOptionsByIdAsync(productGuid)).ReturnsAsync(mockProductOptions);

            // Act
            var result = await _controller.GetOptionsAsync(productGuid);

            // Assert
            Assert.IsNotNull(result);
            _mockProductOptionService.Verify(service => service.GetAllProductOptionsByIdAsync(productGuid), Times.Once);
        }

        [TestMethod]
        public async Task GetOption_CallsGetProductOptionById_Once()
        {
            // Arrange
            var productOption = _fixture.Create<ProductOption>();
            productOption.IsNew = false;

            _mockProductOptionService.Setup(service => service.GetProductOptionByIdAsync(productOption.ProductId, productOption.Id)).ReturnsAsync(productOption);

            // Act
            var result = await _controller.GetOptionAsync(productOption.ProductId, productOption.Id);

            // Assert
            Assert.IsNotNull(result);
            _mockProductOptionService.Verify(service => service.GetProductOptionByIdAsync(productOption.ProductId, productOption.Id), Times.Once);
        }

        [TestMethod]
        public async Task CreateOption_CallsSaveProductOption_Once()
        {
            // Arrange
            var productOption = _fixture.Create<ProductOption>();
            productOption.IsNew = true;

            _mockProductOptionService.Setup(service => service.SaveProductOptionAsync(productOption));

            // Act
            await _controller.CreateOptionAsync(productOption.ProductId, productOption);

            // Assert
            _mockProductOptionService.Verify(service => service.SaveProductOptionAsync(productOption), Times.Once);
        }

        [TestMethod]
        public async Task UpdateOption_CallsUpsertProductOption_Once()
        {
            // Arrange
            var productOption = _fixture.Create<ProductOption>();
            productOption.IsNew = false;

            _mockProductOptionService.Setup(service => service.UpsertProductOptionAsync(productOption.ProductId, productOption.Id, productOption));

            // Act
            await _controller.UpdateOptionAsync(productOption.ProductId, productOption.Id, productOption);

            // Assert
            _mockProductOptionService.Verify(service => service.UpsertProductOptionAsync(productOption.ProductId, productOption.Id, productOption), Times.Once);
        }

        [TestMethod]
        public async Task DeleteOption_CallsDeleteProductOption_Once()
        {
            // Arrange
            var productOption = _fixture.Create<ProductOption>();
            productOption.IsNew = false;

            _mockProductOptionService.Setup(service => service.DeleteProductOptionAsync(productOption.ProductId, productOption.Id));

            // Act
            await _controller.DeleteOptionAsync(productOption.ProductId, productOption.Id);

            // Assert
            _mockProductOptionService.Verify(service => service.DeleteProductOptionAsync(productOption.ProductId, productOption.Id), Times.Once);
        }
    }
}
