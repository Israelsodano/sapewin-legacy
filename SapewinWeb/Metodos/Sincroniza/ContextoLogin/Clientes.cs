using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Painel.Models
{
    public class Clientes
    {
        public virtual int IDCliente { get; set; }
        public virtual int IDSistema { get; set; }
        public virtual String Razao { get; set; }
        public virtual int IDRevenda { get; set; }
        public virtual bool Ativo { get; set; }
        public virtual String IE { get; set; }
        public virtual String Logo { get; set; }
        public virtual String Documento { get; set; }
        public virtual bool Cnpj { get; set; }
        public virtual int? IDMatriz { get; set; }
        public virtual int IDServidor { get; set; }

        public virtual Revendas Revenda { get; set; }
        public virtual Servidores Servidor { get; set; }
        public virtual List<LoginSistema> LoginSistema { get; set; }
        public virtual IList<ProdutosCliente> ProdutosCliente { get; set; }

        public virtual IList<Clientes> Filiais { get; set; }

        public virtual Clientes Matriz { get; set; }

    }
}
