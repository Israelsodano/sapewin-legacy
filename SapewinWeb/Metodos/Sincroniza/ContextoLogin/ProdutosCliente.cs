using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Painel.Models
{
    public class ProdutosCliente
    {
        public virtual int IDCliente { get; set; }
        public virtual int IDProduto { get; set; }
        public virtual DateTime Validade { get; set; }
        public virtual bool Demonstracao { get; set; }

        public virtual Produtos Produto { get; set; }
        public virtual Clientes Cliente { get; set; }
    }
}
