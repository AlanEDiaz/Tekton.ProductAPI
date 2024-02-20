using AutoMapper;
using LiteDB;
using MediatR;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Tekton.ProductAPI.Cqrs.Handlers.Commands.CreateProduct;
using Tekton.ProductAPI.Cqrs.Handlers.Commands.UpdateProduct;
using Tekton.ProductAPI.Cqrs.Handlers.Queries.GetAllProducts;
using Tekton.ProductAPI.Cqrs.Handlers.Queries.GetProduct;
using Tekton.ProductAPI.Models;

namespace Tekton.ProductAPI.Services
{
    public class ProductService
    {
        private readonly IMediator _mediator;
        private readonly IMapper _mapper;

        public ProductService(IMediator mediator, IMapper mapper)
        {
            _mediator = mediator;
            _mapper = mapper;
        }

        public async Task<IEnumerable<ProductDto>> GetAllProductsAsync()
        {
            var products = await _mediator.Send(new GetAllProductsQuery());
            return _mapper.Map<IEnumerable<ProductDto>>(products);
        }

        public async Task<ProductDto> GetProductByIdAsync(Guid id)
        {
            var product = await _mediator.Send(new GetProductQuery(id));
            return _mapper.Map<ProductDto>(product);
        }

        public async Task<Guid> CreateProductAsync(CreateProductDto product)
        {
            var command = await _mediator.Send(new CreateProductCommand
            {
                Name = product.Name,
                Description = product.Description,
                Price = product.Price,
                Stock = product.Stock,
                Discount = product.Discount

            });
            return command;
        }

        public async Task<bool> UpdateProductAsync(UpdateProductDto product)
        {
            var command = new UpdateProductCommand {
            ProductId=product.ProductId,
            Name= product.Name,
            Description = product.Description,  
            Price = product.Price.Value,
            Stock = product.Stock.Value,
            Discount = product.Discount.Value
            };
            return await _mediator.Send(command);
        }
    }
}
