using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Painel.Models
{
    public class LoginFabRevenda
    {
        public virtual int IDLoginfabrevenda { get; set; }
        public virtual String Login { get; set; }
        public virtual String Senha { get; set; }
        public virtual bool CadCliente { get; set; }
        public virtual bool Master { get; set; }
        public virtual bool CadRevenda { get; set; }
        public virtual int IDRevenda { get; set; }
        public virtual String Email { get; set; }
        public virtual String Telefone { get; set; }
        public virtual String Nome { get; set; }
        public virtual bool ForcaAlteracao { get; set; }
        public virtual bool Ativo { get; set; }

        public virtual Revendas Revenda { get; set; }
        public virtual List<LogSistema> LogSistema { get; set; }
    }
}
