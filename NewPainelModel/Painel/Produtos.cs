using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Login.Models
{
    [Table("pnl_produtos")]
    public class Produtos
    {
        public virtual int IDProduto { get; set; }
        public virtual String Nome { get; set; }
        public virtual String Imagem { get; set; }
        public virtual String Descricao { get; set; }
        public virtual String Url { get; set; }
        public virtual DateTime? Validade { get; set; }
        public virtual bool Demontracao { get; set; }

        public virtual List<Funcoes> Funcoes { get; set; }
        public virtual List<FuncoesDeTelas> FuncoesDeTelas { get; set; }
        public virtual List<Telas> Telas { get; set; }

        public virtual List<ProdutosCliente> ProdutosCliente { get; set; }
        public virtual List<ProdutosRevenda> ProdutosRevenda { get; set; }

    }
}
