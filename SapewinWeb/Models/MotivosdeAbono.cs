using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SapewinWeb.Models
{
    public class MotivosdeAbono
    {       
        public virtual int IDEmpresa { get; set; }

        public virtual String Nome { get; set; }

        public virtual String Abreviacao { get; set; }

        public virtual String EventDia { get; set; }

        public virtual String EventHora { get; set; }

        public virtual String Tipo { get; set; }

        public virtual bool Favorito { get; set; }

        public virtual Empresas Empresa { get; set; }
    }
}