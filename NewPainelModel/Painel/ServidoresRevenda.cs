using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Login.Models
{
    [Table("pnl_servidoresrevenda")]
    public class ServidoresRevenda
    {
        public virtual int IDRevenda { get; set; }
        public virtual int IDServidor { get; set; }

        public virtual Servidores Servidor { get; set; }
        public virtual Revendas Revenda { get; set; }

    }
}
