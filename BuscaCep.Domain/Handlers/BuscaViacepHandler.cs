using BuscaCep.Domain.Exceptions;
using BuscaCep.Domain.Interfaces;
using BuscaCep.Domain.Models;
using MediatR;
using Microsoft.Extensions.Caching.Distributed;
using System.Text.Json;

namespace BuscaCep.Domain.Handlers
{
    public class BuscaViacepHandler : IRequestHandler<CepEntrada, CepSaida>
    {
        private readonly IViaCepRefit _viaCep;
        private readonly IDistributedCache _distributedCache;
        private const string CEP_KEY = "ViaCep";

        public BuscaViacepHandler(IViaCepRefit viaCep, IDistributedCache distributedCache)
        {
            _viaCep = viaCep;
            _distributedCache = distributedCache;
        }

        public async Task<CepSaida> Handle(CepEntrada request, CancellationToken cancellationToken)
        {
            request.Validate();

            /*Para rodar o redis no docker:
             * docker container run -d -p 6379:6379 --name redis redis
             */
            var cache = await _distributedCache.GetStringAsync($"{CEP_KEY}:{request.Cep}");
            if (!String.IsNullOrWhiteSpace(cache))
            {
                var responseCache = JsonSerializer.Deserialize<CepSaida>(cache, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
                return responseCache!;
            }

            var retorno = await _viaCep.GetCep(request.Cep);

            if (retorno.cep == null)
            {
                throw new CepNaoEncontradoException($"Cep {request.Cep} não foi encontrado.");
            }

            var response = new CepSaida(request.Cep, retorno.logradouro, retorno.bairro, retorno.localidade, retorno.uf);

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
    }
}
