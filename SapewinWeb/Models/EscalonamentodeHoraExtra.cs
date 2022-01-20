using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SapewinWeb.Models
{
    public class EscalonamentodeHoraExtra
    {
        public virtual int IDParametro { get; set; }

        public virtual int IDEmpresa { get; set; }

        public virtual String Horas { get; set; }

        public virtual int Porcentagem { get; set; }

        public virtual int? Adicional { get; set; }

        public virtual tipo Tipo { get; set; }

        public enum tipo
        {
            Uteis = 1, Sabados = 2, Domingos = 3, Feriados = 4, Folgas = 5
        }

        public Parametros Parametro { get; set; }
    }
}