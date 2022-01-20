using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Painel
{
    public class FuncoesDeTelas
    {
        public virtual String IDFuncaoTela { get; set; }
        public virtual int IDFuncao { get; set; }
        public virtual int IDTela { get; set; }
        public virtual int IDProduto { get; set; }

        public virtual Funcoes Funcao { get; set; }
        public virtual Telas Tela { get; set; }
        
    }
}
