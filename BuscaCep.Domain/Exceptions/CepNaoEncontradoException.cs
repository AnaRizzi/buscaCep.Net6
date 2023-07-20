namespace BuscaCep.Domain.Exceptions
{
    public class CepNaoEncontradoException : ApplicationException
    {
        public CepNaoEncontradoException() : base("Cep Não Encontrado")
        {
        }

        public CepNaoEncontradoException(string mensagem) : base(mensagem)
        {
        }
    }
}
