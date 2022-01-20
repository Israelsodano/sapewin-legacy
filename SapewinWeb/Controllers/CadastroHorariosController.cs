using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json.Linq;
using SapewinWeb.Metodos;
using SapewinWeb.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;

namespace SapewinWeb.Controllers
{

    public class CadastroHorariosController : Controller
    {
        Login.Models.LoginModel BankLogin = new Login.Models.LoginModel();
        MyContext Bank = new MyContext();

        public static List<ListadeObjs> Lista = new List<ListadeObjs>();

        [HttpGet]
        [VerificaLogin]
        public ActionResult CadastroHorarios_Abrir()
        {
            Login.Models.LoginSistema UsuarioLogado = BankLogin
                .LoginSistema.AsNoTracking()
                .FirstOrDefault(x => x.IDLoginsistema == Convert.ToInt32(Session["Usuario"].ToString())); // carrega usuario logado

            ViewBag.PermissoesIndices = UsuarioLogado.PerfiMaster
              ? Bank.FuncoesdeTelas.AsNoTracking().Select(x => x.IDFuncaoTela).ToList()
              : Bank.PermissoesdeTelas.AsNoTracking().Where(x => x.IDUsuario == UsuarioLogado.IDLoginsistema && Session["Empresa"].ToString().Contains(x.IDEmpresa.ToString("0000"))).Select(x => x.IDFuncaoTela).ToList(); //carreha lista de funçoes de telas qiue o usuario possui

            List<Horarios> Horarios = Bank.Horarios.AsNoTracking().Where(x => Session["Empresa"].ToString().Contains(x.IDEmpresa.ToString("0000"))).ToList(); // Carrega Lista de Horarios do sistema

            int Total = Horarios.Count;

            Horarios = Horarios.Count < 10 ? Horarios : Horarios.GetRange(0, 10); // cria pagina 

            ViewBag.Paginas = Math.Ceiling(Convert.ToDecimal(Convert.ToDouble(Total) / 10)); //conta paginas

            ViewBag.Total = $"Registro(s): {Total} - Exibindo de {(Horarios.Count > 0 ? 1 : 0)} a {Horarios.Count} - {ViewBag.Paginas} Página(s)"; //monta string do footer

            return VerificaLoad.IsAjax(HttpContext.Request) ? PartialView(Horarios) : (ViewResultBase)View(Horarios);
        }

        [HttpGet]
        [VerificaLoad]
        public ActionResult CadastroHorarios_Abrir_Grid(String pesquisa, int Range, int Pagina, String Order, bool Condicao)
        {
            if (String.IsNullOrEmpty(pesquisa)) { pesquisa = ""; }

            pesquisa = pesquisa.ToLower().Replace('+', ' ');

            Login.Models.LoginSistema UsuarioLogado = BankLogin
                .LoginSistema
                .FirstOrDefault(x => x.IDLoginsistema == Convert.ToInt32(Session["Usuario"].ToString())); // carrega usuario logado           

            ViewBag.PermissoesIndices = UsuarioLogado.PerfiMaster
             ? Bank.FuncoesdeTelas.AsNoTracking().Select(x => x.IDFuncaoTela).ToList()
             : Bank.PermissoesdeTelas.AsNoTracking().Where(x => x.IDUsuario == UsuarioLogado.IDLoginsistema && Session["Empresa"].ToString().Contains(x.IDEmpresa.ToString("0000"))).Select(x => x.IDFuncaoTela).ToList(); //carreha lista de funçoes de telas qiue o usuario possui

            List<Horarios> Horarios = new List<Models.Horarios>();

            switch (Order.ToUpper())
            {
                case "CODIGO-UP":

                    if (Condicao || pesquisa == "")
                    {
                        Horarios = Bank
                       .Horarios.AsNoTracking()
                       .Where(x => x.Descricao.ToLower().Contains(pesquisa.ToLower()) || x.IDHorario.ToString().Contains(pesquisa) && Session["Empresa"].ToString().Contains(x.IDEmpresa.ToString("0000"))).OrderBy(x => x.IDHorario).ToList();
                    }
                    else
                    {
                        Horarios = Bank
                       .Horarios.AsNoTracking()
                       .Where(x => !x.Descricao.ToLower().Contains(pesquisa.ToLower()) && !x.IDHorario.ToString().Contains(pesquisa) && Session["Empresa"].ToString().Contains(x.IDEmpresa.ToString("0000"))).OrderBy(x => x.IDHorario).ToList();
                    }
                    break;

                case "CODIGO-DOWN":

                    if (Condicao || pesquisa == "")
                    {
                        Horarios = Bank
                       .Horarios.AsNoTracking()
                       .Where(x => x.Descricao.ToLower().Contains(pesquisa.ToLower()) || x.IDHorario.ToString().Contains(pesquisa) && Session["Empresa"].ToString().Contains(x.IDEmpresa.ToString("0000"))).OrderByDescending(x => x.IDHorario).ToList();
                    }
                    else
                    {
                        Horarios = Bank
                       .Horarios.AsNoTracking()
                       .Where(x => !x.Descricao.ToLower().Contains(pesquisa.ToLower()) && !x.IDHorario.ToString().Contains(pesquisa) && Session["Empresa"].ToString().Contains(x.IDEmpresa.ToString("0000"))).OrderByDescending(x => x.IDHorario).ToList();
                    }
                    break;

                case "DESCRICAO-UP":

                    if (Condicao || pesquisa == "")
                    {
                        Horarios = Bank
                       .Horarios.AsNoTracking()
                       .Where(x => x.Descricao.ToLower().Contains(pesquisa.ToLower()) || x.IDHorario.ToString().Contains(pesquisa) && Session["Empresa"].ToString().Contains(x.IDEmpresa.ToString("0000"))).OrderBy(x => x.Descricao).ToList();
                    }
                    else
                    {
                        Horarios = Bank
                       .Horarios.AsNoTracking()
                       .Where(x => !x.Descricao.ToLower().Contains(pesquisa.ToLower()) && !x.IDHorario.ToString().Contains(pesquisa) && Session["Empresa"].ToString().Contains(x.IDEmpresa.ToString("0000"))).OrderBy(x => x.Descricao).ToList();
                    }
                    break;

                case "DESCRICAO-DOWN":

                    if (Condicao || pesquisa == "")
                    {
                        Horarios = Bank
                       .Horarios.AsNoTracking()
                       .Where(x => x.Descricao.ToLower().Contains(pesquisa.ToLower()) || x.IDHorario.ToString().Contains(pesquisa) && Session["Empresa"].ToString().Contains(x.IDEmpresa.ToString("0000"))).OrderByDescending(x => x.Descricao).ToList();
                    }
                    else
                    {
                        Horarios = Bank
                       .Horarios.AsNoTracking()
                       .Where(x => !x.Descricao.ToLower().Contains(pesquisa.ToLower()) && !x.IDHorario.ToString().Contains(pesquisa) && Session["Empresa"].ToString().Contains(x.IDEmpresa.ToString("0000"))).OrderByDescending(x => x.Descricao).ToList();
                    }
                    break;

                case "TIPO-UP":

                    if (Condicao || pesquisa == "")
                    {
                        Horarios = Bank
                       .Horarios.AsNoTracking()
                       .Where(x => x.Descricao.ToLower().Contains(pesquisa.ToLower()) || x.IDHorario.ToString().Contains(pesquisa) && Session["Empresa"].ToString().Contains(x.IDEmpresa.ToString("0000"))).OrderBy(x => x.Tipo).ToList();
                    }
                    else
                    {
                        Horarios = Bank
                       .Horarios.AsNoTracking()
                       .Where(x => !x.Descricao.ToLower().Contains(pesquisa.ToLower()) && !x.IDHorario.ToString().Contains(pesquisa) && Session["Empresa"].ToString().Contains(x.IDEmpresa.ToString("0000"))).OrderBy(x => x.Tipo).ToList();
                    }
                    break;

                case "TIPO-DOWN":

                    if (Condicao || pesquisa == "")
                    {
                        Horarios = Bank
                       .Horarios.AsNoTracking()
                       .Where(x => x.Descricao.ToLower().Contains(pesquisa.ToLower()) || x.IDHorario.ToString().Contains(pesquisa) && Session["Empresa"].ToString().Contains(x.IDEmpresa.ToString("0000"))).OrderByDescending(x => x.Tipo).ToList();
                    }
                    else
                    {
                        Horarios = Bank
                       .Horarios.AsNoTracking()
                       .Where(x => !x.Descricao.ToLower().Contains(pesquisa.ToLower()) && !x.IDHorario.ToString().Contains(pesquisa) && Session["Empresa"].ToString().Contains(x.IDEmpresa.ToString("0000"))).OrderByDescending(x => x.Tipo).ToList();
                    }
                    break;

                default:
                    break;
            }            

            int Total = Horarios.Count; // lista total

            ViewBag.Paginas = Math.Ceiling(Convert.ToDecimal(Convert.ToDouble(Total) / Range)); // conta paginas

            Pagina = Pagina > ViewBag.Paginas ? Convert.ToInt32(ViewBag.Paginas) - 1 : Pagina; // trata a pagina para nn ser maior que as paginas possiveis do obj

            Pagina = Horarios.Count < Range ? Pagina * Horarios.Count : Pagina * Range; // trata pagina para ser o primeiro registro da pagina

            Range = Range + Pagina > Horarios.Count ? Horarios.Count - Pagina : Range; // trata o range para nn ser maior que o range do obj

            Horarios = Horarios.Count < Range ? Horarios : Horarios.GetRange(Pagina, Range); // monta pagina

            ViewBag.Total = $"Registro(s): {Total} - Exibindo de {(Pagina == 0 ? 1 : Pagina + 1)} a {(Pagina == 0 ? Range : Pagina + Range)} - { ViewBag.Paginas} Página(s)"; // monta string do footer

            return PartialView(Horarios);
        }

        [HttpGet]
        [VerificaLogin]
        public ActionResult CadastroHorarios_Incluir()
        {            
            ListaMemoria.Lista.RemoveAll(x => x.SessionID == Session.SessionID);
            return PartialView();
        }

        [HttpPost]
        
        [VerificaLoad]
        public ActionResult CadastroHorarios_Incluir_Salvar(int? id, string CargaSemanal, [Bind(Include = "Descricao,Entrada,EntradaIntervalo,SaidaIntervalo,Saida,ViradadoDia,NDescontarIntervalo,VintqHoras,Carga,TotaldoIntervalo")] Horarios Horario)
        {

            if (String.IsNullOrEmpty(Horario.Carga))
            {

                if (String.IsNullOrEmpty(Horario.Entrada) || String.IsNullOrEmpty(Horario.Saida))
                {
                    return Json(new { status = false, msg = "Preencha os Campos obrigatórios" });
                }
                else
                {
                    if ((String.IsNullOrEmpty(Horario.EntradaIntervalo) && !String.IsNullOrEmpty(Horario.SaidaIntervalo)) || (!String.IsNullOrEmpty(Horario.EntradaIntervalo) && String.IsNullOrEmpty(Horario.SaidaIntervalo)))
                    {
                        return Json(new { status = false, msg = "Campos de Intervalo não preenchidos corretamente" });
                    }
                    else
                    {
                        try
                        {
                            int totalJornada = 0;

                            if (Horario.Entrada.Length != 5 || (!String.IsNullOrEmpty(Horario.EntradaIntervalo) && Horario.EntradaIntervalo.Length != 5) || (!String.IsNullOrEmpty(Horario.SaidaIntervalo) && Horario.SaidaIntervalo.Length != 5) || Horario.Saida.Length != 5)
                            {
                                throw new Exception("Campos de Intervalo não preenchidos corretamente");
                            }

                            if (!String.IsNullOrEmpty(Horario.EntradaIntervalo))
                            {

                                int minutosEntrada = CalculosdeHora.Tempo_Minuto(new string[] { Horario.Entrada })[0];
                                int minutosIntervalo = CalculosdeHora.Tempo_Minuto(new string[] { Horario.EntradaIntervalo })[0];
                                int totaldoPeriodo1 = minutosIntervalo < minutosEntrada ? (minutosIntervalo + 1440) - minutosEntrada : minutosIntervalo - minutosEntrada;

                                Horario.TotaldoPeriodo1 = CalculosdeHora.Minuto_Tempo(new int[] { totaldoPeriodo1 })[0];
                            }

                            if (!String.IsNullOrEmpty(Horario.SaidaIntervalo))
                            {
                                int minutosSaida = CalculosdeHora.Tempo_Minuto(new string[] { Horario.Saida })[0];
                                int minutosIntervalo = CalculosdeHora.Tempo_Minuto(new string[] { Horario.SaidaIntervalo })[0];
                                int totaldoPeriodo2 = minutosSaida < minutosIntervalo ? (minutosSaida + 1440) - minutosIntervalo : minutosSaida - minutosIntervalo;

                                Horario.TotaldoPeriodo2 = CalculosdeHora.Minuto_Tempo(new int[] { totaldoPeriodo2 })[0];
                            }

                            if (!String.IsNullOrEmpty(Horario.EntradaIntervalo))
                            {
                                int minutosEntrada = CalculosdeHora.Tempo_Minuto(new string[] { Horario.EntradaIntervalo })[0];
                                int minutosSaida = CalculosdeHora.Tempo_Minuto(new string[] { Horario.SaidaIntervalo })[0];
                                int totaldoIntervalo = minutosSaida < minutosEntrada ? (minutosSaida + 1440) - minutosEntrada : minutosSaida - minutosEntrada;

                                Horario.TotaldoIntervalo = CalculosdeHora.Minuto_Tempo(new int[] { totaldoIntervalo })[0];
                            }

                            if (!String.IsNullOrEmpty(Horario.TotaldoPeriodo1) && !String.IsNullOrEmpty(Horario.TotaldoPeriodo2))
                            {
                                totalJornada = CalculosdeHora.SomaHoras_Minutos(new string[] {
                            (String.IsNullOrEmpty(Horario.TotaldoPeriodo1) ? "00:00" : Horario.TotaldoPeriodo1),
                            (String.IsNullOrEmpty(Horario.TotaldoPeriodo2) ? "00:00" : Horario.TotaldoPeriodo2),
                            (String.IsNullOrEmpty(Horario.TotaldoIntervalo) ? "00:00" : Horario.TotaldoIntervalo) });
                            }
                            else
                            {
                                int Entrada = CalculosdeHora.Tempo_Minuto(new string[] { Horario.Entrada })[0];

                                int Saida = CalculosdeHora.Tempo_Minuto(new string[] { Horario.Saida })[0];

                                int Intervalo = !String.IsNullOrEmpty(Horario.TotaldoIntervalo) ? CalculosdeHora.Tempo_Minuto(new string[] { Horario.TotaldoIntervalo })[0] : 0;

                                totalJornada = Saida <= Entrada ? (Saida + 1440) - Entrada : Saida - Entrada;

                                totalJornada = totalJornada + Intervalo;
                            }

                            if (totalJornada >= 1440 || (Horario.VintqHoras && totalJornada > 1440))
                            {
                                throw new Exception($"jornada de {CalculosdeHora.Minuto_Tempo(new int[] { totalJornada })[0]} Horas, esse valor de jornada não é permitido");
                            }

                            Horario.TotalJornada = CalculosdeHora.Minuto_Tempo(new int[] { (!Horario.NDescontarIntervalo ? totalJornada - (CalculosdeHora.Tempo_Minuto(new string[] { String.IsNullOrEmpty(Horario.TotaldoIntervalo) ? "00:00" : Horario.TotaldoIntervalo })[0]) : totalJornada) })[0];

                            Horario.Descricao = String.IsNullOrEmpty(Horario.Descricao) ? ($"{Horario.Entrada}-{(String.IsNullOrEmpty(Horario.EntradaIntervalo) ? $"Sem Intervalo-{Horario.Saida}" : $"{Horario.EntradaIntervalo}-{Horario.SaidaIntervalo}-{Horario.Saida}")}") : Horario.Descricao;

                            if (Bank.Horarios.FirstOrDefault(x => x.IDHorario == id) != null)
                            {
                                throw new Exception("Este ID ja esta em uso");
                            }

                            Horario.Tipo = Horarios.tipo.Fixo;

                            Horario.IDHorario = (id != null) ? Convert.ToInt32(id) : (Bank.Horarios.LastOrDefault() != null) ? Bank.Horarios.Last().IDHorario + 1 : 1;

                            Horario.IDEmpresa = Bank.Empresas.First(x => Session["Empresa"].ToString().Contains(x.IDEmpresa.ToString("0000"))).IDEmpresa;

                            Bank.Horarios.Add(Horario);

                            int contador = 1;

                            foreach (var IntervloMemoria in ListaMemoria.Lista.Where(x => x.SessionID == Session.SessionID).OrderBy(x => x.Obj.Order))
                            {
                                if (IntervloMemoria.Obj.Tipo == ObjIntervalosAuxiliares.tipo.Fixo)
                                {
                                    Bank.IntervalosAuxiliares.Add(new IntervalosAuxiliares
                                    {
                                        Order = contador,
                                        Inicio = IntervloMemoria.Obj.Inicio,
                                        Fim = IntervloMemoria.Obj.Fim,
                                        Tipo = IntervalosAuxiliares.tipo.Fixo,
                                        IDIntervalo = Bank.IntervalosAuxiliares.OrderBy(x => x.IDIntervalo).LastOrDefault() == null ? 1 : Bank.IntervalosAuxiliares.OrderBy(x => x.IDIntervalo).LastOrDefault().IDIntervalo + 1,
                                        IDEmpresa = Horario.IDEmpresa,
                                        IDHorario = Horario.IDHorario,
                                        Horarios = Horario,
                                        Empresa = Horario.Empresa
                                    });

                                }
                                else
                                {
                                    Bank.IntervalosAuxiliares.Add(new IntervalosAuxiliares
                                    {
                                        Order = contador,
                                        Carga = IntervloMemoria.Obj.Carga,
                                        Tipo = IntervalosAuxiliares.tipo.Carga,
                                        IDIntervalo = Bank.IntervalosAuxiliares.OrderBy(x => x.IDIntervalo).LastOrDefault() == null ? 1 : Bank.IntervalosAuxiliares.OrderBy(x => x.IDIntervalo).LastOrDefault().IDIntervalo + 1,
                                        IDEmpresa = Horario.IDEmpresa,
                                        IDHorario = Horario.IDHorario,
                                        Horarios = Horario,
                                        Empresa = Horario.Empresa
                                    });
                                }

                                Bank.SaveChanges();
                                contador++;
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
            else
            {
                try
                {
                    Horario.CargaSemanal = !String.IsNullOrEmpty(CargaSemanal);

                    if ((Horario.CargaSemanal && CalculosdeHora.Tempo_Minuto(new string[] { Horario.Carga })[0] > 10080) || (!Horario.CargaSemanal && CalculosdeHora.Tempo_Minuto(new string[] { Horario.Carga })[0] > 1440))
                    {
                        throw new Exception("Carga máxima excedida");
                    }

                    Horario.Tipo = Horarios.tipo.Carga;

                    Horario.IDHorario = (id != null) ? Convert.ToInt32(id) : (Bank.Horarios.LastOrDefault() != null) ? Bank.Horarios.Last().IDHorario + 1 : 1;

                    Horario.Descricao = String.IsNullOrEmpty(Horario.Descricao) ? Horario.CargaSemanal ? "Carga Semanal - " + Horario.Carga : "Carga Diária - " + Horario.Carga : Horario.Descricao;

                    Horario.IDEmpresa = Bank.Empresas.First(x => Session["Empresa"].ToString().Contains(x.IDEmpresa.ToString("0000"))).IDEmpresa;

                    Bank.Horarios.Add(Horario);

                    int contador = 1;

                    foreach (var IntervloMemoria in ListaMemoria.Lista.Where(x => x.SessionID == Session.SessionID).OrderBy(x => x.Obj.Order))
                    {
                        if (IntervloMemoria.Obj.Tipo == ObjIntervalosAuxiliares.tipo.Fixo)
                        {
                            Bank.IntervalosAuxiliares.Add(new IntervalosAuxiliares
                            {
                                Order = contador,
                                Inicio = IntervloMemoria.Obj.Inicio,
                                Fim = IntervloMemoria.Obj.Fim,
                                Tipo = IntervalosAuxiliares.tipo.Fixo,
                                IDIntervalo = Bank.IntervalosAuxiliares.OrderBy(x => x.IDIntervalo).LastOrDefault() == null ? 1 : Bank.IntervalosAuxiliares.OrderBy(x => x.IDIntervalo).LastOrDefault().IDIntervalo + 1,
                                IDEmpresa = Horario.IDEmpresa,
                                IDHorario = Horario.IDHorario,
                                Horarios = Horario,
                                Empresa = Horario.Empresa
                            });

                        }
                        else
                        {
                            Bank.IntervalosAuxiliares.Add(new IntervalosAuxiliares
                            {
                                Order = contador,
                                Carga = IntervloMemoria.Obj.Carga,
                                Tipo = IntervalosAuxiliares.tipo.Carga,
                                IDIntervalo = Bank.IntervalosAuxiliares.OrderBy(x => x.IDIntervalo).LastOrDefault() == null ? 1 : Bank.IntervalosAuxiliares.OrderBy(x => x.IDIntervalo).LastOrDefault().IDIntervalo + 1,
                                IDEmpresa = Horario.IDEmpresa,
                                IDHorario = Horario.IDHorario,
                                Horarios = Horario,
                                Empresa = Horario.Empresa
                            });
                        }

                        Bank.SaveChanges();
                        contador++;
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

        [HttpGet]
        [VerificaLogin]
        public PartialViewResult CadastroHorarios_Alterar(int id)
        {
            ListaMemoria.Lista.RemoveAll(x => x.SessionID == Session.SessionID);
            Lista.RemoveAll(x => x.SessionID == Session.SessionID);
            Horarios Horario = Bank.Horarios.Include(x => x.IntervalosAuxiliares).First(x => x.IDHorario == id && Session["Empresa"].ToString().Contains(x.IDEmpresa.ToString("0000")));

            foreach (var item in Horario.IntervalosAuxiliares)
            {
                if (item.Tipo == IntervalosAuxiliares.tipo.Carga)
                {
                    ListaMemoria.Lista.Add(new ListadeObjs
                    {
                        Obj = new ObjIntervalosAuxiliares
                        {
                            Carga = item.Carga,
                            Order = item.Order,
                            Tipo = ObjIntervalosAuxiliares.tipo.Carga
                        },
                        SessionID = Session.SessionID,
                        ID = Lista.Where(x => x.SessionID == Session.SessionID).Count() > 0 ? Lista.OrderBy(x => x.ID).LastOrDefault(x => x.SessionID == Session.SessionID).ID + 1 : 1,
                    });

                    Lista.Add(new ListadeObjs
                    {
                        Obj = new ObjIntervalosAuxiliares
                        {
                            Carga = item.Carga,
                            Order = item.Order,
                            Tipo = ObjIntervalosAuxiliares.tipo.Carga
                        },
                        SessionID = Session.SessionID,
                        ID = Lista.Where(x => x.SessionID == Session.SessionID).Count() > 0 ? Lista.OrderBy(x => x.ID).LastOrDefault(x => x.SessionID == Session.SessionID).ID + 1 : 1,
                    });
                }
                else
                {
                    ListaMemoria.Lista.Add(new ListadeObjs
                    {
                        Obj = new ObjIntervalosAuxiliares
                        {
                            Inicio = item.Inicio,
                            Fim = item.Fim,
                            Order = item.Order,
                            Tipo = ObjIntervalosAuxiliares.tipo.Fixo
                        },                        
                        SessionID = Session.SessionID,
                        ID = Lista.Where(x => x.SessionID == Session.SessionID).Count() > 0 ? Lista.OrderBy(x => x.ID).LastOrDefault(x => x.SessionID == Session.SessionID).ID + 1 : 1,
                    });

                    Lista.Add(new ListadeObjs
                    {
                        Obj = new ObjIntervalosAuxiliares
                        {
                            Inicio = item.Inicio,
                            Fim = item.Fim,
                            Order = item.Order,
                            Tipo = ObjIntervalosAuxiliares.tipo.Fixo
                        },
                        SessionID = Session.SessionID,
                        ID = Lista.Where(x => x.SessionID == Session.SessionID).Count() > 0 ? Lista.OrderBy(x => x.ID).LastOrDefault(x => x.SessionID == Session.SessionID).ID + 1 : 1,
                    });
                }               
            }

            return PartialView(Horario);
        }

        [HttpPost]
        
        [VerificaLogin]
        public ActionResult CadastroHorarios_Alterar_Salvar(int id, string CargaSemanal, [Bind(Include = "Descricao,Entrada,EntradaIntervalo,SaidaIntervalo,Saida,ViradadoDia,NDescontarIntervalo,VintqHoras,Carga,TotaldoIntervalo")] Horarios Horario)
        {
            if (String.IsNullOrEmpty(Horario.Carga))
            {
                if (String.IsNullOrEmpty(Horario.Entrada) || String.IsNullOrEmpty(Horario.Saida))
                {
                    return Json(new { status = false, msg = "Preencha os campos obrigatórios de 'Entrada' e 'Saida'" });
                }
                else
                {
                    if ((String.IsNullOrEmpty(Horario.EntradaIntervalo) && !String.IsNullOrEmpty(Horario.SaidaIntervalo)) || (!String.IsNullOrEmpty(Horario.EntradaIntervalo) && String.IsNullOrEmpty(Horario.SaidaIntervalo)))
                    {
                        return Json(new { status = false, msg = "Campos de Intervalo não preenchidos corretamente" });
                    }
                    else
                    {
                        try
                        {
                            int totalJornada = 0;

                            if (Horario.Entrada.Length != 5 || (!String.IsNullOrEmpty(Horario.EntradaIntervalo) && Horario.EntradaIntervalo.Length != 5) || (!String.IsNullOrEmpty(Horario.SaidaIntervalo) && Horario.SaidaIntervalo.Length != 5) || Horario.Saida.Length != 5)
                            {
                                throw new Exception("Campos de Intervalo não preenchidos corretamente");
                            }

                            if (!String.IsNullOrEmpty(Horario.EntradaIntervalo))
                            {

                                int minutosEntrada = CalculosdeHora.Tempo_Minuto(new string[] { Horario.Entrada })[0];
                                int minutosIntervalo = CalculosdeHora.Tempo_Minuto(new string[] { Horario.EntradaIntervalo })[0];
                                int totaldoPeriodo1 = minutosIntervalo < minutosEntrada ? (minutosIntervalo + 1440) - minutosEntrada : minutosIntervalo - minutosEntrada;

                                Horario.TotaldoPeriodo1 = CalculosdeHora.Minuto_Tempo(new int[] { totaldoPeriodo1 })[0];
                            }

                            if (!String.IsNullOrEmpty(Horario.SaidaIntervalo))
                            {
                                int minutosSaida = CalculosdeHora.Tempo_Minuto(new string[] { Horario.Saida })[0];
                                int minutosIntervalo = CalculosdeHora.Tempo_Minuto(new string[] { Horario.SaidaIntervalo })[0];
                                int totaldoPeriodo2 = minutosSaida < minutosIntervalo ? (minutosSaida + 1440) - minutosIntervalo : minutosSaida - minutosIntervalo;

                                Horario.TotaldoPeriodo2 = CalculosdeHora.Minuto_Tempo(new int[] { totaldoPeriodo2 })[0];
                            }

                            if (!String.IsNullOrEmpty(Horario.EntradaIntervalo))
                            {
                                int minutosEntrada = CalculosdeHora.Tempo_Minuto(new string[] { Horario.EntradaIntervalo })[0];
                                int minutosSaida = CalculosdeHora.Tempo_Minuto(new string[] { Horario.SaidaIntervalo })[0];
                                int totaldoIntervalo = minutosSaida < minutosEntrada ? (minutosSaida + 1440) - minutosEntrada : minutosSaida - minutosEntrada;

                                Horario.TotaldoIntervalo = CalculosdeHora.Minuto_Tempo(new int[] { totaldoIntervalo })[0];
                            }


                            if (!String.IsNullOrEmpty(Horario.TotaldoPeriodo1) && !String.IsNullOrEmpty(Horario.TotaldoPeriodo2))
                            {
                                totalJornada = CalculosdeHora.SomaHoras_Minutos(new string[] {
                            (String.IsNullOrEmpty(Horario.TotaldoPeriodo1) ? "00:00" : Horario.TotaldoPeriodo1),
                            (String.IsNullOrEmpty(Horario.TotaldoPeriodo2) ? "00:00" : Horario.TotaldoPeriodo2),
                            (String.IsNullOrEmpty(Horario.TotaldoIntervalo) ? "00:00" : Horario.TotaldoIntervalo) });
                            }
                            else
                            {

                                int Entrada = CalculosdeHora.Tempo_Minuto(new string[] { Horario.Entrada })[0];

                                int Saida = CalculosdeHora.Tempo_Minuto(new string[] { Horario.Saida })[0];

                                int Intervalo = !String.IsNullOrEmpty(Horario.TotaldoIntervalo) ? CalculosdeHora.Tempo_Minuto(new string[] { Horario.TotaldoIntervalo })[0] : 0;

                                totalJornada = Saida <= Entrada ? (Saida + 1440) - Entrada : Saida - Entrada;

                                totalJornada = totalJornada + Intervalo;
                            }

                            if (totalJornada >= 1440 || (Horario.VintqHoras && totalJornada > 1440))
                            {
                                throw new Exception($"jornada de {CalculosdeHora.Minuto_Tempo(new int[] { totalJornada })[0]} Horas, esse valor de jornada não é permitido");
                            }

                            Horario.TotalJornada = CalculosdeHora.Minuto_Tempo(new int[] { (!Horario.NDescontarIntervalo ? totalJornada - (CalculosdeHora.Tempo_Minuto(new string[] { String.IsNullOrEmpty(Horario.TotaldoIntervalo) ? "00:00" : Horario.TotaldoIntervalo })[0]) : totalJornada) })[0];

                            Horarios HorarioAlterar = Bank.Horarios.Include(x => x.IntervalosAuxiliares).First(x => x.IDHorario == id && Session["Empresa"].ToString().Contains(x.IDEmpresa.ToString("0000")));

                            bool padrao = String.IsNullOrEmpty(Horario.Descricao) ? true : (Horario.Descricao.Split('-').Count() == 4 && Horario.Descricao.Split('-')[0].Split(':').Count() == 2 && Horario.Descricao.Split('-')[1].Split(':').Count() == 2 && Horario.Descricao.Split('-')[2].Split(':').Count() == 2 && Horario.Descricao.Split('-')[3].Split(':').Count() == 2 && Horario.Descricao.Count() == 23) || (Horario.Descricao.Split('-').Count() == 3 && Horario.Descricao.Split('-')[1] == "Sem Intervalo") ? true : false;

                            Horario.Descricao = padrao || (Horario.Descricao.StartsWith("Carga Semanal - ") || Horario.Descricao.StartsWith("Carga Diária - ")) ? $"{Horario.Entrada}-{(!String.IsNullOrEmpty(Horario.EntradaIntervalo) ? ($"{Horario.EntradaIntervalo}-{Horario.SaidaIntervalo}-{Horario.Saida}") : ($"Sem Intervalo-{Horario.Saida}"))}" : Horario.Descricao;

                            HorarioAlterar.Descricao = Horario.Descricao;
                            HorarioAlterar.NDescontarIntervalo = Horario.NDescontarIntervalo;
                            HorarioAlterar.Entrada = Horario.Entrada;
                            HorarioAlterar.EntradaIntervalo = Horario.EntradaIntervalo;
                            HorarioAlterar.Saida = Horario.Saida;
                            HorarioAlterar.SaidaIntervalo = Horario.SaidaIntervalo;
                            HorarioAlterar.TotaldoIntervalo = Horario.TotaldoIntervalo;
                            HorarioAlterar.TotaldoPeriodo1 = Horario.TotaldoPeriodo1;
                            HorarioAlterar.TotaldoPeriodo2 = Horario.TotaldoPeriodo2;
                            HorarioAlterar.TotalJornada = Horario.TotalJornada;
                            HorarioAlterar.VintqHoras = Horario.VintqHoras;
                            HorarioAlterar.ViradadoDia = Horario.ViradadoDia;
                            HorarioAlterar.Carga = Horario.Carga;
                            HorarioAlterar.CargaSemanal = !String.IsNullOrEmpty(CargaSemanal);

                            HorarioAlterar.Tipo = Horarios.tipo.Fixo;

                            HorarioAlterar.IntervalosAuxiliares.Clear();

                            int contador = 1;

                            foreach (var IntervloMemoria in ListaMemoria.Lista.Where(x => x.SessionID == Session.SessionID).OrderBy(x => x.Obj.Order))
                            {
                                if (IntervloMemoria.Obj.Tipo == ObjIntervalosAuxiliares.tipo.Fixo)
                                {
                                    Bank.IntervalosAuxiliares.Add(new IntervalosAuxiliares
                                    {
                                        Order = contador,
                                        Inicio = IntervloMemoria.Obj.Inicio,
                                        Fim = IntervloMemoria.Obj.Fim,
                                        Tipo = IntervalosAuxiliares.tipo.Fixo,
                                        IDIntervalo = Bank.IntervalosAuxiliares.OrderBy(x => x.IDIntervalo).LastOrDefault() == null ? 1 : Bank.IntervalosAuxiliares.OrderBy(x => x.IDIntervalo).LastOrDefault().IDIntervalo + 1,
                                        IDEmpresa = HorarioAlterar.IDEmpresa,
                                        IDHorario = HorarioAlterar.IDHorario,
                                        Horarios = HorarioAlterar,
                                        Empresa = HorarioAlterar.Empresa
                                    });

                                }
                                else
                                {
                                    Bank.IntervalosAuxiliares.Add(new IntervalosAuxiliares
                                    {
                                        Order = contador,
                                        Carga = IntervloMemoria.Obj.Carga,
                                        Tipo = IntervalosAuxiliares.tipo.Carga,
                                        IDIntervalo = Bank.IntervalosAuxiliares.OrderBy(x => x.IDIntervalo).LastOrDefault() == null ? 1 : Bank.IntervalosAuxiliares.OrderBy(x => x.IDIntervalo).LastOrDefault().IDIntervalo + 1,
                                        IDEmpresa = HorarioAlterar.IDEmpresa,
                                        IDHorario = HorarioAlterar .IDHorario,
                                        Horarios = HorarioAlterar ,
                                        Empresa = HorarioAlterar.Empresa
                                    });
                                }

                                Bank.SaveChanges();
                                contador++;
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
            else
            {
                try
                {
                    Horario.CargaSemanal = !String.IsNullOrEmpty(CargaSemanal);

                    if ((Horario.CargaSemanal && CalculosdeHora.Tempo_Minuto(new string[] { Horario.Carga })[0] > 10080) || (!Horario.CargaSemanal && CalculosdeHora.Tempo_Minuto(new string[] { Horario.Carga })[0] > 1440))
                    {
                        throw new Exception("Carga máxima excedida");
                    }

                    bool padrao = String.IsNullOrEmpty(Horario.Descricao) ? true : (Horario.Descricao.Split('-').Count() == 4 && Horario.Descricao.Split('-')[0].Split(':').Count() == 2 && Horario.Descricao.Split('-')[1].Split(':').Count() == 2 && Horario.Descricao.Split('-')[2].Split(':').Count() == 2 && Horario.Descricao.Split('-')[3].Split(':').Count() == 2 && Horario.Descricao.Count() == 23) || (Horario.Descricao.Split('-').Count() == 3 && Horario.Descricao.Split('-')[1] == "Sem Intervalo") ? true : false;

                    Horarios HorarioAlterar = Bank.Horarios.Include(x => x.IntervalosAuxiliares).First(x => x.IDHorario == id && Session["Empresa"].ToString().Contains(x.IDEmpresa.ToString("0000")));

                    HorarioAlterar.NDescontarIntervalo = Horario.NDescontarIntervalo;
                    HorarioAlterar.Entrada = Horario.Entrada;
                    HorarioAlterar.EntradaIntervalo = Horario.EntradaIntervalo;
                    HorarioAlterar.Saida = Horario.Saida;
                    HorarioAlterar.SaidaIntervalo = Horario.SaidaIntervalo;
                    HorarioAlterar.TotaldoIntervalo = Horario.TotaldoIntervalo;
                    HorarioAlterar.TotaldoPeriodo1 = Horario.TotaldoPeriodo1;
                    HorarioAlterar.TotaldoPeriodo2 = Horario.TotaldoPeriodo2;
                    HorarioAlterar.TotalJornada = Horario.TotalJornada;
                    HorarioAlterar.VintqHoras = Horario.VintqHoras;
                    HorarioAlterar.ViradadoDia = Horario.ViradadoDia;
                    HorarioAlterar.Carga = Horario.Carga;
                    HorarioAlterar.CargaSemanal = !String.IsNullOrEmpty(CargaSemanal);
                    HorarioAlterar.Tipo = Horarios.tipo.Carga;
                    HorarioAlterar.Descricao = padrao || (Horario.Descricao.StartsWith("Carga Semanal - ") || Horario.Descricao.StartsWith("Carga Diária - ")) ? Horario.CargaSemanal ? "Carga Semanal - " + Horario.Carga : "Carga Diária - " + Horario.Carga : Horario.Descricao;

                    HorarioAlterar.IntervalosAuxiliares.Clear();

                    int contador = 1;

                    foreach (var IntervloMemoria in ListaMemoria.Lista.Where(x => x.SessionID == Session.SessionID).OrderBy(x=>x.Obj.Order))
                    {
                        if (IntervloMemoria.Obj.Tipo == ObjIntervalosAuxiliares.tipo.Fixo)
                        {
                            Bank.IntervalosAuxiliares.Add(new IntervalosAuxiliares
                            {
                                Order = contador,
                                Inicio = IntervloMemoria.Obj.Inicio,
                                Fim = IntervloMemoria.Obj.Fim,
                                Tipo = IntervalosAuxiliares.tipo.Fixo,
                                IDIntervalo = Bank.IntervalosAuxiliares.OrderBy(x => x.IDIntervalo).LastOrDefault() == null ? 1 : Bank.IntervalosAuxiliares.OrderBy(x => x.IDIntervalo).LastOrDefault().IDIntervalo + 1,
                                IDEmpresa = HorarioAlterar.IDEmpresa,
                                IDHorario = HorarioAlterar.IDHorario,
                                Horarios = HorarioAlterar,
                                Empresa = HorarioAlterar.Empresa
                            });

                        }
                        else
                        {
                            Bank.IntervalosAuxiliares.Add(new IntervalosAuxiliares
                            {
                                Order = contador,
                                Carga = IntervloMemoria.Obj.Carga,
                                Tipo = IntervalosAuxiliares.tipo.Carga,
                                IDIntervalo = Bank.IntervalosAuxiliares.OrderBy(x => x.IDIntervalo).LastOrDefault() == null ? 1 : Bank.IntervalosAuxiliares.OrderBy(x => x.IDIntervalo).LastOrDefault().IDIntervalo + 1,
                                IDEmpresa = HorarioAlterar.IDEmpresa,
                                IDHorario = HorarioAlterar.IDHorario,
                                Horarios = HorarioAlterar,
                                Empresa = HorarioAlterar.Empresa
                            });
                        }

                        Bank.SaveChanges();
                        contador++;
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

        [HttpGet]
        [VerificaLogin]
        public ActionResult CadastroHorarios_Remover(int id)
        {
            Horarios Horario = Bank.Horarios.AsNoTracking().First(x => x.IDHorario == id && Session["Empresa"].ToString().Contains(x.IDEmpresa.ToString("0000")));
            return PartialView(Horario);
        }

        [HttpPost]
        
        [VerificaLogin]
        public ActionResult CadastroHorarios_Remover_Salvar(int id)
        {
            try
            {
                Horarios Horario = Bank.Horarios.First(x => x.IDHorario == id && Session["Empresa"].ToString().Contains(x.IDEmpresa.ToString("0000")));
                Bank.Horarios.Remove(Horario);
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
        public ActionResult CadastroHorarios_Remover_Selecao(int[] id)
        {
            List<Horarios> Horarios = Bank.Horarios.AsNoTracking().Where(x => id.Contains(x.IDHorario) && Session["Empresa"].ToString().Contains(x.IDEmpresa.ToString("0000"))).ToList();
            return PartialView(Horarios);
        }

        [HttpPost]
        
        [VerificaLogin]
        public ActionResult CadastroHorarios_Remover_Selecao_Salvar(int[] id)
        {
            try
            {
                foreach (var IDHorario in id)
                {
                    Horarios Horario = Bank.Horarios.First(x => x.IDHorario == IDHorario && Session["Empresa"].ToString().Contains(x.IDEmpresa.ToString("0000")));
                    Bank.Horarios.Remove(Horario);
                }

                Bank.SaveChanges();
                return Json(new { status = true });
            }
            catch (Exception e)
            {
                return Json(new { status = false, msg = e.Message });
            }
        }

        [HttpGet]
        [VerificaLoad]
        public ActionResult GridIntervalos(string str, int? Remover)
        {
            try
            {
                if (!String.IsNullOrEmpty(str) && (str.Length == 11 || str.Length == 16) && str.Contains("/") && str.Contains(":"))
                {
                    if (str.Split('/')[1].ToLower() == "carga")
                    {
                        Lista.Add(new ListadeObjs
                        {
                            Obj = new ObjIntervalosAuxiliares
                            {
                                Carga = str.Split('/')[0],
                                Order = Lista.Where(x => x.SessionID == Session.SessionID).Count() > 0 ? Lista.OrderBy(x => x.Obj.Order).LastOrDefault(x => x.SessionID == Session.SessionID).Obj.Order + 1 : 1,
                                Tipo = ObjIntervalosAuxiliares.tipo.Carga
                            },
                            ID = Lista.Where(x => x.SessionID == Session.SessionID).Count() > 0 ? Lista.OrderBy(x => x.ID).LastOrDefault(x => x.SessionID == Session.SessionID).ID + 1 : 1,
                            SessionID = Session.SessionID                            
                        });
                    }else
                    {
                        Lista.Add(new ListadeObjs
                        {
                            Obj = new ObjIntervalosAuxiliares
                            {
                                Inicio = str.Split('/')[0].Split('-')[0],
                                Fim = str.Split('/')[0].Split('-')[1],
                                Order = Lista.Where(x => x.SessionID == Session.SessionID).Count() > 0 ? Lista.OrderBy(x => x.Obj.Order).LastOrDefault(x => x.SessionID == Session.SessionID).Obj.Order + 1 : 1,
                                Tipo = ObjIntervalosAuxiliares.tipo.Fixo
                            },
                            ID = Lista.Where(x => x.SessionID == Session.SessionID).Count() > 0 ? Lista.OrderBy(x => x.ID).LastOrDefault(x => x.SessionID == Session.SessionID).ID + 1 : 1,
                            SessionID = Session.SessionID
                        });
                    }
                }

                if (!String.IsNullOrEmpty(str) && str.ToLower() == "salvar")
                {
                    ListaMemoria.Lista.RemoveAll(x => x.SessionID == Session.SessionID);

                    foreach (var item in Lista.Where(x=>x.SessionID == Session.SessionID))
                    {
                        if (item.Obj.Tipo == ObjIntervalosAuxiliares.tipo.Carga)
                        {
                            ListaMemoria.Lista.Add(new ListadeObjs
                            {
                                Obj = new ObjIntervalosAuxiliares
                                {
                                    Carga = item.Obj.Carga,
                                    Tipo = ObjIntervalosAuxiliares.tipo.Carga,
                                    Order = item.Obj.Order
                                },
                                ID = item.ID,                                
                                SessionID = Session.SessionID
                            });
                        }
                        else
                        {
                            ListaMemoria.Lista.Add(new ListadeObjs
                            {
                                Obj = new ObjIntervalosAuxiliares
                                {
                                    Inicio = item.Obj.Inicio,
                                    Fim = item.Obj.Fim,
                                    Tipo = ObjIntervalosAuxiliares.tipo.Fixo,
                                    Order = item.Obj.Order
                                },
                                ID = item.ID,                               
                                SessionID = Session.SessionID
                            });
                        }                     
                    }
                }

                if (!String.IsNullOrEmpty(str) && str.ToLower() == "cancelar")
                {
                    Lista.RemoveAll(x => x.SessionID == Session.SessionID);

                    foreach (var item in ListaMemoria.Lista.Where(x => x.SessionID == Session.SessionID))
                    {
                        if (item.Obj.Tipo == ObjIntervalosAuxiliares.tipo.Carga)
                        {
                            Lista.Add(new ListadeObjs
                            {
                                Obj = new ObjIntervalosAuxiliares
                                {
                                    Carga = item.Obj.Carga,
                                    Tipo = ObjIntervalosAuxiliares.tipo.Carga,
                                    Order = item.Obj.Order
                                },
                                ID = item.ID,                                
                                SessionID = Session.SessionID
                            });
                        }
                        else
                        {
                            Lista.Add(new ListadeObjs
                            {
                                Obj = new ObjIntervalosAuxiliares
                                {
                                    Inicio = item.Obj.Inicio,
                                    Fim = item.Obj.Fim,
                                    Tipo = ObjIntervalosAuxiliares.tipo.Fixo,
                                    Order = item.Obj.Order
                                },
                                ID = item.ID,                                
                                SessionID = Session.SessionID
                            });
                        }
                    }

                }

                if (Remover != null)
                {
                    Lista.RemoveAll(x => x.SessionID == Session.SessionID && x.ID == Remover);
                }

                if (String.IsNullOrEmpty(str) && Remover == null)
                {
                    Lista.RemoveAll(x => x.SessionID == Session.SessionID);

                    foreach (var item in ListaMemoria.Lista.Where(x=>x.SessionID == Session.SessionID))
                    {
                        if (item.Obj.Tipo == ObjIntervalosAuxiliares.tipo.Carga)
                        {
                            Lista.Add(new ListadeObjs
                            {
                                Obj = new ObjIntervalosAuxiliares
                                {
                                    Carga = item.Obj.Carga,
                                    Tipo = ObjIntervalosAuxiliares.tipo.Carga,
                                    Order = item.Obj.Order
                                },
                                ID = item.ID,
                                SessionID = Session.SessionID
                            });
                        }
                        else
                        {
                            Lista.Add(new ListadeObjs
                            {
                                Obj = new ObjIntervalosAuxiliares
                                {
                                    Inicio = item.Obj.Inicio,
                                    Fim = item.Obj.Fim,
                                    Tipo = ObjIntervalosAuxiliares.tipo.Fixo,
                                    Order = item.Obj.Order
                                },
                                ID = item.ID,
                                SessionID = Session.SessionID
                            });
                        }
                    }
                    return PartialView(ListaMemoria.Lista.Where(x=>x.SessionID == Session.SessionID).OrderBy(x=>x.Obj.Order));
                }else
                {
                    return PartialView(Lista.Where(x => x.SessionID == Session.SessionID).OrderBy(x => x.Obj.Order));
                }
            }
            catch
            {
                return new HttpUnauthorizedResult();
            }
            
        }

        [HttpGet]
        [VerificaLoad]
        public ActionResult MudaOrdem(int id, string sentido)
        {
            try
            {
                if (id != 0)
                {
                    if (String.IsNullOrEmpty(sentido))
                    {
                        return new HttpUnauthorizedResult();
                    }

                    int ordem = 0;

                    if (sentido.ToLower() == "down")
                    {
                       

                        ListadeObjs objselecionado = Lista.FirstOrDefault(x => x.SessionID == Session.SessionID && x.ID == id);

                        ListadeObjs objtrocado = Lista.OrderBy(x => x.Obj.Order).FirstOrDefault(x => x.SessionID == Session.SessionID && x.Obj.Order > objselecionado.Obj.Order);

                        if (objtrocado != null && !(objselecionado.Obj.Tipo == ObjIntervalosAuxiliares.tipo.Fixo && objtrocado.Obj.Tipo == ObjIntervalosAuxiliares.tipo.Fixo))
                        {
                            ordem = objtrocado.Obj.Order;

                            objtrocado.Obj.Order = objselecionado.Obj.Order;

                            objselecionado.Obj.Order = ordem;
                        }                        
                    }

                    if (sentido.ToLower() == "up")
                    {
                        
                        ListadeObjs objselecionado = Lista.FirstOrDefault(x => x.SessionID == Session.SessionID && x.ID == id);

                        ListadeObjs objtrocado = Lista.OrderByDescending(x=>x.Obj.Order).FirstOrDefault(x => x.SessionID == Session.SessionID && x.Obj.Order < objselecionado.Obj.Order);

                        if (objtrocado != null && !(objselecionado.Obj.Tipo == ObjIntervalosAuxiliares.tipo.Fixo && objtrocado.Obj.Tipo == ObjIntervalosAuxiliares.tipo.Fixo))
                        {
                            ordem = objtrocado.Obj.Order; 

                            objtrocado.Obj.Order = objselecionado.Obj.Order;

                            objselecionado.Obj.Order = ordem;
                        }                        
                    }
                }


                return PartialView("GridIntervalos", Lista.Where(x => x.SessionID == Session.SessionID).OrderBy(x => x.Obj.Order).ToList());
            }
            catch
            {
                return new HttpNotFoundResult();
            }            
        }
    }
}