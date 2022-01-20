using Microsoft.EntityFrameworkCore;
using SapewinWeb.Metodos;
using SapewinWeb.Models;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading;
using System.Web;
using System.Web.Mvc;

namespace SapewinWeb.Controllers
{
    public class CadastroFuncionariosController : Controller
    {
        Login.Models.LoginModel BankLogin = new Login.Models.LoginModel();
        MyContext Bank = new MyContext();

        [HttpGet]
        [VerificaLogin]
        public ActionResult CadastroFuncionarios_Abrir()
        {
            Login.Models.LoginSistema UsuarioLogado = BankLogin
                 .LoginSistema.AsNoTracking()
                 .FirstOrDefault(x => x.IDLoginsistema == Convert.ToInt32(Session["Usuario"].ToString())); // carrega usuario logado           

            ViewBag.PermissoesIndices = UsuarioLogado.PerfiMaster
                ? Bank.FuncoesdeTelas.AsNoTracking().Select(x => x.IDFuncaoTela).ToList()
                : Bank.PermissoesdeTelas.AsNoTracking().Where(x => x.IDUsuario == UsuarioLogado.IDLoginsistema && Session["Empresa"].ToString().Contains(x.IDEmpresa.ToString("0000"))).Select(x => x.IDFuncaoTela).ToList(); //carreha lista de funçoes de telas qiue o usuario possui

            int IDEmpresa = Bank.Empresas.First(x => Session["Empresa"].ToString().Contains(x.IDEmpresa.ToString("0000"))).IDEmpresa;

            List<Funcionarios> Funcionarios = UsuarioLogado.PerfiMaster ? Bank.Funcionarios.Where(x => x.IDEmpresa == IDEmpresa).ToList() : Bank.PermissoesdeFuncionarios.Where(x=>x.IDUsuario == UsuarioLogado.IDLoginsistema && x.IDEmpresa == IDEmpresa).Select(x=>x.Funcionario).ToList();

            int Total = Funcionarios.Count;

            Funcionarios = Funcionarios.Count < 10 ? Funcionarios : Funcionarios.GetRange(0, 10); // cria pagina 

            ViewBag.Paginas = Math.Ceiling(Convert.ToDecimal(Convert.ToDouble(Total) / 10)); //conta paginas

            ViewBag.Total = $"Registro(s): {Total} - Exibindo de {(Funcionarios.Count > 0 ? 1 : 0)} a {Funcionarios.Count} - {ViewBag.Paginas} Página(s)"; //monta string do footer

            return VerificaLoad.IsAjax(HttpContext.Request) ? PartialView(Funcionarios) : (ViewResultBase)View(Funcionarios);
        }

        [HttpGet]
        [VerificaLoad]
        public ActionResult CadastroFuncionarios_Abrir_Grid(String pesquisa, int Range, int Pagina, String Order, bool Condicao)
        {
            if (String.IsNullOrEmpty(pesquisa)) { pesquisa = ""; }

            pesquisa = pesquisa.ToLower().Replace('+', ' ');

            Login.Models.LoginSistema UsuarioLogado = BankLogin
                .LoginSistema.AsNoTracking()
                .FirstOrDefault(x => x.IDLoginsistema == Convert.ToInt32(Session["Usuario"].ToString())); // carrega usuario logado

            ViewBag.PermissoesIndices = UsuarioLogado.PerfiMaster
             ? Bank.FuncoesdeTelas.AsNoTracking().Select(x => x.IDFuncaoTela).ToList()
             : Bank.PermissoesdeTelas.AsNoTracking().Where(x => x.IDUsuario == UsuarioLogado.IDLoginsistema && Session["Empresa"].ToString().Contains(x.IDEmpresa.ToString("0000"))).Select(x => x.IDFuncaoTela).ToList(); //carreha lista de funçoes de telas qiue o usuario possui

            int IDEmpresa = Bank.Empresas.First(x => Session["Empresa"].ToString().Contains(x.IDEmpresa.ToString("0000"))).IDEmpresa;

            List<Funcionarios> Funcionarios = new List<Models.Funcionarios>();

            switch (Order.ToUpper())
            {
                case "CODIGO-UP":

                    if (Condicao || pesquisa == "")
                    {
                        Funcionarios = UsuarioLogado.PerfiMaster ? Bank.Funcionarios.AsNoTracking().Where(x => x.IDEmpresa == IDEmpresa && (x.IDFuncionario.ToString().StartsWith(pesquisa) || x.Nome.ToLower().StartsWith(pesquisa))).OrderBy(x => x.IDFuncionario).ToList() : Bank.PermissoesdeFuncionarios.Where(x => x.IDUsuario == UsuarioLogado.IDLoginsistema && x.IDEmpresa == IDEmpresa).Select(x => x.Funcionario).Where(x => x.IDFuncionario.ToString().StartsWith(pesquisa) || x.Nome.ToLower().Contains(pesquisa)).OrderBy(x => x.IDFuncionario).ToList();
                    }
                    else
                    {
                        Funcionarios = UsuarioLogado.PerfiMaster ? Bank.Funcionarios.AsNoTracking().Where(x => x.IDEmpresa == IDEmpresa && (!x.IDFuncionario.ToString().StartsWith(pesquisa) && !x.Nome.ToLower().StartsWith(pesquisa))).OrderBy(x => x.IDFuncionario).ToList() : Bank.PermissoesdeFuncionarios.Where(x => x.IDUsuario == UsuarioLogado.IDLoginsistema && x.IDEmpresa == IDEmpresa).Select(x => x.Funcionario).Where(x => !x.IDFuncionario.ToString().StartsWith(pesquisa) && !x.Nome.ToLower().Contains(pesquisa)).OrderBy(x => x.IDFuncionario).ToList();
                    }
                    break;

                case "CODIGO-DOWN":

                    if (Condicao || pesquisa == "")
                    {
                        Funcionarios = UsuarioLogado.PerfiMaster ? Bank.Funcionarios.AsNoTracking().Where(x => x.IDEmpresa == IDEmpresa && (x.IDFuncionario.ToString().StartsWith(pesquisa) || x.Nome.ToLower().StartsWith(pesquisa))).OrderByDescending(x => x.IDFuncionario).ToList() : Bank.PermissoesdeFuncionarios.Where(x => x.IDUsuario == UsuarioLogado.IDLoginsistema && x.IDEmpresa == IDEmpresa).Select(x => x.Funcionario).Where(x => x.IDFuncionario.ToString().StartsWith(pesquisa) || x.Nome.ToLower().Contains(pesquisa)).OrderByDescending(x => x.IDFuncionario).ToList();
                    }
                    else
                    {
                        Funcionarios = UsuarioLogado.PerfiMaster ? Bank.Funcionarios.AsNoTracking().Where(x => x.IDEmpresa == IDEmpresa && (!x.IDFuncionario.ToString().StartsWith(pesquisa) && !x.Nome.ToLower().StartsWith(pesquisa))).OrderByDescending(x => x.IDFuncionario).ToList() : Bank.PermissoesdeFuncionarios.Where(x => x.IDUsuario == UsuarioLogado.IDLoginsistema && x.IDEmpresa == IDEmpresa).Select(x => x.Funcionario).Where(x => !x.IDFuncionario.ToString().StartsWith(pesquisa) && !x.Nome.ToLower().Contains(pesquisa)).OrderByDescending(x => x.IDFuncionario).ToList();
                    }
                    break;

                case "DESCRICAO-UP":

                    if (Condicao || pesquisa == "")
                    {
                        Funcionarios = UsuarioLogado.PerfiMaster ? Bank.Funcionarios.AsNoTracking().Where(x => x.IDEmpresa == IDEmpresa && (x.IDFuncionario.ToString().StartsWith(pesquisa) || x.Nome.ToLower().StartsWith(pesquisa))).OrderBy(x => x.Nome).ToList() : Bank.PermissoesdeFuncionarios.Where(x => x.IDUsuario == UsuarioLogado.IDLoginsistema && x.IDEmpresa == IDEmpresa).Select(x => x.Funcionario).Where(x => x.IDFuncionario.ToString().StartsWith(pesquisa) || x.Nome.ToLower().Contains(pesquisa)).OrderBy(x => x.Nome).ToList();
                    }
                    else
                    {
                        Funcionarios = UsuarioLogado.PerfiMaster ? Bank.Funcionarios.AsNoTracking().Where(x => x.IDEmpresa == IDEmpresa && (!x.IDFuncionario.ToString().StartsWith(pesquisa) && !x.Nome.ToLower().StartsWith(pesquisa))).OrderBy(x => x.Nome).ToList() : Bank.PermissoesdeFuncionarios.Where(x => x.IDUsuario == UsuarioLogado.IDLoginsistema && x.IDEmpresa == IDEmpresa).Select(x => x.Funcionario).Where(x => !x.IDFuncionario.ToString().StartsWith(pesquisa) && !x.Nome.ToLower().Contains(pesquisa)).OrderBy(x => x.Nome).ToList();
                    }
                    break;

                case "DESCRICAO-DOWN":

                    if (Condicao || pesquisa == "")
                    {
                        Funcionarios = UsuarioLogado.PerfiMaster ? Bank.Funcionarios.AsNoTracking().Where(x => x.IDEmpresa == IDEmpresa && (x.IDFuncionario.ToString().StartsWith(pesquisa) || x.Nome.ToLower().StartsWith(pesquisa))).OrderByDescending(x => x.Nome).ToList() : Bank.PermissoesdeFuncionarios.Where(x => x.IDUsuario == UsuarioLogado.IDLoginsistema && x.IDEmpresa == IDEmpresa).Select(x => x.Funcionario).Where(x => x.IDFuncionario.ToString().StartsWith(pesquisa) || x.Nome.ToLower().Contains(pesquisa)).OrderByDescending(x => x.Nome).ToList();
                    }
                    else
                    {
                        Funcionarios = UsuarioLogado.PerfiMaster ? Bank.Funcionarios.AsNoTracking().Where(x => x.IDEmpresa == IDEmpresa && (!x.IDFuncionario.ToString().StartsWith(pesquisa) && !x.Nome.ToLower().StartsWith(pesquisa))).OrderByDescending(x => x.Nome).ToList() : Bank.PermissoesdeFuncionarios.Where(x => x.IDUsuario == UsuarioLogado.IDLoginsistema && x.IDEmpresa == IDEmpresa).Select(x => x.Funcionario).Where(x => !x.IDFuncionario.ToString().StartsWith(pesquisa) && !x.Nome.ToLower().Contains(pesquisa)).OrderByDescending(x => x.Nome).ToList();
                    }
                    break;

                default:
                    break;
            }

            int Total = Funcionarios.Count; // lista total

            ViewBag.Paginas = Math.Ceiling(Convert.ToDecimal(Convert.ToDouble(Total) / Range)); // conta paginas

            Pagina = Pagina > ViewBag.Paginas ? Convert.ToInt32(ViewBag.Paginas) - 1 : Pagina; // trata a pagina para nn ser maior que as paginas possiveis do obj

            Pagina = Funcionarios.Count < Range ? Pagina * Funcionarios.Count : Pagina * Range; // trata pagina para ser o primeiro registro da pagina

            Range = Range + Pagina > Funcionarios.Count ? Funcionarios.Count - Pagina : Range; // trata o range para nn ser maior que o range do obj

            Funcionarios = Funcionarios.Count < Range ? Funcionarios : Funcionarios.GetRange(Pagina, Range); // monta pagina

            ViewBag.Total = $"Registro(s): {Total} - Exibindo de {(Pagina == 0 ? 1 : Pagina + 1)} a {(Pagina == 0 ? Range : Pagina + Range)} - { ViewBag.Paginas} Página(s)"; // monta string do footer

            return PartialView(Funcionarios);
        }

        [HttpGet]
        [VerificaLogin]
        public ActionResult CadastroFuncionarios_Incluir()
        {
            var imagempadrao = System.IO.File.ReadAllBytes(Server.MapPath(@"\Fotos\FotoPadrao.png"));

            ViewBag.ImagemPadrao = Convert.ToBase64String(imagempadrao);

            return PartialView();
        }

        [HttpPost]
        
        [VerificaLogin]     
        public ActionResult CadastroFuncionarios_Incluir_Salvar(Funcionarios Funcionario, string[] Cartoes, string[] Afastamentos, string[] Folgas, string[] Horarios, string[] Mensagens, String Salariostr, String Imagem)
        {
            try
            {
                if (String.IsNullOrEmpty(Funcionario.Nome) || String.IsNullOrEmpty(Funcionario.Pis) || (Funcionario.Admissao != null && Funcionario.Admissao.Equals(new DateTime())) || String.IsNullOrEmpty(Funcionario.Nome))
                {
                    throw new Exception("Preencha todos os campos obrigatorios");
                }

                if ((Funcionario.Cpf != null && !String.IsNullOrEmpty(Funcionario.Cpf)) && !Funcionario.Cpf.CpfeValido())
                {
                    throw new Exception("Cpf Digitado não corresponde a um Cpf valido");
                }                

                if (!Funcionario.Pis.PiseValido())
                {
                    throw new Exception("Pis Digitado não corresponde a um Pis valido");
                }               

                Funcionario.IDFuncionario = Funcionario.IDFuncionario == 0 ? Bank.Funcionarios.Where(x => Session["Empresa"].ToString().Contains(x.IDEmpresa.ToString())).FirstOrDefault() == null ? 1 : Bank.Funcionarios.Where(x => Session["Empresa"].ToString().Contains(x.IDEmpresa.ToString())).OrderBy(x => x.IDFuncionario).LastOrDefault().IDFuncionario + 1 : Funcionario.IDFuncionario;

                int IDEmpresa = Bank.Empresas.FirstOrDefault(x => Session["Empresa"].ToString().Contains(x.IDEmpresa.ToString("0000"))).IDEmpresa;

                if (Bank.Funcionarios.Where(x => x.IDEmpresa == IDEmpresa && x.IDFuncionario == Funcionario.IDFuncionario).Count() > 0)
                {
                    throw new Exception("Já existe um Funcionario com esse código");
                }

                if (Cartoes != null && !VerificaCartoes(Cartoes, Funcionario))
                {
                    throw new Exception("Ja existe um cartão de proximidade cadastrado para o mesmo periodo");
                }

                long IDFuncionario = Funcionario.IDFuncionario;

                

                Funcionario.IDEmpresa = IDEmpresa;               

                if (Cartoes != null && !String.IsNullOrEmpty(Cartoes[0]))
                {                                        
                    for (int i = 0; i < Cartoes.Length; i++)
                    {
                        Bank.CartaoProximidade.Add(new Models.CartaoProximidade { DataInicial = Convert.ToDateTime(Cartoes[i].Split('/')[0]), DataFinal = String.IsNullOrEmpty(Cartoes[i].Split('/')[1]) ? null : (DateTime?)Convert.ToDateTime(Cartoes[i].Split('/')[1]), IDFuncionario = IDFuncionario, NumerodoCartao = Cartoes[i].Split('/')[2], IDCartao = Bank.CartaoProximidade.OrderBy(x=>x.IDCartao).LastOrDefault() == null ? 1 : Bank.CartaoProximidade.OrderBy(x => x.IDCartao).LastOrDefault().IDCartao + 1 + i, IDEmpresa = IDEmpresa });
                    }
                }

                if (Afastamentos != null && !String.IsNullOrEmpty(Afastamentos[0]))
                {
                    for (int i = 0; i < Afastamentos.Length; i++)
                    {
                        Bank.Afastamentos.Add(new Models.Afastamentos { DataInicial = Convert.ToDateTime(Afastamentos[i].Split('/')[0]), DataFinal = String.IsNullOrEmpty(Afastamentos[i].Split('/')[1]) ? null: (DateTime?)Convert.ToDateTime(Afastamentos[i].Split('/')[1]) , Abreviacao = Afastamentos[i].Split('/')[2], IDFuncionario = IDFuncionario, IDAfastamento = Bank.Afastamentos.Where(x=> x.IDEmpresa == IDEmpresa).Count() > 0 ? Bank.Afastamentos.Where(x => x.IDEmpresa == IDEmpresa).OrderBy(x=>x.IDAfastamento).LastOrDefault().IDAfastamento + 1 + i : 1, IDEmpresa = IDEmpresa });
                    }
                }
               
                if (Folgas != null && !String.IsNullOrEmpty(Folgas[0]))
                {
                    for (int i = 0; i < Folgas.Length; i++)
                    {
                        Bank.Folgas.Add(new Models.Folgas { Data = Convert.ToDateTime(Folgas[i]), IDFolga = Bank.Folgas.OrderBy(x=>x.IDFolga).LastOrDefault() == null ? 1 : Bank.Folgas.OrderBy(x => x.IDFolga).LastOrDefault().IDFolga + 1 + i, IDFuncionario = IDFuncionario, IDEmpresa = IDEmpresa});
                    }
                }

                if (Horarios != null && !String.IsNullOrEmpty(Horarios[0]))
                {
                    for (int i = 0; i < Horarios.Length; i++)
                    {
                        Bank.HorariosOcasionais.Add(new Models.HorariosOcasionais { Data = Convert.ToDateTime(Horarios[i].Split('/')[0]), IDFuncionario = IDFuncionario, IDHorarioOcasional = Bank.HorariosOcasionais.OrderBy(x => x.IDHorarioOcasional).LastOrDefault() == null ? 1 : Bank.HorariosOcasionais.OrderBy(x => x.IDHorarioOcasional).LastOrDefault().IDHorarioOcasional + 1 + i, IDHorario = Convert.ToInt32(Horarios[i].Split('/')[1]), IDEmpresa = IDEmpresa });
                    }                   
                }

                if (Mensagens != null && !String.IsNullOrEmpty(Mensagens[0]))
                {
                    for (int i = 0; i < Mensagens.Length; i++)
                    {
                        Bank.MensagensFuncionarios.Add(new MensagensFuncionarios { DataInicial = Convert.ToDateTime(Mensagens[i].Split('/')[0]), DataFinal = String.IsNullOrEmpty(Mensagens[i].Split('/')[1]) ? null : (DateTime?)Convert.ToDateTime(Mensagens[i].Split('/')[1]), IDEmpresa = IDEmpresa, IDFuncionario = IDFuncionario, IDMensagem = Bank.Mensagem.FirstOrDefault(x=>x.Nome == Mensagens[i].Split('/')[2]).IDMensagem });
                    }                    
                }
                Salariostr = Salariostr.Replace(",", ".");
                Salariostr = Regex.Replace(Salariostr, @"[A-z]", "");

                Funcionario.Salario = String.IsNullOrEmpty(Salariostr) ? Funcionario.Salario : Salariostr;

                Bank.Funcionarios.Add(Funcionario);

                Login.Models.LoginSistema UsuarioLogado = BankLogin
                          .LoginSistema.AsNoTracking()
                          .Include(x => x.Cliente)
                          .FirstOrDefault(x => x.IDLoginsistema == Convert.ToInt32(Session["Usuario"].ToString())); // carrega usuario logado              

                if (!String.IsNullOrEmpty(Imagem))
                {
                    Funcionario.FotoPadrao = false;

                    var thread = new Thread(() => {                        

                        var PastaCliente = Server.MapPath(@"\Fotos\") + UsuarioLogado.Cliente.Documento;

                        if (!System.IO.Directory.Exists(PastaCliente))
                        {
                            System.IO.Directory.CreateDirectory(PastaCliente);
                        }

                        Byte[] bytes = Convert.FromBase64String(Imagem);

                        System.IO.File.WriteAllBytes(PastaCliente + @"\" + Funcionario.IDEmpresa.ToString() + "_" + Funcionario.IDFuncionario.ToString() + ".png", bytes);

                    });

                    thread.Priority = ThreadPriority.Highest;

                    thread.Start();
                }
                else
                {
                    Funcionario.FotoPadrao = true;
                }                

                foreach (var Login in BankLogin.Clientes.Include(x => x.LoginSistema).FirstOrDefault(x => x.IDCliente == UsuarioLogado.IDCliente).LoginSistema.ToList())
                {
                    Bank.PermissoesdeFuncionarios.Add(new PermissoesdeFuncionarios { IDFuncionario = Funcionario.IDFuncionario, IDUsuario = Login.IDLoginsistema, IDEmpresa = IDEmpresa });
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
        [VerificaLogin]
        public ActionResult CadastroFuncionarios_Alterar(int id)
        {
            Funcionarios Funcionario = Bank.Funcionarios.Include(x=>x.CartoesProximidade).Include(x=>x.HorariosOcasionais).Include(x=>x.MensagensCartao).Include(x=>x.Folgas).Include(x=>x.Afastamentos).FirstOrDefault(x => Session["Empresa"].ToString().Contains(x.IDEmpresa.ToString("0000")) && x.IDFuncionario == id);

            Login.Models.LoginSistema UsuarioLogado = BankLogin
                  .LoginSistema.AsNoTracking()
                  .Include(x => x.Cliente)
                  .FirstOrDefault(x => x.IDLoginsistema == Convert.ToInt32(Session["Usuario"].ToString())); // carrega usuario logado


            if (!UsuarioLogado.PerfiMaster && Bank.PermissoesdeFuncionarios.Where(x => x.IDFuncionario == Funcionario.IDFuncionario && x.IDUsuario == UsuarioLogado.IDLoginsistema).Count() == 0)
            {
                return new HttpUnauthorizedResult();
            }

            var imagempadrao = System.IO.File.ReadAllBytes(Server.MapPath(@"\Fotos\FotoPadrao.png"));

            ViewBag.ImagemPadraoPadrao = Convert.ToBase64String(imagempadrao);

            if (!System.IO.File.Exists(Server.MapPath(@"\Fotos\" + UsuarioLogado.Cliente.Documento + $"\\{Funcionario.IDEmpresa}_{Funcionario.IDFuncionario}.png")) && !Funcionario.FotoPadrao)
            {
                Funcionario.FotoPadrao = true;
                Bank.SaveChanges();
            }

            ViewBag.ImagemPadrao = Funcionario.FotoPadrao ? Convert.ToBase64String(System.IO.File.ReadAllBytes(Server.MapPath(@"\Fotos\FotoPadrao.png"))) : Convert.ToBase64String(System.IO.File.ReadAllBytes(Server.MapPath(@"\Fotos\" + UsuarioLogado.Cliente.Documento + $"\\{Funcionario.IDEmpresa}_{Funcionario.IDFuncionario}.png" )));

            return PartialView(Funcionario);
        }

        [HttpPost]
        
        [VerificaLogin]
        public ActionResult CadastroFuncionarios_Alterar_Salvar(Funcionarios FuncionarioAlterar, string[] Cartoes, string[] Afastamentos, string[] Folgas, string[] Horarios, string[] Mensagens, String Salariostr, String Imagem)
        {
            try
            {

                Login.Models.LoginSistema UsuarioLogado = BankLogin
                  .LoginSistema.AsNoTracking()
                  .Include(x => x.Cliente)
                  .FirstOrDefault(x => x.IDLoginsistema == Convert.ToInt32(Session["Usuario"].ToString())); // carrega usuario logado
              

                if (!UsuarioLogado.PerfiMaster && Bank.PermissoesdeFuncionarios.Where(x => x.IDFuncionario == FuncionarioAlterar.IDFuncionario && x.IDUsuario == UsuarioLogado.IDLoginsistema).Count() == 0)
                {
                    return new HttpUnauthorizedResult();
                }

                if (String.IsNullOrEmpty(FuncionarioAlterar.Nome) || String.IsNullOrEmpty(FuncionarioAlterar.Pis) || (FuncionarioAlterar.Admissao != null && FuncionarioAlterar.Admissao.Equals(new DateTime())) || String.IsNullOrEmpty(FuncionarioAlterar.Nome))
                {
                    throw new Exception("Preencha todos os campos obrigatorios");
                }

                if ((FuncionarioAlterar.Cpf != null && !String.IsNullOrEmpty(FuncionarioAlterar.Cpf)) && !FuncionarioAlterar.Cpf.CpfeValido())
                {
                    throw new Exception("Cpf Digitado não corresponde a um Cpf valido");
                }               

                if (!FuncionarioAlterar.Pis.PiseValido())
                {
                    throw new Exception("Pis Digitado não corresponde a um Pis valido");
                }

                int IDEmpresa = Bank.Empresas.FirstOrDefault(x => Session["Empresa"].ToString().Contains(x.IDEmpresa.ToString("0000"))).IDEmpresa;

                Funcionarios Funcionario = Bank.Funcionarios.Include(x=>x.MensagensCartao).Include(x=>x.CartoesProximidade).Include(x=>x.Afastamentos).Include(x=>x.HorariosOcasionais).Include(x=>x.Folgas).FirstOrDefault(x=>x.IDFuncionario == FuncionarioAlterar.IDFuncionario && x.IDEmpresa == IDEmpresa);

                if (Cartoes != null && !VerificaCartoes(Cartoes, Funcionario))
                {
                    throw new Exception("Ja existe um cartão de proximidade cadastrado para o mesmo periodo");
                }

                long IDFuncionario = Funcionario.IDFuncionario;

                Funcionario.Admissao = FuncionarioAlterar.Admissao;
                Funcionario.Cidade = FuncionarioAlterar.Cidade;
                Funcionario.Cpf = FuncionarioAlterar.Cpf;
                Funcionario.CTPSNum = FuncionarioAlterar.CTPSNum;
                Funcionario.Endereco = FuncionarioAlterar.Endereco;
                Funcionario.IDCargo = FuncionarioAlterar.IDCargo;
                Funcionario.IDDepartamento = FuncionarioAlterar.IDDepartamento;
                Funcionario.IDEscala = FuncionarioAlterar.IDEscala;
                Funcionario.IDFeriado = FuncionarioAlterar.IDFeriado;
                Funcionario.IDFolha = FuncionarioAlterar.IDFolha;
                Funcionario.IDParametro = FuncionarioAlterar.IDParametro;
                Funcionario.IDSetor = FuncionarioAlterar.IDSetor;
                Funcionario.Intervalo = FuncionarioAlterar.Intervalo;
                Funcionario.IntervaloFixo = FuncionarioAlterar.IntervaloFixo;
                Funcionario.Nome = FuncionarioAlterar.Nome;
                Funcionario.Observacoes = FuncionarioAlterar.Observacoes;
                Funcionario.Pis = FuncionarioAlterar.Pis;
                Funcionario.Rescisao = FuncionarioAlterar.Rescisao;
                Funcionario.RG = FuncionarioAlterar.RG;
                Funcionario.Feriado = FuncionarioAlterar.Feriado;


                Salariostr = Salariostr.Replace(",", ".");
                Salariostr = Regex.Replace(Salariostr, @"[A-z]", "");

                Funcionario.Salario = String.IsNullOrEmpty(Salariostr) ? FuncionarioAlterar.Salario : Salariostr;

                Funcionario.Serie = FuncionarioAlterar.Serie;
                Funcionario.Telefone = FuncionarioAlterar.Telefone;                

                Funcionario.MensagensCartao.Clear();
                Funcionario.CartoesProximidade.Clear();
                Funcionario.Afastamentos.Clear();
                Funcionario.HorariosOcasionais.Clear();
                Funcionario.Folgas.Clear();

                Bank.SaveChanges();

                if (Cartoes != null && !String.IsNullOrEmpty(Cartoes[0]))
                {
                    for (int i = 0; i < Cartoes.Length; i++)
                    {
                        Bank.CartaoProximidade.Add(new Models.CartaoProximidade { DataInicial = Convert.ToDateTime(Cartoes[i].Split('/')[0]), DataFinal = String.IsNullOrEmpty(Cartoes[i].Split('/')[1]) ? null : (DateTime?)Convert.ToDateTime(Cartoes[i].Split('/')[1]), IDFuncionario = IDFuncionario, NumerodoCartao = Cartoes[i].Split('/')[2], IDCartao = Bank.CartaoProximidade.OrderBy(x => x.IDCartao).LastOrDefault() == null ? 1 + i : Bank.CartaoProximidade.OrderBy(x => x.IDCartao).LastOrDefault().IDCartao + 1, IDEmpresa = IDEmpresa });
                    }
                }

                if (Afastamentos != null && !String.IsNullOrEmpty(Afastamentos[0]))
                {
                    for (int i = 0; i < Afastamentos.Length; i++)
                    {
                        Bank.Afastamentos.Add(new Models.Afastamentos { DataInicial = Convert.ToDateTime(Afastamentos[i].Split('/')[0]), DataFinal = String.IsNullOrEmpty(Afastamentos[i].Split('/')[1]) ? null : (DateTime?)Convert.ToDateTime(Afastamentos[i].Split('/')[1]), Abreviacao = Afastamentos[i].Split('/')[2], IDFuncionario = IDFuncionario, IDAfastamento = Bank.Afastamentos.Where(x => x.IDEmpresa == IDEmpresa).Count() > 0 ? Bank.Afastamentos.Where(x => x.IDEmpresa == IDEmpresa).OrderBy(x => x.IDAfastamento).LastOrDefault().IDAfastamento + 1 + i : 1, IDEmpresa = IDEmpresa });
                    }
                }

                if (Folgas != null && !String.IsNullOrEmpty(Folgas[0]))
                {
                    for (int i = 0; i < Folgas.Length; i++)
                    {
                        Bank.Folgas.Add(new Models.Folgas { Data = Convert.ToDateTime(Folgas[i]), IDFolga = Bank.Folgas.OrderBy(x => x.IDFolga).LastOrDefault() == null ? 1 : Bank.Folgas.OrderBy(x => x.IDFolga).LastOrDefault().IDFolga + 1 + i, IDFuncionario = IDFuncionario, IDEmpresa = IDEmpresa });
                    }
                }

                if (Horarios != null && !String.IsNullOrEmpty(Horarios[0]))
                {
                    for (int i = 0; i < Horarios.Length; i++)
                    {
                        Bank.HorariosOcasionais.Add(new Models.HorariosOcasionais { Data = Convert.ToDateTime(Horarios[i].Split('/')[0]), IDFuncionario = IDFuncionario, IDHorarioOcasional = Bank.HorariosOcasionais.OrderBy(x => x.IDHorarioOcasional).LastOrDefault() == null ? 1 : Bank.HorariosOcasionais.OrderBy(x => x.IDHorarioOcasional).LastOrDefault().IDHorarioOcasional + 1 + i, IDHorario = Convert.ToInt32(Horarios[i].Split('/')[1]), IDEmpresa = IDEmpresa });
                    }
                }

                if (Mensagens != null && !String.IsNullOrEmpty(Mensagens[0]))
                {
                    for (int i = 0; i < Mensagens.Length; i++)
                    {
                        Bank.MensagensFuncionarios.Add(new MensagensFuncionarios { DataInicial = Convert.ToDateTime(Mensagens[i].Split('/')[0]), DataFinal = String.IsNullOrEmpty(Mensagens[i].Split('/')[1]) ? null : (DateTime?)Convert.ToDateTime(Mensagens[i].Split('/')[1]), IDEmpresa = IDEmpresa, IDFuncionario = IDFuncionario, IDMensagem = Bank.Mensagem.FirstOrDefault(x => x.Nome == Mensagens[i].Split('/')[2]).IDMensagem });
                    }
                }               

                if (!String.IsNullOrEmpty(Imagem) && Imagem != "none")
                {
                    Funcionario.FotoPadrao = false;

                    var thread = new Thread(() => {                       

                        var PastaCliente = Server.MapPath(@"\Fotos\") + UsuarioLogado.Cliente.Documento;

                        if (!System.IO.Directory.Exists(PastaCliente))
                        {
                            System.IO.Directory.CreateDirectory(PastaCliente);
                        }

                        Byte[] bytes = Convert.FromBase64String(Imagem);

                        System.IO.File.WriteAllBytes(PastaCliente + @"\" + Funcionario.IDEmpresa.ToString() + "_" + Funcionario.IDFuncionario.ToString() + ".png", bytes);

                    });

                    thread.Priority = ThreadPriority.Highest;

                    thread.Start();
                }
                else
                {
                    var PastaCliente = Server.MapPath(@"\Fotos\") + UsuarioLogado.Cliente.Documento;

                    if (!System.IO.File.Exists(PastaCliente + $"\\{Funcionario.IDEmpresa}_{Funcionario.IDFuncionario}.png") || Imagem == "none")
                    {                        
                        if (System.IO.File.Exists(PastaCliente + $"\\{Funcionario.IDEmpresa}_{Funcionario.IDFuncionario}.png"))
                        {
                            System.IO.File.Delete(PastaCliente + $"\\{Funcionario.IDEmpresa}_{Funcionario.IDFuncionario}.png");
                        }

                        Funcionario.FotoPadrao = true;
                    }                    
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
        [VerificaLogin]
        public ActionResult CadastroFuncionarios_Remover(int id)
        {

            Login.Models.LoginSistema UsuarioLogado = BankLogin
                 .LoginSistema.AsNoTracking()
                 .Include(x => x.Cliente)
                 .FirstOrDefault(x => x.IDLoginsistema == Convert.ToInt32(Session["Usuario"].ToString())); // carrega usuario logado


            if (!UsuarioLogado.PerfiMaster && Bank.PermissoesdeFuncionarios.Where(x => x.IDFuncionario == id && x.IDUsuario == UsuarioLogado.IDLoginsistema).Count() == 0)
            {
                return new HttpUnauthorizedResult();
            }

            Funcionarios Funcionario = Bank.Funcionarios.AsNoTracking().First(x => x.IDFuncionario == id);           

            return PartialView(Funcionario); //retorna obj por id e sessao para a view
        }

        [HttpPost]
        
        [VerificaLogin]
        public ActionResult CadastroFuncionarios_Remover_Salvar(int id)
        {
            try
            {
                Login.Models.LoginSistema UsuarioLogado = BankLogin
                 .LoginSistema.AsNoTracking()
                 .Include(x => x.Cliente)
                 .FirstOrDefault(x => x.IDLoginsistema == Convert.ToInt32(Session["Usuario"].ToString())); // carrega usuario logado


                if (!UsuarioLogado.PerfiMaster && Bank.PermissoesdeFuncionarios.Where(x => x.IDFuncionario == id && x.IDUsuario == UsuarioLogado.IDLoginsistema).Count() == 0)
                {
                    return new HttpUnauthorizedResult();
                }

                Funcionarios Funcionario = Bank.Funcionarios.First(x => x.IDFuncionario == id);

                Bank.Funcionarios.Remove(Funcionario);

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
        public ActionResult CadastroFuncionarios_Remover_Selecao(long[] id)
        {
            Login.Models.LoginSistema UsuarioLogado = BankLogin
                .LoginSistema.AsNoTracking()
                .Include(x => x.Cliente)
                .FirstOrDefault(x => x.IDLoginsistema == Convert.ToInt32(Session["Usuario"].ToString())); // carrega usuario logado


            if (!UsuarioLogado.PerfiMaster && Bank.PermissoesdeFuncionarios.Where(x =>id.Contains(x.IDFuncionario) && x.IDUsuario == UsuarioLogado.IDLoginsistema).Count() == 0)
            {
                return new HttpUnauthorizedResult();
            }

            List<Funcionarios> Funcionarios = Bank.Funcionarios.AsNoTracking().Where(x => id.Contains(x.IDFuncionario)).ToList();

            return PartialView(Funcionarios); //retorna obj por id e sessao para a view
        }

        [HttpPost]
        
        [VerificaLogin]
        public ActionResult CadastroFuncionarios_Remover_Selecao_Salvar(long[] id)
        {
            try
            {
                Login.Models.LoginSistema UsuarioLogado = BankLogin
                .LoginSistema.AsNoTracking()
                .Include(x => x.Cliente)
                .FirstOrDefault(x => x.IDLoginsistema == Convert.ToInt32(Session["Usuario"].ToString())); // carrega usuario logado


                if (!UsuarioLogado.PerfiMaster && Bank.PermissoesdeFuncionarios.Where(x => id.Contains(x.IDFuncionario) && x.IDUsuario == UsuarioLogado.IDLoginsistema).Count() == 0)
                {
                    return new HttpUnauthorizedResult();
                }

                List<Funcionarios> Funcionarios = Bank.Funcionarios.Where(x => id.Contains(x.IDFuncionario)).ToList();

                Bank.Funcionarios.RemoveRange(Funcionarios);

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
        public ActionResult Select(tipos Select)
        {            
            return PartialView(Select);
        }

        public enum tipos
        {
            Setores = 1,Parametros = 2, Departamentos = 3, Cargos = 4, Escalas = 5 
        }

        [HttpGet]
        [VerificaLoad]
        public ActionResult CartaoProximidade(int? id)
        {

            ViewBag.IDFuncionario = id;

            return PartialView();
        }

        public bool isNewIn(string[] Array)
        {
            bool retorno = true;

            List<CartaoProximidade> Cartoes = new List<Models.CartaoProximidade>();

            for (int i = 0; i < Array.Length; i++)
            {
                Cartoes.Add(new Models.CartaoProximidade { DataInicial = Convert.ToDateTime(Array[i].Split('/')[0]), DataFinal = String.IsNullOrEmpty(Array[i].Split('/')[1]) ? null : (DateTime?)Convert.ToDateTime(Array[i].Split('/')[1]), NumerodoCartao = Array[i].Split('/')[2], IDCartao = 0, IDFuncionario = 0 });
            }

            for (int i = 0; i < Array.Length; i++)
            {
                var inicio = Convert.ToDateTime(Array[i].Split('/')[0]);

                var fim = String.IsNullOrEmpty(Array[i].Split('/')[1]) ? null : (DateTime?)Convert.ToDateTime(Array[i].Split('/')[1]);

                if (Cartoes.Where(x=> inicio >= x.DataInicial && (inicio <= x.DataFinal || x.DataFinal == null)).Count() > 1)
                {
                    retorno = false;
                }

                if (Cartoes.Where(x => fim >= x.DataInicial && (fim == null || (fim <= x.DataFinal || x.DataFinal == null))).Count() > 1)
                {
                    retorno = false;
                }
            }

            return retorno;
        }

        [HttpGet]
        [VerificaLoad]
        public ActionResult GridCartaoProximidade(string[] Array)
        {
            try
            {
                if (Array == null || String.IsNullOrEmpty(Array[0]))
                {
                    return PartialView(new List<CartaoProximidade>());
                }else
                {
                    if (!isNewIn(Array))
                    {
                        throw new Exception();
                    }

                    List<CartaoProximidade> Cartoes = new List<Models.CartaoProximidade>();

                    for (int i = 0; i < Array.Length; i++)
                    {
                        Cartoes.Add(new Models.CartaoProximidade { DataInicial = Convert.ToDateTime(Array[i].Split('/')[0]), DataFinal = String.IsNullOrEmpty(Array[i].Split('/')[1]) ? null : (DateTime?)Convert.ToDateTime(Array[i].Split('/')[1]), NumerodoCartao = Array[i].Split('/')[2], IDCartao = 0, IDFuncionario = 0 });
                    }

                    return PartialView(Cartoes.OrderBy(x=>x.DataInicial));
                }                

            }
            catch (Exception)
            {
                return new HttpUnauthorizedResult("Já existe um cartão proximidade cadastrado para o mesmo período");
            }
          
        }

        [HttpGet]
        [VerificaLoad]
        public ActionResult Mensagens()
        {
            return PartialView(Bank.Mensagem.ToList());
        }

        protected bool VerificaCartoes(string[] Cartoes, Funcionarios Funcionario)
        {
            for (int i = 0; i < Cartoes.Length; i++)
            {
                var ini = Convert.ToDateTime(Cartoes[i].Split('/')[0]);

                var fim = String.IsNullOrEmpty(Cartoes[i].Split('/')[1]) ? null : (DateTime?)Convert.ToDateTime(Cartoes[i].Split('/')[1]);

                var t = Bank.CartaoProximidade.AsNoTracking().Where(x => (ini >= x.DataInicial && (x.DataFinal == null || ini <= x.DataFinal)) || (fim == null && (x.DataInicial >= ini || (x.DataFinal == null || x.DataFinal >= ini))) || (fim != null && (fim >= x.DataInicial) && ((x.DataFinal == null && fim != null) && (fim <= x.DataFinal)))).ToList();

                var numero = Cartoes[i].Split('/')[2];

                t.RemoveAll(x=>x.IDFuncionario == Funcionario.IDFuncionario && x.IDEmpresa == Funcionario.IDEmpresa);

                t.RemoveAll(x => x.NumerodoCartao != numero);

                if (t.Count > 0)
                {
                    return false;
                }
            }

            return true;
        }
        
        [HttpPost]     
        public ActionResult VerificaCartao(string[] Cartoes, int? id)
        {
            int IDEmpresa = Bank.Empresas.FirstOrDefault(x => Session["Empresa"].ToString().Contains(x.IDEmpresa.ToString("0000"))).IDEmpresa;

            for (int i = 0; i < Cartoes.Length; i++)
            {
                var ini = Convert.ToDateTime(Cartoes[i].Split('/')[0]);

                var fim = String.IsNullOrEmpty(Cartoes[i].Split('/')[1]) ? null : (DateTime?)Convert.ToDateTime(Cartoes[i].Split('/')[1]);

                var numero = Cartoes[i].Split('/')[2];

                var list = Bank.CartaoProximidade.AsNoTracking().Where(x => (ini >= x.DataInicial && (x.DataFinal == null || ini <= x.DataFinal)) || (fim == null && (x.DataInicial >= ini || (x.DataFinal == null || x.DataFinal >= ini))) || (fim != null && (fim >= x.DataInicial) && ((x.DataFinal == null && fim != null) && (fim <= x.DataFinal)))).ToList();

                list.RemoveAll(x => (id.HasValue ? (x.IDFuncionario == id && x.IDEmpresa == IDEmpresa) : (false)));

                list.RemoveAll(x=>x.NumerodoCartao != numero);

                if (list.Count() > 0)
                {
                    return Json(new { status = false, Cartoes = Cartoes.Where(x => x != Cartoes[i]).ToArray() });
                }
            }
            return Json(new { status = true });
        }

        [VerificaLoad]
        public ActionResult GridMensagens(string[] Array)
        {
            try
            {
                if (Array == null || String.IsNullOrEmpty(Array[0]))
                {
                    return PartialView(new List<MensagensFuncionarios>());
                }
                else
                {
                    if (!isNewIn(Array))
                    {
                        throw new Exception();
                    }

                    List<MensagensFuncionarios> Mensagens = new List<MensagensFuncionarios>();

                    for (int i = 0; i < Array.Length; i++)
                    {
                        Mensagens.Add(new Models.MensagensFuncionarios { DataInicial = Convert.ToDateTime(Array[i].Split('/')[0]), DataFinal = String.IsNullOrEmpty(Array[i].Split('/')[1]) ? null : (DateTime?)Convert.ToDateTime(Array[i].Split('/')[1]), Mensagem = new Mensagem { Nome = Array[i].Split('/')[2] }, IDMensagem = 0, IDFuncionario = 0 });
                    }

                    return PartialView(Mensagens.OrderBy(x => x.DataInicial));
                }

            }
            catch (Exception)
            {
                return new HttpUnauthorizedResult("Já existe uma mensagem cadastrada para o mesmo período");
            }
        }

        [HttpGet]
        [VerificaLoad]
        public ActionResult Afastamentos()
        {
            List<MotivosdeAbono> Motivos = Bank.MotivosdeAbono.Where(x => Session["Empresa"].ToString().Contains(x.IDEmpresa.ToString("000"))).ToList();

            return PartialView(Motivos.OrderBy(x => $"{!x.Favorito}{x.Abreviacao}"));
        }

        [HttpGet]
        [VerificaLoad]
        public ActionResult GridAfastamento(string[] Array)
        {
            try
            {
                if (Array == null || String.IsNullOrEmpty(Array[0]))
                {
                    return PartialView(new List<Afastamentos>());
                }
                else
                {
                    if (!isNewIn(Array))
                    {
                        throw new Exception();
                    }

                    List<Afastamentos> Afastamentos = new List<Afastamentos>();

                    for (int i = 0; i < Array.Length; i++)
                    {
                        Afastamentos.Add(new Models.Afastamentos { DataInicial = Convert.ToDateTime(Array[i].Split('/')[0]), DataFinal = String.IsNullOrEmpty(Array[i].Split('/')[1]) ? null : (DateTime?)Convert.ToDateTime(Array[i].Split('/')[1]), Abreviacao = Array[i].Split('/')[2] });
                    }

                    return PartialView(Afastamentos.OrderBy(x => x.DataInicial));
                }

            }
            catch (Exception)
            {
                return new HttpUnauthorizedResult("Já existe um Afastamento cadastrado para o mesmo período");
            }
        }

        [HttpGet]
        [VerificaLoad]
        public ActionResult Folgas()
        {
            return PartialView();
        }

        [HttpGet]
        [VerificaLoad]
        public ActionResult GridFolgas(string[] Array)
        {
            try
            {
                if (Array == null || String.IsNullOrEmpty(Array[0]))
                {
                    return PartialView(new List<Folgas>());
                }
                else
                {
                    if (Array.Where(x=> Array.Where(z=>z == x).Count() > 1).Count() > 0)
                    {
                        throw new Exception();
                    }

                    List<Folgas> Folgas = new List<Folgas>();

                    for (int i = 0; i < Array.Length; i++)
                    {
                        Folgas.Add(new Models.Folgas { Data= Convert.ToDateTime(Array[i]) });
                    }

                    return PartialView(Folgas.OrderBy(x => x.Data));
                }

            }
            catch (Exception)
            {
                return new HttpUnauthorizedResult("Já existe uma Folga cadastrada para o mesmo período");
            }
        }

        [HttpGet]
        [VerificaLoad]
        public ActionResult HorariosOcasionais()
        {
            List<Horarios> Horarios = Bank.Horarios.Where(x => Session["Empresa"].ToString().Contains(x.IDEmpresa.ToString("0000")) && !x.CargaSemanal).ToList();

            return PartialView(Horarios);
        }

        [HttpGet]
        [VerificaLoad]
        public ActionResult GridHorarios(string[] Array)
        {
            try
            {
                if (Array == null || String.IsNullOrEmpty(Array[0]))
                {
                    return PartialView(new List<HorariosOcasionais>());
                }
                else
                {
                    if (Array.Where(x=> Array.Where(z=> Convert.ToDateTime(z.Split('/')[0]) == Convert.ToDateTime(x.Split('/')[0])).Count() > 1).Count() > 0)
                    {
                        throw new Exception();
                    }

                    List<HorariosOcasionais> HorariosOcasionais = new List<HorariosOcasionais>();

                    for (int i = 0; i < Array.Length; i++)
                    {
                        HorariosOcasionais.Add(new Models.HorariosOcasionais { Data = Convert.ToDateTime(Array[i].Split('/')[0]), Horario = Bank.Horarios.AsNoTracking().FirstOrDefault(x=>x.IDHorario == Convert.ToInt32(Array[i].Split('/')[1])) });
                    }

                    return PartialView(HorariosOcasionais.OrderBy(x => x.Data));
                }

            }
            catch (Exception)
            {
                return new HttpUnauthorizedResult("Já existe um Horario cadastrada para o mesmo período");
            }
        }

        [HttpPost]
        [VerificaLoad]
        public ActionResult RetornaHorariotxt(int id)
        {
            var hr = Bank.Horarios.FirstOrDefault(x => x.IDHorario == id && Session["Empresa"].ToString().Contains(x.IDEmpresa.ToString("0000")));

            return Json(new { horario = $"Cumprindo o horário: {hr.IDHorario.ToString("0000")} - {hr.Descricao}" });
        }
    }
}