namespace BuscaCep.Domain.Dtos
{
    public record ViaCepDto(
        string cep, 
        string logradouro, 
        string complemento, 
        string bairro, 
        string localidade, 
        string uf, 
        string ddd
        );
}
