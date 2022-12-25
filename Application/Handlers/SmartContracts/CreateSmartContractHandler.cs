using Application.CQRS.Commands;
using Domain.Models;
using Infrastructure.Persistence.Interfaces;
using MediatR;

namespace Application.Handlers.SmartContracts
{
    public class CreateSmartContractHandler : SmartContract, IRequestHandler<CreateSmartContractCommand, SmartContract>
    {
        private readonly IUnitOfWork _unitOfWork;

        public CreateSmartContractHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
    }
}
