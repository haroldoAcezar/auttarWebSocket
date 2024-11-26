using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Auttar.Application.ViewModels
{
    public class VendaViewModel
    {
        public string operacao { get; set; } = string.Empty;
        public long valorTransacao { get; set; } = 0;
        public int numeroParcelas { get; set; } = 1;
        public int numeroTransacao { get; set; } = 1;
    }
}
