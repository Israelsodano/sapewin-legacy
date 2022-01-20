using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SapewinWeb.Metodos
{
    public class ObjExtras
    {
        public tipo Tipo { get; set; }

        public String Horas { get; set; }

        public int Percent { get; set; }

        public int? Adicional { get; set; }

        public enum tipo
        {
            Uteis = 1, Sabados = 2, Domingos = 3, Feriados = 4, Folgas = 5
        }
    }

    public class ObjExtraabstrata
    {
        public static string[] ArraysTipo(List<SapewinWeb.Models.EscalonamentodeHoraExtra> Escalonamento, SapewinWeb.Models.EscalonamentodeHoraExtra.tipo Tipo)
        {
            var arrays = Escalonamento.Where(x => x.Tipo == Tipo).Select(x => $"{x.Horas}/{x.Porcentagem}/{x.Adicional}/{Convert.ToInt32(x.Tipo)}").ToArray();

            return arrays;
        }
    }
}