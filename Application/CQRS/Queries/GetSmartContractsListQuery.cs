using Domain.Models;
using MediatR;

namespace Application.CQRS.Queries
{
    public class GetSmartContractsListQuery : IRequest<IEnumerable<SmartContract>>
    {
        public int PageSize { get; set; }
        public int PageNumber { get; set; }

        public GetSmartContractsListQuery(int pageSize, int pageNumber)
        {
            PageSize = pageSize;
            PageNumber = pageNumber;
        }
    }
}
