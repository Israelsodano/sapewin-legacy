using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Login.Models
{
    [Table("pnl_funcoesdetelas")]
    public class FuncoesDeTelas
    {
        public virtual String IDFuncaoTela { get; set; }
        public virtual int IDFuncao { get; set; }
        public virtual int IDTela { get; set; }
        public virtual int IDProduto { get; set; }

        public virtual Funcoes Funcao { get; set; }
        public virtual Telas Tela { get; set; }
        public virtual Produtos Produto { get; set; }
    }
}
