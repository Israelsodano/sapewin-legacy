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
    public class CadastroMensagensController : Controller
    {
        Login.Models.LoginModel BankLogin = new Login.Models.LoginModel();
        MyContext Bank = new MyContext();

        [HttpGet]
        [VerificaLogin]
        public ActionResult CadastroMensagens_Abrir()
        {
            Login.Models.LoginSistema UsuarioLogado = BankLogin
                .LoginSistema
                .FirstOrDefault(x => x.IDLoginsistema == Convert.ToInt32(Session["Usuario"].ToString())); // carrega usuario logado

            ViewBag.PermissoesIndices = UsuarioLogado.PerfiMaster
              ? Bank.FuncoesdeTelas.Select(x => x.IDFuncaoTela).ToList()
              : Bank.PermissoesdeTelas.Where(x => x.IDUsuario == UsuarioLogado.IDLoginsistema && Session["Empresa"].ToString().Contains(x.IDEmpresa.ToString("0000"))).Select(x => x.IDFuncaoTela).ToList(); //carreha lista de funçoes de telas qiue o usuario possui

            List<Mensagem> Mensagens = Bank.Mensagem.AsNoTracking().ToList(); // Carrega Lista de Mensagens do sistema

            int Total = Mensagens.Count;

            Mensagens = Mensagens.Count < 10 ? Mensagens : Mensagens.GetRange(0, 10); // cria pagina 

            ViewBag.Paginas = Math.Ceiling(Convert.ToDecimal(Convert.ToDouble(Total) / 10)); //conta paginas

            ViewBag.Total = $"Registro(s): {Total} - Exibindo de {(Mensagens.Count > 0 ? 1 : 0)} a {Mensagens.Count} - {ViewBag.Paginas} Página(s)"; //monta string do footer

            return VerificaLoad.IsAjax(HttpContext.Request) ? PartialView(Mensagens) : (ViewResultBase)View(Mensagens);

        }

        [HttpGet]
        [VerificaLoad]
        public ActionResult CadastroMensagens_Abrir_Grid(String pesquisa, int Range, int Pagina, String Order, bool Condicao)
        {
            if (String.IsNullOrEmpty(pesquisa)) { pesquisa = ""; }

            pesquisa = pesquisa.ToLower().Replace('+', ' ');

            Login.Models.LoginSistema UsuarioLogado = BankLogin
                .LoginSistema
                .FirstOrDefault(x => x.IDLoginsistema == Convert.ToInt32(Session["Usuario"].ToString())); // carrega usuario logado 

            ViewBag.PermissoesIndices = UsuarioLogado.PerfiMaster
             ? Bank.FuncoesdeTelas.AsNoTracking().Select(x => x.IDFuncaoTela).ToList()
             : Bank.PermissoesdeTelas.AsNoTracking().Where(x => x.IDUsuario == UsuarioLogado.IDLoginsistema && Session["Empresa"].ToString().Contains(x.IDEmpresa.ToString("0000"))).Select(x => x.IDFuncaoTela).ToList(); //carreha lista de funçoes de telas qiue o usuario possui

            List<Mensagem> Mensagens = new List<Mensagem>();

            switch (Order.ToUpper())
            {
                case "CODIGO-UP":

                    if (Condicao || pesquisa == "")
                    {
                        Mensagens = Bank
                       .Mensagem
                       .Where(x => x.Nome.ToLower().Contains(pesquisa.ToLower()) || x.IDMensagem.ToString().Contains(pesquisa)).OrderBy(x => x.IDMensagem).ToList();
                    }
                    else
                    {
                        Mensagens = Bank
                       .Mensagem
                       .Where(x => !x.Nome.ToLower().Contains(pesquisa.ToLower()) && !x.IDMensagem.ToString().Contains(pesquisa)).OrderBy(x => x.IDMensagem).ToList();
                    }
                    break;

                case "CODIGO-DOWN":

                    if (Condicao || pesquisa == "")
                    {
                        Mensagens = Bank
                       .Mensagem
                       .Where(x => x.Nome.ToLower().Contains(pesquisa.ToLower()) || x.IDMensagem.ToString().Contains(pesquisa)).OrderByDescending(x => x.IDMensagem).ToList();
                    }
                    else
                    {
                        Mensagens = Bank
                       .Mensagem
                       .Where(x => !x.Nome.ToLower().Contains(pesquisa.ToLower()) && !x.IDMensagem.ToString().Contains(pesquisa)).OrderByDescending(x => x.IDMensagem).ToList();
                    }
                    break;

                case "DESCRICAO-UP":

                    if (Condicao || pesquisa == "")
                    {
                        Mensagens = Bank
                       .Mensagem
                       .Where(x => x.Nome.ToLower().Contains(pesquisa.ToLower()) || x.IDMensagem.ToString().Contains(pesquisa)).OrderBy(x => x.Nome).ToList();
                    }
                    else
                    {
                        Mensagens = Bank
                       .Mensagem
                       .Where(x => !x.Nome.ToLower().Contains(pesquisa.ToLower()) && !x.IDMensagem.ToString().Contains(pesquisa)).OrderBy(x => x.Nome).ToList();
                    }
                    break;

                case "DESCRICAO-DOWN":

                    if (Condicao || pesquisa == "")
                    {
                        Mensagens = Bank
                       .Mensagem
                       .Where(x => x.Nome.ToLower().Contains(pesquisa.ToLower()) || x.IDMensagem.ToString().Contains(pesquisa)).OrderByDescending(x => x.Nome).ToList();
                    }
                    else
                    {
                        Mensagens = Bank
                       .Mensagem
                       .Where(x => !x.Nome.ToLower().Contains(pesquisa.ToLower()) && !x.IDMensagem.ToString().Contains(pesquisa)).OrderByDescending(x => x.Nome).ToList();
                    }
                    break;

                default:
                    break;
            }           

            int Total = Mensagens.Count; // lista total

            ViewBag.Paginas = Math.Ceiling(Convert.ToDecimal(Convert.ToDouble(Total) / Range)); // conta paginas

            Pagina = Pagina > ViewBag.Paginas ? Convert.ToInt32(ViewBag.Paginas) - 1 : Pagina; // trata a pagina para nn ser maior que as paginas possiveis do obj

            Pagina = Mensagens.Count < Range ? Pagina * Mensagens.Count : Pagina * Range; // trata pagina para ser o primeiro registro da pagina

            Range = Range + Pagina > Mensagens.Count ? Mensagens.Count - Pagina : Range; // trata o range para nn ser maior que o range do obj

            Mensagens = Mensagens.Count < Range ? Mensagens : Mensagens.GetRange(Pagina, Range); // monta pagina

            ViewBag.Total = $"Registro(s): {Total} - Exibindo de {(Pagina == 0 ? 1 : Pagina + 1)} a {(Pagina == 0 ? Range : Pagina + Range)} - { ViewBag.Paginas} Página(s)"; // monta string do footer

            return PartialView(Mensagens);
        }

        [HttpGet]
        [VerificaLogin]
        public ActionResult CadastroMensagens_Incluir()
        {
            return PartialView();
        }

        [HttpPost]
        
        [VerificaLogin]
        public ActionResult CadastroMensagens_Incluir_Salvar(int? IDMensagem, String Nome, String Msg)
        {
            if (String.IsNullOrEmpty(Nome) || String.IsNullOrEmpty(Msg))
            {
                return Json(new { status = false, msg = "Preencha todos os campos" });
            }
            else
            {
                try
                {
                    Login.Models.LoginSistema UsuarioLogado = BankLogin
                   .LoginSistema.AsNoTracking()
                   .FirstOrDefault(x => x.IDLoginsistema == Convert.ToInt32(Session["Usuario"].ToString())); // carrega usuario logado

                    Mensagem Mensagem = new Mensagem
                    {
                        IDMensagem = IDMensagem == null ? Bank.Mensagem.AsNoTracking().OrderBy(x => x.IDMensagem).LastOrDefault() == null ? 1 : Bank.Mensagem.AsNoTracking().OrderBy(x => x.IDMensagem).LastOrDefault().IDMensagem + 1 : Convert.ToInt32(IDMensagem),
                        Nome = Nome,
                        Conteudo = Msg
                    };

                    Bank.Mensagem.Add(Mensagem);
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
        public ActionResult CadastroMensagens_Alterar(int id)
        {
            Mensagem Mensagem = Bank.Mensagem.AsNoTracking().FirstOrDefault(x => x.IDMensagem == id);
            return PartialView(Mensagem); //retorna obj por id e sessao para a view
        }

        [HttpPost]
        
        [VerificaLogin]
        public ActionResult CadastroMensagens_Alterar_Salvar(int id, String Nome, String Msg)
        {
            if (String.IsNullOrEmpty(Nome) || String.IsNullOrEmpty(Msg))
            {
                return Json(new { status = false, msg = "Preencha todos os campos" });
            }
            else
            {
                try
                {
                    Mensagem Mensagem = Bank.Mensagem.First(x => x.IDMensagem == id);
                    Mensagem.Nome = Nome;
                    Mensagem.Conteudo = Msg;
                    Bank.SaveChanges();// SaveChanges novas informaçoes no obj
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
        public ActionResult CadastroMensagens_Remover(int id)
        {
            Mensagem Cargo = Bank.Mensagem.AsNoTracking().First(x => x.IDMensagem == id);
            
            return PartialView(Cargo); //retorna obj por id e sessao para a view
        }

        [HttpPost]
        
        [VerificaLogin]
        public ActionResult CadastroMensagens_Remover_Salvar(int id)
        {
            try
            {
                Mensagem Mensagem = Bank.Mensagem.First(x => x.IDMensagem == id);
                
                Bank.Mensagem.Remove(Mensagem);
                Bank.SaveChanges(); // remove obj por id e sessão
                return Json(new { status = true });
            }
            catch (Exception e)
            {
                return Json(new { status = false, msg = e.Message });
            }
        }

        [HttpGet]
        [VerificaLogin]
        public ActionResult CadastroMensagens_Remover_Selecao(int[] id)
        {
            List<Mensagem> Mensagens = Bank.Mensagem.AsNoTracking().Where(x => id.Contains(x.IDMensagem)).ToList();
            return PartialView(Mensagens);
        }

        [HttpPost]
        [VerificaLogin]
        
        public ActionResult CadastroMensagens_Remover_Selecao_Salvar(int[] id)
        {
            try
            {
                List<Mensagem> Mensagem = Bank.Mensagem.Where(x => id.Contains(x.IDMensagem)).ToList();
                               
                Bank.Mensagem.RemoveRange(Mensagem);

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