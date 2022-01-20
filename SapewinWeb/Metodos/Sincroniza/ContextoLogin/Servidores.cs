using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Painel.Models
{
    public class Servidores
    {
        public virtual int IDServidor { get; set; }
        public virtual String CamihoBancoAtual { get; set; }
        public virtual String Usuario { get; set; }
        public virtual String Senha { get; set; }
        public virtual String Descricao { get; set; }
        public virtual String Tipo { get; set; }
        public virtual int Porta { get; set; }
        public virtual String NomeBanco { get; set; }
        public virtual bool Ativo { get; set; }

        public virtual List<Clientes> Clientes { get; set; }
        public virtual List<ServidoresRevenda> ServidoresRevenda { get; set; }
    }
}
