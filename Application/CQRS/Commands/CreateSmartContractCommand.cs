using Domain.Models;
using MediatR;

namespace Application.CQRS.Commands
{
    public class CreateSmartContractCommand : IRequest<SmartContract>
    {
    }
}
