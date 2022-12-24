using FluentValidation;

namespace Application.Validators
{
    public class SmartContractDtoValidator : AbstractValidator<SmartContractDTO>
    {
        public SmartContractDtoValidator()
        {
            RuleFor(x => x.Bytecode).NotNull();
            RuleFor(x => x.Bytecode).NotEmpty();

            RuleFor(x => x.Abi).NotNull();
            RuleFor(x => x.Abi).NotEmpty();
        }
    }
}
