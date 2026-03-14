using System;
using TransacaoFinanceira.Repositories;

namespace TransacaoFinanceira.Services
{
    public class TransacaoService
    {
        private readonly IContaRepository _repository;
        private static readonly object _lock = new object();

        public TransacaoService(IContaRepository repository)
        {
            _repository = repository;
        }

        public void Transferir(int correlationId, long origemId, long destinoId, decimal valor)
        {
            lock (_lock)
            {
                var origem = _repository.ObterPorId(origemId);
                var destino = _repository.ObterPorId(destinoId);

                if (origem != null && destino != null && origem.Saldo >= valor)
                {
                    origem.Saldo -= valor;
                    destino.Saldo += valor;
                    _repository.Atualizar(origem);
                    _repository.Atualizar(destino);
                    Console.WriteLine($"Transacao {correlationId} Sucesso! Origem: {origem.Saldo} | Destino: {destino.Saldo}");
                }
                else
                {
                    Console.WriteLine($"Transacao {correlationId} Cancelada (Saldo Insuficiente ou Conta Inexistente)");
                }
            }
        }
    }
}