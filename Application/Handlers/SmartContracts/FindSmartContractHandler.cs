using Application.CQRS.Queries;
using Domain.Models;
using Infrastructure.Persistence.Interfaces;

namespace Application.Handlers.SmartContracts
{
    public class FindSmartContractHandler
    {
        private readonly IUnitOfWorkRepository _unitOfWork;

        public FindSmartContractHandler(IUnitOfWorkRepository unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<SmartContract> Handle(FindSmartContractQuery request, CancellationToken cancellationToken)
        {
            var smartContract = await _unitOfWork.SmartContractRepository.Find(request. == );
            return smartContract;
        }
    }
}
