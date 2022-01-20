using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SapewinWeb.Models
{
    public class Afastamentos
    {
        public virtual int IDAfastamento { get; set; }

        public virtual long IDFuncionario { get; set; }

        public virtual int IDEmpresa { get; set; }

        public virtual String Abreviacao { get; set; }

        public virtual DateTime DataInicial { get; set; }

        public virtual DateTime? DataFinal { get; set; }

        public virtual Funcionarios Funcionario { get; set; }
    }
}