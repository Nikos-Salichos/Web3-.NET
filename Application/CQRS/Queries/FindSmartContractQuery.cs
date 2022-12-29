using Domain.Models;
using MediatR;
using System.Linq.Expressions;

namespace Application.CQRS.Queries
{
    public class FindSmartContractQuery : IRequest<IEnumerable<SmartContract>>
    {
        public Expression<Func<SmartContract, bool>> Predicate { get; set; }

        public FindSmartContractQuery(Expression<Func<SmartContract, bool>> predicate)
        {
            Predicate = predicate;
        }
    }
}
