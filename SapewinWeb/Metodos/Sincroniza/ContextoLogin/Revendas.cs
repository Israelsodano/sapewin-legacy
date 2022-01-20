using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Painel.Models
{
    public class Revendas
    {
        public virtual int IDRevenda { get; set; }
        public virtual String Nome { get; set; }
        public virtual String Documento { get; set; }
        public virtual bool Ativo { get; set; }
        public virtual DateTime DataInclusao { get; set; }
        public virtual int? ID_Fabrica { get; set; }
        public virtual bool CNPJ { get; set; }

        public virtual List<Clientes> Cliente { get; set; }
        public virtual List<ServidoresRevenda> ServidoresRevenda { get; set; }
        public virtual List<ProdutosRevenda> ProdutosRevenda { get; set; }
        public virtual List<LoginFabRevenda> LoginFabRevenda { get; set; }

        public virtual Revendas Fabricante { get; set; }

        public virtual IList<Revendas> Revendedoras { get; set; }
    }
}
