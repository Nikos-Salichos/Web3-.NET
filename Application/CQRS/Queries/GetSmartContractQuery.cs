using Domain.Models;
using MediatR;

namespace Application.CQRS.Queries
{
    public class GetSmartContractQuery : IRequest<SmartContract>
    {
        public long Id { get; set; }

        public GetSmartContractQuery(long id)
        {
            Id = id;
        }
    }
}
