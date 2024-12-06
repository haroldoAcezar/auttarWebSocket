using Auttar.Application.ViewModels;

namespace Auttar.Application.Interfaces
{
    public interface IPinPadServices
    {
        Task<RespostaTransacaoViewModel> Post(VendaViewModel venda);
        Task<RespostaTransacaoViewModel> Cancel(CancelamentoViewModel cancelamento);
        Task<RespostaTransacaoViewModel> GetPix(ConsultaPixViewModel pix);        
    }
}
