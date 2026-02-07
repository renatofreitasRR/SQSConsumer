using ConsumerExample.Domain.Enums;

namespace ConsumerExample.Domain.Entities
{
    public class BloqueioEntity : BaseEntity
    {
        public BloqueioEntity(DateTime dataMovimento, string codigoProtocolo, int ordemBloqueio, decimal valorBloqueio, string motivoBloqueio, string codigoOperacao, int anoOperacao)
        {
            Id = Guid.NewGuid();
            DataMovimento = dataMovimento;
            CodigoProtocolo = codigoProtocolo;
            OrdemBloqueio = ordemBloqueio;
            StatusProcessamento = StatusBloqueio.SOLICITADO;
            ValorBloqueio = valorBloqueio;
            MotivoBloqueio = motivoBloqueio;
            CodigoOperacao = codigoOperacao;
            AnoOperacao = anoOperacao;
        }

        public Guid Id { get; protected set; }
        public DateTime DataMovimento { get; protected set; }
        public string CodigoProtocolo { get; protected set; }
        public int OrdemBloqueio { get; protected set; }
        public StatusBloqueio StatusProcessamento { get; protected set; }
        public decimal ValorBloqueio { get; protected set; }
        public string MotivoBloqueio { get; protected set; }
        public string CodigoOperacao { get; protected set; }
        public int AnoOperacao { get; protected set; }

        public bool ValidarBloqueio()
        {
            this.ValidarAnoOperacao();
            this.ValidarDataMovimento();
            this.ValidarCodigoOperacao();
            this.ValidarValorBloqueio();

            return this.HasErrors;
        }

        private void ValidarDataMovimento()
        {
            if (this.DataMovimento == default)
                this.AddError("Data de movimento inválida.");

            var dataAtual = DateTime.UtcNow;

            if (this.DataMovimento > dataAtual)
                this.AddError("Data de movimento não pode ser futura");
        }

        private void ValidarAnoOperacao()
        {
            if (this.AnoOperacao <= 2025)
                this.AddError("Ano operacao inválido menor ou igual a 2025.");
        }

        private void ValidarCodigoOperacao()
        {
            if (string.IsNullOrEmpty(this.CodigoOperacao))
                this.AddError("Codigo operacao vazio ou nulo.");
        }

        private void ValidarValorBloqueio()
        {
            if (this.ValorBloqueio <= 0)
                this.AddError("Valor bloqueio deve ser maior que zero.");
        }
    }
}
