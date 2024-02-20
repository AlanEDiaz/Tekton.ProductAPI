using MediatR;
using System.Threading.Tasks;
using System.Threading;
using Tekton.ProductAPI.Domain.Entities;
using Tekton.ProductAPI.Models;
using System.Collections.Generic;
using Tekton.ProductAPI.Cqrs.Handlers.Queries.GetAllProducts;
using AutoMapper;
using Tekton.ProductAPI.Infrastructure.Repositories;
using Tekton.ProductAPI.Infrastructure.Caching;
using System.Net.Http;
using System;
using System.Diagnostics.Eventing.Reader;
using System.Text.Json;
using System.Linq;
using Tekton.ProductAPI.Infrastructure.Logger;
using System.Diagnostics;

namespace Tekton.ProductAPI.Cqrs.Handlers.Queries.GetProduct
{
    public class GetProductQueryHandler : IRequestHandler<GetProductQuery, GetProductQueryResult>
    {
        private readonly IProductRepository _productRepository;
        private readonly IProductCache _cache;
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly ICommandRepository _commandRepository;
        private readonly IAPILogger _logger;


        public GetProductQueryHandler(IProductRepository productRepository, IProductCache cache, IHttpClientFactory httpClientFactory, ICommandRepository commandRepository, IAPILogger logger)
        {
            _productRepository = productRepository;
            _cache = cache;
            _httpClientFactory = httpClientFactory;
            _commandRepository = commandRepository;
            _logger = logger;

        }


        public async Task<GetProductQueryResult> Handle(GetProductQuery request, CancellationToken cancellationToken)
        {
            var stopWatch = Stopwatch.StartNew();
            var product = _cache.TryGetValue(request.ProductId, out CachedProduct cachedProduct).Result ? cachedProduct : await _productRepository.GetById(request.ProductId);
            var requestUri = $"https://65d265bc987977636bfc4c1c.mockapi.io/api/v1/Discount?productId={product.ProductId}";
            using var response = await _httpClientFactory.CreateClient().GetAsync(requestUri, cancellationToken);
            if (response.IsSuccessStatusCode)
            {
                var discount = new List<Discount>();
                try
                {
                    var contentStream = await response.Content.ReadAsStreamAsync();
                    discount = await JsonSerializer.DeserializeAsync<List<Discount>>(contentStream, cancellationToken: cancellationToken);
                }
                catch (Exception ex)
                {

                    throw new Exception($"Failed to deserialize discount Message: {ex.Message};");
                }
                if (product is CachedProduct cachedProductlist)
                {
                    var productResult = product != null ? new GetProductQueryResult
                    {
                        ProductId = cachedProductlist.ProductId,
                        Description = cachedProductlist.Description,
                        Name = cachedProductlist.Name,
                        Price = cachedProductlist.Price ?? 0,
                        StatusName = cachedProductlist.StatusName,
                        Stock = cachedProductlist.Stock ?? 0,
                        Discount = cachedProductlist.Discount ?? 0,
                        FinalPrice = cachedProductlist.Price ?? 0 * (100 - discount.FirstOrDefault().Percentage) / 100




                    } : null;
                    await _commandRepository.Push(productResult);

                    stopWatch.Stop();
                    var elapsedTime = stopWatch.ElapsedMilliseconds;
                    _logger.LogInfo($"Response time for GetProductQuery query: {elapsedTime} milliseconds");


                    return productResult;
                }
                else
                {
                    await _productRepository.AddProductCache(product);

                    var productResult = product != null ? new GetProductQueryResult
                    {

                        ProductId = product.ProductId,
                        Description = product.Description,
                        Name = product.Name,
                        Price = product.Price ?? 0,
                        StatusName = 0,
                        Stock = product.Stock ?? 0,
                        Discount = product.Discount ?? 0,
                        FinalPrice = product.Price ?? 0 * (100 - discount.FirstOrDefault().Percentage) / 100



                    } : null;

                    await _commandRepository.Push(productResult);

                    stopWatch.Stop();
                    var elapsedTime = stopWatch.ElapsedMilliseconds;
                    _logger.LogInfo($"Response time for GetProductQuery query: {elapsedTime} milliseconds");

                    return productResult;
                }
            }
            else if (response.StatusCode == System.Net.HttpStatusCode.NotFound)
            {
                await _productRepository.AddProductCache(product);

                var productResult = new GetProductQueryResult
                {
                    ProductId = product.ProductId,
                    Description = product.Description,
                    Discount = product.Discount ?? 0,
                    Price = product.Price ?? 0,
                    FinalPrice = (product.Price ?? 0) * (100 - (product.Discount ?? 0)) / 100,
                    Name = product.Name,
                    Stock = product.Stock ?? 0,
                    StatusName = 0

                };

                await _commandRepository.Push(productResult);

                stopWatch.Stop();
                var elapsedTime = stopWatch.ElapsedMilliseconds;
                _logger.LogInfo($"Response time for GetProductQuery query: {elapsedTime} milliseconds");

                return productResult;

            }
            else
            {
                var exception = new Exception($"Failed to validate productID on Discount server. Status code: {response.StatusCode}");
                stopWatch.Stop();
                var elapsedTime = stopWatch.ElapsedMilliseconds;
                _logger.LogInfo($"Response time for GetProductQuery query: {elapsedTime} milliseconds");
                _logger.LogError(exception.Message);


                throw exception;
            }

        }




    }

}


