using MediatR;
using System.Threading.Tasks;
using System.Threading;
using Tekton.ProductAPI.Cqrs.Handlers.Commands.CreateProduct;
using Tekton.ProductAPI.Domain.Entities;
using Tekton.ProductAPI.Infrastructure.Repositories;
using Tekton.ProductAPI.Infrastructure.Caching;
using System.Collections.Generic;
using Tekton.ProductAPI.Models;
using AutoMapper;
using System;
using Tekton.ProductAPI.Infrastructure.Logger;
using System.Diagnostics;
using System.Net.Http;
using System.Text;

namespace Tekton.ProductAPI.Cqrs.Handlers.Commands.UpdateProduct
{
    public class UpdateProductCommandHandler : IRequestHandler<UpdateProductCommand, bool>
    {
        private readonly IProductRepository _productRepository;
        private readonly ICommandRepository _commandRepository;
        private readonly IProductCache _cache;
        private readonly IMapper _mapper; 
        private readonly IAPILogger _logger;
        private readonly IHttpClientFactory _httpClientFactory;


        public UpdateProductCommandHandler(IProductRepository productRepository, ICommandRepository commandRepository,IProductCache productCache,IMapper mapper,IAPILogger logger, IHttpClientFactory httpClientFactory)
        {
            _productRepository = productRepository;
            _commandRepository = commandRepository;
            _cache = productCache;
            _mapper = mapper;
            _logger = logger;
            _httpClientFactory = httpClientFactory;
        }
        public async Task<bool> Handle(UpdateProductCommand request, CancellationToken cancellationToken)
        {
            var stopWatch = Stopwatch.StartNew();

            var product=new Product() {
            ProductId = request.ProductId,
            Description=request.Description,
            Discount=request.Discount,
            Name=request.Name,
            Price=request.Price,
            Stock=request.Stock,
            UpdatedOn = DateTime.Now
            };

            if (_cache.TryGetValue(request.ProductId,out CachedProduct cachedProduct).Result)
            {
                var newProductToCatch= new CachedProduct {

                    LastAccessed= DateTime.Now,
                    UpdatedOn = DateTime.Now,
                    ProductId = request.ProductId,
                    Description = request.Description,
                    Discount = request.Discount,
                    Name = request.Name,
                    Price = request.Price,
                    Stock = request.Stock


                };
                await _cache.TryUpdate(request.ProductId, cachedProduct, newProductToCatch);
            }
            try
            {
                var requestUri = $"https://65d265bc987977636bfc4c1c.mockapi.io/api/v1/Discount/{request.ProductId}";

                var content = new StringContent($"{{\"Percentage\": {request.Discount}, \"productId\": \"{request.ProductId}\"}}", Encoding.UTF8, "application/json");

                using var response = await _httpClientFactory.CreateClient().PutAsync(requestUri, content, cancellationToken);
                response.EnsureSuccessStatusCode();
            }
            catch (Exception ex)
            {
                stopWatch.Stop();
                var Time = stopWatch.ElapsedMilliseconds;
                _logger.LogError(ex.Message);
                _logger.LogInfo($"Response time for UpdateProductCommand query:{Time} miliseconds");
                throw new HttpRequestException("Can't communicate with server");
            }

            await _commandRepository.Push(product);
            var IsUpdated=await _productRepository.UpdateProductAsync(product);

            stopWatch.Stop();
            var elapsedTime = stopWatch.ElapsedMilliseconds;
            _logger.LogInfo($"Response time for UpdateProductCommand query: {elapsedTime} milliseconds");
            return IsUpdated;
        }
    }
}
