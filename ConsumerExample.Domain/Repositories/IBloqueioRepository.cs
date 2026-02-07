using ConsumerExample.Domain.Entities;

namespace ConsumerExample.Domain.Repositories
{
    public interface IBloqueioRepository
    {
        Task<BloqueioEntity> BuscarBloqueioAsync(string codigoProcotolo, int ordemBloqueio, DateTime dataMovimento);
        Task SalvarBloqueio(BloqueioEntity bloqueio);
        Task AtualizarBloqueio(BloqueioEntity bloqueio);
    }
}
