using MediatR;
using System.Collections.Generic;

namespace Tekton.ProductAPI.Cqrs.Handlers.Queries.GetAllProducts
{
    public class GetAllProductsQuery : IRequest<IEnumerable<GetAllProductsQueryResult>> { }

}
