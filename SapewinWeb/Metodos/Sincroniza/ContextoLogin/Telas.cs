using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Painel.Models
{
    public class Telas
    {
        public virtual int IDTela { get; set; }
        public virtual String Nome { get; set; }
        public virtual int IDProduto { get; set; }

        public virtual List<FuncoesDeTelas> FuncoesDeTelas { get; set; }
        public virtual Produtos Produto { get; set; }

    }
}
