using Microsoft.AspNetCore.Mvc;
using System.Text;
using System.Text.Json;
using System.Net.WebSockets;
using Microsoft.Extensions.Hosting;
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
            return View();
        }

        [HttpPost]
        public async Task<JsonResult> EnviarVenda(VendaViewModel venda)
        {
            RespostaVendaViewModel respostaVenda = new RespostaVendaViewModel();

            respostaVenda = await _pinPadServices.Post(venda);

            return Json(respostaVenda);
        }

        [HttpPost]
        public async Task<JsonResult> EnviarVenda1(VendaViewModel venda)
        {
            using var ws = new ClientWebSocket();
            await ws.ConnectAsync(new Uri("ws://localhost:2500"), CancellationToken.None);

            string jsonString = JsonSerializer.Serialize(venda);

            var bytes = Encoding.UTF8.GetBytes(jsonString);
            var arraySegment = new ArraySegment<byte>(bytes, 0, bytes.Length);

            await ws.SendAsync(arraySegment,
                                WebSocketMessageType.Text,
                                true,
                                CancellationToken.None);


            RespostaVendaViewModel respostaVenda = new RespostaVendaViewModel();

            var receiveTask = Task.Run(async () =>
            {
                var buffer = new byte[1024 * 4];
                var result = await ws.ReceiveAsync(new ArraySegment<byte>(buffer), CancellationToken.None);
                var message = Encoding.UTF8.GetString(buffer, 0, result.Count);

                respostaVenda = JsonSerializer.Deserialize<RespostaVendaViewModel>(message);
            });

            await receiveTask;

            return Json(respostaVenda);            
        }
    }
}
