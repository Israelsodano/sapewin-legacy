using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SapewinWeb.Metodos
{
    public class ListaMemoriaHorarios
    {
        public static List<ObjHorarios> Lista = new List<ObjHorarios>();

        public static List<ObjHorariosCargaSemanal> ListaCargaSemanal = new List<ObjHorariosCargaSemanal>();

        public static bool ReturnFixo(string id)
        {
            if (id == "Folga" || id == "Sabado" || id == "Domingo")
            {
                return true;
            }else
            {
                Models.MyContext Bank = new Models.MyContext();
                if (Bank.Horarios.FirstOrDefault(x=> HttpContext.Current.Session["Empresa"].ToString().Contains(x.IDEmpresa.ToString("0000")) && x.IDHorario == Convert.ToInt32(id)).Tipo == Models.Horarios.tipo.Fixo)
                {
                    return true;
                }else
                {
                    return false;
                }
            }
        }

        public enum tipo
        {
            Fixo=1,Carga=2,CargaSemanal=3
        }

        public static List<SapewinWeb.Models.Horarios> RetornaListadeHorarios(tipo Tipo)
        {
            Models.MyContext Bank = new Models.MyContext();

            if (Tipo == tipo.Fixo)
            {
                return Bank.Horarios.AsNoTracking().Where(x => HttpContext.Current.Session["Empresa"].ToString().Contains(x.IDEmpresa.ToString("0000")) && !x.CargaSemanal && x.Tipo == Models.Horarios.tipo.Fixo).ToList();
            }else if(Tipo == tipo.Carga)
            {
                return Bank.Horarios.AsNoTracking().Where(x => HttpContext.Current.Session["Empresa"].ToString().Contains(x.IDEmpresa.ToString("0000")) && !x.CargaSemanal && x.Tipo == Models.Horarios.tipo.Carga).ToList();
            }
            else
            {
                return Bank.Horarios.AsNoTracking().Where(x => HttpContext.Current.Session["Empresa"].ToString().Contains(x.IDEmpresa.ToString("0000")) && x.CargaSemanal && x.Tipo == Models.Horarios.tipo.Carga).ToList();
            }
        }
    }

    public class ObjHorarios
    {
        public int Order { get; set; }

        public int IDRef { get; set; }

        public int ID { get; set; }

        public int Dias { get; set; }

        public String SessionID { get; set; }
    }

    public class ObjHorariosCargaSemanal
    {
        public Models.EscalasHorarios.diainicio DiaInicio { get; set; }

        public bool Direto { get; set; }

        public string HoradeEntrada { get; set; }

        public int Order { get; set; }

        public int IDRef { get; set; }

        public int ID { get; set; }

        public List<int> Dias { get; set; }

        public String SessionID { get; set; }       
    }
}