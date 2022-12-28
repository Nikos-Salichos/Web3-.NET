using Domain.Models;
using MediatR;

namespace Application.CQRS.Queries
{
    public class GetSmartContractQuery : IRequest<SmartContract>
    {
        public string Id { get; set; }

        public GetSmartContractQuery(string id)
        {
            Id = id;
        }
    }
}
