using AutoMapper;
using FakeItEasy;
using FluentAssertions;
using Moq;
using Moq.Protected;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Reflection.Metadata;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Tekton.ProductAPI.Cqrs.Handlers.Commands.CreateProduct;
using Tekton.ProductAPI.Cqrs.Handlers.Commands.UpdateProduct;
using Tekton.ProductAPI.Domain.Entities;
using Tekton.ProductAPI.Infrastructure.Caching;
using Tekton.ProductAPI.Infrastructure.Logger;
using Tekton.ProductAPI.Infrastructure.Mapping;
using Tekton.ProductAPI.Infrastructure.Repositories;

namespace Tekton.ProductAPI.Tests.Handlers
{

    [TestFixture]
    public class HandlersTestCommands
    {
        private CreateProductCommandHandler _createHandler;
        private UpdateProductCommandHandler _updateHandler;
        private IProductRepository _productRepository;
        private ICommandRepository _commandRepository;
        private IAPILogger _apiLogger;
        private IProductCache _cache;
        private IMapper _mapper;
        private IHttpClientFactory _httpClientFactory;
        private Guid _expectedProductId;

        [SetUp]
        public void SetUp()
        {
            _productRepository = A.Fake<IProductRepository>();
            _commandRepository = A.Fake<ICommandRepository>();
            _apiLogger = A.Fake<IAPILogger>();
            _cache = A.Fake<IProductCache>();
            _httpClientFactory = A.Fake<IHttpClientFactory>();
            _mapper = new Mapper(new MapperConfiguration(cfg => cfg.AddProfile(new MappingProfile())));
            _expectedProductId = Guid.NewGuid();

            A.CallTo(() => _productRepository.Add(A<Product>._)).Returns(Task.FromResult(_expectedProductId));

            _createHandler = new CreateProductCommandHandler(
                _productRepository,
                _httpClientFactory,
                _commandRepository,
                _apiLogger);

            _updateHandler = new UpdateProductCommandHandler(
                _productRepository,
                _commandRepository,
                _cache,
                _mapper,
                _apiLogger,
                _httpClientFactory);
        }

        [Test]
        public async Task Handle_WithNegativePrice_ShouldThrowException()
        {
            // Arrange
            var createProductCommand = new CreateProductCommand
            {
                Name = "Test Product",
                Description = "Test Description",
                Price = -10.5m, // Negative price
                Stock = 100,
                Discount = 5
            };

            // Act & Assert
            Assert.ThrowsAsync<ArgumentException>(() => _createHandler.Handle(createProductCommand, CancellationToken.None));
        }

        [Test]
        public async Task Handle_WithPositivePrice_ShouldReturnProductId()
        {
            // Arrange
            var createProductCommand = new CreateProductCommand
            {
                Name = "Test Product",
                Description = "Test Description",
                Price = 10.5m,
                Stock = 100,
                Discount = 5
            };

            // Act
            var result = await _createHandler.Handle(createProductCommand, CancellationToken.None);

            // Assert
            result.Should().Be(_expectedProductId);
        }
        [Test]
        public async Task Handle_WhenProductExists_ShouldReturnTrue()
        {
            // Arrange
            var request = new UpdateProductCommand
            {
                ProductId = Guid.NewGuid(),
                Name = "Updated Product",
                Description = "Updated Description",
                Stock = 100,
                Price = 50.0m,
                Discount = 10.0m
            };

            var cachedProduct = new CachedProduct
            {
                ProductId = request.ProductId,
                LastAccessed = DateTime.Now,
                Name = "Updated Product",
                Description = "Updated Description",
                Stock = 100,
                Price = 50.0m,
                Discount = 10.0m
            };

            A.CallTo(() => _cache.TryGetValue(request.ProductId, out cachedProduct)).Returns(true);

            var httpClient = new HttpClient(new Mock<HttpMessageHandler>().Object);
            A.CallTo(() => _httpClientFactory.CreateClient(It.IsAny<string>())).Returns(httpClient);

            A.CallTo(() => _productRepository.UpdateProductAsync(A<Product>._)).Returns(true);

            // Act
            var result = await _updateHandler.Handle(request, CancellationToken.None);

            // Assert
            result.Should().BeTrue();
        }

        [Test]
        public void Handle_WhenProductDoesNotExist_ShouldThrowException()
        {
            // Arrange
            var request = new UpdateProductCommand
            {
                ProductId = Guid.NewGuid(),
                Name = "Updated Product",
                Description = "Updated Description",
                Stock = 100,
                Price = 50.0m,
                Discount = 10.0m
            };

            CachedProduct cachedProduct;
            A.CallTo(() => _cache.TryGetValue(request.ProductId, out cachedProduct))
                .Returns(false);

            // Act & Assert
            Func<Task> act = async () => await _updateHandler.Handle(request, CancellationToken.None);
            act.Should().ThrowAsync<InvalidOperationException>();
        }

    }
}

