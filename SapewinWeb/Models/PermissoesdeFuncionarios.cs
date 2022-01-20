using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace SapewinWeb.Models
{
    public class PermissoesdeFuncionarios
    {       
        public virtual int IDUsuario { get; set; }

        public virtual long IDFuncionario { get; set; }

        public virtual int IDEmpresa { get; set; }

        public Funcionarios Funcionario { get; set; }

        public Empresas Empresa { get; set; }
    }
}