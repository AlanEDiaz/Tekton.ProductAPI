using Moq;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using Tekton.ProductAPI.Cqrs.Handlers.Queries.GetAllProducts;
using Tekton.ProductAPI.Domain.Entities;
using Tekton.ProductAPI.Infrastructure.Logger;
using Tekton.ProductAPI.Infrastructure.Repositories;
using FluentAssertions;
using System.Text.Json;
using FakeItEasy;

namespace Tekton.ProductAPI.Tests.Handlers
{
    [TestFixture]
    public class HandlersTestQueries
    {
        private GetAllProductsQueryHandler _handler;
        private IProductRepository _productRepository;
        private IHttpClientFactory _httpClientFactory;
        private ICommandRepository _commandRepository;
        private IAPILogger _apiLogger;

        [SetUp]
        public void SetUp()
        {
            _productRepository = A.Fake<IProductRepository>();
            _httpClientFactory = A.Fake<IHttpClientFactory>();
            _commandRepository = A.Fake<ICommandRepository>();
            _apiLogger = A.Fake<IAPILogger>();

            _handler = new GetAllProductsQueryHandler(
                _productRepository,
                _httpClientFactory,
                _commandRepository,
                _apiLogger);
        }

        [Test]
        public async Task Handle_WhenProductsExist_ReturnsResults()
        {
            // Arrange
            var products = new List<Product> { /* add your products here */ };
            A.CallTo(() => _productRepository.GetAll()).Returns(products);

            var httpClient = new HttpClient(new Mock<HttpMessageHandler>().Object);
            A.CallTo(() => _httpClientFactory.CreateClient(It.IsAny<string>())).Returns(httpClient);

            var handler = new GetAllProductsQueryHandler(_productRepository, _httpClientFactory, _commandRepository, _apiLogger);

            // Act
            var result = await handler.Handle(new GetAllProductsQuery(), CancellationToken.None);

            // Assert
            result.Should().NotBeNull(); // Add more assertions as needed
        }
    }
}
