namespace webAPP_ASPNET.Models
{
    public class PedidoViewModel
    {
        // Cliente
        public string NomeCliente { get; set; }
        public string EmailCliente { get; set; }

        // Endereço
        public string Cep { get; set; }
        public string Rua { get; set; }
        public string Numero { get; set; }
        public string Complemento { get; set; }

        // Pagamento
        public string FormaPagamento { get; set; }
    }
}