using Auttar.Application.ViewModels;

namespace Auttar.Application.Interfaces
{
    public interface IPinPadServices
    {
        Task<RespostaVendaViewModel> Post(VendaViewModel venda);
    }
}
