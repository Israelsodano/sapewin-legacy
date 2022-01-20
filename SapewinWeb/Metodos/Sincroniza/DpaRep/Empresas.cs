using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DpaRep
{
    public class Empresas
    {
        public virtual int IDEmpresa { get; set; }
        public virtual String Nome { get; set; }
        public virtual int IDFolha { get; set; }
        public virtual String Endereco { get; set; }
        public virtual String Cep { get; set; }
        public virtual String Cidade { get; set; }
        public virtual String Bairro { get; set; }
        public virtual String UF { get; set; }
        public virtual String Documento { get; set; }
        public virtual String IE { get; set; }       

        public virtual IList<PermissoesdeTelas> PermissoesdeTelas { get; set; }
    }
}