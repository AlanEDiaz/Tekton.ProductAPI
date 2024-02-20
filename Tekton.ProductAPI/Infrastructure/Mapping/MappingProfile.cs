using AutoMapper;
using Tekton.ProductAPI.Cqrs.Handlers.Queries.GetAllProducts;
using Tekton.ProductAPI.Cqrs.Handlers.Queries.GetProduct;
using Tekton.ProductAPI.Domain.Entities;
using Tekton.ProductAPI.Models;

namespace Tekton.ProductAPI.Infrastructure.Mapping
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<GetAllProductsQueryResult, ProductDto>();
            CreateMap<GetProductQueryResult, ProductDto>();

        }
    }
}
