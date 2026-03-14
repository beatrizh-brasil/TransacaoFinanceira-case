namespace TransacaoFinanceira.Models
{
    public class ContaSaldo
    {
        public long Conta { get; set; }
        public decimal Saldo { get; set; }

        public ContaSaldo(long conta, decimal saldo)
        {
            Conta = conta;
            Saldo = saldo;
        }
    }
}