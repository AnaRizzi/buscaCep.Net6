using BuscaCep.Domain.Dtos;
using Refit;

namespace BuscaCep.Domain.Interfaces
{
    public interface IViaCepRefit
    {
        [Get("/ws/{cep}/json/")]
        Task<ViaCepDto> GetCep(string cep);
    }
}
