using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SapewinWeb.Metodos;
using SapewinWeb.Models;
using Microsoft.EntityFrameworkCore;

namespace SapewinWeb.Controllers
{
   
    public class CadastroMotivosdeAbonoController : Controller
    {
        Login.Models.LoginModel BankLogin = new Login.Models.LoginModel();
        MyContext Bank = new MyContext();

        [HttpGet]
        [VerificaLogin]
        public ActionResult CadastroMotivosdeAbono_Abrir()
        {
            Login.Models.LoginSistema UsuarioLogado = BankLogin
                .LoginSistema.AsNoTracking()
                .FirstOrDefault(x => x.IDLoginsistema == Convert.ToInt32(Session["Usuario"].ToString())); // carrega usuario logado          

            ViewBag.PermissoesIndices = UsuarioLogado.PerfiMaster
                ? Bank.FuncoesdeTelas.AsNoTracking().Select(x => x.IDFuncaoTela).ToList()
                : Bank.PermissoesdeTelas.AsNoTracking().Where(x => x.IDUsuario == UsuarioLogado.IDLoginsistema && Session["Empresa"].ToString().Contains(x.IDEmpresa.ToString("0000"))).Select(x => x.IDFuncaoTela).ToList(); //carreha lista de funçoes de telas qiue o usuario possui

            List<MotivosdeAbono> MotivosdeAbono = Bank
                   .MotivosdeAbono.AsNoTracking()
                   .Where(x => (Session["Empresa"].ToString().Contains(x.IDEmpresa.ToString("0000")))).OrderBy(x=> $"{!x.Favorito}{x.Abreviacao}").ToList(); //garrega lista de objs que o usuario possui

            int Total = MotivosdeAbono.Count;

            MotivosdeAbono = MotivosdeAbono.Count < 10 ? MotivosdeAbono : MotivosdeAbono.GetRange(0, 10); // cria pagina 

            ViewBag.Paginas = Math.Ceiling(Convert.ToDecimal(Convert.ToDouble(Total) / 10)); //conta paginas

            ViewBag.Total = $"Registro(s): {Total} - Exibindo de {(MotivosdeAbono.Count > 0 ? 1 : 0)} a {MotivosdeAbono.Count} - {ViewBag.Paginas} Página(s)"; //monta string do footer

            return VerificaLoad.IsAjax(HttpContext.Request) ? PartialView(MotivosdeAbono) : (ViewResultBase)View(MotivosdeAbono);
        }

        [HttpGet]
        [VerificaLoad]
        public ActionResult CadastroMotivosdeAbono_Abrir_Grid(String pesquisa, int Range, int Pagina, String Order, bool Condicao)
        {
            if (String.IsNullOrEmpty(pesquisa)) { pesquisa = ""; }

            pesquisa = pesquisa.ToLower().Replace('+', ' ');

            Login.Models.LoginSistema UsuarioLogado = BankLogin
                .LoginSistema
                .FirstOrDefault(x => x.IDLoginsistema == Convert.ToInt32(Session["Usuario"].ToString())); // carrega usuario logado           

            ViewBag.PermissoesIndices = UsuarioLogado.PerfiMaster
             ? Bank.FuncoesdeTelas.AsNoTracking().Select(x => x.IDFuncaoTela).ToList()
             : Bank.PermissoesdeTelas.AsNoTracking().Where(x => x.IDUsuario == UsuarioLogado.IDLoginsistema && Session["Empresa"].ToString().Contains(x.IDEmpresa.ToString("0000"))).Select(x => x.IDFuncaoTela).ToList(); //carreha lista de funçoes de telas qiue o usuario possui

            List<MotivosdeAbono> MotivosdeAbono = new List<Models.MotivosdeAbono>();

            switch (Order.ToUpper())
            {
                case "CODIGO-UP":

                    if (Condicao || pesquisa == "")
                    {
                        MotivosdeAbono = Bank
                            .MotivosdeAbono.AsNoTracking()
                            .Where(x => Session["Empresa"].ToString()
                            .Contains(x.IDEmpresa.ToString("0000"))
                            && (x.Nome.ToLower().Contains(pesquisa.ToLower())
                            || x.Abreviacao.ToLower().Contains(pesquisa.ToLower())
                            || x.EventDia.ToLower().Contains(pesquisa.ToLower())
                            || x.EventHora.ToLower().Contains(pesquisa.ToLower()))).OrderBy(x => $"{!x.Favorito}{x.Abreviacao}").ToList();

                    }
                    else
                    {
                        MotivosdeAbono = Bank
                             .MotivosdeAbono.AsNoTracking()
                             .Where(x => Session["Empresa"].ToString()
                             .Contains(x.IDEmpresa.ToString("0000"))
                             && (!x.Nome.ToLower().Contains(pesquisa.ToLower())
                             && !x.Abreviacao.ToLower().Contains(pesquisa.ToLower())
                             && !x.EventDia.ToLower().Contains(pesquisa.ToLower())
                             && !x.EventHora.ToLower().Contains(pesquisa.ToLower()))).OrderBy(x => $"{!x.Favorito}{x.Abreviacao}").ToList();
                    }
                    break;

                case "CODIGO-DOWN":

                    if (Condicao || pesquisa == "")
                    {
                        MotivosdeAbono = Bank
                               .MotivosdeAbono.AsNoTracking()
                               .Where(x => Session["Empresa"].ToString()
                               .Contains(x.IDEmpresa.ToString("0000"))
                               && (x.Nome.ToLower().Contains(pesquisa.ToLower())
                               || x.Abreviacao.ToLower().Contains(pesquisa.ToLower())
                               || x.EventDia.ToLower().Contains(pesquisa.ToLower())
                               || x.EventHora.ToLower().Contains(pesquisa.ToLower()))).OrderByDescending(x => $"{x.Favorito}{x.Abreviacao}").ToList();
                    }
                    else
                    {
                        MotivosdeAbono = Bank
                              .MotivosdeAbono.AsNoTracking()
                              .Where(x => Session["Empresa"].ToString()
                              .Contains(x.IDEmpresa.ToString("0000"))
                              && (!x.Nome.ToLower().Contains(pesquisa.ToLower())
                              && !x.Abreviacao.ToLower().Contains(pesquisa.ToLower())
                              && !x.EventDia.ToLower().Contains(pesquisa.ToLower())
                              && !x.EventHora.ToLower().Contains(pesquisa.ToLower()))).OrderByDescending(x => $"{x.Favorito}{x.Abreviacao}").ToList();
                    }
                    break;

                case "DESCRICAO-UP":

                    if (Condicao || pesquisa == "")
                    {
                        MotivosdeAbono = Bank
                                 .MotivosdeAbono.AsNoTracking()
                                 .Where(x => Session["Empresa"].ToString()
                                 .Contains(x.IDEmpresa.ToString("0000"))
                                 && (x.Nome.ToLower().Contains(pesquisa.ToLower())
                                 || x.Abreviacao.ToLower().Contains(pesquisa.ToLower())
                                 || x.EventDia.ToLower().Contains(pesquisa.ToLower())
                                 || x.EventHora.ToLower().Contains(pesquisa.ToLower()))).OrderBy(x => $"{!x.Favorito}{x.Nome}").ToList();
                    }
                    else
                    {
                        MotivosdeAbono = Bank
                              .MotivosdeAbono.AsNoTracking()
                              .Where(x => Session["Empresa"].ToString()
                              .Contains(x.IDEmpresa.ToString("0000"))
                              && (!x.Nome.ToLower().Contains(pesquisa.ToLower())
                              && !x.Abreviacao.ToLower().Contains(pesquisa.ToLower())
                              && !x.EventDia.ToLower().Contains(pesquisa.ToLower())
                              && !x.EventHora.ToLower().Contains(pesquisa.ToLower()))).OrderBy(x => $"{!x.Favorito}{x.Nome}").ToList();
                    }
                    break;

                case "DESCRICAO-DOWN":

                    if (Condicao || pesquisa == "")
                    {
                        MotivosdeAbono = Bank
                                     .MotivosdeAbono.AsNoTracking()
                                     .Where(x => Session["Empresa"].ToString()
                                     .Contains(x.IDEmpresa.ToString("0000"))
                                     && (x.Nome.ToLower().Contains(pesquisa.ToLower())
                                     || x.Abreviacao.ToLower().Contains(pesquisa.ToLower())
                                     || x.EventDia.ToLower().Contains(pesquisa.ToLower())
                                     || x.EventHora.ToLower().Contains(pesquisa.ToLower()))).OrderByDescending(x => $"{x.Favorito}{x.Nome}").ToList();
                    }
                    else
                    {
                        MotivosdeAbono = Bank
                                .MotivosdeAbono.AsNoTracking()
                                .Where(x => Session["Empresa"].ToString()
                                .Contains(x.IDEmpresa.ToString("0000"))
                                && (!x.Nome.ToLower().Contains(pesquisa.ToLower())
                                && !x.Abreviacao.ToLower().Contains(pesquisa.ToLower())
                                && !x.EventDia.ToLower().Contains(pesquisa.ToLower())
                                && !x.EventHora.ToLower().Contains(pesquisa.ToLower()))).OrderByDescending(x => $"{x.Favorito}{x.Nome}").ToList();
                    }
                    break;

                default:
                    break;
            }               

            int Total = MotivosdeAbono.Count; // lista total

            ViewBag.Paginas = Math.Ceiling(Convert.ToDecimal(Convert.ToDouble(Total) / Range)); // conta paginas

            Pagina = Pagina > ViewBag.Paginas ? Convert.ToInt32(ViewBag.Paginas) - 1 : Pagina; // trata a pagina para nn ser maior que as paginas possiveis do obj

            Pagina = MotivosdeAbono.Count < Range ? Pagina * MotivosdeAbono.Count : Pagina * Range; // trata pagina para ser o primeiro registro da pagina

            Range = Range + Pagina > MotivosdeAbono.Count ? MotivosdeAbono.Count - Pagina : Range; // trata o range para nn ser maior que o range do obj

            MotivosdeAbono = MotivosdeAbono.Count < Range ? MotivosdeAbono : MotivosdeAbono.GetRange(Pagina, Range); // monta pagina

            ViewBag.Total = $"Registro(s): {Total} - Exibindo de {(Pagina == 0 ? 1 : Pagina + 1)} a {(Pagina == 0 ? Range : Pagina + Range)} - { ViewBag.Paginas} Página(s)"; // monta string do footer

            return PartialView(MotivosdeAbono);
        }

        [HttpGet]
        [VerificaLogin]
        public ActionResult CadastroMotivosdeAbono_Incluir()
        {
            return PartialView();
        }

        [HttpPost]
        
        [VerificaLogin]
        public ActionResult CadastroMotivosdeAbono_Incluir_Salvar([Bind(Include ="Nome,EventDia,EventHora,Tipo,Abreviacao,Favorito")] MotivosdeAbono Motivos)
        {
            if (String.IsNullOrEmpty(Motivos.Nome) || String.IsNullOrEmpty(Motivos.Tipo) || String.IsNullOrEmpty(Motivos.Abreviacao))
            {
                return Json(new { status = false, msg = "Preencha todos os campos" });
            }
            else
            {
                try
                {
                    if (Bank.MotivosdeAbono.Select(x=>x.Abreviacao).Contains(Motivos.Abreviacao))
                    {
                        throw new Exception("Esse registro já está cadastrado");
                    }
                    Motivos.Abreviacao = Motivos.Abreviacao.ToUpper();                    
                    Motivos.IDEmpresa = Bank.Empresas.AsNoTracking().First(x => Session["Empresa"].ToString().Contains(x.IDEmpresa.ToString("0000"))).IDEmpresa;
                    Bank.MotivosdeAbono.Add(Motivos);
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
        public ActionResult CadastroMotivosdeAbono_Alterar(string id)
        {
            MotivosdeAbono Motivo = Bank.MotivosdeAbono.AsNoTracking().First(x=>x.Abreviacao == id);
            return PartialView(Motivo);
        }

        [HttpPost]
        
        [VerificaLogin]
        public ActionResult CadastroMotivosdeAbono_Alterar_Salvar(string id, String Nome,String EventDia, String EventHora, String Tipo, bool Favorito)
        {
            if (String.IsNullOrEmpty(Nome) || String.IsNullOrEmpty(Tipo))
            {
                return Json(new { status = false, msg = "Preencha todos os campos" });
            }
            else
            {
                try
                {                   
                    MotivosdeAbono Motivos = Bank.MotivosdeAbono.First(x => x.Abreviacao == id);
                    Motivos.Nome = Nome;
                    Motivos.EventDia = EventDia;
                    Motivos.EventHora = EventHora;
                    Motivos.Tipo = Tipo;
                    Motivos.Favorito = Favorito;
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
        public ActionResult CadastroMotivosdeAbono_Remover(string id)
        {
            MotivosdeAbono Motivo = Bank.MotivosdeAbono.AsNoTracking().First(x => x.Abreviacao == id);
            return PartialView(Motivo);
        }

        [HttpPost]
        
        [VerificaLogin]
        public ActionResult CadastroMotivosdeAbono_Remover_Salvar(string id)
        {
            try
            {
                MotivosdeAbono Motivo = Bank.MotivosdeAbono.First(x => x.Abreviacao == id);
                Bank.MotivosdeAbono.Remove(Motivo);
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
        public ActionResult CadastroMotivosdeAbono_Incluir_Padroes()
        {
            
            return PartialView();
        }
        

        [HttpPost]
        
        [VerificaLogin]
        public ActionResult CadastroMotivosdeAbono_Incluir_Padroes_Salvar()
        {
            try
            {
                if (Bank.MotivosdeAbono.AsNoTracking().FirstOrDefault(x=>x.Abreviacao == "MEDIC" && Session["Empresa"].ToString().Contains(x.IDEmpresa.ToString("0000"))) == null)
                {
                    Bank.MotivosdeAbono.Add(new MotivosdeAbono
                    {
                        Abreviacao = "MEDIC",
                        Empresa = Bank.Empresas.First(x=> Session["Empresa"].ToString().Contains(x.IDEmpresa.ToString("0000"))),
                        IDEmpresa = Bank.Empresas.First(x => Session["Empresa"].ToString().Contains(x.IDEmpresa.ToString("0000"))).IDEmpresa,
                        Tipo = "A",
                        Nome = "Atestado Médico"                        
                    });
                }

                if (Bank.MotivosdeAbono.AsNoTracking().FirstOrDefault(x => x.Abreviacao == "AJUS" && Session["Empresa"].ToString().Contains(x.IDEmpresa.ToString("0000"))) == null)
                {
                    Bank.MotivosdeAbono.Add(new MotivosdeAbono
                    {
                        Abreviacao = "AJUS",
                        Empresa = Bank.Empresas.First(x => Session["Empresa"].ToString().Contains(x.IDEmpresa.ToString("0000"))),
                        IDEmpresa = Bank.Empresas.First(x => Session["Empresa"].ToString().Contains(x.IDEmpresa.ToString("0000"))).IDEmpresa,
                        Tipo = "A",
                        Nome = "Atraso Justificado"
                    });
                }

                if (Bank.MotivosdeAbono.AsNoTracking().FirstOrDefault(x => x.Abreviacao == "ACTRA" && Session["Empresa"].ToString().Contains(x.IDEmpresa.ToString("0000"))) == null)
                {
                    Bank.MotivosdeAbono.Add(new MotivosdeAbono
                    {
                        Abreviacao = "ACTRA",
                        Empresa = Bank.Empresas.First(x => Session["Empresa"].ToString().Contains(x.IDEmpresa.ToString("0000"))),
                        IDEmpresa = Bank.Empresas.First(x => Session["Empresa"].ToString().Contains(x.IDEmpresa.ToString("0000"))).IDEmpresa,
                        Tipo = "A",
                        Nome = "Acidente de Trabalho"
                    });
                }

                if (Bank.MotivosdeAbono.AsNoTracking().FirstOrDefault(x => x.Abreviacao == "AJMA" && Session["Empresa"].ToString().Contains(x.IDEmpresa.ToString("0000"))) == null)
                {
                    Bank.MotivosdeAbono.Add(new MotivosdeAbono
                    {
                        Abreviacao = "AJMA",
                        Empresa = Bank.Empresas.First(x => Session["Empresa"].ToString().Contains(x.IDEmpresa.ToString("0000"))),
                        IDEmpresa = Bank.Empresas.First(x => Session["Empresa"].ToString().Contains(x.IDEmpresa.ToString("0000"))).IDEmpresa,
                        Tipo = "B",
                        Nome = "Ajuste de Marcação"
                    });
                }

                if (Bank.MotivosdeAbono.AsNoTracking().FirstOrDefault(x => x.Abreviacao == "COMP" && Session["Empresa"].ToString().Contains(x.IDEmpresa.ToString("0000"))) == null)
                {
                    Bank.MotivosdeAbono.Add(new MotivosdeAbono
                    {
                        Abreviacao = "COMP",
                        Empresa = Bank.Empresas.First(x => Session["Empresa"].ToString().Contains(x.IDEmpresa.ToString("0000"))),
                        IDEmpresa = Bank.Empresas.First(x => Session["Empresa"].ToString().Contains(x.IDEmpresa.ToString("0000"))).IDEmpresa,
                        Tipo = "B",
                        Nome = "Compensação de Horas"
                    });
                }

                if (Bank.MotivosdeAbono.AsNoTracking().FirstOrDefault(x => x.Abreviacao == "EXTER" && Session["Empresa"].ToString().Contains(x.IDEmpresa.ToString("0000"))) == null)
                {
                    Bank.MotivosdeAbono.Add(new MotivosdeAbono
                    {
                        Abreviacao = "EXTER",
                        Empresa = Bank.Empresas.First(x => Session["Empresa"].ToString().Contains(x.IDEmpresa.ToString("0000"))),
                        IDEmpresa = Bank.Empresas.First(x => Session["Empresa"].ToString().Contains(x.IDEmpresa.ToString("0000"))).IDEmpresa,
                        Tipo = "B",
                        Nome = "Serviço Externo"
                    });
                }

                if (Bank.MotivosdeAbono.AsNoTracking().FirstOrDefault(x => x.Abreviacao == "FJUS" && Session["Empresa"].ToString().Contains(x.IDEmpresa.ToString("0000"))) == null)
                {
                    Bank.MotivosdeAbono.Add(new MotivosdeAbono
                    {
                        Abreviacao = "FJUS",
                        Empresa = Bank.Empresas.First(x => Session["Empresa"].ToString().Contains(x.IDEmpresa.ToString("0000"))),
                        IDEmpresa = Bank.Empresas.First(x => Session["Empresa"].ToString().Contains(x.IDEmpresa.ToString("0000"))).IDEmpresa,
                        Tipo = "A",
                        Nome = "Falta Justificada"
                    });
                }

                if (Bank.MotivosdeAbono.AsNoTracking().FirstOrDefault(x => x.Abreviacao == "FERIA" && Session["Empresa"].ToString().Contains(x.IDEmpresa.ToString("0000"))) == null)
                {
                    Bank.MotivosdeAbono.Add(new MotivosdeAbono
                    {
                        Abreviacao = "FERIA",
                        Empresa = Bank.Empresas.First(x => Session["Empresa"].ToString().Contains(x.IDEmpresa.ToString("0000"))),
                        IDEmpresa = Bank.Empresas.First(x => Session["Empresa"].ToString().Contains(x.IDEmpresa.ToString("0000"))).IDEmpresa,
                        Tipo = "C",
                        Nome = "Ferias Descansadas"
                    });
                }

                if (Bank.MotivosdeAbono.AsNoTracking().FirstOrDefault(x => x.Abreviacao == "FTMAR" && Session["Empresa"].ToString().Contains(x.IDEmpresa.ToString("0000"))) == null)
                {
                    Bank.MotivosdeAbono.Add(new MotivosdeAbono
                    {
                        Abreviacao = "FTMAR",
                        Empresa = Bank.Empresas.First(x => Session["Empresa"].ToString().Contains(x.IDEmpresa.ToString("0000"))),
                        IDEmpresa = Bank.Empresas.First(x => Session["Empresa"].ToString().Contains(x.IDEmpresa.ToString("0000"))).IDEmpresa,
                        Tipo = "B",
                        Nome = "Falta de Marcação"
                    });
                }

                if (Bank.MotivosdeAbono.AsNoTracking().FirstOrDefault(x => x.Abreviacao == "HEDES" && Session["Empresa"].ToString().Contains(x.IDEmpresa.ToString("0000"))) == null)
                {
                    Bank.MotivosdeAbono.Add(new MotivosdeAbono
                    {
                        Abreviacao = "HEDES",
                        Empresa = Bank.Empresas.First(x => Session["Empresa"].ToString().Contains(x.IDEmpresa.ToString("0000"))),
                        IDEmpresa = Bank.Empresas.First(x => Session["Empresa"].ToString().Contains(x.IDEmpresa.ToString("0000"))).IDEmpresa,
                        Tipo = "C",
                        Nome = "H.E não Autorizada"
                    });
                }

                if (Bank.MotivosdeAbono.AsNoTracking().FirstOrDefault(x => x.Abreviacao == "INSS" && Session["Empresa"].ToString().Contains(x.IDEmpresa.ToString("0000"))) == null)
                {
                    Bank.MotivosdeAbono.Add(new MotivosdeAbono
                    {
                        Abreviacao = "INSS",
                        Empresa = Bank.Empresas.First(x => Session["Empresa"].ToString().Contains(x.IDEmpresa.ToString("0000"))),
                        IDEmpresa = Bank.Empresas.First(x => Session["Empresa"].ToString().Contains(x.IDEmpresa.ToString("0000"))).IDEmpresa,
                        Tipo = "C",
                        Nome = "INSS"
                    });
                }

                if (Bank.MotivosdeAbono.AsNoTracking().FirstOrDefault(x => x.Abreviacao == "LMAT" && Session["Empresa"].ToString().Contains(x.IDEmpresa.ToString("0000"))) == null)
                {
                    Bank.MotivosdeAbono.Add(new MotivosdeAbono
                    {
                        Abreviacao = "LMAT",
                        Empresa = Bank.Empresas.First(x => Session["Empresa"].ToString().Contains(x.IDEmpresa.ToString("0000"))),
                        IDEmpresa = Bank.Empresas.First(x => Session["Empresa"].ToString().Contains(x.IDEmpresa.ToString("0000"))).IDEmpresa,
                        Tipo = "A",
                        Nome = "Licença Maternidade"
                    });
                }

                if (Bank.MotivosdeAbono.AsNoTracking().FirstOrDefault(x => x.Abreviacao == "LPAT" && Session["Empresa"].ToString().Contains(x.IDEmpresa.ToString("0000"))) == null)
                {
                    Bank.MotivosdeAbono.Add(new MotivosdeAbono
                    {
                        Abreviacao = "LPAT",
                        Empresa = Bank.Empresas.First(x => Session["Empresa"].ToString().Contains(x.IDEmpresa.ToString("0000"))),
                        IDEmpresa = Bank.Empresas.First(x => Session["Empresa"].ToString().Contains(x.IDEmpresa.ToString("0000"))).IDEmpresa,
                        Tipo = "A",
                        Nome = "Licença Paternidade"
                    });
                }

                if (Bank.MotivosdeAbono.AsNoTracking().FirstOrDefault(x => x.Abreviacao == "MIND" && Session["Empresa"].ToString().Contains(x.IDEmpresa.ToString("0000"))) == null)
                {
                    Bank.MotivosdeAbono.Add(new MotivosdeAbono
                    {
                        Abreviacao = "MIND",
                        Empresa = Bank.Empresas.First(x => Session["Empresa"].ToString().Contains(x.IDEmpresa.ToString("0000"))),
                        IDEmpresa = Bank.Empresas.First(x => Session["Empresa"].ToString().Contains(x.IDEmpresa.ToString("0000"))).IDEmpresa,
                        Tipo = "B",
                        Nome = "Marcação Indevida"
                    });
                }               

                if (Bank.MotivosdeAbono.AsNoTracking().FirstOrDefault(x => x.Abreviacao == "OBITO" && Session["Empresa"].ToString().Contains(x.IDEmpresa.ToString("0000"))) == null)
                {
                    Bank.MotivosdeAbono.Add(new MotivosdeAbono
                    {
                        Abreviacao = "OBITO",
                        Empresa = Bank.Empresas.First(x => Session["Empresa"].ToString().Contains(x.IDEmpresa.ToString("0000"))),
                        IDEmpresa = Bank.Empresas.First(x => Session["Empresa"].ToString().Contains(x.IDEmpresa.ToString("0000"))).IDEmpresa,
                        Tipo = "A",
                        Nome = "Falecimento de Parentes"
                    });
                }

                if (Bank.MotivosdeAbono.AsNoTracking().FirstOrDefault(x => x.Abreviacao == "PNT_D" && Session["Empresa"].ToString().Contains(x.IDEmpresa.ToString("0000"))) == null)
                {
                    Bank.MotivosdeAbono.Add(new MotivosdeAbono
                    {
                        Abreviacao = "PNT_D",
                        Empresa = Bank.Empresas.First(x => Session["Empresa"].ToString().Contains(x.IDEmpresa.ToString("0000"))),
                        IDEmpresa = Bank.Empresas.First(x => Session["Empresa"].ToString().Contains(x.IDEmpresa.ToString("0000"))).IDEmpresa,
                        Tipo = "A",
                        Nome = "Ponte Realizada Sobre Domingo"
                    });
                }

                if (Bank.MotivosdeAbono.AsNoTracking().FirstOrDefault(x => x.Abreviacao == "PNT_F" && Session["Empresa"].ToString().Contains(x.IDEmpresa.ToString("0000"))) == null)
                {
                    Bank.MotivosdeAbono.Add(new MotivosdeAbono
                    {
                        Abreviacao = "PNT_F",
                        Empresa = Bank.Empresas.First(x => Session["Empresa"].ToString().Contains(x.IDEmpresa.ToString("0000"))),
                        IDEmpresa = Bank.Empresas.First(x => Session["Empresa"].ToString().Contains(x.IDEmpresa.ToString("0000"))).IDEmpresa,
                        Tipo = "A",
                        Nome = "Ponte Realizada Sobre Feriado ou Folga"
                    });
                }

                if (Bank.MotivosdeAbono.AsNoTracking().FirstOrDefault(x => x.Abreviacao == "PNT_S" && Session["Empresa"].ToString().Contains(x.IDEmpresa.ToString("0000"))) == null)
                {
                    Bank.MotivosdeAbono.Add(new MotivosdeAbono
                    {
                        Abreviacao = "PNT_S",
                        Empresa = Bank.Empresas.First(x => Session["Empresa"].ToString().Contains(x.IDEmpresa.ToString("0000"))),
                        IDEmpresa = Bank.Empresas.First(x => Session["Empresa"].ToString().Contains(x.IDEmpresa.ToString("0000"))).IDEmpresa,
                        Tipo = "A",
                        Nome = "Ponte Realizada Sobre Sábado"
                    });
                }

                if (Bank.MotivosdeAbono.AsNoTracking().FirstOrDefault(x => x.Abreviacao == "PNT_U" && Session["Empresa"].ToString().Contains(x.IDEmpresa.ToString("0000"))) == null)
                {
                    Bank.MotivosdeAbono.Add(new MotivosdeAbono
                    {
                        Abreviacao = "PNT_U",
                        Empresa = Bank.Empresas.First(x => Session["Empresa"].ToString().Contains(x.IDEmpresa.ToString("0000"))),
                        IDEmpresa = Bank.Empresas.First(x => Session["Empresa"].ToString().Contains(x.IDEmpresa.ToString("0000"))).IDEmpresa,
                        Tipo = "A",
                        Nome = "Ponte Realizada Sobre Dia Util"
                    });
                }

                if (Bank.MotivosdeAbono.AsNoTracking().FirstOrDefault(x => x.Abreviacao == "SJUS" && Session["Empresa"].ToString().Contains(x.IDEmpresa.ToString("0000"))) == null)
                {
                    Bank.MotivosdeAbono.Add(new MotivosdeAbono
                    {
                        Abreviacao = "SJUS",
                        Empresa = Bank.Empresas.First(x => Session["Empresa"].ToString().Contains(x.IDEmpresa.ToString("0000"))),
                        IDEmpresa = Bank.Empresas.First(x => Session["Empresa"].ToString().Contains(x.IDEmpresa.ToString("0000"))).IDEmpresa,
                        Tipo = "A",
                        Nome = "Saída Justificada"
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
        [HttpGet]
        [VerificaLogin]
        public ActionResult CadastroMotivosdeAbono_Remover_Selecao(string[] id)
        {
            List<MotivosdeAbono> Motivo = Bank.MotivosdeAbono.AsNoTracking().Where(x => id.Contains(x.Abreviacao) && Session["Empresa"].ToString().Contains(x.IDEmpresa.ToString("0000"))).ToList();
            return PartialView(Motivo);
        }        

        [HttpPost]
        
        [VerificaLogin]
        public ActionResult CadastroMotivosdeAbono_Remover_Selecao_Salvar(string[] id)
        {
            try
            {
                foreach (var iditem in id)
                {
                    MotivosdeAbono Motivo = Bank.MotivosdeAbono.First(x => x.Abreviacao == iditem && Session["Empresa"].ToString().Contains(x.IDEmpresa.ToString("0000")));
                    Bank.MotivosdeAbono.Remove(Motivo);                    
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