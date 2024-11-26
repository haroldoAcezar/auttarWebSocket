using Auttar.Application.Interfaces;
using Auttar.Application.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.WebSockets;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Auttar.Application.Services
{
    public class PinPadServices : IPinPadServices
    {
        public PinPadServices() { }

        public async Task<RespostaVendaViewModel> Post(VendaViewModel venda)
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

            return respostaVenda;
        }
    }
}
