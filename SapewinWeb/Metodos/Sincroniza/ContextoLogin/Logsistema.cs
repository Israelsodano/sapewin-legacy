using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Painel.Models
{
    public class LogSistema
    {
        public virtual int IDLogsistema { get; set; }
        public virtual String IP { get; set; }
        public virtual int IDLoginfabrevenda { get; set; }
        public virtual String Funcao { get; set; }
        public virtual String Tela { get; set; }
        public virtual String Descricao { get; set; }
        public virtual DateTime DataHora { get; set; }

        public virtual LoginFabRevenda LoginFabRevenda { get; set; }
    }
}
