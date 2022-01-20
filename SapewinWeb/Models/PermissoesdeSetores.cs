using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace SapewinWeb.Models
{
    public class PermissoesdeSetores
    {       
        public virtual int IDUsuario { get; set; }

        public virtual long IDSetor { get; set; }

        public virtual int IDEmpresa { get; set; }

        public virtual Setores Setor { get; set; }

        public virtual Empresas Empresa { get; set; }
    }
}