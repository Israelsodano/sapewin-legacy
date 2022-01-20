using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SapewinWeb.Models
{
    public class Escalas
    {        
        public virtual int IDEscala { get; set; }

        public virtual int IDEmpresa { get; set; }        

        public virtual String Descricao { get; set; }

        public virtual tipo Tipo { get; set; }

        public virtual DateTime DataInicio { get; set; }

        public virtual bool Liv_AdcSab { get; set; }

        public virtual bool Liv_AdcDom { get; set; }

        public virtual bool Liv_Fer { get; set; }

        public virtual bool Liv_Fol { get; set; }

        public virtual bool Liv_Virarda { get; set; }

        public virtual viradaSemana ViradaSemana { get; set; }

        public virtual Empresas Empresa { get; set; }

        public IList<EscalasHorarios> EscalasHorarios { get; set; }

        public enum tipo
        {            
            Semanal=1, Revezamento=2, CargaSemanal=3, Livre=4
        };

        public enum viradaSemana
        {
            Segunda=1, Terca=2, Quarta=3, Quinta=4, Sexta=5, Sabado=6, Domingo=7
        };

        public virtual IList<Funcionarios> Funcionarios { get; set; }
    }
}