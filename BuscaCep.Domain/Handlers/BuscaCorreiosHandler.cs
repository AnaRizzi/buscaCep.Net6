using BuscaCep.Domain.Exceptions;
using BuscaCep.Domain.Models;
using MediatR;
using Microsoft.Extensions.Caching.Distributed;
using System.Text.Json;

namespace BuscaCep.Domain.Handlers
{
    public class BuscaCorreiosHandler : IRequestHandler<CepEntradaCorreios, CepSaidaCorreios>
    {
        private readonly IDistributedCache _distributedCache;
        private const string CEP_KEY = "Correios";

        public BuscaCorreiosHandler(IDistributedCache distributedCache)
        {
            _distributedCache = distributedCache;
        }
        public async Task<CepSaidaCorreios> Handle(CepEntradaCorreios request, CancellationToken cancellationToken)
        {
            request.Validate();

            /*Para rodar o redis no docker:
             * docker container run -d -p 6379:6379 --name redis redis
             */
            var cache = await _distributedCache.GetStringAsync($"{CEP_KEY}:{request.Cep}");
            if (!String.IsNullOrWhiteSpace(cache))
            {
                var responseCache = JsonSerializer.Deserialize<CepSaidaCorreios>(cache, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
                return responseCache!;
            }

            /* Para inserir o WebService dos Correios:
             * Clicar com o botão direito no projeto, Add, Connected Service
             * Na janela, selecionar Add Service Reference
             * Colocar o link do web service e clicar em Go (https://apps.correios.com.br/SigepMasterJPA/AtendeClienteService/AtendeCliente?wsdl)
             * Seleciona o serviço que vc quer e cria o namespace, depois é só finalizar
             * Ele irá criar a pasta Connected Service com toda a referência aos Correios             * 
             */
            var correios = new WebServiceCorreios.AtendeClienteClient();

            try
            {
                var retorno = await correios.consultaCEPAsync(request.Cep);
                if (retorno.@return == null)
                {
                    throw new CepNaoEncontradoException("Informações não encontradas.");
                }
                var response = new CepSaidaCorreios(request.Cep, retorno.@return.end, retorno.@return.bairro, retorno.@return.cidade, retorno.@return.uf);

                var memoryCacheEntryOptions = new DistributedCacheEntryOptions
                {
                    //tempo total de expiração em segundos, 2 horas:
                    AbsoluteExpirationRelativeToNow = TimeSpan.FromSeconds(7200),
                    //tempo relativo, se passar o tempo sem acesso, expira, reinicia a cada acesso:
                    SlidingExpiration = TimeSpan.FromSeconds(7200)
                };

                //serializar as informações que nos interessam:
                var json = JsonSerializer.Serialize(response);

                await _distributedCache.SetStringAsync($"{CEP_KEY}:{request.Cep}", json, memoryCacheEntryOptions);

                return response;                
            }
            catch (Exception ex)
            {
                throw new CepNaoEncontradoException($"Cep {request.Cep} não foi encontrado, erro: {ex.Message}");
            }
        }
    }
}