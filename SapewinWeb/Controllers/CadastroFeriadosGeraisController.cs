using Microsoft.EntityFrameworkCore;
using SapewinWeb.Metodos;
using SapewinWeb.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SapewinWeb.Controllers
{
    
    public class CadastroFeriadosGeraisController : Controller
    {
        Login.Models.LoginModel BankLogin = new Login.Models.LoginModel();
        MyContext Bank = new MyContext();

        [HttpGet]
        [VerificaLogin]
        public ActionResult CadastroFeriadosGerais_Abrir()
        {
            Login.Models.LoginSistema UsuarioLogado = BankLogin
                .LoginSistema.AsNoTracking()
                .FirstOrDefault(x => x.IDLoginsistema == Convert.ToInt32(Session["Usuario"].ToString())); // carrega usuario logado

            ViewBag.PermissoesIndices = UsuarioLogado.PerfiMaster
              ? Bank.FuncoesdeTelas.AsNoTracking().Select(x => x.IDFuncaoTela).ToList()
              : Bank.PermissoesdeTelas.AsNoTracking().Where(x => x.IDUsuario == UsuarioLogado.IDLoginsistema && Session["Empresa"].ToString().Contains(x.IDEmpresa.ToString("0000"))).Select(x => x.IDFuncaoTela).ToList(); //carreha lista de funçoes de telas qiue o usuario possui

            List<FeriadosGerais> FeriadosGerais = Bank.FeriadosGerais.AsNoTracking().OrderBy(x=> Convert.ToDateTime($"{x.Dia}/{x.Mes}")).ToList(); // Carrega Lista de FeriadosGerais do sistema

            int Total = FeriadosGerais.Count;

            FeriadosGerais = FeriadosGerais.Count < 10 ? FeriadosGerais : FeriadosGerais.GetRange(0, 10); // cria pagina 

            ViewBag.Anos = Bank.FeriadosGerais.Where(x => x.Ano != null).Select(x => x.Ano).Distinct().ToList();

            ViewBag.Paginas = Math.Ceiling(Convert.ToDecimal(Convert.ToDouble(Total) / 10)); //conta paginas

            ViewBag.Total = $"Registro(s): {Total} - Exibindo de {(FeriadosGerais.Count > 0 ? 1 : 0)} a {FeriadosGerais.Count} - {ViewBag.Paginas} Página(s)"; //monta string do footer

            return VerificaLoad.IsAjax(HttpContext.Request) ? PartialView(FeriadosGerais) : (ViewResultBase)View(FeriadosGerais);
        }

        [HttpGet]
        [VerificaLoad]
        public ActionResult CadastroFeriadosGerais_Abrir_Grid(String pesquisa, int Range, int Pagina, String Order, bool Condicao, int Ano)
        {
            if (String.IsNullOrEmpty(pesquisa)) { pesquisa = ""; }

            pesquisa = pesquisa.ToLower().Replace('+', ' ');

            Login.Models.LoginSistema UsuarioLogado = BankLogin
                .LoginSistema.AsNoTracking()
                .FirstOrDefault(x => x.IDLoginsistema == Convert.ToInt32(Session["Usuario"].ToString())); // carrega usuario logado

            ViewBag.PermissoesIndices = UsuarioLogado.PerfiMaster
             ? Bank.FuncoesdeTelas.AsNoTracking().Select(x => x.IDFuncaoTela).ToList()
             : Bank.PermissoesdeTelas.AsNoTracking().Where(x => x.IDUsuario == UsuarioLogado.IDLoginsistema && Session["Empresa"].ToString().Contains(x.IDEmpresa.ToString("0000"))).Select(x => x.IDFuncaoTela).ToList(); //carreha lista de funçoes de telas qiue o usuario possui

            List<FeriadosGerais> FeriadosGerais = new List<Models.FeriadosGerais>();

            switch (Order.ToUpper())
            {
                case "DATA-UP":

                    if (Condicao || pesquisa == "")
                    {
                        FeriadosGerais = Bank
                       .FeriadosGerais.AsNoTracking().ToList();
                    }
                    else
                    {
                        FeriadosGerais = Bank
                       .FeriadosGerais.AsNoTracking().ToList();
                    }
                    break;

                case "DATA-DOWN":

                    if (Condicao || pesquisa == "")
                    {
                        FeriadosGerais = Bank
                       .FeriadosGerais.AsNoTracking().ToList();
                    }
                    else
                    {
                        FeriadosGerais = Bank
                       .FeriadosGerais.AsNoTracking().ToList();
                    }
                    break;

                case "DESCRICAO-UP":

                    if (Condicao || pesquisa == "")
                    {
                        FeriadosGerais = Bank
                       .FeriadosGerais.AsNoTracking().ToList();
                    }
                    else
                    {
                        FeriadosGerais = Bank
                       .FeriadosGerais.AsNoTracking().ToList();
                    }
                    break;

                case "DESCRICAO-DOWN":

                    if (Condicao || pesquisa == "")
                    {
                        FeriadosGerais = Bank
                       .FeriadosGerais.AsNoTracking().ToList();
                    }
                    else
                    {
                        FeriadosGerais = Bank
                      .FeriadosGerais.AsNoTracking().ToList();
                    }
                    break;

                default:
                    break;
            }

            int Total = FeriadosGerais.Count; // lista total

            ViewBag.Paginas = Math.Ceiling(Convert.ToDecimal(Convert.ToDouble(Total) / Range)); // conta paginas

            Pagina = Pagina > ViewBag.Paginas ? Convert.ToInt32(ViewBag.Paginas) - 1 : Pagina; // trata a pagina para nn ser maior que as paginas possiveis do obj

            Pagina = FeriadosGerais.Count < Range ? Pagina * FeriadosGerais.Count : Pagina * Range; // trata pagina para ser o primeiro registro da pagina

            Range = Range + Pagina > FeriadosGerais.Count ? FeriadosGerais.Count - Pagina : Range; // trata o range para nn ser maior que o range do obj

            FeriadosGerais = FeriadosGerais.Count < Range ? FeriadosGerais : FeriadosGerais.GetRange(Pagina, Range); // monta pagina

            ViewBag.Total = $"Registro(s): {Total} - Exibindo de {(Pagina == 0 ? 1 : Pagina + 1)} a {(Pagina == 0 ? Range : Pagina + Range)} - { ViewBag.Paginas} Página(s)"; // monta string do footer

           
            return PartialView(FeriadosGerais);
        }

        [HttpGet]
        [VerificaLogin]
        public ActionResult CadastroFeriadosGerais_Incluir()
        {
            return PartialView();
        }

        [HttpPost]
        
        [VerificaLogin]
        public ActionResult CadastroFeriadosGerais_Incluir_Salvar(String Descricao, DateTime ? Data, bool Tdd)
        {
            if (String.IsNullOrEmpty(Descricao) || String.IsNullOrEmpty(Data.ToString()))
            {
                return Json(new { status = false, msg = "Preencha todos os campos" });
            }
            else
            {               
                try
                {                  
                    string[] arraydata = Data.ToString().Substring(0,10).Split('/');

                    if (Bank.FeriadosGerais.AsNoTracking().Where(x=> x.Dia == Convert.ToInt32(arraydata[0]) && x.Mes == Convert.ToInt32(arraydata[1])).Count() > 0 || Bank.FeriadosEspecificos.Where(x => x.Dia == Convert.ToInt32(arraydata[0]) && x.Mes == Convert.ToInt32(arraydata[1])).Count() > 0)
                    {
                        throw new Exception($"Esta data {(Convert.ToInt32(arraydata[0]) < 10 ? "0" + Convert.ToInt32(arraydata[0]) : "" + Convert.ToInt32(arraydata[0]))}/{(Convert.ToInt32(arraydata[1]) < 10 ? "0" + Convert.ToInt32(arraydata[1]) : "" + Convert.ToInt32(arraydata[1]))} ja possui um feriado cadastrado");
                    }

                    FeriadosGerais FeriadoGeral = new FeriadosGerais
                    {
                        IDFeriado = Bank.FeriadosGerais.OrderBy(x => x.IDFeriado).LastOrDefault() == null ? 1 : Bank.FeriadosGerais.OrderBy(x => x.IDFeriado).LastOrDefault().IDFeriado + 1,
                        Descricao = Descricao,
                        Dia = Convert.ToInt32(arraydata[0]),
                        Mes = Convert.ToInt32(arraydata[1]),
                    };

                    if (!Tdd) { FeriadoGeral.Ano = Convert.ToInt32(arraydata[2]); }

                    Bank.FeriadosGerais.Add(FeriadoGeral);
                    Bank.SaveChanges();

                    return Json(new { status = true });
                }
                catch (Exception e)
                {
                    return Json(new { status = false, msg = e.Message });
                }
            }
        }

        [HttpGet]
        [VerificaLogin]
        public ActionResult CadastroFeriadosGerais_Alterar(int id)
        {
            FeriadosGerais Feriado = Bank.FeriadosGerais.AsNoTracking().FirstOrDefault(x => x.IDFeriado == id);
            return PartialView(Feriado); //retorna obj por id e sessao para a view
        }

        [HttpPost]
        
        [VerificaLogin]
        public ActionResult CadastroFeriadosGerais_Alterar_Salvar(int id, String Descricao, DateTime Data, bool Tdd)
        {
            if (String.IsNullOrEmpty(Descricao) || String.IsNullOrEmpty(Data.ToString()))
            {
                return Json(new { status = false, msg = "Preencha todos os campos" });
            }
            else
            {
                try
                {
                    string[] arraydatas = Data.ToString().Substring(0, 10).Split('/');                    

                    if (Bank.FeriadosGerais.AsNoTracking().Where(x => x.Dia == Convert.ToInt32(arraydatas[0]) && x.Mes == Convert.ToInt32(arraydatas[1]) && x.IDFeriado != id).Count() > 0 || Bank.FeriadosEspecificos.Where(x => x.Dia == Convert.ToInt32(arraydatas[0]) && x.Mes == Convert.ToInt32(arraydatas[1])).Count() > 0)
                    {
                        throw new Exception($"Esta data {(Convert.ToInt32(arraydatas[0]) < 10 ? "0" + Convert.ToInt32(arraydatas[0]) : "" + Convert.ToInt32(arraydatas[0]))}/{(Convert.ToInt32(arraydatas[1]) < 10 ? "0" + Convert.ToInt32(arraydatas[1]) : "" + Convert.ToInt32(arraydatas[1]))} ja possui um feriado cadastrado");
                    }

                    FeriadosGerais Feriado = Bank.FeriadosGerais.Find(id);
                    Feriado.Descricao = Descricao;
                    Feriado.Dia = Convert.ToInt32(arraydatas[0]);
                    Feriado.Mes = Convert.ToInt32(arraydatas[1]);
                    if (!Tdd) { Feriado.Ano = Convert.ToInt32(arraydatas[2]); } else { Feriado.Ano = null; }

                    Bank.SaveChanges();

                    return Json(new { status = true });
                }
                catch (Exception e)
                {
                    return Json(new { status = false, msg = e.Message });
                }
            }
        }

        [HttpGet]
        [VerificaLogin]
        public ActionResult CadastroFeriadosGerais_Remover(int id)
        {
            FeriadosGerais Feriado = Bank.FeriadosGerais.AsNoTracking().First(x=>x.IDFeriado == id);
            return PartialView(Feriado);
        }

        [HttpPost]
        
        [VerificaLogin]
        public ActionResult CadastroFeriadosGerais_Remover_Salvar(int id)
        {
            try
            {
                FeriadosGerais Feriado = Bank.FeriadosGerais.Find(id);
                Bank.FeriadosGerais.Remove(Feriado);
                Bank.SaveChanges();
                return Json(new { status = true });
            }
            catch(Exception e)
            {
                return Json(new { status = false, msg = e.Message });
            }
        }

        [HttpGet]
        [VerificaLogin]
        public ActionResult CadastroFeriadosGerais_Incluir_Padroes()
        {
            return PartialView();
        }

        [HttpPost]
        
        [VerificaLogin]
        public ActionResult CadastroFeriadosGerais_Incluir_Padroes_Salvar()
        {
            try
            {
                int teste = Bank.FeriadosGerais.AsNoTracking().Where(x => x.Dia == 1 && x.Mes == 1).Count();
                int teste2 = Bank.FeriadosEspecificos.AsNoTracking().Where(x => x.Dia == 1 && x.Mes == 1).Count();
                if (Bank.FeriadosGerais.AsNoTracking().Where(x => x.Dia == 1 && x.Mes == 1).Count() == 0 && Bank.FeriadosEspecificos.AsNoTracking().Where(x => x.Dia == 1 && x.Mes == 1).Count() == 0)
                {
                    Bank.FeriadosGerais.Add(new FeriadosGerais
                    {
                        Dia = 1,
                        Mes = 1,
                        Descricao = "Ano Novo",
                        IDFeriado = Bank.FeriadosGerais.AsNoTracking().LastOrDefault() == null ? 1 : Bank.FeriadosGerais.AsNoTracking().LastOrDefault().IDFeriado + 1
                    });

                    Bank.SaveChanges();
                }

                if (Bank.FeriadosGerais.AsNoTracking().Where(x => x.Dia == 21 && x.Mes == 4).Count() == 0 && Bank.FeriadosEspecificos.AsNoTracking().Where(x => x.Dia == 21 && x.Mes == 4).Count() == 0)
                {
                    Bank.FeriadosGerais.Add(new FeriadosGerais
                    {
                        Dia = 21,
                        Mes = 4,
                        Descricao = "Tiradentes",
                        IDFeriado = Bank.FeriadosGerais.AsNoTracking().LastOrDefault() == null ? 1 : Bank.FeriadosGerais.AsNoTracking().LastOrDefault().IDFeriado + 1
                    });

                    Bank.SaveChanges();
                }


                if (Bank.FeriadosGerais.AsNoTracking().Where(x => x.Dia == 1 && x.Mes == 5).Count() == 0 && Bank.FeriadosEspecificos.AsNoTracking().Where(x => x.Dia == 1 && x.Mes == 5).Count() == 0)
                {
                    Bank.FeriadosGerais.Add(new FeriadosGerais
                    {
                        Dia = 1,
                        Mes = 5,
                        Descricao = "Dia do Trabalhador",
                        IDFeriado = Bank.FeriadosGerais.AsNoTracking().LastOrDefault() == null ? 1 : Bank.FeriadosGerais.AsNoTracking().LastOrDefault().IDFeriado + 1
                    });

                    Bank.SaveChanges();
                }


                if (Bank.FeriadosGerais.AsNoTracking().Where(x => x.Dia == 7 && x.Mes == 9).Count() == 0 && Bank.FeriadosEspecificos.AsNoTracking().Where(x => x.Dia == 7 && x.Mes == 9).Count() == 0)
                {
                    Bank.FeriadosGerais.Add(new FeriadosGerais
                    {
                        Dia = 7,
                        Mes = 9,
                        Descricao = "Dia da Independência",
                        IDFeriado = Bank.FeriadosGerais.AsNoTracking().LastOrDefault() == null ? 1 : Bank.FeriadosGerais.AsNoTracking().LastOrDefault().IDFeriado + 1
                    });

                    Bank.SaveChanges();
                }


                if (Bank.FeriadosGerais.AsNoTracking().Where(x => x.Dia == 12 && x.Mes == 10).Count() == 0 && Bank.FeriadosEspecificos.AsNoTracking().Where(x => x.Dia == 12 && x.Mes == 10).Count() == 0)
                {
                    Bank.FeriadosGerais.Add(new FeriadosGerais
                    {
                        Dia = 12,
                        Mes = 10,
                        Descricao = "Nossa Senhora da Aparecida",
                        IDFeriado = Bank.FeriadosGerais.AsNoTracking().LastOrDefault() == null ? 1 : Bank.FeriadosGerais.AsNoTracking().LastOrDefault().IDFeriado + 1
                    });

                    Bank.SaveChanges();
                }

                if (Bank.FeriadosGerais.AsNoTracking().Where(x => x.Dia == 2 && x.Mes == 11).Count() == 0 && Bank.FeriadosEspecificos.AsNoTracking().Where(x => x.Dia == 2 && x.Mes == 11).Count() == 0)
                {
                    Bank.FeriadosGerais.Add(new FeriadosGerais
                    {
                        Dia = 2,
                        Mes = 11,
                        Descricao = "Finados",
                        IDFeriado = Bank.FeriadosGerais.AsNoTracking().LastOrDefault() == null ? 1 : Bank.FeriadosGerais.AsNoTracking().LastOrDefault().IDFeriado + 1
                    });

                    Bank.SaveChanges();
                }

                if (Bank.FeriadosGerais.AsNoTracking().Where(x => x.Dia == 15 && x.Mes == 11).Count() == 0 && Bank.FeriadosEspecificos.AsNoTracking().Where(x => x.Dia == 15 && x.Mes == 11).Count() == 0)
                {
                    Bank.FeriadosGerais.Add(new FeriadosGerais
                    {
                        Dia = 15,
                        Mes = 11,
                        Descricao = "Dia da Republica",
                        IDFeriado = Bank.FeriadosGerais.AsNoTracking().LastOrDefault() == null ? 1 : Bank.FeriadosGerais.AsNoTracking().LastOrDefault().IDFeriado + 1
                    });

                    Bank.SaveChanges();
                }

                if (Bank.FeriadosGerais.AsNoTracking().Where(x => x.Dia == 25 && x.Mes == 12).Count() == 0 && Bank.FeriadosEspecificos.AsNoTracking().Where(x => x.Dia == 25 && x.Mes == 12).Count() == 0)
                {
                    Bank.FeriadosGerais.Add(new FeriadosGerais
                    {
                        Dia = 25,
                        Mes = 12,
                        Descricao = "Natal",
                        IDFeriado = Bank.FeriadosGerais.AsNoTracking().LastOrDefault() == null ? 1 : Bank.FeriadosGerais.AsNoTracking().LastOrDefault().IDFeriado + 1
                    });

                    Bank.SaveChanges();
                }

                
                return Json(new { status = true });
            }
            catch (Exception e)
            {
                return Json(new { status = true, msg = e.Message });          }
        }

        [HttpGet]
        [VerificaLogin]
        public ActionResult CadastroFeriadosGerais_Remover_Selecao (int[] id)
        {
            List<FeriadosGerais> Feriado = Bank.FeriadosGerais.AsNoTracking().Where(x=>id.Contains(x.IDFeriado)).ToList();
            return PartialView(Feriado);
          
        }

        [HttpPost]
        
        [VerificaLogin]
        public ActionResult CadastroFeriadosGerais_Remover_Selecao_Salvar(int[] id)
        {
            try
            {
                List<FeriadosGerais> Feriado = Bank.FeriadosGerais.Where(x => id.Contains(x.IDFeriado)).ToList();

                foreach (var item in Feriado)
                {
                    Bank.FeriadosGerais.Remove(item);
                }

                Bank.SaveChanges();
                return Json(new { status = true });
            }catch(Exception e)
            {
                return Json(new { status = true, msg = e.Message });
            }
        }
    }
}