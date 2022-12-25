using Application.CQRS.Queries;
using Domain.Models;
using Infrastructure.Persistence.Interfaces;
using MediatR;

namespace Application.Handlers.SmartContracts
{
    public class SmartContractHandler : IRequestHandler<GetSmartContractsListQuery, IEnumerable<SmartContract>>
    {
        private readonly IUnitOfWork _unitOfWork;

        private readonly IMediator _mediator;
        public SmartContractHandler(IUnitOfWork unitOfWork, IMediator mediator)
        {
            _unitOfWork = unitOfWork;
            _mediator = mediator;
        }

        public async Task<IEnumerable<SmartContract>> Handle(GetSmartContractsListQuery request, CancellationToken cancellationToken)
        {
            var allSmartContracts = await _unitOfWork.SmartContractRepository.GetSmartContracts();
            return allSmartContracts;
        }
    }
}
