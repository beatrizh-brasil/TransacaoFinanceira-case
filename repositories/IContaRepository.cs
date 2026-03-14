using TransacaoFinanceira.Models;

namespace TransacaoFinanceira.Repositories
{
    public interface IContaRepository
    {
        Models.ContaSaldo ObterPorId(long id);
        void Atualizar(Models.ContaSaldo conta);
    }
}