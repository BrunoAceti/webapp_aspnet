using System.Collections.Generic;

namespace webAPP_ASPNET.DTOs
{
    public class PedidoCreateDto
    {
        public string NomeCliente { get; set; }
        public string EmailCliente { get; set; }

        public string Cep { get; set; }
        public string Rua { get; set; }
        public string Numero { get; set; }
        public string Complemento { get; set; }

        public string FormaPagamento { get; set; }
        public decimal Total { get; set; }

        public List<PedidoItemDto> Itens { get; set; }
    }
}