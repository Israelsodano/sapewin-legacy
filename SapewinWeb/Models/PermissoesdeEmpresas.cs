using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace SapewinWeb.Models
{
    public class PermissoesdeEmpresas
    {       
        public virtual int IDUsuario { get; set; }

        public virtual int IDEmpresa { get; set; }

        public virtual Empresas Empresa { get; set; }
    }
}