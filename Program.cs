using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;
using TransacaoFinanceira.Repositories;
using TransacaoFinanceira.Services;

namespace TransacaoFinanceira
{
    // A classe de mapeamento deve ficar aqui, dentro do namespace, mas fora da classe Program
    public class TransacaoInput 
    {
        public int correlation_id { get; set; }
        public string datetime { get; set; }
        public long conta_origem { get; set; }
        public long conta_destino { get; set; }
        public decimal valor { get; set; }
    }

    class Program
    {
        static void Main(string[] args)
        {
            try 
            {
                // 1. Lendo as transações do JSON
                string jsonTransacoes = File.ReadAllText("transacoes.json");
                
                // Usamos o CaseInsensitive para garantir que ele leia mesmo se o JSON variar
                var options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
                var transacoes = JsonSerializer.Deserialize<List<TransacaoInput>>(jsonTransacoes, options);

                // 2. Instanciando as dependências
                IContaRepository repository = new ContaRepository();
                var service = new TransacaoService(repository);

                // 3. Sua lógica de ordenação (O ponto crucial de robustez)
                var ordenadas = transacoes.OrderBy(x => DateTime.Parse(x.datetime)).ToList();

                Console.WriteLine("--- Iniciando Processamento via JSON ---\n");

                foreach (var t in ordenadas)
                {
                    service.Transferir(t.correlation_id, t.conta_origem, t.conta_destino, t.valor);
                }
                
                Console.WriteLine("\n--- Processamento Finalizado ---");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Erro ao processar arquivos: {ex.Message}");
            }
        }
    }
}