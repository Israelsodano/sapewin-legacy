using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace SapewinWeb.Models
{
    public class PermissoesdeDepartamentos
    {
      
        public virtual int IDUsuario { get; set; }

        public virtual long IDDepartamento { get; set; }

        public virtual int IDEmpresa { get; set; }

        public virtual Departamentos Departamento { get; set; }

        public virtual Empresas Empresa { get; set; }
    }
}