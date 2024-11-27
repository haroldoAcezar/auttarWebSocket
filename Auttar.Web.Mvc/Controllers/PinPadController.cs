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

        [HttpPost]
        public async Task<IActionResult> Venda(VendaViewModel venda)
        {
            RespostaVendaViewModel respostaVenda = new RespostaVendaViewModel();

            respostaVenda = await _pinPadServices.Post(venda);

            string jsonString = JsonSerializer.Serialize(respostaVenda);
            
            venda.resposta = jsonString;

            return View(venda);
        }       
    }
}
