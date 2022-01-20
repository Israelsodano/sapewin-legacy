using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Painel.Models
{
    public class ProdutosRevenda
    {
        public virtual int IDProduto { get; set; }
        public virtual int IDRevenda { get; set; }

        public virtual Produtos Produto { get; set; }
        public virtual Revendas Revenda { get; set; }
    }
}
