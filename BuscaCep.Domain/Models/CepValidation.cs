using FluentValidation;

namespace BuscaCep.Domain.Models
{
    public class CepValidation : AbstractValidator<CepEntrada>
    {
        public CepValidation()
        {
            RuleFor(x => x.Cep).NotNull().NotEmpty();
            RuleFor(x => x.Cep).Length(8);
            RuleFor(x => x.Cep).Matches("\\d{8}");
        }
    }
}
