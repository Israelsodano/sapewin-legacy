using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Sapewin
{
    public class FuncoesdeTelas
    {
        public virtual String IDFuncaoTela { get; set; }
        public virtual int IDFuncao { get; set; }
        public virtual int IDTela { get; set; }

       
        public virtual Funcoes Funcao { get; set; }
        public virtual Telas Tela { get; set; }
    }
}