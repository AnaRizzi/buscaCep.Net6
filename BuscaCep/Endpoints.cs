using BuscaCep.Domain.Exceptions;
using BuscaCep.Domain.Models;
using MediatR;

namespace BuscaCep
{
    public static class Endpoints
    {
        public static void MapAppEndpoints(this IEndpointRouteBuilder app)
        {
            app.MapGet("teste", Teste)
                .WithName("Teste")
                .WithTags("Teste")
                .Produces(200, typeof(string));

            app.MapGet("cep/viacep/{cep}", CepViacep)
                .WithName("ViaCep")
                .WithTags("Cep")
                .Produces(200, typeof(CepSaida))
                .Produces(404);

            app.MapGet("cep/correios/{cep}", CepCorreios)
                .WithName("Correios")
                .WithTags("Cep")
                .Produces(200, typeof(CepSaidaCorreios))
                .Produces(404);
        }

        private static string Teste()
        {
            return "funciona!";
        }

        private static async Task<IResult> CepViacep(IMediator m, string cep)
        {
            try
            {
                var result = await m.Send(new CepEntrada(cep));
                return Results.Ok(result);
            }
            catch (CepNaoEncontradoException ex)
            {
                return Results.NotFound(ex.Message);
            }
            catch (Exception ex)
            {
                return Results.BadRequest(ex.Message);
            }
        }

        private static async Task<IResult> CepCorreios(IMediator m, string cep)
        {
            try
            {
                var result = await m.Send(new CepEntradaCorreios(cep));
                return Results.Ok(result);
            }
            catch (CepNaoEncontradoException ex)
            {
                return Results.NotFound(ex.Message);
            }
            catch (Exception ex)
            {
                return Results.BadRequest(ex.Message);
            }
        }
    }
}
