using Application.CQRS.Queries;
using Domain.DTOs;
using Infrastructure.Persistence.Interfaces;
using MediatR;

namespace Application.Handlers.SmartContracts
{
    public class SmartContractHandler : IRequestHandler<GetSmartContractsListQuery, IEnumerable<SmartContractDTO>>
    {
        private readonly IUnitOfWork _unitOfWork;

        public SmartContractHandler()
        {

        }

        public Task<IEnumerable<SmartContractDTO>> Handle(GetSmartContractsListQuery request, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
    }
}
