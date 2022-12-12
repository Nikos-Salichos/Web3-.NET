using Domain.Models;
using FluentValidation;

namespace Application.Validators
{
    public class SmartContractValidator : AbstractValidator<SmartContract>
    {
        public SmartContractValidator()
        {
            RuleFor(x => x.Bytecode).NotNull();
            RuleFor(x => x.Bytecode).NotEmpty();

            RuleFor(x => x.Abi).NotNull();
            RuleFor(x => x.Abi).NotEmpty();
        }
    }
}
