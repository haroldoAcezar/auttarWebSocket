using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Auttar.Application.ViewModels
{
    public class RespostaVendaViewModel
    {
        public int operacao { get; set; } = 0;
        public int retorno { get; set; } = 0;
        public string codigoErro { get; set; } = string.Empty;
        public List<Display> display { get; set; }
        public string codigoRespAutorizadora { get; set; } = string.Empty;
        public string codigoAprovacao { get; set; } = string.Empty;
        public string valorTransacao { get; set; } = string.Empty;
        public int nsuCTF { get; set; } = 0;
        public int nsuAutorizadora { get; set; } = 0;
        public string bandeira { get; set; } = string.Empty;
        public string redeAdquirente { get; set; } = string.Empty;
        public string cartao { get; set; } = string.Empty;
        public string nomeTransacao { get; set; } = string.Empty;
        public List<Cupom> cupomEstabelecimento { get; set; }
        public List<Cupom> cupomCliente { get; set; }
        public List<Cupom> cupomReduzido { get; set; }
    }

    public class Display
    {
        public string mensagem { get; set; } = string.Empty;
    }

    public class Cupom
    {
        public string linha { get; set; } = string.Empty;
    }
}
