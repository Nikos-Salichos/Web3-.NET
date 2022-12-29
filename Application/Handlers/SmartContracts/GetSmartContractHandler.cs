using Application.CQRS.Queries;
using Domain.Models;
using Infrastructure.Persistence.Interfaces;
using MediatR;

namespace Application.Handlers.SmartContracts
{
    internal class GetSmartContractHandler : IRequestHandler<GetSmartContractQuery, SmartContract>
    {
        private readonly IUnitOfWorkRepository _unitOfWork;

        public GetSmartContractHandler(IUnitOfWorkRepository unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<SmartContract> Handle(GetSmartContractQuery request, CancellationToken cancellationToken)
        {
            var smartContract = await _unitOfWork.SmartContractRepository.GetSmartContractAsync(request.Id);
            return smartContract;
        }
    }
}
