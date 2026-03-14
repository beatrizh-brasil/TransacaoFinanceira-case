using System;
using TransacaoFinanceira.Repositories;

namespace TransacaoFinanceira.Services
{
    public class TransacaoService
    {
        private readonly IContaRepository _repository;

        public TransacaoService(IContaRepository repository)
        {
            _repository = repository;
        }

        public void Transferir(int correlationId, long origemId, long destinoId, decimal valor)
        {
                var origem = _repository.ObterPorId(origemId);
                var destino = _repository.ObterPorId(destinoId);

                if (origem == null || destino == null)
            {
                Console.WriteLine($"Transacao {correlationId} Cancelada (Conta Inexistente)");
                return;
            }

            if (origem.Saldo < valor)
            {
                Console.WriteLine($"Transacao {correlationId} Cancelada (Saldo Insuficiente)");
                return;
            }

            origem.Saldo -= valor;
            destino.Saldo += valor;

            _repository.Atualizar(origem);
            _repository.Atualizar(destino);

            Console.WriteLine($"Transacao {correlationId} Sucesso! Origem: {origem.Saldo} | Destino: {destino.Saldo}");
            
        }
    }
}