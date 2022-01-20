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
    public class CadastroParametrosController : Controller
    {
        Login.Models.LoginModel BankLogin = new Login.Models.LoginModel();
        MyContext Bank = new MyContext();

        [HttpGet]
        [VerificaLogin]
        public ActionResult CadastroParametros_Abrir()
        {
            Login.Models.LoginSistema UsuarioLogado = BankLogin
                .LoginSistema.AsNoTracking()
                .FirstOrDefault(x => x.IDLoginsistema == Convert.ToInt32(Session["Usuario"].ToString())); // carrega usuario logado           

            ViewBag.PermissoesIndices = UsuarioLogado.PerfiMaster
                ? Bank.FuncoesdeTelas.AsNoTracking().Select(x => x.IDFuncaoTela).ToList()
                : Bank.PermissoesdeTelas.AsNoTracking().Where(x => x.IDUsuario == UsuarioLogado.IDLoginsistema && Session["Empresa"].ToString().Contains(x.IDEmpresa.ToString("0000"))).Select(x => x.IDFuncaoTela).ToList(); //carreha lista de funçoes de telas qiue o usuario possui


            List<Parametros> Parametros = new List<Parametros>();                  

            int Total = Parametros.Count;

            Parametros = Parametros.Count < 10 ? Parametros : Parametros.GetRange(0, 10); // cria pagina 

            ViewBag.Paginas = Math.Ceiling(Convert.ToDecimal(Convert.ToDouble(Total) / 10)); //conta paginas

            ViewBag.Total = $"Registro(s): {Total} - Exibindo de {(Parametros.Count > 0 ? 1 : 0)} a {Parametros.Count} - {ViewBag.Paginas} Página(s)"; //monta string do footer

            return VerificaLoad.IsAjax(HttpContext.Request) ? PartialView(Parametros) : (ViewResultBase)View(Parametros);
        }

        [HttpGet]
        [VerificaLoad]
        public ActionResult CadastroParametros_Abrir_Grid(String pesquisa, int Range, int Pagina, String Order, bool Condicao)
        {
            if (String.IsNullOrEmpty(pesquisa)) { pesquisa = ""; }

            pesquisa = pesquisa.ToLower().Replace('+', ' ');

            Login.Models.LoginSistema UsuarioLogado = BankLogin
                .LoginSistema.AsNoTracking()
                .FirstOrDefault(x => x.IDLoginsistema == Convert.ToInt32(Session["Usuario"].ToString())); // carrega usuario logado

            ViewBag.PermissoesIndices = UsuarioLogado.PerfiMaster
             ? Bank.FuncoesdeTelas.AsNoTracking().Select(x => x.IDFuncaoTela).ToList()
             : Bank.PermissoesdeTelas.AsNoTracking().Where(x => x.IDUsuario == UsuarioLogado.IDLoginsistema && Session["Empresa"].ToString().Contains(x.IDEmpresa.ToString("0000"))).Select(x => x.IDFuncaoTela).ToList(); //carreha lista de funçoes de telas qiue o usuario possui

            List<Parametros> Parametros = new List<Models.Parametros>();

            switch (Order.ToUpper())
            {
                case "CODIGO-UP":

                    if (Condicao || pesquisa == "")
                    {
                        Parametros = Bank
                       .Parametros.AsNoTracking()
                       .Where(x => (Session["Empresa"].ToString().Contains(x.IDEmpresa.ToString("0000")))
                       && (x.IDParametro.ToString().Contains(pesquisa) || x.Nome.ToLower().Contains(pesquisa.ToLower()))).OrderBy(x => x.IDParametro).ToList();
                    }
                    else
                    {
                        Parametros = Bank
                       .Parametros.AsNoTracking()
                       .Where(x => (Session["Empresa"].ToString().Contains(x.IDEmpresa.ToString("0000")))
                       && (!x.IDParametro.ToString().Contains(pesquisa) && !x.Nome.ToLower().Contains(pesquisa.ToLower()))).OrderBy(x => x.IDParametro).ToList();
                    }
                    break;


                case "CODIGO-DOWN":

                    if (Condicao || pesquisa == "")
                    {
                        Parametros = Bank
                      .Parametros.AsNoTracking()
                      .Where(x => (Session["Empresa"].ToString().Contains(x.IDEmpresa.ToString("0000")))
                      && (x.IDParametro.ToString().Contains(pesquisa) || x.Nome.ToLower().Contains(pesquisa.ToLower()))).OrderByDescending(x => x.IDParametro).ToList();
                    }
                    else
                    {
                        Parametros = Bank
                      .Parametros.AsNoTracking()
                      .Where(x => (Session["Empresa"].ToString().Contains(x.IDEmpresa.ToString("0000")))
                      && (!x.IDParametro.ToString().Contains(pesquisa) && !x.Nome.ToLower().Contains(pesquisa.ToLower()))).OrderByDescending(x => x.IDParametro).ToList();
                    }
                    break;

                case "DESCRICAO-UP":

                    if (Condicao || pesquisa == "")
                    {
                        Parametros = Bank
                          .Parametros.AsNoTracking()
                          .Where(x => (Session["Empresa"].ToString().Contains(x.IDEmpresa.ToString("0000")))
                          && (x.IDParametro.ToString().Contains(pesquisa) || x.Nome.ToLower().Contains(pesquisa.ToLower()))).OrderBy(x => x.Nome).ToList();
                    }
                    else
                    {
                        Parametros = Bank
                           .Parametros.AsNoTracking()
                           .Where(x => (Session["Empresa"].ToString().Contains(x.IDEmpresa.ToString("0000")))
                           && (!x.IDParametro.ToString().Contains(pesquisa) && !x.Nome.ToLower().Contains(pesquisa.ToLower()))).OrderBy(x => x.Nome).ToList();
                    }
                    break;

                case "DESCRICAO-DOWN":

                    if (Condicao || pesquisa == "")
                    {
                        Parametros = Bank
                         .Parametros.AsNoTracking()
                         .Where(x => (Session["Empresa"].ToString().Contains(x.IDEmpresa.ToString("0000")))
                         && (x.IDParametro.ToString().Contains(pesquisa) || x.Nome.ToLower().Contains(pesquisa.ToLower()))).OrderByDescending(x => x.Nome).ToList();
                    }
                    else
                    {
                        Parametros = Bank
                     .Parametros.AsNoTracking()
                     .Where(x => (Session["Empresa"].ToString().Contains(x.IDEmpresa.ToString("0000")))
                     && (!x.IDParametro.ToString().Contains(pesquisa) && !x.Nome.ToLower().Contains(pesquisa.ToLower()))).OrderByDescending(x => x.Nome).ToList();
                    }
                    break;

                default:
                    break;
            }           

            int Total = Parametros.Count; // lista total

            ViewBag.Paginas = Math.Ceiling(Convert.ToDecimal(Convert.ToDouble(Total) / Range)); // conta paginas

            Pagina = Pagina > ViewBag.Paginas ? Convert.ToInt32(ViewBag.Paginas) - 1 : Pagina; // trata a pagina para nn ser maior que as paginas possiveis do obj

            Pagina = Parametros.Count < Range ? Pagina * Parametros.Count : Pagina * Range; // trata pagina para ser o primeiro registro da pagina

            Range = Range + Pagina > Parametros.Count ? Parametros.Count - Pagina : Range; // trata o range para nn ser maior que o range do obj

            Parametros = Parametros.Count < Range ? Parametros : Parametros.GetRange(Pagina, Range); // monta pagina

            ViewBag.Total = $"Registro(s): {Total} - Exibindo de {(Pagina == 0 ? 1 : Pagina + 1)} a {(Pagina == 0 ? Range : Pagina + Range)} - { ViewBag.Paginas} Página(s)"; // monta string do footer

            return PartialView(Parametros);
        }

        [HttpGet]
        [VerificaLogin]
        public ActionResult CadastroParametros_Incluir()
        {
            return PartialView();
        }

        [HttpGet]
        [VerificaLoad]
        public ActionResult DadosToleranciaIndividual(int? id)
        {
            return id.HasValue ? PartialView(Bank.Parametros.FirstOrDefault(x => x.IDParametro == id)) : PartialView();
        }

        [HttpGet]
        public ActionResult DadosToleranciaGeral(int? id)
        {
            return id.HasValue ? PartialView(Bank.Parametros.FirstOrDefault(x=>x.IDParametro == id)) : PartialView();
        }
        
        [HttpGet]
        [VerificaLoad]
        public ActionResult GridExtras(string[] Array)
        {
            if (!String.IsNullOrEmpty(Array[0]))
            {
                List<ObjExtras> ListadeExtras = new List<ObjExtras>();

                for (int i = 0; i < Array.Length; i++)
                {
                    ListadeExtras.Add(new ObjExtras {Horas = Array[i].Split('/')[0], Percent = Convert.ToInt32(Array[i].Split('/')[1]), Adicional = String.IsNullOrEmpty(Array[i].Split('/')[2]) ? new int? { } :Convert.ToInt32(Array[i].Split('/')[2]), Tipo = (Convert.ToInt32(Array[i].Split('/')[3]) == 1 ? ObjExtras.tipo.Uteis : Convert.ToInt32(Array[i].Split('/')[3]) == 2 ? ObjExtras.tipo.Sabados : Convert.ToInt32(Array[i].Split('/')[3]) == 3 ? ObjExtras.tipo.Domingos : Convert.ToInt32(Array[i].Split('/')[3]) == 4 ? ObjExtras.tipo.Feriados : ObjExtras.tipo.Folgas) });
                }

                return PartialView(ListadeExtras.OrderBy(x=>x.Horas));
            }else
            {
                return PartialView(new List<ObjExtras>());
            }            
        }

        [HttpPost]
        
        [VerificaLogin]
        public ActionResult CadastroParametros_Incluir_Salvar(int? TipoTolerancia, String AtrasoPeriodo1, String AtrasoPeriodo2, String AtrasoJornada, int? AtrasoTipo, String SaidaPeriodo1, String SaidaPeriodo2, String SaidaJornada, int? SaidaTipo, String ExtraPeriodo1, String ExtraPeriodo2, String ExtraIntervalo, String ExtraJornada, int? ExtraTipo, int? TolGeralTipo, String TolGeralPeriodo1, String TolGeralPeriodo2, String TolGeralIntervalo, String TolGeralJornada, bool PagaHrExtraemAdicionalNt, String AdicionalNoturnoInicio, String AdicionalNoturnoFim, float? AdicionalNoturnoCalculo, bool CalcHrextRealAdcNotAcHrNorAdcNot, bool CalcHrExtRelAdcNotAcHrExNor, bool PgHrAboCmAdcNot, String Acada, String AddHrr, String DsrSabado, String DsrDomingo, String DsrFeriado, String DsrFolga, bool DsrContrAutomatico, bool? DsrProporcionalHoras, bool DescontarDsrSemana, bool? DescDsrAnterioraFalta, String OcorrenciaSemanalDsr, String Nome, string[] Uteis, string[] Sabados, string[] Domingos, string[] Feriados, string[] Folgas, bool PgDiautilvirada, bool? DescIntervalo, bool SabadoUtil, bool DomingoUtil, bool FerAgregaDomingo, bool FolAgregaDomingo, int? AcimadexhorasDomingo, int AcimaHEDomingo, int AcimaHEFeriado, int? AcimadexhorasFeriado, int AcimaHEFolga, int? AcimadexhorasFolga, int TipodeTabela, bool MostrarIntervaloSeparado, bool AdcnFinaldoexpediente)
        {
            try
            {
                if (String.IsNullOrEmpty(Nome))
                {
                    throw new Exception("Este parametro não possui Descrição", new Exception { HelpLink = "Tolerancia" });
                }

                Parametros Parametro = new Parametros();

                Parametro.Nome = Nome;
                Parametro.IDParametro = Bank.Parametros.Where(x => Session["Empresa"].ToString().Contains(x.IDEmpresa.ToString("0000"))).OrderBy(x => x.IDParametro).LastOrDefault() == null ? 1 : Bank.Parametros.Where(x => Session["Empresa"].ToString().Contains(x.IDEmpresa.ToString("0000"))).OrderBy(x => x.IDParametro).LastOrDefault().IDParametro + 1;
                Parametro.IDEmpresa = Bank.Empresas.First(x => Session["Empresa"].ToString().Contains(x.IDEmpresa.ToString("0000"))).IDEmpresa;

                if (TipoTolerancia == 1)
                {
                    if (AtrasoTipo == 1)
                    {
                        
                        if (String.IsNullOrEmpty(AtrasoPeriodo1) || String.IsNullOrEmpty(AtrasoPeriodo2))
                        {
                            throw new Exception("Prencha os campos de Tolerância de Atrasos corretamente", new Exception { HelpLink = "Tolerancia" });
                        }

                        Parametro.TipodeAtraso = Parametros.Tipodeatraso.Período;

                        Parametro.AtrasoTotal1P = AtrasoPeriodo1;

                        Parametro.AtrasoTotal2P = AtrasoPeriodo2;
                    }
                    else
                    {
                        if (String.IsNullOrEmpty(AtrasoJornada))
                        {
                            throw new Exception("Prencha os campos de Tolerância de Atrasos corretamente", new Exception { HelpLink = "Tolerancia" });
                        }
                        Parametro.TipodeAtraso = Parametros.Tipodeatraso.Diario;

                        Parametro.AtrasoJornada = AtrasoJornada;
                    }

                    if (SaidaTipo == 1)
                    {
                        if (String.IsNullOrEmpty(SaidaPeriodo1) || String.IsNullOrEmpty(SaidaPeriodo2))
                        {
                            throw new Exception("Prencha os campos de Tolerância de Saída Antecipada corretamente", new Exception { HelpLink = "Tolerancia" });
                        }
                        Parametro.TipodeSaida = Parametros.Tipodesaida.Período;

                        Parametro.SaidaTotal1P = SaidaPeriodo1;

                        Parametro.SaidaTotal2P = SaidaPeriodo2;
                    }
                    else
                    {
                        if (String.IsNullOrEmpty(SaidaJornada))
                        {
                            throw new Exception("Prencha os campos de Tolerância de Atrasos corretamente", new Exception { HelpLink = "Tolerancia" });
                        }

                        Parametro.TipodeSaida = Parametros.Tipodesaida.Diario;

                        Parametro.SaidaTotal1P = SaidaJornada;
                    }

                    if (ExtraTipo == 1)
                    {
                        if (String.IsNullOrEmpty(ExtraPeriodo1) || String.IsNullOrEmpty(ExtraPeriodo2) || String.IsNullOrEmpty(ExtraIntervalo))
                        {
                            throw new Exception("Prencha os campos de Tolerância de Extra a Partir corretamente", new Exception { HelpLink = "Tolerancia" });
                        }

                        Parametro.TipoExtra = Parametros.Tipoextra.Período;

                        Parametro.ExtraTotal1P = ExtraPeriodo1;

                        Parametro.ExtraTotalIntervalo = ExtraIntervalo;

                        Parametro.ExtraTotal2P = ExtraPeriodo2;
                    }
                    else
                    {
                        if (String.IsNullOrEmpty(ExtraJornada))
                        {
                            throw new Exception("Prencha os campos de Tolerância de Extra a Partir corretamente", new Exception { HelpLink = "Tolerancia" });
                        }

                        Parametro.TipoExtra = Parametros.Tipoextra.Diario;
                    }

                    Parametro.TipodeTolerancia = Parametros.Tipodetolerancia.Individual;

                    Parametro.ExtraJornada = ExtraJornada;
                }
                else
                {
                    if (TolGeralTipo == 1)
                    {
                        if (String.IsNullOrEmpty(TolGeralPeriodo1) || String.IsNullOrEmpty(TolGeralPeriodo2) || String.IsNullOrEmpty(TolGeralIntervalo))
                        {
                            throw new Exception("Prencha os campos de Tolerância Geral corretamente", new Exception { HelpLink = "Tolerancia" });
                        }

                        Parametro.ToleranciaGeralTipo = Parametros.ToleranciaGeraltipo.Período;

                        Parametro.ToleranciaGeral1P = TolGeralPeriodo1;

                        Parametro.ToleranciaGeralIntervalo = TolGeralIntervalo;

                        Parametro.ToleranciaGeral2P = TolGeralPeriodo2;
                       
                    }else
                    {
                        if (String.IsNullOrEmpty(TolGeralJornada))
                        {
                            throw new Exception("Prencha os campos de Tolerância Geral corretamente", new Exception { HelpLink = "Tolerancia" });
                        }

                        Parametro.ToleranciaGeralTipo = Parametros.ToleranciaGeraltipo.Diario;

                        Parametro.ToleranciaGeralJornada = TolGeralJornada;
                    }

                    Parametro.TipodeTolerancia = Parametros.Tipodetolerancia.Geral;
                }

                if (String.IsNullOrEmpty(AdicionalNoturnoInicio) || String.IsNullOrEmpty(AdicionalNoturnoFim) || !AdicionalNoturnoCalculo.HasValue)
                {
                    throw new Exception("Prencha os campos obrigatórios de Adicional Noturno", new Exception { HelpLink = "AdicionalNoturno"});
                }

                Parametro.AdicionalNoturnoInicio = AdicionalNoturnoInicio;
                Parametro.AdicionalNoturnoFim = AdicionalNoturnoFim;
                Parametro.CalculoAdicional = AdicionalNoturnoCalculo.Value;

                Parametro.PgExtraAdcn = PagaHrExtraemAdicionalNt;
                Parametro.ExtraAdicionalMaisAdicional = CalcHrextRealAdcNotAcHrNorAdcNot;
                Parametro.ExtraAdicionalAcresNormais = CalcHrExtRelAdcNotAcHrExNor;
                Parametro.PagaAdcAbono = PgHrAboCmAdcNot;

                Parametro.ReduzidoaCada = Acada;
                Parametro.ReduzidoAdiciona = AddHrr;


                Parametro.DsrSabado = DsrSabado;
                Parametro.DsrDomingo = DsrDomingo;
                Parametro.DsrFeriado = DsrFeriado;
                Parametro.DsrFolga = DsrFolga;

                Parametro.ControleAutomaticoDsr = DsrContrAutomatico;
                Parametro.DsrProporcionalHoras = DsrProporcionalHoras;
                Parametro.DescontarDsrSemana = DescontarDsrSemana;
                Parametro.DescDsrAnterioraFalta = DescDsrAnterioraFalta;
                Parametro.OcorrenciaSemanalDsr = OcorrenciaSemanalDsr;

                if (Uteis == null || Sabados == null || Domingos == null || Feriados == null || Folgas == null)
                {
                    throw new Exception("Adicione ao menos um escalonamento de hora extra de cada tipo", new Exception { HelpLink = "Escalonamento" });
                }

                Parametro.TipodeTabela = TipodeTabela == 1 ? Parametros.Tipodetabela.Diario : TipodeTabela == 2 ? Parametros.Tipodetabela.Semanal : Parametros.Tipodetabela.Mensal;

                Parametro.PgDiautilvirada = PgDiautilvirada;
                Parametro.DescIntervalo = DescIntervalo;

                Parametro.SabadoUtil = SabadoUtil;
                Parametro.DomingoUtil = DomingoUtil;
                Parametro.FerAgregaDomingo = FerAgregaDomingo;
                Parametro.FolAgregaDomingo = FolAgregaDomingo;

                if (!AcimadexhorasDomingo.HasValue || !AcimadexhorasFeriado.HasValue || !AcimadexhorasFolga.HasValue)
                {
                    throw new Exception("Preencha todos os campos de hora extra acima de horas", new Exception { HelpLink = "Escalonamento"});
                }

                Parametro.AcimadexhorasDomingo = AcimadexhorasDomingo.Value;
                Parametro.AcimaHEDomingo = AcimaHEDomingo;
                Parametro.AcimadexhorasFeriado = AcimadexhorasFeriado.Value;
                Parametro.AcimaHEFeriado = AcimaHEFeriado;
                Parametro.AcimadexhorasFolga = AcimadexhorasFolga.Value;
                Parametro.AcimaHEFolga = AcimaHEFolga;
                Parametro.MostrarIntervaloSeparado = MostrarIntervaloSeparado;
                Parametro.AdcnFinaldoexpediente = AdcnFinaldoexpediente;


                Bank.Parametros.Add(Parametro);

                for (int i = 0; i < Uteis.Length; i++)
                {
                    Bank.EscalonamentodeHoraExtra.Add(new EscalonamentodeHoraExtra {IDEmpresa = Parametro.IDEmpresa, IDParametro = Parametro.IDParametro, Horas = Uteis[i].Split('/')[0], Porcentagem = Convert.ToInt32(Uteis[i].Split('/')[1]), Adicional = String.IsNullOrEmpty(Uteis[i].Split('/')[2]) ? new int? { } : Convert.ToInt32(Uteis[i].Split('/')[2]), Tipo = EscalonamentodeHoraExtra.tipo.Uteis });
                }

                for (int i = 0; i < Sabados.Length; i++)
                {
                    Bank.EscalonamentodeHoraExtra.Add(new EscalonamentodeHoraExtra { IDEmpresa = Parametro.IDEmpresa, IDParametro = Parametro.IDParametro, Horas = Sabados[i].Split('/')[0], Porcentagem = Convert.ToInt32(Sabados[i].Split('/')[1]), Adicional = String.IsNullOrEmpty(Sabados[i].Split('/')[2]) ? new int? { } : Convert.ToInt32(Sabados[i].Split('/')[2]), Tipo = EscalonamentodeHoraExtra.tipo.Sabados });
                }

                for (int i = 0; i < Domingos.Length; i++)
                {
                    Bank.EscalonamentodeHoraExtra.Add(new EscalonamentodeHoraExtra { IDEmpresa = Parametro.IDEmpresa, IDParametro = Parametro.IDParametro, Horas = Domingos[i].Split('/')[0], Porcentagem = Convert.ToInt32(Domingos[i].Split('/')[1]), Adicional = String.IsNullOrEmpty(Domingos[i].Split('/')[2]) ? new int? { } : Convert.ToInt32(Domingos[i].Split('/')[2]), Tipo = EscalonamentodeHoraExtra.tipo.Domingos });
                }

                for (int i = 0; i < Feriados.Length; i++)
                {
                    Bank.EscalonamentodeHoraExtra.Add(new EscalonamentodeHoraExtra { IDEmpresa = Parametro.IDEmpresa, IDParametro = Parametro.IDParametro, Horas = Feriados[i].Split('/')[0], Porcentagem = Convert.ToInt32(Feriados[i].Split('/')[1]), Adicional = String.IsNullOrEmpty(Feriados[i].Split('/')[2]) ? new int? { } : Convert.ToInt32(Feriados[i].Split('/')[2]), Tipo = EscalonamentodeHoraExtra.tipo.Feriados });
                }

                for (int i = 0; i < Folgas.Length; i++)
                {
                    Bank.EscalonamentodeHoraExtra.Add(new EscalonamentodeHoraExtra { IDEmpresa = Parametro.IDEmpresa, IDParametro = Parametro.IDParametro, Horas = Folgas[i].Split('/')[0], Porcentagem = Convert.ToInt32(Folgas[i].Split('/')[1]), Adicional = String.IsNullOrEmpty(Folgas[i].Split('/')[2]) ? new int? { } : Convert.ToInt32(Folgas[i].Split('/')[2]), Tipo = EscalonamentodeHoraExtra.tipo.Folgas });
                }

                Bank.SaveChanges();

                return Json(new { status = true });

            }
            catch (Exception e)
            {
                return Json(new { status = false, msg = e.Message, Tab = e.InnerException.HelpLink.ToUpper() });
            }
        }

        [HttpGet]
        [VerificaLogin]
        public ActionResult CadastroParametros_Alterar(int id)
        {
            Parametros Parametro = Bank.Parametros.Include(x=>x.EscalonamentodeHoraExtra).FirstOrDefault(x=>x.IDParametro == id);

            return PartialView(Parametro);
        }

        [HttpPost]
        
        [VerificaLogin]
        public ActionResult CadastroParametros_Alterar_Salvar(int id, int? TipoTolerancia, String AtrasoPeriodo1, String AtrasoPeriodo2, String AtrasoJornada, int? AtrasoTipo, String SaidaPeriodo1, String SaidaPeriodo2, String SaidaJornada, int? SaidaTipo, String ExtraPeriodo1, String ExtraPeriodo2, String ExtraIntervalo, String ExtraJornada, int? ExtraTipo, int? TolGeralTipo, String TolGeralPeriodo1, String TolGeralPeriodo2, String TolGeralIntervalo, String TolGeralJornada, bool PagaHrExtraemAdicionalNt, String AdicionalNoturnoInicio, String AdicionalNoturnoFim, float? AdicionalNoturnoCalculo, bool CalcHrextRealAdcNotAcHrNorAdcNot, bool CalcHrExtRelAdcNotAcHrExNor, bool PgHrAboCmAdcNot, String Acada, String AddHrr, String DsrSabado, String DsrDomingo, String DsrFeriado, String DsrFolga, bool DsrContrAutomatico, bool? DsrProporcionalHoras, bool DescontarDsrSemana, bool? DescDsrAnterioraFalta, String OcorrenciaSemanalDsr, String Nome, string[] Uteis, string[] Sabados, string[] Domingos, string[] Feriados, string[] Folgas, bool PgDiautilvirada, bool? DescIntervalo, bool SabadoUtil, bool DomingoUtil, bool FerAgregaDomingo, bool FolAgregaDomingo, int? AcimadexhorasDomingo, int AcimaHEDomingo, int AcimaHEFeriado, int? AcimadexhorasFeriado, int AcimaHEFolga, int? AcimadexhorasFolga, int TipodeTabela, bool MostrarIntervaloSeparado, bool AdcnFinaldoexpediente)
        {
            try
            {
                if (String.IsNullOrEmpty(Nome))
                {
                    throw new Exception("Este parametro não possui Descrição");
                }

                Parametros Parametro = Bank.Parametros.Include(x => x.EscalonamentodeHoraExtra).FirstOrDefault(x => x.IDParametro == id);

                Parametro.Nome = Nome;

                if (TipoTolerancia == 1)
                {
                    if (AtrasoTipo == 1)
                    {

                        if (String.IsNullOrEmpty(AtrasoPeriodo1) || String.IsNullOrEmpty(AtrasoPeriodo2))
                        {
                            throw new Exception("Prencha os campos de Tolerância de Atrasos corretamente", new Exception { HelpLink = "Tolerancia" });
                        }

                        Parametro.TipodeAtraso = Parametros.Tipodeatraso.Período;

                        Parametro.AtrasoTotal1P = AtrasoPeriodo1;

                        Parametro.AtrasoTotal2P = AtrasoPeriodo2;

                        Parametro.AtrasoJornada = null;
                    }
                    else
                    {
                        if (String.IsNullOrEmpty(AtrasoJornada))
                        {
                            throw new Exception("Prencha os campos de Tolerância de Atrasos corretamente", new Exception { HelpLink = "Tolerancia" });
                        }
                        Parametro.TipodeAtraso = Parametros.Tipodeatraso.Diario;

                        Parametro.AtrasoJornada = AtrasoJornada;

                        Parametro.AtrasoTotal1P = null;

                        Parametro.AtrasoTotal2P = null;
                    }

                    if (SaidaTipo == 1)
                    {
                        if (String.IsNullOrEmpty(SaidaPeriodo1) || String.IsNullOrEmpty(SaidaPeriodo2))
                        {
                            throw new Exception("Prencha os campos de Tolerância de Saída Antecipada corretamente", new Exception { HelpLink = "Tolerancia" });
                        }
                        Parametro.TipodeSaida = Parametros.Tipodesaida.Período;

                        Parametro.SaidaTotal1P = SaidaPeriodo1;

                        Parametro.SaidaTotal2P = SaidaPeriodo2;

                        Parametro.SaidaJornada = null;
                    }
                    else
                    {
                        if (String.IsNullOrEmpty(SaidaJornada))
                        {
                            throw new Exception("Prencha os campos de Tolerância de Atrasos corretamente", new Exception { HelpLink = "Tolerancia" });
                        }

                        Parametro.TipodeSaida = Parametros.Tipodesaida.Diario;

                        Parametro.SaidaJornada = SaidaJornada;

                        Parametro.SaidaTotal1P = null;

                        Parametro.SaidaTotal2P = null;
                    }

                    if (ExtraTipo == 1)
                    {
                        if (String.IsNullOrEmpty(ExtraPeriodo1) || String.IsNullOrEmpty(ExtraPeriodo2) || String.IsNullOrEmpty(ExtraIntervalo))
                        {
                            throw new Exception("Prencha os campos de Tolerância de Extra a Partir corretamente", new Exception { HelpLink = "Tolerancia" });
                        }

                        Parametro.TipoExtra = Parametros.Tipoextra.Período;

                        Parametro.ExtraTotal1P = ExtraPeriodo1;

                        Parametro.ExtraTotalIntervalo = ExtraIntervalo;

                        Parametro.ExtraTotal2P = ExtraPeriodo2;

                        Parametro.ExtraJornada = null;
                    }
                    else
                    {
                        if (String.IsNullOrEmpty(ExtraJornada))
                        {
                            throw new Exception("Prencha os campos de Tolerância de Extra a Partir corretamente", new Exception { HelpLink = "Tolerancia" });
                        }

                        Parametro.TipoExtra = Parametros.Tipoextra.Diario;

                        Parametro.ExtraJornada = ExtraJornada;

                        Parametro.ExtraTotal1P = null;

                        Parametro.ExtraTotalIntervalo = null;

                        Parametro.ExtraTotal2P = null;
                    }

                    Parametro.TipodeTolerancia = Parametros.Tipodetolerancia.Individual;

                    Parametro.ToleranciaGeralTipo = null;

                    Parametro.ToleranciaGeralJornada = null;

                    Parametro.ToleranciaGeral1P = null;

                    Parametro.ToleranciaGeralIntervalo = null;

                    Parametro.ToleranciaGeral2P = null;


                }
                else
                {
                    if (TolGeralTipo == 1)
                    {
                        if (String.IsNullOrEmpty(TolGeralPeriodo1) || String.IsNullOrEmpty(TolGeralPeriodo2) || String.IsNullOrEmpty(TolGeralIntervalo))
                        {
                            throw new Exception("Prencha os campos de Tolerância Geral corretamente", new Exception { HelpLink = "Tolerancia" });
                        }

                        Parametro.ToleranciaGeralTipo = Parametros.ToleranciaGeraltipo.Período;

                        Parametro.ToleranciaGeral1P = TolGeralPeriodo1;

                        Parametro.ToleranciaGeralIntervalo = TolGeralIntervalo;

                        Parametro.ToleranciaGeral2P = TolGeralPeriodo2;

                        Parametro.ToleranciaGeralJornada = null;

                    }
                    else
                    {
                        if (String.IsNullOrEmpty(TolGeralJornada))
                        {
                            throw new Exception("Prencha os campos de Tolerância Geral corretamente", new Exception { HelpLink = "Tolerancia" });
                        }

                        Parametro.ToleranciaGeralTipo = Parametros.ToleranciaGeraltipo.Diario;

                        Parametro.ToleranciaGeralJornada = TolGeralJornada;

                        Parametro.ToleranciaGeral1P = null;

                        Parametro.ToleranciaGeralIntervalo = null;

                        Parametro.ToleranciaGeral2P = null;
                    }

                    Parametro.TipodeTolerancia = Parametros.Tipodetolerancia.Geral;


                    Parametro.TipodeAtraso = null;

                    Parametro.AtrasoJornada = null;

                    Parametro.AtrasoTotal1P = null;

                    Parametro.AtrasoTotal2P = null;


                    Parametro.TipodeSaida = null;

                    Parametro.SaidaJornada = null;

                    Parametro.SaidaTotal1P = null;

                    Parametro.SaidaTotal2P = null;


                    Parametro.TipoExtra = null;

                    Parametro.ExtraJornada = null;

                    Parametro.ExtraTotal1P = null;

                    Parametro.ExtraTotalIntervalo = null;

                    Parametro.ExtraTotal2P = null;

                }

                if (String.IsNullOrEmpty(AdicionalNoturnoInicio) || String.IsNullOrEmpty(AdicionalNoturnoFim) || !AdicionalNoturnoCalculo.HasValue)
                {
                    throw new Exception("Prencha os campos obrigatórios de Adicional Noturno", new Exception { HelpLink = "AdicionalNoturno" });
                }

                Parametro.AdicionalNoturnoInicio = AdicionalNoturnoInicio;
                Parametro.AdicionalNoturnoFim = AdicionalNoturnoFim;
                Parametro.CalculoAdicional = AdicionalNoturnoCalculo.Value;

                Parametro.PgExtraAdcn = PagaHrExtraemAdicionalNt;
                Parametro.ExtraAdicionalMaisAdicional = CalcHrextRealAdcNotAcHrNorAdcNot;
                Parametro.ExtraAdicionalAcresNormais = CalcHrExtRelAdcNotAcHrExNor;
                Parametro.PagaAdcAbono = PgHrAboCmAdcNot;

                Parametro.ReduzidoaCada = Acada;
                Parametro.ReduzidoAdiciona = AddHrr;


                Parametro.DsrSabado = DsrSabado;
                Parametro.DsrDomingo = DsrDomingo;
                Parametro.DsrFeriado = DsrFeriado;
                Parametro.DsrFolga = DsrFolga;

                Parametro.ControleAutomaticoDsr = DsrContrAutomatico;
                Parametro.DsrProporcionalHoras = DsrProporcionalHoras;
                Parametro.DescontarDsrSemana = DescontarDsrSemana;
                Parametro.DescDsrAnterioraFalta = DescDsrAnterioraFalta;
                Parametro.OcorrenciaSemanalDsr = OcorrenciaSemanalDsr;

                if (Uteis == null || Sabados == null || Domingos == null || Feriados == null || Folgas == null)
                {
                    throw new Exception("Adicione ao menos um escalonamento de hora extra de cada tipo", new Exception { HelpLink = "Escalonamento" });
                }

                Parametro.TipodeTabela = TipodeTabela == 1 ? Parametros.Tipodetabela.Diario : TipodeTabela == 2 ? Parametros.Tipodetabela.Semanal : Parametros.Tipodetabela.Mensal;

                Parametro.PgDiautilvirada = PgDiautilvirada;
                Parametro.DescIntervalo = DescIntervalo;

                Parametro.SabadoUtil = SabadoUtil;
                Parametro.DomingoUtil = DomingoUtil;
                Parametro.FerAgregaDomingo = FerAgregaDomingo;
                Parametro.FolAgregaDomingo = FolAgregaDomingo;

                if (!AcimadexhorasDomingo.HasValue || !AcimadexhorasFeriado.HasValue || !AcimadexhorasFolga.HasValue)
                {
                    throw new Exception("Preencha todos os campos de hora extra acima de horas", new Exception { HelpLink = "Escalonamento" });
                }

                Parametro.AcimadexhorasDomingo = AcimadexhorasDomingo.Value;
                Parametro.AcimaHEDomingo = AcimaHEDomingo;
                Parametro.AcimadexhorasFeriado = AcimadexhorasFeriado.Value;
                Parametro.AcimaHEFeriado = AcimaHEFeriado;
                Parametro.AcimadexhorasFolga = AcimadexhorasFolga.Value;
                Parametro.AcimaHEFolga = AcimaHEFolga;
                Parametro.MostrarIntervaloSeparado = MostrarIntervaloSeparado;
                Parametro.AdcnFinaldoexpediente = AdcnFinaldoexpediente;

                Parametro.EscalonamentodeHoraExtra.Clear();

                Bank.SaveChanges();

                for (int i = 0; i < Uteis.Length; i++)
                {
                    Bank.EscalonamentodeHoraExtra.Add(new EscalonamentodeHoraExtra { IDEmpresa = Parametro.IDEmpresa, IDParametro = Parametro.IDParametro, Horas = Uteis[i].Split('/')[0], Porcentagem = Convert.ToInt32(Uteis[i].Split('/')[1]), Adicional = String.IsNullOrEmpty(Uteis[i].Split('/')[2]) ? new int? { } : Convert.ToInt32(Uteis[i].Split('/')[2]), Tipo = EscalonamentodeHoraExtra.tipo.Uteis });
                }

                for (int i = 0; i < Sabados.Length; i++)
                {
                    Bank.EscalonamentodeHoraExtra.Add(new EscalonamentodeHoraExtra { IDEmpresa = Parametro.IDEmpresa, IDParametro = Parametro.IDParametro, Horas = Sabados[i].Split('/')[0], Porcentagem = Convert.ToInt32(Sabados[i].Split('/')[1]), Adicional = String.IsNullOrEmpty(Sabados[i].Split('/')[2]) ? new int? { } : Convert.ToInt32(Sabados[i].Split('/')[2]), Tipo = EscalonamentodeHoraExtra.tipo.Sabados });
                }

                for (int i = 0; i < Domingos.Length; i++)
                {
                    Bank.EscalonamentodeHoraExtra.Add(new EscalonamentodeHoraExtra { IDEmpresa = Parametro.IDEmpresa, IDParametro = Parametro.IDParametro, Horas = Domingos[i].Split('/')[0], Porcentagem = Convert.ToInt32(Domingos[i].Split('/')[1]), Adicional = String.IsNullOrEmpty(Domingos[i].Split('/')[2]) ? new int? { } : Convert.ToInt32(Domingos[i].Split('/')[2]), Tipo = EscalonamentodeHoraExtra.tipo.Domingos });
                }

                for (int i = 0; i < Feriados.Length; i++)
                {
                    Bank.EscalonamentodeHoraExtra.Add(new EscalonamentodeHoraExtra { IDEmpresa = Parametro.IDEmpresa, IDParametro = Parametro.IDParametro, Horas = Feriados[i].Split('/')[0], Porcentagem = Convert.ToInt32(Feriados[i].Split('/')[1]), Adicional = String.IsNullOrEmpty(Feriados[i].Split('/')[2]) ? new int? { } : Convert.ToInt32(Feriados[i].Split('/')[2]), Tipo = EscalonamentodeHoraExtra.tipo.Feriados });
                }

                for (int i = 0; i < Folgas.Length; i++)
                {
                    Bank.EscalonamentodeHoraExtra.Add(new EscalonamentodeHoraExtra { IDEmpresa = Parametro.IDEmpresa, IDParametro = Parametro.IDParametro, Horas = Folgas[i].Split('/')[0], Porcentagem = Convert.ToInt32(Folgas[i].Split('/')[1]), Adicional = String.IsNullOrEmpty(Folgas[i].Split('/')[2]) ? new int? { } : Convert.ToInt32(Folgas[i].Split('/')[2]), Tipo = EscalonamentodeHoraExtra.tipo.Folgas });
                }

                Bank.SaveChanges();

                return Json(new { status = true });

            }
            catch (Exception e)
            {
                return Json(new { status = false, msg = e.Message, Tab = e.InnerException.HelpLink.ToUpper() });
            }
        }

        [HttpGet]
        [VerificaLogin]
        public ActionResult CadastroParametros_Remover(int id)
        {
            Parametros Parametro = Bank.Parametros.Include(x => x.EscalonamentodeHoraExtra).FirstOrDefault(x => x.IDParametro == id);

            return PartialView(Parametro);
        }

        [HttpPost]
        
        [VerificaLogin]
        public ActionResult CadastroParametros_Remover_Salvar(int id)
        {
            try
            {
                Parametros Parametro = Bank.Parametros.Include(x => x.EscalonamentodeHoraExtra).FirstOrDefault(x => x.IDParametro == id);

                Bank.Remove(Parametro);
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
        public ActionResult CadastroParametros_Remover_Selecao(int[] id)
        {
            List<Parametros> Parametros = Bank.Parametros.Where(x => id.Contains(x.IDParametro)).ToList();

            return PartialView(Parametros);
        }

        [HttpPost]
        
        [VerificaLogin]
        public ActionResult CadastroParametros_Remover_Selecao_Salvar(int[] id)
        {
            try
            {
                List<Parametros> Parametros = Bank.Parametros.Where(x => id.Contains(x.IDParametro)).ToList();

                Bank.RemoveRange(Parametros);
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