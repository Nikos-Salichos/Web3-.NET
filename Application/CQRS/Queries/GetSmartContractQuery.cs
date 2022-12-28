using Domain.Models;
using MediatR;

namespace Application.CQRS.Queries
{
    public class GetSmartContractQuery : IRequest<SmartContract>
    {
    }
}
