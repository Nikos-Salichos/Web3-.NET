using Application.CQRS.Commands;
using Domain.Models;
using Infrastructure.Persistence.Interfaces;
using MediatR;

namespace Application.Handlers.SmartContracts
{
    public class CreateSmartContractHandler : SmartContract, IRequestHandler<CreateSmartContractCommand, SmartContract>
    {
        private readonly IUnitOfWorkRepository _unitOfWork;

        public CreateSmartContractHandler(IUnitOfWorkRepository unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<SmartContract> Handle(CreateSmartContractCommand request, CancellationToken cancellationToken)
        {
            await _unitOfWork.SmartContractRepository.AddSmartContractAsync(request.SmartContract);
            return request.SmartContract;
        }
    }
}
