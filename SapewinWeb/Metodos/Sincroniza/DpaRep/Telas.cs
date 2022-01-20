using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DpaRep
{ 
    public class Telas
    {
        public virtual int IDTela { get; set; }
        public virtual String Nome { get; set; }

        public virtual IList<FuncoesdeTelas> FuncoesdeTelas { get; set; }

    }
}