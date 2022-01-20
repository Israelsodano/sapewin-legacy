using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SapewinWeb.Models
{
    public class FeriadosGerais
    {
        public virtual int IDFeriado { get; set; }

        public virtual String Descricao { get; set; }

        public virtual int Dia { get; set; }

        public virtual int Mes { get; set; }

        public virtual int? Ano { get; set; }       

    }
}