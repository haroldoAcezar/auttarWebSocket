namespace Auttar.Domain.Entities
{
    public class ConsultaPix : Consulta
    {        
        public Pix pix { get; set; }
    }

    public class Pix
    {
        public string transactionId { get; set; }
        public string receiverPsp { get; set; }
    } 
}
