using MediatR;
using System.Threading.Tasks;
using System.Threading;
using Tekton.ProductAPI.Cqrs.Handlers.Queries.GetProduct;
using Tekton.ProductAPI.Domain.Entities;
using Tekton.ProductAPI.Models;
using System.Collections.Generic;
using AutoMapper;
using System.Net.Http;
using System.Text;
using System;
using System.Text.Json;
using Tekton.ProductAPI.Infrastructure.Repositories;
using System.Linq;
using Tekton.ProductAPI.Infrastructure.Logger;
using System.Diagnostics;


namespace Tekton.ProductAPI.Cqrs.Handlers.Queries.GetAllProducts
{
    public class GetAllProductsQueryHandler : IRequestHandler<GetAllProductsQuery, IEnumerable<GetAllProductsQueryResult>>
    {
        private readonly IProductRepository _productRepository;
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly ICommandRepository _commandRepository;
        private readonly IAPILogger _APILogger;
        public GetAllProductsQueryHandler(IProductRepository productRepository, IHttpClientFactory httpClientFactory,ICommandRepository commandRepository,IAPILogger apiLogger)
        {
            _productRepository = productRepository;
            _httpClientFactory = httpClientFactory;
            _commandRepository = commandRepository;
            _APILogger = apiLogger;
        }

        public async Task<IEnumerable<GetAllProductsQueryResult>> Handle(GetAllProductsQuery request, CancellationToken cancellationToken)
        {
            var stopWatch = Stopwatch.StartNew();
            var products = await _productRepository.GetAll();
            var productsResult=new List<GetAllProductsQueryResult>();
            if (products!=null)
            {
                foreach (var product in products)
                {
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
                            _APILogger.LogError(ex.Message);
                            throw new Exception($"Failed to deserialize discount Message: {ex.Message};");
                        }
                       

                        

                        productsResult.Add(new GetAllProductsQueryResult
                        {
                            ProductId = product.ProductId,
                            Description = product.Description,
                            Discount = discount.FirstOrDefault().Percentage,
                            Price = product.Price ?? 0,
                            FinalPrice = (product.Price ?? 0) * (100 - discount.FirstOrDefault().Percentage) / 100,
                            Name = product.Name,
                            Stock = product.Stock ?? 0,
                            StatusName = 2

                        });


                    }
                    else if (response.StatusCode == System.Net.HttpStatusCode.NotFound)
                    {
                        productsResult.Add(new GetAllProductsQueryResult
                        {
                            ProductId = product.ProductId,
                            Description = product.Description,
                            Discount = product.Discount ?? 0,
                            Price = product.Price ?? 0,
                            FinalPrice = (product.Price ?? 0) * (100 - (product.Discount ?? 0)) / 100,
                            Name = product.Name,
                            Stock = product.Stock ?? 0,
                            StatusName = 2

                        });

                    }
                    else
                    {
                        var exception= new Exception($"Failed to validate product ID. Status code: {response.StatusCode}");
                        _APILogger.LogError(exception.Message);

                        throw exception;
                    }

                    await _commandRepository.Push(product);
                }



                
                stopWatch.Stop();
                var elapsedTime = stopWatch.ElapsedMilliseconds;
                _APILogger.LogInfo($"Response time for GetAllProductsQuery query: {elapsedTime} milliseconds");
                return productsResult;
            }
            else {
                stopWatch.Stop();
                var elapsedTime = stopWatch.ElapsedMilliseconds;
                _APILogger.LogInfo($"Response time for GetAllProductsQuery query: {elapsedTime} milliseconds");

                return null; }
        }

    }
}
