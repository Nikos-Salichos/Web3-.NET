using Domain.DTOs;
using MediatR;

namespace Application.CQRS.Queries
{
    public class GetSmartContractsListQuery : IRequest<IEnumerable<SmartContractDTO>>
    {
    }
}
