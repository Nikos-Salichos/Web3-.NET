using Application.CQRS.Queries;
using Domain.Models;
using Infrastructure.Persistence.Interfaces;
using MediatR;

namespace Application.Handlers.SmartContracts
{
    public class SmartContractHandler : IRequestHandler<GetSmartContractsListQuery, IEnumerable<SmartContract>>
    {
        private readonly IUnitOfWork _unitOfWork;

        public SmartContractHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<IEnumerable<SmartContract>> Handle(GetSmartContractsListQuery request, CancellationToken cancellationToken)
        {
            return await _unitOfWork.SmartContractRepository.GetSmartContracts();
        }
    }
}
