using Domain.DTOs;
using MediatR;

namespace Application.CQRS.Commands
{
    public class CreateSmartContractCommand : IRequest<SmartContractDTO>
    {
    }
}
