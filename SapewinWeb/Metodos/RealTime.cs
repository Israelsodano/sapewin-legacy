using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SapewinWeb.Metodos
{
    public class RealTime
    {
        public static List<ObjVerificador> ListaEmpresas = new List<ObjVerificador>();

        public class ObjVerificador
        {
            public String Documento { get; set; }

            public List<String> FuncTela { get; set; }

            public List<int> IDUsuariosRealTime { get; set; }
        }
    }
}