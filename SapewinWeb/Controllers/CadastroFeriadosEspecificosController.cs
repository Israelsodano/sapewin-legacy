using Microsoft.EntityFrameworkCore;
using SapewinWeb.Metodos;
using SapewinWeb.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Mvc;

namespace SapewinWeb.Controllers
{
    
    public class CadastroFeriadosEspecificosController : Controller
    {
        Login.Models.LoginModel BankLogin = new Login.Models.LoginModel();
        MyContext Bank = new MyContext();

        [HttpGet]
        [VerificaLogin]
        public ActionResult CadastroFeriadosEspecificos_Abrir()
        {
            Login.Models.LoginSistema UsuarioLogado = BankLogin
                .LoginSistema.AsNoTracking()
                .FirstOrDefault(x => x.IDLoginsistema == Convert.ToInt32(Session["Usuario"].ToString())); // carrega usuario logado

            ViewBag.PermissoesIndices = UsuarioLogado.PerfiMaster
              ? Bank.FuncoesdeTelas.AsNoTracking().Select(x => x.IDFuncaoTela).ToList()
              : Bank.PermissoesdeTelas.AsNoTracking().Where(x => x.IDUsuario == UsuarioLogado.IDLoginsistema && Session["Empresa"].ToString().Contains(x.IDEmpresa.ToString("0000"))).Select(x => x.IDFuncaoTela).ToList(); //carreha lista de funçoes de telas qiue o usuario possui

            List<GrupodeFeriados> GrupodeFeriados = Bank.GrupodeFeriados.AsNoTracking().Include(x=>x.FeriadosEspecificos).AsNoTracking().ToList(); // Carrega Lista de GrupodeFeriados do sistema

            int Total = GrupodeFeriados.Count;

            GrupodeFeriados = GrupodeFeriados.Count < 10 ? GrupodeFeriados : GrupodeFeriados.GetRange(0, 10); // cria pagina 

            ViewBag.Paginas = Math.Ceiling(Convert.ToDecimal(Convert.ToDouble(Total) / 10)); //conta paginas

            ViewBag.Total = $"Registro(s): {Total} - Exibindo de {(GrupodeFeriados.Count > 0 ? 1 : 0)} a {GrupodeFeriados.Count} - {ViewBag.Paginas} Página(s)"; //monta string do footer

            foreach (var Grupo in GrupodeFeriados)
            {
                foreach (var Feriado in Grupo.FeriadosEspecificos.ToList())
                {
                    Grupo.ListaFeriados += $"{(Feriado.Dia < 10 ? "0" + Feriado.Dia : "" + Feriado.Dia)}/{(Feriado.Mes < 10 ? "0" + Feriado.Mes : "" + Feriado.Mes)}/{(Feriado.Ano == 0 ? "Todos" : "" + Feriado.Ano)} - {Feriado.Descricao}<br>";
                }

            }
                       
            return VerificaLoad.IsAjax(HttpContext.Request) ? PartialView(GrupodeFeriados) : (ViewResultBase)View(GrupodeFeriados);
        }

        [HttpGet]
        [VerificaLoad]
        public ActionResult CadastroFeriadosEspecificos_Abrir_Grid(String pesquisa, int Range, int Pagina, String Order, bool Condicao)
        {
            if (String.IsNullOrEmpty(pesquisa)) { pesquisa = ""; }

            pesquisa = pesquisa.ToLower().Replace('+', ' ');

            Login.Models.LoginSistema UsuarioLogado = BankLogin
                .LoginSistema.AsNoTracking()
                .FirstOrDefault(x => x.IDLoginsistema == Convert.ToInt32(Session["Usuario"].ToString())); // carrega usuario logado           

            ViewBag.PermissoesIndices = UsuarioLogado.PerfiMaster
             ? Bank.FuncoesdeTelas.AsNoTracking().Select(x => x.IDFuncaoTela).ToList()
             : Bank.PermissoesdeTelas.AsNoTracking().Where(x => x.IDUsuario == UsuarioLogado.IDLoginsistema && Session["Empresa"].ToString().Contains(x.IDEmpresa.ToString("0000"))).Select(x => x.IDFuncaoTela).ToList(); //carreha lista de funçoes de telas qiue o usuario possui

            List<GrupodeFeriados> GrupodeFeriados = new List<Models.GrupodeFeriados>();

            switch (Order.ToUpper())
            {
                case "DESCRICAO-UP":

                    if (Condicao || pesquisa == "")
                    {
                        GrupodeFeriados = Bank
                       .GrupodeFeriados.AsNoTracking()
                       .Include(x => x.FeriadosEspecificos).AsNoTracking()
                       .Where(x => x.Descricao.ToLower().Contains(pesquisa.ToLower())).OrderBy(x => x.Descricao).ToList();
                    }
                    else
                    {
                        GrupodeFeriados = Bank
                       .GrupodeFeriados.AsNoTracking()
                       .Include(x => x.FeriadosEspecificos).AsNoTracking()
                       .Where(x => !x.Descricao.ToLower().Contains(pesquisa.ToLower())).OrderBy(x => x.Descricao).ToList();
                    }
                    break;

                case "DESCRICAO-DOWN":

                    if (Condicao || pesquisa == "")
                    {
                        GrupodeFeriados = Bank
                       .GrupodeFeriados.AsNoTracking()
                       .Include(x => x.FeriadosEspecificos).AsNoTracking()
                       .Where(x => x.Descricao.ToLower().Contains(pesquisa.ToLower())).OrderByDescending(x => x.Descricao).ToList();
                    }
                    else
                    {
                        GrupodeFeriados = Bank
                       .GrupodeFeriados.AsNoTracking()
                       .Include(x => x.FeriadosEspecificos).AsNoTracking()
                       .Where(x => !x.Descricao.ToLower().Contains(pesquisa.ToLower())).OrderByDescending(x => x.Descricao).ToList();
                    }
                    break;

                default:
                    break;
            }           

            int Total = GrupodeFeriados.Count; // lista total

            ViewBag.Paginas = Math.Ceiling(Convert.ToDecimal(Convert.ToDouble(Total) / Range)); // conta paginas

            Pagina = Pagina > ViewBag.Paginas ? Convert.ToInt32(ViewBag.Paginas) - 1 : Pagina; // trata a pagina para nn ser maior que as paginas possiveis do obj

            Pagina = GrupodeFeriados.Count < Range ? Pagina * GrupodeFeriados.Count : Pagina * Range; // trata pagina para ser o primeiro registro da pagina

            Range = Range + Pagina > GrupodeFeriados.Count ? GrupodeFeriados.Count - Pagina : Range; // trata o range para nn ser maior que o range do obj

            GrupodeFeriados = GrupodeFeriados.Count < Range ? GrupodeFeriados : GrupodeFeriados.GetRange(Pagina, Range); // monta pagina

            ViewBag.Total = $"Registro(s): {Total} - Exibindo de {(Pagina == 0 ? 1 : Pagina + 1)} a {(Pagina == 0 ? Range : Pagina + Range)} - { ViewBag.Paginas} Página(s)"; // monta string do footer

            foreach (var Grupo in GrupodeFeriados)
            {
                foreach (var Feriado in Grupo.FeriadosEspecificos.ToList())
                {
                    Grupo.ListaFeriados += $"{(Feriado.Dia < 10 ? "0" + Feriado.Dia : "" + Feriado.Dia)}/{(Feriado.Mes < 10 ? "0"+ Feriado.Mes : "" + Feriado.Mes)}/{(Feriado.Ano == 0 ? "Todos" : "" + Feriado.Ano)} - {Feriado.Descricao}<br>";
                }
                
            }

            return PartialView(GrupodeFeriados);
        }

        [HttpGet]
        [VerificaLogin]
        public ActionResult CadastroFeriadosEspecificos_Incluir()
        {            
            return PartialView();            
        }

        [HttpGet]
        [VerificaLoad]
        public ActionResult TabFeriados(string[] Feriados, int? id)
        {
            List<FeriadosEspecificos> FeriadosEspecificos = new List<Models.FeriadosEspecificos>();
            if (id != null)
            {
                FeriadosEspecificos = Bank.GrupodeFeriados.AsNoTracking().Include(x => x.FeriadosEspecificos).AsNoTracking().FirstOrDefault(x => x.IDFeriado == id).FeriadosEspecificos.ToList();
            }
            else
            {                
                if (Feriados != null && !String.IsNullOrEmpty(Feriados[0]))
                {
                    foreach (var Feriado in Feriados)
                    { 
                        FeriadosEspecificos.Add(new Models.FeriadosEspecificos
                        {
                            Descricao = Criptografia.TiraAcentos(Feriado.Split('-')[0]),
                            Dia = Convert.ToInt32(Feriado.Split('-')[3]),
                            Mes = Convert.ToInt32(Feriado.Split('-')[2]),
                            Ano = Convert.ToInt32(Feriado.Split('-')[1])
                        });
                    }
                }
            }

            return PartialView(FeriadosEspecificos);
        }

        [HttpPost]
        
        [VerificaLogin]
        public ActionResult CadastroFeriadosEspecificos_Incluir_Salvar(String Descricao, string[] Feriados)
        {
            if (String.IsNullOrEmpty(Descricao) || Feriados == null || String.IsNullOrEmpty(Feriados[0]))
            {
                return Json(new { status = false, msg = "Preencha todos os campos" });
            }else
            {
                try
                {
                    GrupodeFeriados Grupo = new GrupodeFeriados
                    {
                        Descricao = Descricao,
                        IDFeriado = Bank.GrupodeFeriados.OrderBy(x => x.IDFeriado).LastOrDefault() == null ? 1 : Bank.GrupodeFeriados.OrderBy(x => x.IDFeriado).LastOrDefault().IDFeriado + 1,
                    };

                    Bank.GrupodeFeriados.Add(Grupo);                    

                    foreach (var Feriado in Feriados)
                    {
                        
                        if (Bank.FeriadosGerais.Where(x => x.Dia == Convert.ToInt32(Feriado.Split('-')[3]) && x.Mes == Convert.ToInt32(Feriado.Split('-')[2])).Count() > 0 || Bank.FeriadosEspecificos.Where(x => x.Dia == Convert.ToInt32(Feriado.Split('-')[3]) && x.Mes == Convert.ToInt32(Feriado.Split('-')[2])).Count() > 0)
                        {
                            throw new Exception($"Esta data {(Convert.ToInt32(Feriado.Split('-')[3]) < 10 ? "0" + Convert.ToInt32(Feriado.Split('-')[3]) : "" + Convert.ToInt32(Feriado.Split('-')[3]))}/{(Convert.ToInt32(Feriado.Split('-')[2]) < 10 ? "0" + Convert.ToInt32(Feriado.Split('-')[2]) : "" + Convert.ToInt32(Feriado.Split('-')[2]))} ja possui um feriado cadastrado");
                        }

                        Bank.FeriadosEspecificos.Add(new FeriadosEspecificos
                        {
                            IDFeriado = Grupo.IDFeriado,
                            GrupodeFeriados = Grupo,
                            Descricao = Criptografia.TiraAcentos(Feriado.Split('-')[0]),
                            Ano = Convert.ToInt32(Feriado.Split('-')[1]),
                            Mes = Convert.ToInt32(Feriado.Split('-')[2]),
                            Dia = Convert.ToInt32(Feriado.Split('-')[3])
                        });
                    }
                    Bank.SaveChanges();
                    return Json(new { status = true });

                }
                catch(Exception e)
                {
                    return Json(new { status = false, msg = e.Message });
                }
            }
        }

        [HttpGet]
        [VerificaLogin]
        public ActionResult CadastroFeriadosEspecificos_Alterar(int id)
        {
            GrupodeFeriados Grupo = Bank.GrupodeFeriados.AsNoTracking().Include(x => x.FeriadosEspecificos).AsNoTracking().First(x => x.IDFeriado == id);
            return PartialView(Grupo);
        }

        [HttpPost]
        
        [VerificaLogin]
        public ActionResult CadastroFeriadosEspecificos_Alterar_Salvar(int id, String Descricao, string[] Feriados)
        {
            if (String.IsNullOrEmpty(Descricao) || Feriados == null || String.IsNullOrEmpty(Feriados[0]))
            {
                return Json(new { status = false, msg = "Preencha todos os campos" });
            }else
            {
                try
                {
                    GrupodeFeriados Grupo = Bank.GrupodeFeriados.Include(x=>x.FeriadosEspecificos).FirstOrDefault(x => x.IDFeriado == id);
                    Grupo.Descricao = Descricao;
                    Grupo.FeriadosEspecificos.Clear();
                    Bank.SaveChanges();
                    foreach (var Feriado in Feriados)
                    {
                        
                        if (Bank.FeriadosGerais.Where(x => x.Dia == Convert.ToInt32(Feriado.Split('-')[3]) && x.Mes == Convert.ToInt32(Feriado.Split('-')[2])).Count() > 0 || Bank.FeriadosEspecificos.Where(x => x.Dia == Convert.ToInt32(Feriado.Split('-')[3]) && x.Mes == Convert.ToInt32(Feriado.Split('-')[2])).Count() > 0)
                        {
                            throw new Exception($"Esta data {(Convert.ToInt32(Feriado.Split('-')[3]) < 10 ? "0" + Convert.ToInt32(Feriado.Split('-')[3]) : "" + Convert.ToInt32(Feriado.Split('-')[3]))}/{(Convert.ToInt32(Feriado.Split('-')[2]) < 10 ? "0" + Convert.ToInt32(Feriado.Split('-')[2]) : "" + Convert.ToInt32(Feriado.Split('-')[2]))} ja possui um feriado cadastrado");
                        }

                        Grupo.FeriadosEspecificos.Add(new FeriadosEspecificos
                        {
                            Descricao = Criptografia.TiraAcentos(Feriado.Split('-')[0]),
                            Ano = Convert.ToInt32(Feriado.Split('-')[1]),
                            Mes = Convert.ToInt32(Feriado.Split('-')[2]),
                            Dia = Convert.ToInt32(Feriado.Split('-')[3]),
                            GrupodeFeriados = Grupo, IDFeriado = Grupo.IDFeriado
                        });
                    }

                    Bank.SaveChanges();
                    return Json(new { status = true });
                }
                catch(Exception e)
                {
                    return Json(new { status = false, msg = e.Message });
                }
            }
        }

        [HttpGet]
        [VerificaLogin]
        public ActionResult CadastroFeriadosEspecificos_Remover(int id)
        {
            GrupodeFeriados Grupo = Bank.GrupodeFeriados.AsNoTracking().First(x=>x.IDFeriado == id);
            return PartialView(Grupo);
        }

        [HttpPost]
        
        [VerificaLogin]
        public ActionResult CadastroFeriadosEspecificos_Remover_Salvar(int id)
        {
            try
            {
                GrupodeFeriados Grupo = Bank.GrupodeFeriados.Find(id);
                Bank.GrupodeFeriados.Remove(Grupo);
                Bank.SaveChanges();
                return Json(new { status = true });
            }
            catch (Exception e)
            {
                return Json(new { status = false, msg = e.Message });
            }
        }

        [HttpGet]
        [VerificaLogin]
        public ActionResult CadastroFeriadosEspecificos_Remover_Selecao(int[] id)
        {
            List<FeriadosEspecificos> FeriadosEspecificos = Bank.FeriadosEspecificos.AsNoTracking().Where(x => id.Contains(x.IDFeriado)).ToList();
            return PartialView(FeriadosEspecificos);
        }

        [HttpPost]
        
        [VerificaLogin]
        public ActionResult CadastroFeriadosEspecificos_Remover_Selecao_Salvar(int[] id)
        {
            try
            {
                foreach (var iditem in id)
                {
                    GrupodeFeriados FeriadoEspecifico = Bank.GrupodeFeriados.First(x => x.IDFeriado == iditem);
                    Bank.GrupodeFeriados.Remove(FeriadoEspecifico);
                }
                Bank.SaveChanges();

                return Json(new { status = true });
            }
            catch (Exception e)
            {
                return Json(new { status = false, msg = e.Message });
            }
        }
    }
}