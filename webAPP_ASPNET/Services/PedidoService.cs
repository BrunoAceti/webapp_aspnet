using System.Net.Http;
using System.Net.Http.Json;
using webAPP_ASPNET.DTOs;

namespace webAPP_ASPNET.Services
{
    public class PedidoService
    {
        private readonly HttpClient _httpClient;

        public PedidoService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<int> CriarPedido(PedidoCreateDto dto)
        {
            var response = await _httpClient.PostAsJsonAsync("/api/pedido", dto);

            response.EnsureSuccessStatusCode();

            // Ler JSON no formato { "pedidoId": 123 }
            var result = await response.Content.ReadFromJsonAsync<PedidoResponseDto>();
            return result.PedidoId;
        }
    }
}