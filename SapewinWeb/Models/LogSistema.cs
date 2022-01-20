﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SapewinWeb.Models
{
    public class LogSistema
    {
        public virtual int IDLog { get; set; }

        public virtual String IP { get; set; }

        public virtual int IDUsuario { get; set; }

        public virtual String Funcao { get; set; }

        public virtual String Tela { get; set; }

        public virtual String Descricao { get; set; }

        public virtual DateTime DataHora { get; set; }
    }
}