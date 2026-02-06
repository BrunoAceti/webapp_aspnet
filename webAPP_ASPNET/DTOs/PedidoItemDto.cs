namespace webAPP_ASPNET.DTOs
{
    public class PedidoItemDto
    {
        public int ProdutoId { get; set; }
        public string NomeProduto { get; set; }
        public decimal PrecoUnitario { get; set; }
        public int Quantidade { get; set; }
    }
}