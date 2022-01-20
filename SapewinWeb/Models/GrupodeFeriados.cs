using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SapewinWeb.Models
{
    public class GrupodeFeriados
    {
        public virtual int IDFeriado { get; set; }

        public virtual String Descricao { get; set; }

        public virtual IList<FeriadosEspecificos> FeriadosEspecificos { get; set; }

        public String ListaFeriados;
    }
}