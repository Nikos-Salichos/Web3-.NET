using Domain.Models;
using MediatR;

namespace Application.CQRS.Commands
{
    public class CreateSmartContractCommand : SmartContract, IRequest<SmartContract>
    {
        public SmartContract SmartContract { get; set; }

        public CreateSmartContractCommand(SmartContract smartContract)
        {
            SmartContract = smartContract;
        }
    }
}
