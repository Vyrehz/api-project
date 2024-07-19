using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using refactor_this.Models;
using refactor_this.Repositories;
using refactor_this.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RefactorThis.Tests.ServiceTests
{
    [TestClass]
    public class ProductServiceTests
    {
        private Mock<IProductRepository> _mockRepo;
        private ProductService _service;

        [TestInitialize]
        public void Initialize()
        {
            _mockRepo = new Mock<IProductRepository>();
            _service = new ProductService(_mockRepo.Object);
        }

        [TestMethod]
        public async Task GetAllProductsAsync_ReturnsAllProducts()
        {
            // Arrange
            var products = new List<Product>
            {
                new Product { Id = Guid.NewGuid(), Name = "Product 1" },
                new Product { Id = Guid.NewGuid(), Name = "Product 2" }
            };
            _mockRepo.Setup(repo => repo.GetAllAsync()).ReturnsAsync(products);

            // Act
            var result = await _service.GetAllProductsAsync();

            // Assert
            Assert.AreEqual(2, result.Count());
        }

        [TestMethod]
        public async Task GetProductsByNameAsync_ReturnsFilteredProducts()
        {
            // Arrange
            var name = "Product 1";
            var products = new List<Product>
            {
                new Product { Id = Guid.NewGuid(), Name = name }
            };
            _mockRepo.Setup(repo => repo.GetByNameAsync(name)).ReturnsAsync(products);

            // Act
            var result = await _service.GetProductsByNameAsync(name);

            // Assert
            var resultList = result.ToList();
            Assert.IsTrue(resultList.Count() == 1);
            Assert.AreEqual(name, resultList.First().Name);
        }

        [TestMethod]
        public async Task GetProductByIdAsync_ReturnsProduct()
        {
            // Arrange
            var id = Guid.NewGuid();
            var product = new Product { Id = id, Name = "Product 1" };
            _mockRepo.Setup(repo => repo.GetByIdAsync(id)).ReturnsAsync(product);

            // Act
            var result = await _service.GetProductByIdAsync(id);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(id, result.Id);
        }

        [TestMethod]
        public async Task SaveProductAsync_IsNew_AddsNewProduct()
        {
            // Arrange
            var product = new Product { IsNew = true, Name = "New Product" };
            _mockRepo.Setup(repo => repo.AddAsync(product)).Returns(Task.CompletedTask).Verifiable();

            // Act
            await _service.SaveProductAsync(product);

            // Assert
            _mockRepo.Verify(repo => repo.AddAsync(product), Times.Once);
            _mockRepo.Verify(repo => repo.UpdateAsync(product), Times.Never);
        }

        [TestMethod]
        public async Task SaveProductAsync_IsNotNew_UpdatesProduct()
        {
            // Arrange
            var product = new Product { IsNew = false, Name = "New Product" };
            _mockRepo.Setup(repo => repo.AddAsync(product)).Returns(Task.CompletedTask).Verifiable();

            // Act
            await _service.SaveProductAsync(product);

            // Assert
            _mockRepo.Verify(repo => repo.AddAsync(product), Times.Never);
            _mockRepo.Verify(repo => repo.UpdateAsync(product), Times.Once);
        }

        [TestMethod]
        public async Task DeleteProductAsync_DeletesProduct()
        {
            // Arrange
            var id = Guid.NewGuid();
            var product = new Product { Id = id, Name = "Product to Delete" };
            _mockRepo.Setup(repo => repo.GetByIdAsync(id)).ReturnsAsync(product);
            _mockRepo.Setup(repo => repo.DeleteAsync(id)).Returns(Task.CompletedTask).Verifiable();

            // Act
            await _service.DeleteProductAsync(id);

            // Assert
            _mockRepo.Verify(repo => repo.DeleteAsync(id), Times.Once);
        }
    }
}