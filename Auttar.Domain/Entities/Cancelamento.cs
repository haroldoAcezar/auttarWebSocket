namespace Auttar.Domain.Entities
{
    public class Cancelamento
    {
        public string operacao { get; set; } = "128";
        public string valorTransacao { get; set; }
        public string dataTransacao { get; set; } //ddmmyy
        public string nsuCTF { get; set; }
    }
}
