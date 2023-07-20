using FluentValidation;

namespace BuscaCep.Domain.Models
{
    public class CepCorreiosValidation : AbstractValidator<CepEntradaCorreios>
    {
        public CepCorreiosValidation()
        {
            RuleFor(x => x.Cep).NotNull().NotEmpty();
            RuleFor(x => x.Cep).Length(8);
            RuleFor(x => x.Cep).Matches("\\d{8}");
            //RuleFor(x => x.Cep).Matches("\\d{5}-\\d{3}");
        }
    }
}
