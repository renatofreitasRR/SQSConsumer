using ConsumerExample.Domain.Entities;
using ConsumerExample.Domain.Repositories;

namespace ConsumerExample.Infrastructure.Repositories
{
    public class FakeRepository : IBloqueioRepository
    {
        public async Task AtualizarBloqueio(BloqueioEntity bloqueio)
        {
            Task.Delay(1000);
        }

        public async Task<BloqueioEntity> BuscarBloqueioAsync(string codigoProcotolo, int ordemBloqueio, DateTime dataMovimento)
        {
            return new BloqueioEntity(dataMovimento, codigoProcotolo, ordemBloqueio, 100, "", "", 2025);
        }

        public async Task SalvarBloqueio(BloqueioEntity bloqueio)
        {
            Task.Delay(1000);
        }
    }
}
