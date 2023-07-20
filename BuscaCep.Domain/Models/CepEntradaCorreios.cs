using MediatR;

namespace BuscaCep.Domain.Models
{
    public record class CepEntradaCorreios(string Cep) : IRequest<CepSaidaCorreios>
    {
        public void Validate()
        {
            CepCorreiosValidation validator = new CepCorreiosValidation();
            var results = validator.Validate(this);
            var erros = new List<string>();

            if (!results.IsValid)
            {
                foreach (var failure in results.Errors)
                {
                    erros.Add("Campo " + failure.PropertyName + " invalido. Erro: " + failure.ErrorMessage);
                }
            }

            if (erros.Count() != 0)
            {
                var mensagemErro = "";
                foreach (string erro in erros)
                {
                    mensagemErro += $"{erro} ; ";
                }
                throw new Exception(mensagemErro);
            }
        }
    }
}
