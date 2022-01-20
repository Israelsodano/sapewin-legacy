﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SapewinWeb.Models
{
    public class IntervalosAuxiliares
    {
        public virtual int IDIntervalo { get; set; }

        public virtual int IDEmpresa { get; set; }

        public virtual int IDHorario { get; set; }

        public virtual String Inicio { get; set; }

        public virtual String Fim { get; set; }

        public virtual String Carga { get; set; }

        public virtual tipo Tipo { get; set; }

        public virtual Empresas Empresa { get; set; }

        public virtual Horarios Horarios { get; set; }

        public virtual int Order { get; set; }

        public enum tipo
        {
            Fixo=1, Carga=2
        }

    }
}