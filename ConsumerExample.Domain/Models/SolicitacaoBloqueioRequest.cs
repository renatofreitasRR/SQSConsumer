namespace ConsumerExample.Domain.Models
{
    public class SolicitacaoBloqueioRequest : Request
    {
        public DateTime DataMovimento { get; set; }
        public string CodigoProtocolo { get; set; }
        public int OrdemBloqueio { get; set; }
        public decimal ValorBloqueio { get; set; }
        public string MotivoBloqueio { get; set; }
        public string CodigoOperacao { get; set; }
        public int AnoOperacao { get; set; }
    }
}
