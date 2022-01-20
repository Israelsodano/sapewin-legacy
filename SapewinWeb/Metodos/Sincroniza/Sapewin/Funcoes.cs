using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Sapewin
{
    public class Funcoes
    {
        public virtual int IDFuncao { get; set; }
        public virtual String Nome { get; set; }

        public virtual IList<FuncoesdeTelas> FuncoesdeTelas { get; set; } 
    }
}