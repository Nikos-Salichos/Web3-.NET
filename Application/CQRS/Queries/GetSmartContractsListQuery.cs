using Domain.Models;
using MediatR;

namespace Application.CQRS.Queries
{
    public class GetSmartContractsListQuery : IRequest<IEnumerable<SmartContract>>
    {
    }
}
