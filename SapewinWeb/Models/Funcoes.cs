using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace SapewinWeb.Models
{
    public class Funcoes
    {               
        public virtual int IDFuncao { get; set; }
        
        public virtual String Nome { get; set; }

        public virtual IList<FuncoesdeTelas> FuncoesdeTelas { get; set; }
    }
}