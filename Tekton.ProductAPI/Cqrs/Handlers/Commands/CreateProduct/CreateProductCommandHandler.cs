using MediatR;
using System.Threading.Tasks;
using System.Threading;
using Tekton.ProductAPI.Domain.Entities;
using System;
using System.Net.Http;
using System.Text;
using Tekton.ProductAPI.Infrastructure.Repositories;
using Tekton.ProductAPI.Infrastructure.Caching;
using Tekton.ProductAPI.Infrastructure.Logger;
using System.Diagnostics;
using System.Timers;

namespace Tekton.ProductAPI.Cqrs.Handlers.Commands.CreateProduct
{
    public class CreateProductCommandHandler : IRequestHandler<CreateProductCommand, Guid>
    {
        private readonly IProductRepository _productRepository;
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly ICommandRepository _commandRepository;
        private readonly IAPILogger _apiLogger;


        public CreateProductCommandHandler(IProductRepository productRepository, IHttpClientFactory httpClientFactory, ICommandRepository commandRepository, IAPILogger apiLogger)
        {
            _productRepository = productRepository;
            _httpClientFactory = httpClientFactory;
            _commandRepository = commandRepository;
            _apiLogger = apiLogger;

        }
        public async Task<Guid> Handle(CreateProductCommand request, CancellationToken cancellationToken)
        {
            var stopWatch = Stopwatch.StartNew();

            if (request.Price <= 0 | request.Stock <= 0 )
            {
                var exception= new ArgumentException("Not Valid values for Price,Stock or Discount.");
                stopWatch.Stop();
                var Time = stopWatch.ElapsedMilliseconds;
                _apiLogger.LogError(exception.Message);
                _apiLogger.LogInfo($"Response time for CreateProductCommand query:{Time} miliseconds");
                throw exception;
            }


            var product = new Product
            {
                Name = request.Name,
                Description = request.Description,
                Price = request.Price,
                Stock = request.Stock,
                Discount = request.Discount,
                CreatedOn = DateTime.Now,
                UpdatedOn = null
            };


            var ProductId = await _productRepository.Add(product);
            await _productRepository.AddProductCache(product);

            try
            {
                var requestUri = "https://65d265bc987977636bfc4c1c.mockapi.io/api/v1/Discount";

                var content = new StringContent($"{{\"Percentage\": {request.Discount}, \"productId\": \"{ProductId}\"}}", Encoding.UTF8, "application/json");

                using var response = await _httpClientFactory.CreateClient().PostAsync(requestUri, content, cancellationToken);
                response.EnsureSuccessStatusCode();
            }
            catch (Exception ex)
            {
                stopWatch.Stop();
                var Time = stopWatch.ElapsedMilliseconds;
                _apiLogger.LogError(ex.Message);
                _apiLogger.LogInfo($"Response time for CreateProductCommand query:{Time} miliseconds");
                throw new HttpRequestException("Can't communicate with server");
            }
            stopWatch.Stop();
            var elapsedTime = stopWatch.ElapsedMilliseconds;
            _apiLogger.LogInfo($"Response time for CreateProductCommand query:{elapsedTime} miliseconds");

            await _commandRepository.Push(product);

            return ProductId;
        }
    }
}
