using Auttar.Application.Interfaces;
using Auttar.Application.ViewModels;
using Auttar.Domain.Entities;
using Microsoft.Extensions.Configuration;
using System.Net.WebSockets;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;

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

        public async Task<RespostaTransacaoViewModel> GetPix(ConsultaPixViewModel pix)
        {
            string message = await SendAsync(pix);

            RespostaTransacaoViewModel respostaVenda = new RespostaTransacaoViewModel();

            respostaVenda = JsonSerializer.Deserialize<RespostaTransacaoViewModel>(message);

            if (respostaVenda == null)
                return null;

            return respostaVenda;
        }

        public async Task<RespostaTransacaoViewModel> Post(VendaViewModel venda)
        {
            string message = await SendAsync(venda);

            RespostaTransacaoViewModel respostaVenda = new RespostaTransacaoViewModel();

            respostaVenda = JsonSerializer.Deserialize<RespostaTransacaoViewModel>(message);

            if (respostaVenda == null)
                return null;

            if (respostaVenda.retorno == 0)
            {
                if (!await Confirm(respostaVenda.nsuCTF))
                    return null;
            }

            return respostaVenda;
        }

        public async Task<RespostaTransacaoViewModel> Cancel(CancelamentoViewModel cancelamento)
        {
            string message = await SendAsync(cancelamento);

            RespostaTransacaoViewModel respostaCancelamento = new RespostaTransacaoViewModel();

            respostaCancelamento = JsonSerializer.Deserialize<RespostaTransacaoViewModel>(message);

            if (respostaCancelamento == null)
                return null;

            if (respostaCancelamento.retorno == 0)
            {
                if (!await Confirm(respostaCancelamento.nsuCTF))
                    return null;
            }

            return respostaCancelamento;
        }

        protected async Task<bool> Confirm(int nsuCTF)
        {
            Confirmacao confirmacao = new Confirmacao() { numeroTransacao = nsuCTF };

            string message = await SendAsync(confirmacao);

            return true;
        }

        protected async Task<bool> Desfazer(int nsuCTF)
        {
            Desfazer desfazer = new Desfazer() { numeroTransacao = nsuCTF };

            string message = await SendAsync(desfazer);

            return true;
        }

        protected async Task<string> SendAsync(object messagePost)
        {
            _wsEndPoint = _configuration.GetSection("Auttar").GetSection("wsEndPoint").Value;  //"ws://localhost:2500";

            using var ws = new ClientWebSocket();
            await ws.ConnectAsync(new Uri(_wsEndPoint), CancellationToken.None);

            JsonSerializerOptions options = new()
            {
                DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull
            };

            string jsonString = JsonSerializer.Serialize(messagePost, options);

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
