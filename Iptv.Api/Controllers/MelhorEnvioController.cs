using Iptv.Core.Requests.MelhorEnvio;
using Iptv.Core.Responses;
using Iptv.Core.Services;
using Microsoft.AspNetCore.Mvc;

namespace Iptv.Api.Controllers;

[ApiController]
[Route("v1/melhor-envio")]
public class MelhorEnvioController(IMelhorEnvioService service) : ControllerBase
{
    [HttpPost("frete")]
    public async Task<ActionResult> CalcularFreteAsync(CalcularFreteRequest request)
    {
        try
        {
            var response = await service.CalcularFreteAsync(request);
            
            return response.IsSuccess ? Ok(response) : BadRequest(response);
        }
        catch (ArgumentNullException ex)
        {
            Console.WriteLine($"Erro de argumento nulo: {ex}");
            return StatusCode(500, new BaseResponse<object>(
                null,
                500,
                "Erro interno: dados necessários não foram fornecidos."
            ));
        }
        catch (InvalidOperationException ex)
        {
            Console.WriteLine($"Erro de operação inválida: {ex}");
            return StatusCode(500, new BaseResponse<object>(
                null,
                500,
                "Erro interno: operação não pode ser concluída."
            ));
        }
        catch (HttpRequestException ex)
        {
            Console.WriteLine($"Erro de comunicação: {ex}");
            return StatusCode(503, new BaseResponse<object>(
                null,
                503,
                "Serviço temporariamente indisponível. Tente novamente mais tarde."
            ));
        }
        catch (UnauthorizedAccessException ex)
        {
            Console.WriteLine($"Erro de autorização: {ex}");
            return Unauthorized(new BaseResponse<object>(
                null,
                401,
                "Acesso negado. Você não tem permissão para acessar este recurso."
            ));
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Erro inesperado: {ex}");
            return StatusCode(500, new BaseResponse<object>(
                null,
                500,
                "Ocorreu um erro inesperado. Por favor, contate o suporte técnico."
            ));
        }
    }
}