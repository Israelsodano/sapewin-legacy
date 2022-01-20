using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SapewinWeb.Metodos
{
    public class ListaMemoria
    {
        public static List<ListadeObjs> Lista = new List<ListadeObjs>();
    }

    public class ListadeObjs
    {
        public int ID { get; set; }

        public String SessionID { get; set; }

        public ObjIntervalosAuxiliares Obj { get; set; }                
    }

    public class ObjIntervalosAuxiliares
    {
        public int Order { get; set; }

        public String Inicio { get; set; }

        public String Fim { get; set; }

        public String Carga { get; set; }

        public tipo Tipo { get; set; }

        public enum tipo
        {
            Fixo = 1, Carga = 2
        };        
    }
}