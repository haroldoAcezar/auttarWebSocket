namespace Auttar.Domain.Entities
{
    public class Venda
    {
        public string operacao { get; set; } = string.Empty;
        public long valorTransacao { get; set; } = 0;
        public int numeroParcelas { get; set; } = 1;
        public int numeroTransacao { get; set; } = 1;
    }
}
