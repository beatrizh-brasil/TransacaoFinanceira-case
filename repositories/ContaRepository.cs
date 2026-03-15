using System;
using System.IO;
using System.Text.Json;
using TransacaoFinanceira.Models;
using System.Collections.Generic;

namespace TransacaoFinanceira.Repositories {
    public class ContaRepository : IContaRepository
    {
        private List<ContaSaldo> _tabelaSaldos;
        private readonly string _filePath = "contas.json";

        public ContaRepository()
        {

            var options = new JsonSerializerOptions 
            { 
                PropertyNameCaseInsensitive = true 
            };

            try {
                string jsonString = File.ReadAllText(_filePath);
                
                // Aplicamos as options aqui
                _tabelaSaldos = JsonSerializer.Deserialize<List<ContaSaldo>>(jsonString, options) 
                                ?? new List<ContaSaldo>();
            }
            catch (Exception ex) {
                Console.WriteLine($"Erro ao carregar contas: {ex.Message}");
                _tabelaSaldos = new List<ContaSaldo>();
            }
        }

        public ContaSaldo ObterPorId(long id) => _tabelaSaldos.Find(x => x.Conta == id);

        public void Atualizar(ContaSaldo conta)
        {
            var index = _tabelaSaldos.FindIndex(x => x.Conta == conta.Conta);
            if (index != -1)
            {
                _tabelaSaldos[index] = conta;
                //SalvarAlteracoes(); // para alterar o saldo no json
            }
        }

        private void SalvarAlteracoes()
        {
            // Indentar o JSON para ficar bonitinho no arquivo
            var options = new JsonSerializerOptions { WriteIndented = true };
            string jsonString = JsonSerializer.Serialize(_tabelaSaldos, options);
            File.WriteAllText(_filePath, jsonString);
        }
    }
}