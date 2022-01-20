using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Login.Models
{
    [Table("pnl_produtosrevenda")]
    public class ProdutosRevenda
    {
        public virtual int IDProduto { get; set; }
        public virtual int IDRevenda { get; set; }

        public virtual Produtos Produto { get; set; }
        public virtual Revendas Revenda { get; set; }
    }
}
