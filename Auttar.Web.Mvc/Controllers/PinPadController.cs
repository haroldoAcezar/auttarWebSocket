using Microsoft.AspNetCore.Mvc;
using System.Text.Json;
using Auttar.Application.ViewModels;
using Auttar.Application.Interfaces;

namespace Auttar.Web.Mvc.Controllers
{
    public class PinPadController : Controller
    {
        private readonly IPinPadServices _pinPadServices;

        public PinPadController(IPinPadServices pinPadServices) 
        {
            _pinPadServices = pinPadServices;
        }

        public IActionResult Venda()
        {
            VendaViewModel venda = new VendaViewModel();

            return View(venda);
        }

        public IActionResult CancelarVenda()
        {
            CancelamentoViewModel cancelamento = new CancelamentoViewModel();

            return View(cancelamento);
        }

        public IActionResult ConsultaVendaPix()
        {
            ConsultaPixViewModel pix = new ConsultaPixViewModel();

            return View(pix);
        }

        [HttpPost]
        public async Task<IActionResult> Venda(VendaViewModel venda)
        {
            RespostaTransacaoViewModel respostaVenda = new RespostaTransacaoViewModel();

            respostaVenda = await _pinPadServices.Post(venda);

            string jsonString = JsonSerializer.Serialize(respostaVenda);
            
            venda.resposta = jsonString;

            return View(venda);
        }

        [HttpPost]
        public async Task<IActionResult> CancelarVenda(CancelamentoViewModel cancelar)
        {
            RespostaTransacaoViewModel respostaCancelamento = new RespostaTransacaoViewModel();

            respostaCancelamento = await _pinPadServices.Cancel(cancelar);

            string jsonString = JsonSerializer.Serialize(respostaCancelamento);

            cancelar.resposta = jsonString;

            return View(cancelar);
        }

        [HttpPost]
        public async Task<IActionResult> ConsultaVendaPix(ConsultaPixViewModel pix)
        {
            RespostaTransacaoViewModel respostaVenda = new RespostaTransacaoViewModel();

            respostaVenda = await _pinPadServices.GetPix(pix);

            string jsonString = JsonSerializer.Serialize(respostaVenda);

            pix.resposta = jsonString;

            return View(pix);
        }       
    }
}
