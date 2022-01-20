using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Login.Models
{
    [Table("pnl_produtoscliente")]
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
