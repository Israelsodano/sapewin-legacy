using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Login.Models
{
    [Table("pnl_loginsistema")]
    public class LoginSistema
    {
        public virtual int IDLoginsistema { get; set; }
        public virtual String Login { get; set; }
        public virtual String Nome { get; set; }
        public virtual String Email { get; set; }
        public virtual String Senha { get; set; }
        public virtual bool PerfiMaster { get; set; }
        public virtual int IDCliente { get; set; } 
        public virtual bool Ativo { get; set; }
        public virtual bool ForcaAlteracao { get; set; }
        public virtual bool Personalizado { get; set; }
        public virtual String Telefone { get; set; }
        public virtual int Tipo { get; set; }

        public virtual Clientes Cliente { get; set; }
    }
}
