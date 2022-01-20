using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace SapewinWeb.Models
{
    public class Departamentos
    {
       
        public virtual long IDDepartamento { get; set; }
                
        public virtual int IDEmpresa { get; set; }
                
        public virtual String Nome { get; set; }

        public virtual IList<Funcionarios> Funcionarios { get; set; }
                
        public virtual Empresas Empresa { get; set; }

        public virtual IList<PermissoesdeDepartamentos> PermissoesdeDepartamentos { get; set; }
    }
}