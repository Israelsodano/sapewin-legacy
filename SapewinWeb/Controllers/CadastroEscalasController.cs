using Microsoft.EntityFrameworkCore;
using SapewinWeb.Metodos;
using SapewinWeb.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Web.Mvc;

namespace SapewinWeb.Controllers
{
  
    public class CadastroEscalasController : Controller
    {
        Login.Models.LoginModel BankLogin = new Login.Models.LoginModel();
        MyContext Bank = new MyContext();

        [HttpGet]
        [VerificaLogin]
        public ActionResult CadastroEscalas_Abrir()
        {
            Login.Models.LoginSistema UsuarioLogado = BankLogin
                .LoginSistema.AsNoTracking()
                .FirstOrDefault(x => x.IDLoginsistema == Convert.ToInt32(Session["Usuario"].ToString())); //carrega usuario
            
            ViewBag.PermissoesIndices = UsuarioLogado.PerfiMaster
                ? Bank.FuncoesdeTelas.AsNoTracking().Select(x => x.IDFuncaoTela).ToList()
                : Bank.PermissoesdeTelas.AsNoTracking().Where(x => x.IDUsuario == UsuarioLogado.IDLoginsistema && Session["Empresa"].ToString().Contains(x.IDEmpresa.ToString("0000"))).Select(x => x.IDFuncaoTela).ToList(); // carrega a lista de funçoes de telas que o usuario possui

            List<Escalas> Escalas = Bank.Escalas.Where(x => Session["Empresa"].ToString().Contains(x.IDEmpresa.ToString("0000"))).ToList();

            int Total = Escalas.Count;

            Escalas = Escalas.Count < 10 ? Escalas : Escalas.GetRange(0, 10); // monta pagina

            ViewBag.Paginas = Math.Ceiling(Convert.ToDecimal(Convert.ToDouble(Total) / 10)); // conta paginas

            ViewBag.Total = $"Registro(s): {Total} - Exibindo de {(Escalas.Count > 0 ? 1 : 0)} a {Escalas.Count} - {ViewBag.Paginas} Página(s)"; //monta string do footer            

            return VerificaLoad.IsAjax(HttpContext.Request) ? PartialView(Escalas) : (ViewResultBase)View(Escalas);
        }

        [HttpGet]
        [VerificaLoad]
        public ActionResult CadastroEscalas_Abrir_Grid(String pesquisa, int Range, int Pagina, String Order, bool Condicao)
        {
            if (String.IsNullOrEmpty(pesquisa)) { pesquisa = ""; }

            pesquisa = pesquisa.ToLower().Replace('+', ' ');

            Login.Models.LoginSistema UsuarioLogado = BankLogin
                .LoginSistema
                .FirstOrDefault(x => x.IDLoginsistema == Convert.ToInt32(Session["Usuario"].ToString())); // carrega usuario logado

            ViewBag.PermissoesIndices = UsuarioLogado.PerfiMaster
             ? Bank.FuncoesdeTelas.AsNoTracking().Select(x => x.IDFuncaoTela).ToList()
             : Bank.PermissoesdeTelas.AsNoTracking().Where(x => x.IDUsuario == UsuarioLogado.IDLoginsistema && Session["Empresa"].ToString().Contains(x.IDEmpresa.ToString("0000"))).Select(x => x.IDFuncaoTela).ToList(); //carreha lista de funçoes de telas qiue o usuario possui

            List<Escalas> Escalas = new List<Models.Escalas>();

            switch (Order.ToUpper())
            {
                case "CODIGO - UP":

                    if (Condicao || pesquisa == "")
                    {
                        Escalas = Bank
                       .Escalas.AsNoTracking()
                       .Where(x => (Session["Empresa"].ToString().Contains(x.IDEmpresa.ToString("0000")))
                       && (x.IDEscala.ToString().Contains(pesquisa) || x.Descricao.ToLower().Contains(pesquisa.ToLower()))).OrderBy(x => x.IDEscala).ToList();
                    }
                    else
                    {
                        Escalas = Bank
                       .Escalas.AsNoTracking()
                       .Where(x => (Session["Empresa"].ToString().Contains(x.IDEmpresa.ToString("0000")))
                       && (!x.IDEscala.ToString().Contains(pesquisa) && !x.Descricao.ToLower().Contains(pesquisa.ToLower()))).OrderBy(x => x.IDEscala).ToList();
                    }
                    break;

                case "CODIGO-DOWN":

                    if (Condicao || pesquisa == "")
                    {
                        Escalas = Bank
                      .Escalas.AsNoTracking()
                      .Where(x => (Session["Empresa"].ToString().Contains(x.IDEmpresa.ToString("0000")))
                      && (x.IDEscala.ToString().Contains(pesquisa) || x.Descricao.ToLower().Contains(pesquisa.ToLower()))).OrderByDescending(x => x.IDEscala).ToList();
                    }
                    else
                    {
                        Escalas = Bank
                      .Escalas.AsNoTracking()
                      .Where(x => (Session["Empresa"].ToString().Contains(x.IDEmpresa.ToString("0000")))
                      && (!x.IDEscala.ToString().Contains(pesquisa) && !x.Descricao.ToLower().Contains(pesquisa.ToLower()))).OrderByDescending(x => x.IDEscala).ToList();
                    }
                    break;

                case "DESCRICAO-UP":

                    if (Condicao || pesquisa == "")
                    {
                        Escalas = Bank
                      .Escalas.AsNoTracking()
                      .Where(x => (Session["Empresa"].ToString().Contains(x.IDEmpresa.ToString("0000")))
                      && (x.IDEscala.ToString().Contains(pesquisa) || x.Descricao.ToLower().Contains(pesquisa.ToLower()))).OrderBy(x => x.Descricao).ToList();
                    }
                    else
                    {
                        Escalas = Bank
                      .Escalas.AsNoTracking()
                      .Where(x => (Session["Empresa"].ToString().Contains(x.IDEmpresa.ToString("0000")))
                      && (!x.IDEscala.ToString().Contains(pesquisa) && !x.Descricao.ToLower().Contains(pesquisa.ToLower()))).OrderBy(x => x.Descricao).ToList();
                    }
                    break;

                case "DESCRICAO-DOWN":

                    if (Condicao || pesquisa == "")
                    {
                        Escalas = Bank
                     .Escalas.AsNoTracking()
                     .Where(x => (Session["Empresa"].ToString().Contains(x.IDEmpresa.ToString("0000")))
                     && (x.IDEscala.ToString().Contains(pesquisa) || x.Descricao.ToLower().Contains(pesquisa.ToLower()))).OrderByDescending(x => x.Descricao).ToList();
                    }
                    else
                    {
                        Escalas = Bank
                     .Escalas.AsNoTracking()
                     .Where(x => (Session["Empresa"].ToString().Contains(x.IDEmpresa.ToString("0000")))
                     && (!x.IDEscala.ToString().Contains(pesquisa) && !x.Descricao.ToLower().Contains(pesquisa.ToLower()))).OrderByDescending(x => x.Descricao).ToList();
                    }
                    break;

                case "TIPO-UP":

                    if (Condicao || pesquisa == "")
                    {
                        Escalas = Bank
                      .Escalas.AsNoTracking()
                      .Where(x => (Session["Empresa"].ToString().Contains(x.IDEmpresa.ToString("0000")))
                      && (x.IDEscala.ToString().Contains(pesquisa) || x.Descricao.ToLower().Contains(pesquisa.ToLower()))).OrderBy(x => x.Tipo).ToList();
                    }
                    else
                    {
                        Escalas = Bank
                      .Escalas.AsNoTracking()
                      .Where(x => (Session["Empresa"].ToString().Contains(x.IDEmpresa.ToString("0000")))
                      && (!x.IDEscala.ToString().Contains(pesquisa) && !x.Descricao.ToLower().Contains(pesquisa.ToLower()))).OrderBy(x => x.Tipo).ToList();
                    }
                    break;

                case "TIPO-DOWN":

                     if (Condicao || pesquisa == "")
                    {
                        Escalas = Bank
                       .Escalas.AsNoTracking()
                       .Where(x => (Session["Empresa"].ToString().Contains(x.IDEmpresa.ToString("0000")))
                       && (x.IDEscala.ToString().Contains(pesquisa) || x.Descricao.ToLower().Contains(pesquisa.ToLower()))).OrderByDescending(x => x.Tipo).ToList();
                    }
                    else
                    {
                        Escalas = Bank
                       .Escalas.AsNoTracking()
                       .Where(x => (Session["Empresa"].ToString().Contains(x.IDEmpresa.ToString("0000")))
                       && (!x.IDEscala.ToString().Contains(pesquisa) && !x.Descricao.ToLower().Contains(pesquisa.ToLower()))).OrderByDescending(x => x.Tipo).ToList();
                    }
                    break;

                default:
                    break;
            }           

            int Total = Escalas.Count;

            ViewBag.Paginas = Math.Ceiling(Convert.ToDecimal(Convert.ToDouble(Total) / Range)); // conta paginas 

            Pagina = Pagina > ViewBag.Paginas ? Convert.ToInt32(ViewBag.Paginas) - 1 : Pagina; // trata pagina para nn ser maior qu o numero maximop possivel de paginas dentro da lista de objs

            Pagina = Escalas.Count < Range ? Pagina * Escalas.Count : Pagina * Range; // trata pagina para ser o endereço do primeiro registro da pagina do obj

            Range = Range + Pagina > Escalas.Count ? Escalas.Count - Pagina : Range; // trata range para que nn seja maior que o tamanho do obj 

            Escalas = Escalas.Count < Range ? Escalas : Escalas.GetRange(Pagina, Range); // monta pagina

            ViewBag.Total = $"Registro(s): {Total} - Exibindo de {(Pagina == 0 ? 1 : Pagina + 1)} a {(Pagina == 0 ? Range : Pagina + Range)} - { ViewBag.Paginas} Página(s)"; // monta string do footer

            return PartialView(Escalas);
        }

        [HttpGet]
        [VerificaLogin]
        public ActionResult CadastroEscalas_Incluir()
        {
            ListaMemoriaHorarios.Lista.RemoveAll(x => x.SessionID == Session.SessionID);
            ListaMemoriaHorarios.ListaCargaSemanal.RemoveAll(x => x.SessionID == Session.SessionID);
            List<Horarios> Horarios = Bank.Horarios.AsNoTracking().Where(x => Session["Empresa"].ToString().Contains(x.IDEmpresa.ToString("0000")) && !x.CargaSemanal && x.Tipo == Models.Horarios.tipo.Fixo).ToList();            
            return PartialView(Horarios);
        }

        [HttpPost]
        
        [VerificaLogin]
        public ActionResult CadastroEscalas_Incluir_Salvar(int? id, int?[] Semana, int? tipo, string Descricao, int? ViradaSemana, DateTime? Inicio)
        {
            try
            {
                if (String.IsNullOrEmpty(Descricao))
                {
                    throw new Exception("Preencha a descrição da Escala");
                }

                if (id.HasValue && Bank.Escalas.Where(x=> Session["Empresa"].ToString().Contains(x.IDEmpresa.ToString("0000")) && x.IDEscala == id.Value).Count() > 0)
                {
                    throw new Exception("Já existe uma Escala com esse código");
                }

                if (tipo == 1)
                {
                    for (int i = 0; i < Semana.Length; i++)
                    {
                        if (!Semana[i].HasValue)
                        {
                            throw new Exception("Horario nao correspondente");
                        }

                        if (Semana[i] != 0 && Semana[i] != -1 && Semana[i] != -2)
                        {
                            if (!Bank.Horarios.Where(x => Session["Empresa"].ToString().Contains(x.IDEmpresa.ToString("0000"))).Select(x => x.IDHorario).Contains(Semana[i].Value))
                            {
                                throw new Exception("Horario nao correspondente");
                            }
                        }
                    }

                    Escalas Escala = new Escalas
                    {
                        DataInicio = CalculosdeHora.RetornaSegundadaSemana(), 
                        Descricao = Descricao,
                        IDEmpresa = Bank.Empresas.FirstOrDefault(x => Session["Empresa"].ToString().Contains(x.IDEmpresa.ToString("0000"))).IDEmpresa,
                        IDEscala = id != null ? Convert.ToInt32(id) : Bank.Escalas.Where(x=> Session["Empresa"].ToString().Contains(x.IDEmpresa.ToString("0000"))).Count() > 0 ? Bank.Escalas.Where(x => Session["Empresa"].ToString().Contains(x.IDEmpresa.ToString("0000"))).OrderBy(x=>x.IDEscala).LastOrDefault().IDEscala + 1 : 1,
                        Tipo = Escalas.tipo.Semanal,
                        ViradaSemana = ViradaSemana == 1 ? Escalas.viradaSemana.Segunda : ViradaSemana == 2 ? Escalas.viradaSemana.Terca : ViradaSemana == 3 ? Escalas.viradaSemana.Quarta : ViradaSemana == 4 ? Escalas.viradaSemana.Quinta : ViradaSemana == 5 ? Escalas.viradaSemana.Sexta : ViradaSemana == 6 ? Escalas.viradaSemana.Sabado : ViradaSemana == 7 ? Escalas.viradaSemana.Domingo : Escalas.viradaSemana.Domingo 
                    }; 
                    
                    for (int i = 0; i < Semana.Length; i++)
                    {
                       Bank.EscalasHorarios.Add(new Models.EscalasHorarios
                       {
                           IDEscala = Escala.IDEscala,
                           IDEmpresa = Escala.IDEmpresa,
                           IDHorario = (Semana[i] == 0 ? "Folga" : Semana[i] == -1 ? "Sabado" : Semana[i] == -2 ? "Domingo" : Semana[i].ToString()),
                           Ordem = i,
                           QuantidadedeDias = 1
                       });
                    }

                    Bank.Escalas.Add(Escala);                    
                                       
                }

                if (tipo == 2)
                {
                    if (Inicio == null)
                    {
                        throw new Exception("Preencha a data de Inicio da Escala de Revezamento");
                    }

                    if (ListaMemoriaHorarios.Lista.Where(x => x.SessionID == Session.SessionID).OrderBy(x => x.Order).Count() == 0)
                    {
                        throw new Exception("Nenhum Horario Cadastrado");
                    }

                    Escalas Escala = new Models.Escalas
                    {
                        DataInicio = Inicio.Value,
                        Descricao = Descricao,
                        IDEmpresa = Bank.Empresas.FirstOrDefault(x => Session["Empresa"].ToString().Contains(x.IDEmpresa.ToString("0000"))).IDEmpresa,
                        IDEscala = id != null ? Convert.ToInt32(id) : Bank.Escalas.Where(x => Session["Empresa"].ToString().Contains(x.IDEmpresa.ToString("0000"))).Count() > 0 ? Bank.Escalas.Where(x => Session["Empresa"].ToString().Contains(x.IDEmpresa.ToString("0000"))).OrderBy(x => x.IDEscala).LastOrDefault().IDEscala + 1 : 1,
                        Tipo = Escalas.tipo.Revezamento,
                        ViradaSemana = ViradaSemana == 1 ? Escalas.viradaSemana.Segunda : ViradaSemana == 2 ? Escalas.viradaSemana.Terca : ViradaSemana == 3 ? Escalas.viradaSemana.Quarta : ViradaSemana == 4 ? Escalas.viradaSemana.Quinta : ViradaSemana == 5 ? Escalas.viradaSemana.Sexta : ViradaSemana == 6 ? Escalas.viradaSemana.Sabado : ViradaSemana == 7 ? Escalas.viradaSemana.Domingo : Escalas.viradaSemana.Domingo
                    };

                    int contador = 1;

                    foreach (var item in ListaMemoriaHorarios.Lista.Where(x=>x.SessionID == Session.SessionID).OrderBy(x=>x.Order))
                    {
                        Bank.EscalasHorarios.Add(new Models.EscalasHorarios
                        {
                            IDEscala = Escala.IDEscala,
                            IDEmpresa = Escala.IDEmpresa,
                            IDHorario = (item.ID == 0 ? "Folga" : item.ID == -1 ? "Sabado" : item.ID == -2 ? "Domingo" : item.ID.ToString()),
                            Ordem = contador,
                            QuantidadedeDias = item.Dias
                        });

                        contador++;
                    }

                    Bank.Escalas.Add(Escala);
                    
                }

                if (tipo == 3)
                {
                    if (ListaMemoriaHorarios.ListaCargaSemanal.Where(x => x.SessionID == Session.SessionID).OrderBy(x => x.Order).Count() == 0)
                    {
                        throw new Exception("Nenhum Horario Cadastrado");
                    }

                    Escalas Escala = new Escalas
                    {
                        DataInicio = CalculosdeHora.RetornaSegundadaSemana(),
                        Descricao = Descricao,
                        IDEmpresa = Bank.Empresas.FirstOrDefault(x => Session["Empresa"].ToString().Contains(x.IDEmpresa.ToString("0000"))).IDEmpresa,
                        IDEscala = id != null ? Convert.ToInt32(id) : Bank.Escalas.Where(x => Session["Empresa"].ToString().Contains(x.IDEmpresa.ToString("0000"))).Count() > 0 ? Bank.Escalas.Where(x => Session["Empresa"].ToString().Contains(x.IDEmpresa.ToString("0000"))).OrderBy(x => x.IDEscala).LastOrDefault().IDEscala + 1 : 1,
                        Tipo = Escalas.tipo.CargaSemanal,
                        ViradaSemana = ViradaSemana == 1 ? Escalas.viradaSemana.Segunda : ViradaSemana == 2 ? Escalas.viradaSemana.Terca : ViradaSemana == 3 ? Escalas.viradaSemana.Quarta : ViradaSemana == 4 ? Escalas.viradaSemana.Quinta : ViradaSemana == 5 ? Escalas.viradaSemana.Sexta : ViradaSemana == 6 ? Escalas.viradaSemana.Sabado : ViradaSemana == 7 ? Escalas.viradaSemana.Domingo : Escalas.viradaSemana.Domingo,
                    };

                    Bank.Escalas.Add(Escala);

                    int contador = 1;

                    foreach (var item in ListaMemoriaHorarios.ListaCargaSemanal.Where(x=>x.SessionID == Session.SessionID).OrderBy(x=>x.Order))
                    {
                        string d = "";
                        for (int i = 0; i < item.Dias.ToArray().Length; i++)
                        {
                          
                            d += +item.Dias.ToArray()[i] + (i == item.Dias.ToArray().Length - 1 ? "" : ";");
                                                        
                        }

                        Bank.EscalasHorarios.Add(new EscalasHorarios
                        {
                            Dias = d,
                            IDEmpresa = Escala.IDEmpresa,
                            IDEscala = Escala.IDEscala,
                            IDHorario = item.ID.ToString(),
                            Ordem = contador,
                            DiaInicio = item.DiaInicio,
                            Direto = item.Direto,
                            HoradeEntrada = item.HoradeEntrada                                       
                        });

                        contador++;
                    }
                }

                ListaMemoriaHorarios.Lista.RemoveAll(x => x.SessionID == Session.SessionID);

                ListaMemoriaHorarios.ListaCargaSemanal.RemoveAll(x => x.SessionID == Session.SessionID);

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
        public ActionResult CadastroEscalas_Alterar(int id)
        {
            try
            {
                ListaMemoriaHorarios.Lista.RemoveAll(x => x.SessionID == Session.SessionID);

                ListaMemoriaHorarios.ListaCargaSemanal.RemoveAll(x => x.SessionID == Session.SessionID);

                Escalas Escala = Bank.Escalas.Include(x=>x.EscalasHorarios).FirstOrDefault(x => Session["Empresa"].ToString().Contains(x.IDEmpresa.ToString("0000")) && x.IDEscala == id);

                foreach (var item in Escala.EscalasHorarios.ToList())
                {
                    if (Escala.Tipo == Escalas.tipo.Revezamento)
                    {
                        ListaMemoriaHorarios.Lista.Add(new ObjHorarios
                        {
                            ID = item.IDHorario == "Folga" ? 0 : item.IDHorario == "Sabado" ? -1 : item.IDHorario == "Domingo" ? -2 : Convert.ToInt32(item.IDHorario),
                            Dias = item.QuantidadedeDias,
                            IDRef = ListaMemoriaHorarios.Lista.Where(x => x.SessionID == Session.SessionID).Count() > 0 ? ListaMemoriaHorarios.Lista.OrderBy(x => x.IDRef).LastOrDefault(x => x.SessionID == Session.SessionID).IDRef + 1 : 1,
                            Order = ListaMemoriaHorarios.Lista.Where(x => x.SessionID == Session.SessionID).Count() > 0 ? ListaMemoriaHorarios.Lista.OrderBy(x => x.Order).LastOrDefault(x => x.SessionID == Session.SessionID).Order + 1 : 1,
                            SessionID = Session.SessionID,

                        });
                    }

                    if (Escala.Tipo == Escalas.tipo.CargaSemanal)
                    {
                        ObjHorariosCargaSemanal obj = new ObjHorariosCargaSemanal
                        {
                            ID = Convert.ToInt32(item.IDHorario),
                            IDRef = ListaMemoriaHorarios.ListaCargaSemanal.Where(x => x.SessionID == Session.SessionID).Count() > 0 ? ListaMemoriaHorarios.ListaCargaSemanal.OrderBy(x => x.IDRef).Last(x => x.SessionID == Session.SessionID).IDRef + 1 : 1,
                            Order = ListaMemoriaHorarios.ListaCargaSemanal.Where(x => x.SessionID == Session.SessionID).Count() > 0 ? ListaMemoriaHorarios.ListaCargaSemanal.OrderBy(x => x.Order).Last(x => x.SessionID == Session.SessionID).Order + 1 : 1,
                            SessionID = Session.SessionID,
                            Dias = new List<int>(),
                            DiaInicio = item.DiaInicio,
                            Direto = item.Direto,
                            HoradeEntrada = item.HoradeEntrada
                        };

                        if (item.Dias != "")
                        {
                            for (int i = 0; i < item.Dias.Split(';').Length; i++)
                            {
                                obj.Dias.Add(Convert.ToInt32(item.Dias.Split(';')[i]));
                            }
                        }
                        

                        ListaMemoriaHorarios.ListaCargaSemanal.Add(obj);
                    }
                }

                return PartialView(Escala);
            }
            catch
            {
                return new HttpUnauthorizedResult();               
            }          
        }

        [HttpPost]
        
        [VerificaLogin]
        public ActionResult CadastroEscalas_Alterar_Salvar(int id, int?[] Semana, int? tipo, string Descricao, int? ViradaSemana, DateTime? Inicio)
        {
            try
            {
                if (String.IsNullOrEmpty(Descricao))
                {
                    throw new Exception("Preencha a descrição da Escala");
                }

                Escalas Escala = Bank.Escalas.Include(x => x.EscalasHorarios).FirstOrDefault(x => Session["Empresa"].ToString().Contains(x.IDEmpresa.ToString("0000")) && x.IDEscala == id);

                if (tipo == 1)
                {
                    for (int i = 0; i < Semana.Length; i++)
                    {
                        if (!Semana[i].HasValue)
                        {
                            throw new Exception("Horario nao correspondente");
                        }

                        if (Semana[i] != 0 && Semana[i] != -1 && Semana[i] != -2)
                        {
                            if (!Bank.Horarios.Where(x => Session["Empresa"].ToString().Contains(x.IDEmpresa.ToString("0000"))).Select(x => x.IDHorario).Contains(Semana[i].Value))
                            {
                                throw new Exception("Horario nao correspondente");
                            }
                        }
                    }
                   
                    Escala.Tipo = Escalas.tipo.Semanal;
                    Escala.Descricao = Descricao;
                    Escala.ViradaSemana = ViradaSemana == 1 ? Escalas.viradaSemana.Segunda : ViradaSemana == 2 ? Escalas.viradaSemana.Terca : ViradaSemana == 3 ? Escalas.viradaSemana.Quarta : ViradaSemana == 4 ? Escalas.viradaSemana.Quinta : ViradaSemana == 5 ? Escalas.viradaSemana.Sexta : ViradaSemana == 6 ? Escalas.viradaSemana.Sabado : ViradaSemana == 7 ? Escalas.viradaSemana.Domingo : Escalas.viradaSemana.Domingo;
                    Escala.EscalasHorarios.Clear();
                    Bank.SaveChanges();

                    for (int i = 0; i < Semana.Length; i++)
                    {
                        Bank.EscalasHorarios.Add(new Models.EscalasHorarios
                        {
                            IDEscala = Escala.IDEscala,
                            IDEmpresa = Escala.IDEmpresa,
                            IDHorario = (Semana[i] == 0 ? "Folga" : Semana[i] == -1 ? "Sabado" : Semana[i] == -2 ? "Domingo" : Semana[i].ToString()),
                            Ordem = i,
                            QuantidadedeDias = 1
                        });
                    }
                }

                if (tipo == 2)
                {
                    if (Inicio == null)
                    {
                        throw new Exception("Preencha a data de Inicio da Escala de Revezamento");
                    }

                    if (ListaMemoriaHorarios.Lista.Where(x => x.SessionID == Session.SessionID).OrderBy(x => x.Order).Count() == 0)
                    {
                        throw new Exception("Nenhum Horario Cadastrado");
                    }

                    Escala.DataInicio = Inicio.Value;
                    Escala.Tipo = Escalas.tipo.Revezamento;
                    Escala.Descricao = Descricao;
                    Escala.ViradaSemana = ViradaSemana == 1 ? Escalas.viradaSemana.Segunda : ViradaSemana == 2 ? Escalas.viradaSemana.Terca : ViradaSemana == 3 ? Escalas.viradaSemana.Quarta : ViradaSemana == 4 ? Escalas.viradaSemana.Quinta : ViradaSemana == 5 ? Escalas.viradaSemana.Sexta : ViradaSemana == 6 ? Escalas.viradaSemana.Sabado : ViradaSemana == 7 ? Escalas.viradaSemana.Domingo : Escalas.viradaSemana.Domingo;
                    Escala.EscalasHorarios.Clear();
                    Bank.SaveChanges();

                    int contador = 1;

                    foreach (var item in ListaMemoriaHorarios.Lista.Where(x => x.SessionID == Session.SessionID).OrderBy(x => x.Order))
                    {
                        Bank.EscalasHorarios.Add(new Models.EscalasHorarios
                        {
                            IDEscala = Escala.IDEscala,
                            IDEmpresa = Escala.IDEmpresa,
                            IDHorario = (item.ID == 0 ? "Folga" : item.ID == -1 ? "Sabado" : item.ID == -2 ? "Domingo" : item.ID.ToString()),
                            Ordem = contador,
                            QuantidadedeDias = item.Dias
                        });

                        contador++;
                    }
                }

                if (tipo == 3)
                {

                    if (ListaMemoriaHorarios.ListaCargaSemanal.Where(x => x.SessionID == Session.SessionID).OrderBy(x => x.Order).Count() == 0)
                    {
                        throw new Exception("Nenhum Horario Cadastrado");
                    }

                    Escala.Descricao = Descricao;
                    Escala.Tipo = Escalas.tipo.CargaSemanal;
                    Escala.ViradaSemana = ViradaSemana == 1 ? Escalas.viradaSemana.Segunda : ViradaSemana == 2 ? Escalas.viradaSemana.Terca : ViradaSemana == 3 ? Escalas.viradaSemana.Quarta : ViradaSemana == 4 ? Escalas.viradaSemana.Quinta : ViradaSemana == 5 ? Escalas.viradaSemana.Sexta : ViradaSemana == 6 ? Escalas.viradaSemana.Sabado : ViradaSemana == 7 ? Escalas.viradaSemana.Domingo : Escalas.viradaSemana.Domingo;
                    Escala.DataInicio = CalculosdeHora.RetornaSegundadaSemana();
                    Escala.EscalasHorarios.Clear();
                    Bank.SaveChanges();
                                   
                    int contador = 1;

                    foreach (var item in ListaMemoriaHorarios.ListaCargaSemanal.Where(x => x.SessionID == Session.SessionID).OrderBy(x => x.Order))
                    {
                        string d = "";

                        if (item.Dias != null)
                        {
                            for (int i = 0; i < item.Dias.ToArray().Length; i++)
                            {

                                d += +item.Dias.ToArray()[i] + (i == item.Dias.ToArray().Length - 1 ? "" : ";");

                            }
                        }                       

                        Bank.EscalasHorarios.Add(new EscalasHorarios
                        {
                            Dias = d,
                            IDEmpresa = Escala.IDEmpresa,
                            IDEscala = Escala.IDEscala,
                            IDHorario = item.ID.ToString(),
                            Ordem = contador,
                            DiaInicio = item.DiaInicio,
                            Direto = item.Direto,
                            HoradeEntrada = item.HoradeEntrada,                            
                            
                        });

                        contador++;
                    }
                }

                ListaMemoriaHorarios.Lista.RemoveAll(x => x.SessionID == Session.SessionID);

                ListaMemoriaHorarios.ListaCargaSemanal.RemoveAll(x => x.SessionID == Session.SessionID);

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
        public ActionResult GridHorarios(int? id, int ? Remover, int? dias)
        {
            List<Horarios> Horarios = new List<Models.Horarios>();            

            if (id != null && dias != null) // Adiciona
            { 
                if (Bank.Horarios.Where(x => Session["Empresa"].ToString().Contains(x.IDEmpresa.ToString("0000")) && x.IDHorario == id).Count() > 0 || (id == 0 || id == -1 || id == -2))
                {
                    ListaMemoriaHorarios.Lista.Add(new ObjHorarios
                    {
                        ID = id.Value,
                        SessionID = Session.SessionID,
                        Dias = dias.Value,
                        IDRef = ListaMemoriaHorarios.Lista.Where(x=>x.SessionID == Session.SessionID).Count() > 0 ? ListaMemoriaHorarios.Lista.Where(x => x.SessionID == Session.SessionID).OrderBy(x=>x.IDRef).LastOrDefault().IDRef + 1 : 1,
                        Order = ListaMemoriaHorarios.Lista.Where(x => x.SessionID == Session.SessionID).Count() > 0 ? ListaMemoriaHorarios.Lista.Where(x => x.SessionID == Session.SessionID).OrderBy(x => x.Order).LastOrDefault().Order + 1 : 1,
                    });
                }
            }

            if (Remover != null) //Remove
            {
                ListaMemoriaHorarios.Lista.RemoveAll(x => x.SessionID == Session.SessionID && x.IDRef == Remover.Value);
            }

            foreach (var item in ListaMemoriaHorarios.Lista.Where(x=>x.SessionID == Session.SessionID))
            {
                Horarios Hora = new Horarios();

                if (item.ID == 0)
                {
                    Hora = new Horarios
                    {
                        Descricao = "Folga",
                        IDHorario = 0,                       
                    };
                }else if (item.ID == -1)
                {
                    Hora = new Horarios
                    {
                        Descricao = "Sabado",
                        IDHorario = -1,                        
                    };
                }else if (item.ID == -2)
                {
                    Hora = new Horarios
                    {
                        Descricao = "Domingo",
                        IDHorario = -2,                       
                    };
                }else
                {
                    Hora = Bank.Horarios.AsNoTracking().FirstOrDefault(x => Session["Empresa"].ToString().Contains(x.IDEmpresa.ToString("0000")) && x.IDHorario == item.ID);
                }
                                
                Hora.Dias = item.Dias;
                Hora.IDRef = item.IDRef;
                Hora.Order = item.Order;
                Horarios.Add(Hora);
            }

            return PartialView(Horarios.OrderBy(x=>x.Order).ToList());
        }

        [HttpGet]
        [VerificaLoad]
        public ActionResult GridHorariosCargaSemanal(int? id, int? Remover, int? Diainicio, string HoraEntrada, bool? Direto)
        {
            try
            {
                List<Horarios> Horarios = new List<Models.Horarios>();

                if (id.HasValue)
                {
                    Horarios Horario = Bank.Horarios.First(x => Session["Empresa"].ToString().Contains(x.IDEmpresa.ToString("0000")) && x.IDHorario == id);

                    ListaMemoriaHorarios.ListaCargaSemanal.Add(new ObjHorariosCargaSemanal
                    {
                        IDRef = ListaMemoriaHorarios.ListaCargaSemanal.OrderBy(x=>x.IDRef).LastOrDefault(x => x.SessionID == Session.SessionID) != null ? ListaMemoriaHorarios.ListaCargaSemanal.OrderBy(x=>x.IDRef).LastOrDefault(x => x.SessionID == Session.SessionID).IDRef + 1 : 1,
                        ID = id.Value,
                        Order = ListaMemoriaHorarios.ListaCargaSemanal.OrderBy(x=>x.Order).LastOrDefault(x => x.SessionID == Session.SessionID) != null ? ListaMemoriaHorarios.ListaCargaSemanal.OrderBy(x=>x.Order).LastOrDefault(x => x.SessionID == Session.SessionID).Order + 1 : 1,
                        SessionID = Session.SessionID,
                        DiaInicio = Diainicio == 1 ? EscalasHorarios.diainicio.Segunda : Diainicio == 2 ? EscalasHorarios.diainicio.Terca : Diainicio == 3 ? EscalasHorarios.diainicio.Quarta : Diainicio == 4 ? EscalasHorarios.diainicio.Quinta : Diainicio == 5 ? EscalasHorarios.diainicio.Sexta : Diainicio == 6 ? EscalasHorarios.diainicio.Sabado : Diainicio == 7 ? EscalasHorarios.diainicio.Domingo : EscalasHorarios.diainicio.Segunda,
                        HoradeEntrada = HoraEntrada,
                        Direto = Direto.HasValue ? Direto.Value: false

                    });                   
                   
                }

                if (Remover.HasValue)
                {
                    ListaMemoriaHorarios.ListaCargaSemanal.RemoveAll(x => x.SessionID == Session.SessionID && x.IDRef == Remover);
                }


                foreach (var item in ListaMemoriaHorarios.ListaCargaSemanal.Where(x => x.SessionID == Session.SessionID))
                {
                    Horarios.Add(new Models.Horarios
                    {
                        Carga = Bank.Horarios.First(x => Session["Empresa"].ToString().Contains(x.IDEmpresa.ToString("0000")) && x.IDHorario == item.ID).Carga,
                        Descricao = Bank.Horarios.First(x => Session["Empresa"].ToString().Contains(x.IDEmpresa.ToString("0000")) && x.IDHorario == item.ID).Descricao,
                        IDHorario = item.ID,
                        IDRef = item.IDRef,
                        Order = item.Order,
                        DiaInicio = item.DiaInicio,
                        Direto = item.Direto,
                        HoradeEntrada = item.HoradeEntrada,
                    });
                }

                return PartialView(Horarios.OrderBy(x=>x.Order).ToList());
            }
            catch
            {
                return new HttpUnauthorizedResult();
            }            
        }

        [HttpGet]
        [VerificaLogin]
        public ActionResult CadastroEscalas_Remover_Selecao(int[] id)
        {
            List<Escalas> Escalas = Bank.Escalas.Where(x => Session["Empresa"].ToString().Contains(x.IDEmpresa.ToString("0000")) && id.Contains(x.IDEscala)).ToList();
            return PartialView(Escalas);
        }

        [HttpPost]
        
        [VerificaLogin]
        public ActionResult CadastroEscalas_Remover_Selecao_Salvar(int[] id)
        {
            try
            {
                List<Escalas> Escalas = Bank.Escalas.Where(x => Session["Empresa"].ToString().Contains(x.IDEmpresa.ToString("0000")) && id.Contains(x.IDEscala)).ToList();

                foreach (var item in Escalas)
                {
                    Bank.Escalas.Remove(item);
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
        [VerificaLoad]
        public ActionResult MudaOrdemCargaSemanal(int id, string sentido)
        {
            try
            {
                List<Horarios> Horarios = new List<Models.Horarios>();

                int ordem = 0;

                if (sentido.ToLower() == "up")
                {
                    ObjHorariosCargaSemanal objSelecionado = ListaMemoriaHorarios.ListaCargaSemanal.FirstOrDefault(x => x.SessionID == Session.SessionID && x.IDRef == id);
                    ObjHorariosCargaSemanal objTrocado = ListaMemoriaHorarios.ListaCargaSemanal.OrderByDescending(x=>x.Order).FirstOrDefault(x => x.SessionID == Session.SessionID && x.Order < objSelecionado.Order);
                    ordem = objTrocado.Order;
                    objTrocado.Order = objSelecionado.Order;
                    objSelecionado.Order = ordem;
                }

                if (sentido.ToLower() == "down")
                {
                    ObjHorariosCargaSemanal objSelecionado = ListaMemoriaHorarios.ListaCargaSemanal.FirstOrDefault(x => x.SessionID == Session.SessionID && x.IDRef == id);
                    ObjHorariosCargaSemanal objTrocado = ListaMemoriaHorarios.ListaCargaSemanal.OrderBy(x => x.Order).FirstOrDefault(x => x.SessionID == Session.SessionID && x.Order > objSelecionado.Order);
                    ordem = objTrocado.Order;
                    objTrocado.Order = objSelecionado.Order;
                    objSelecionado.Order = ordem;
                }

                foreach (var item in ListaMemoriaHorarios.ListaCargaSemanal.Where(x => x.SessionID == Session.SessionID))
                {
                    Horarios.Add(new Models.Horarios
                    {
                        Carga = Bank.Horarios.First(x => Session["Empresa"].ToString().Contains(x.IDEmpresa.ToString("0000")) && x.IDHorario == item.ID).Carga,
                        Descricao = Bank.Horarios.First(x => Session["Empresa"].ToString().Contains(x.IDEmpresa.ToString("0000")) && x.IDHorario == item.ID).Descricao,
                        IDHorario = item.ID,
                        IDRef = item.IDRef,
                        Order = item.Order,
                        DiaInicio = item.DiaInicio,
                        Direto = item.Direto,
                        HoradeEntrada = item.HoradeEntrada,
                    });
                }

                return PartialView("GridHorariosCargaSemanal", Horarios.OrderBy(x=>x.Order).ToList());
            }
            catch
            {
                return new HttpUnauthorizedResult();
            }
        }

        [HttpGet]
        [VerificaLoad]
        public ActionResult GridFolgas(int id)
        {
            try
            {               
                return PartialView(ListaMemoriaHorarios.ListaCargaSemanal.First(x=>x.SessionID == Session.SessionID && x.IDRef == id));
            }
            catch
            {
                return new HttpUnauthorizedResult();

            }
        }

        [HttpPost]
        [VerificaLoad]
        public ActionResult AddFolga(bool[] Folgas, int id)
        {
            if (Folgas.Length == 7)
            {
                try
                {
                    ObjHorariosCargaSemanal obj = ListaMemoriaHorarios.ListaCargaSemanal.First(x => x.SessionID == Session.SessionID && x.IDRef == id);
                    obj.Dias = new List<int>();
                    for (int i = 0; i < Folgas.Length; i++)
                    {
                        if (Folgas[i])
                        {
                            
                            obj.Dias.Add(i + 1);
                        }
                    }

                    return Json(new { status = true });
                }
                catch
                {
                    return Json(new { status = false });
                }
               
            }
            else
            {
                return Json(new { status = false });
            }
        }

        [HttpGet]
        [VerificaLoad]
        public ActionResult MudaOrdem(int id, string sentido)
        {
            try
            {
                List<Horarios> Horarios = new List<Models.Horarios>();

                if (id != 0)
                {
                    
                    int ordem = 0;

                    if (sentido.ToLower() == "up")
                    {
                        
                        ObjHorarios objSelecionado = ListaMemoriaHorarios.Lista.FirstOrDefault(x => x.SessionID == Session.SessionID && x.IDRef == id);
                        ObjHorarios ObjTrocado = ListaMemoriaHorarios.Lista.OrderByDescending(x => x.Order).FirstOrDefault(x => x.SessionID == Session.SessionID && x.Order < objSelecionado.Order);
                        ordem = ObjTrocado.Order;
                        ObjTrocado.Order = objSelecionado.Order;
                        objSelecionado.Order = ordem;
                    }

                    if (sentido == "down")
                    {
                        
                        ObjHorarios objSelecionado = ListaMemoriaHorarios.Lista.FirstOrDefault(x => x.SessionID == Session.SessionID && x.IDRef == id);
                        ObjHorarios ObjTrocado = ListaMemoriaHorarios.Lista.OrderBy(x => x.Order).FirstOrDefault(x => x.SessionID == Session.SessionID && x.Order > objSelecionado.Order);
                        ordem = ObjTrocado.Order;
                        ObjTrocado.Order = objSelecionado.Order;
                        objSelecionado.Order = ordem;
                    }                    
                }

                foreach (var item in ListaMemoriaHorarios.Lista.Where(x=>x.SessionID == Session.SessionID))
                {
                    Horarios Hora = new Horarios();

                    if (item.ID == 0)
                    {
                        Hora = new Horarios
                        {
                            Descricao = "Folga",
                            IDHorario = 0,
                        };
                    }
                    else if (item.ID == -1)
                    {
                        Hora = new Horarios
                        {
                            Descricao = "Sabado",
                            IDHorario = -1,
                        };
                    }
                    else if (item.ID == -2)
                    {
                        Hora = new Horarios
                        {
                            Descricao = "Domingo",
                            IDHorario = -2,
                        };
                    }
                    else
                    {
                        Hora = Bank.Horarios.AsNoTracking().FirstOrDefault(x => Session["Empresa"].ToString().Contains(x.IDEmpresa.ToString("0000")) && x.IDHorario == item.ID);
                    }

                    Hora.Dias = item.Dias;
                    Hora.IDRef = item.IDRef;
                    Hora.Order = item.Order;
                    Horarios.Add(Hora);
                }

                return PartialView("GridHorarios", Horarios.OrderBy(x => x.Order).ToList());
            }
            catch
            {
                return new HttpNotFoundResult();
            }
        }

        [HttpGet]
        [VerificaLogin]
        public ActionResult CadastroEscalas_Remover(int id)
        {
            Escalas Escala = Bank.Escalas.FirstOrDefault(x => Session["Empresa"].ToString().Contains(x.IDEmpresa.ToString()) && x.IDEscala == id);
            return PartialView(Escala);
        }

        [HttpPost]
        
        [VerificaLogin]
        public ActionResult CadastroEscalas_Remover_Salvar(int id)
        {
            try
            {
                Escalas Escala = Bank.Escalas.FirstOrDefault(x => Session["Empresa"].ToString().Contains(x.IDEmpresa.ToString()) && x.IDEscala == id);

                Bank.Escalas.Remove(Escala);

                Bank.SaveChanges();

                return Json(new { status = true });
            }
            catch(Exception e)
            {
                return Json(new { status = false, msg = e.Message });
            }
            
        }

        [HttpPost]
        [VerificaLoad]
        public ActionResult CadastroEscalas_Persquisadia(DateTime date, int id)
        {
            try
            { 
                Escalas Escala = Bank.Escalas.Include(x => x.EscalasHorarios).FirstOrDefault(x => Session["Empresa"].ToString().Contains(x.IDEmpresa.ToString("0000")) && x.IDEscala == id);

                bool Depois = date >= Escala.DataInicio;

                long dias = Depois ? Convert.ToInt64(date.Subtract(Escala.DataInicio).TotalDays) : Convert.ToInt64(Escala.DataInicio.Subtract(date).TotalDays);
                              

                int QtdEscalasHorarios = Escala.EscalasHorarios.Count;

                long diasusados = 0;
                for (int i = 0; i < QtdEscalasHorarios; i++)
                {
                    diasusados += Escala.EscalasHorarios.OrderBy(x => x.Ordem).ToArray()[i].QuantidadedeDias;
                }

                long resto = dias % diasusados;

                int idx = 0;

                if (resto == 0 && !Depois)
                {
                    idx = Escala.EscalasHorarios.Count - 1;
                }

                    while (resto > 0)
                {
                    resto -= Depois ? Escala.EscalasHorarios.OrderBy(x => x.Ordem).ToArray()[idx].QuantidadedeDias : Escala.EscalasHorarios.OrderByDescending(x => x.Ordem).ToArray()[idx].QuantidadedeDias;

                    if (idx == QtdEscalasHorarios)
                    {
                        idx = 0;
                    }

                    if ((resto >= 0 && Depois) || (resto > 0 && !Depois))
                    {
                       idx++;
                    }
                }

                var horario = Depois ? Escala.EscalasHorarios.OrderBy(x => x.Ordem).ToArray()[idx] : Escala.EscalasHorarios.OrderByDescending(x => x.Ordem).ToArray()[idx];

                if (horario.IDHorario == "Folga" || horario.IDHorario == "Sabado" || horario.IDHorario == "Domingo")
                {
                    return Json(new { status = true, horario = $"{horario.IDHorario} " });
                }
                else
                {
                    var Hrr = Bank.Horarios.FirstOrDefault(x => Session["Empresa"].ToString().Contains(x.IDEmpresa.ToString("0000")) && x.IDHorario == Convert.ToInt32(horario.IDHorario));

                    return Json(new { status = true, horario = $"{Hrr.IDHorario.ToString("0000")}-{Hrr.Descricao}" });
                }
            }
            catch(Exception e)
            {
                return Json(new { status = false, msg = e.Message });
            }
        }  

        [HttpPost]
        [VerificaLoad]
        public ActionResult CadastroEscalas_JsonHorarios()
        {
            return Json(new { Horarios = Bank.Horarios.Where(x=> Session["Empresa"].ToString().Contains(x.IDEmpresa.ToString("0000")) && !x.CargaSemanal).ToList() });
        }
    }
}