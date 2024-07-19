using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using refactor_this.Models;
using refactor_this.Repositories;
using refactor_this.Services;

namespace RefactorThis.Tests.ServiceTests
{
    [TestClass]
    public class ProductOptionServiceTests
    {
        private Mock<IProductOptionRepository> _mockRepo;
        private ProductOptionService _service;

        [TestInitialize]
        public void Initialize()
        {
            _mockRepo = new Mock<IProductOptionRepository>();
            _service = new ProductOptionService(_mockRepo.Object);
        }

        [TestMethod]
        public async Task GetAllProductOptionsByIdAsync_ReturnsAllProductOptions()
        {
            // Arrange
            var productId = Guid.NewGuid();
            var productOptions = new List<ProductOption>
            {
                new ProductOption { Id = Guid.NewGuid(), ProductId = productId, Name = "Option 1" },
                new ProductOption { Id = Guid.NewGuid(), ProductId = productId, Name = "Option 2" }
            };
            _mockRepo.Setup(repo => repo.GetAllByProductIdAsync(productId)).ReturnsAsync(productOptions);

            // Act
            var result = await _service.GetAllProductOptionsByIdAsync(productId);

            // Assert
            Assert.AreEqual(2, result.Count());
        }

        [TestMethod]
        public async Task GetProductOptionByIdAsync_ReturnsProductOption()
        {
            // Arrange
            var productId = Guid.NewGuid();
            var optionId = Guid.NewGuid();
            var productOption = new ProductOption { Id = optionId, ProductId = productId, Name = "Option 1" };
            _mockRepo.Setup(repo => repo.GetByIdAsync(productId, optionId)).ReturnsAsync(productOption);

            // Act
            var result = await _service.GetProductOptionByIdAsync(productId, optionId);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(optionId, result.Id);
        }

        [TestMethod]
        public async Task SaveProductOptionAsync_IsNew_AddsNewProductOption()
        {
            // Arrange
            var productOption = new ProductOption { IsNew = true, Name = "New Option" };
            _mockRepo.Setup(repo => repo.AddAsync(productOption)).Returns(Task.CompletedTask).Verifiable();

            // Act
            await _service.SaveProductOptionAsync(productOption);

            // Assert
            _mockRepo.Verify(repo => repo.AddAsync(productOption), Times.Once);
            _mockRepo.Verify(repo => repo.UpdateAsync(productOption), Times.Never);
        }

        [TestMethod]
        public async Task SaveProductOptionAsync_IsNotNew_AddsNewProductOption()
        {
            // Arrange
            var productOption = new ProductOption { IsNew = false, Name = "New Option" };
            _mockRepo.Setup(repo => repo.AddAsync(productOption)).Returns(Task.CompletedTask).Verifiable();

            // Act
            await _service.SaveProductOptionAsync(productOption);

            // Assert
            _mockRepo.Verify(repo => repo.AddAsync(productOption), Times.Never);
            _mockRepo.Verify(repo => repo.UpdateAsync(productOption), Times.Once);
        }

        [TestMethod]
        public async Task DeleteProductOptionAsync_DeletesProductOption()
        {
            // Arrange
            var productId = Guid.NewGuid();
            var optionId = Guid.NewGuid();
            var productOption = new ProductOption { Id = optionId, ProductId = productId, Name = "Option to Delete" };
            _mockRepo.Setup(repo => repo.GetByIdAsync(productId, optionId)).ReturnsAsync(productOption);
            _mockRepo.Setup(repo => repo.DeleteAsync(optionId)).Returns(Task.CompletedTask).Verifiable();

            // Act
            await _service.DeleteProductOptionAsync(productId, optionId);

            // Assert
            _mockRepo.Verify(repo => repo.DeleteAsync(optionId), Times.Once);
        }
    }
}
