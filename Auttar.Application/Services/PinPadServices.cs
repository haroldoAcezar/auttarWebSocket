using Auttar.Application.Interfaces;
using Auttar.Application.ViewModels;
using Auttar.Domain.Entities;
using Microsoft.Extensions.Configuration;
using System.Net.WebSockets;
using System.Text;
using System.Text.Json;

namespace Auttar.Application.Services
{
    public class PinPadServices : IPinPadServices
    {
        private string _wsEndPoint { set; get; }
        private readonly IConfiguration _configuration;

        public PinPadServices(IConfiguration configuration) 
        {
            _configuration = configuration;            
        }

        public async Task<RespostaVendaViewModel> Post(VendaViewModel venda)
        {
            string message = await SendAsync(venda);

            RespostaVendaViewModel respostaVenda = new RespostaVendaViewModel();

            respostaVenda = JsonSerializer.Deserialize<RespostaVendaViewModel>(message);

            if (respostaVenda == null)
                return null;

            if (respostaVenda.retorno == 0)
            {
                if (!await Confirm(respostaVenda.nsuCTF))
                    return null;
            }

            return respostaVenda;
        }

        protected async Task<bool> Confirm(int nsuCTF)
        {
            Confirmacao confirmacao = new Confirmacao() { numeroTransacao = nsuCTF };

            string message = await SendAsync(confirmacao);

            return true;
        }

        protected async Task<string> SendAsync(object messagePost)
        {
            _wsEndPoint = _configuration.GetSection("Auttar").GetSection("wsEndPoint").Value;  //"ws://localhost:2500";

            using var ws = new ClientWebSocket();
            await ws.ConnectAsync(new Uri(_wsEndPoint), CancellationToken.None);

            string jsonString = JsonSerializer.Serialize(messagePost);

            var bytes = Encoding.UTF8.GetBytes(jsonString);
            var arraySegment = new ArraySegment<byte>(bytes, 0, bytes.Length);

            await ws.SendAsync(arraySegment,
                                WebSocketMessageType.Text,
                                        true,
                                CancellationToken.None);

            string messageReceive = "";

            var receiveTask = Task.Run(async () =>
            {
                var buffer = new byte[1024 * 4];
                var result = await ws.ReceiveAsync(new ArraySegment<byte>(buffer), CancellationToken.None);
                messageReceive = Encoding.UTF8.GetString(buffer, 0, result.Count);                
            });

            await receiveTask;

            return messageReceive;
        }        
    }
}
