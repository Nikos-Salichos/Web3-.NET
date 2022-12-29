using Application.CQRS.Queries;
using Domain.Models;
using Infrastructure.Persistence.Interfaces;
using MediatR;

namespace Application.Handlers.SmartContracts
{
    public class FindSmartContractHandler : IRequestHandler<FindSmartContractQuery, IEnumerable<SmartContract>>
    {
        private readonly IUnitOfWorkRepository _unitOfWork;

        public FindSmartContractHandler(IUnitOfWorkRepository unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<IEnumerable<SmartContract>> Handle(FindSmartContractQuery request, CancellationToken cancellationToken)
        {
            var smartContract = await _unitOfWork.SmartContractRepository.FindSmartContractAsync(request.Predicate);
            return smartContract;
        }
    }
}
