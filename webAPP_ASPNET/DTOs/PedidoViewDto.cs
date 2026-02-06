namespace webAPP_ASPNET.DTOs
{
    public class PedidoViewDto
    {
        public int PedidoId { get; set; }          // Número do pedido
        public DateTime DataPedido { get; set; }   // Data do pedido
        public string Status { get; set; }         // Status do pedido (ex: Aguardando Pagamento)
        public decimal Total { get; set; }         // Total do pedido
    }
}