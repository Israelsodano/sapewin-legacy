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
    public class CadastroTesteController : Controller
    {

        Login.Models.LoginModel BankLogin = new Login.Models.LoginModel();
        MyContext Bank = new MyContext();

        [HttpGet]
        [VerificaLogin]
        public ActionResult CadastroTeste_Abrir()
        {
            Login.Models.LoginSistema UsuarioLogado = BankLogin
                .LoginSistema
                .FirstOrDefault(x => x.IDLoginsistema == Convert.ToInt32(Session["Usuario"].ToString())); // carrega usuario logado

            ViewBag.PermissoesIndices = UsuarioLogado.PerfiMaster
              ? Bank.FuncoesdeTelas.Select(x => x.IDFuncaoTela).ToList()
              : Bank.PermissoesdeTelas.Where(x => x.IDUsuario == UsuarioLogado.IDLoginsistema && Session["Empresa"].ToString().Contains(x.IDEmpresa.ToString("0000"))).Select(x => x.IDFuncaoTela).ToList(); //carreha lista de funçoes de telas qiue o usuario possui

            List<Teste> Teste = Bank.Teste.AsNoTracking().ToList(); // Carrega Lista de Teste do sistema

            int Total = Teste.Count;

            Teste = Teste.Count < 10 ? Teste : Teste.GetRange(0, 10); // cria pagina 

            ViewBag.Paginas = Math.Ceiling(Convert.ToDecimal(Convert.ToDouble(Total) / 10)); //conta paginas

            ViewBag.Total = $"Registro(s): {Total} - Exibindo de {(Teste.Count > 0 ? 1 : 0)} a {Teste.Count} - {ViewBag.Paginas} Página(s)"; //monta string do footer

            return VerificaLoad.IsAjax(HttpContext.Request) ? PartialView(Teste) : (ViewResultBase)View(Teste);
        }

        [HttpGet]
        [VerificaLoad]
        public ActionResult CadastroTeste_Abrir_Grid(String pesquisa, int Range, int Pagina, String Order, bool Condicao)
        {
            if (String.IsNullOrEmpty(pesquisa)) { pesquisa = ""; }

            pesquisa = pesquisa.ToLower().Replace('+', ' ');

            Login.Models.LoginSistema UsuarioLogado = BankLogin
                .LoginSistema
                .FirstOrDefault(x => x.IDLoginsistema == Convert.ToInt32(Session["Usuario"].ToString())); // carrega usuario logado 

            ViewBag.PermissoesIndices = UsuarioLogado.PerfiMaster
             ? Bank.FuncoesdeTelas.AsNoTracking().Select(x => x.IDFuncaoTela).ToList()
             : Bank.PermissoesdeTelas.AsNoTracking().Where(x => x.IDUsuario == UsuarioLogado.IDLoginsistema && Session["Empresa"].ToString().Contains(x.IDEmpresa.ToString("0000"))).Select(x => x.IDFuncaoTela).ToList(); //carreha lista de funçoes de telas qiue o usuario possui

            List<Teste> Teste = new List<Models.Teste>();

            switch (Order.ToUpper())
            {
                case "CODIGO-UP":

                    if (Condicao || pesquisa == "")
                    {
                        Teste = Bank
                       .Teste
                       
                       .Where(x => x.Descricao.ToLower().Contains(pesquisa.ToLower()) || x.IDTeste.ToString().Contains(pesquisa)).OrderBy(x => x.IDTeste).ToList();
                    }
                    else
                    {
                        Teste = Bank
                       .Teste
                       
                       .Where(x => !x.Descricao.ToLower().Contains(pesquisa.ToLower()) && !x.IDTeste.ToString().Contains(pesquisa)).OrderBy(x => x.IDTeste).ToList();
                    }
                    break;

                case "CODIGO-DOWN":

                    if (Condicao || pesquisa == "")
                    {
                        Teste = Bank
                       .Teste
                       
                       .Where(x => x.Descricao.ToLower().Contains(pesquisa.ToLower()) || x.IDTeste.ToString().Contains(pesquisa)).OrderByDescending(x => x.IDTeste).ToList();
                    }
                    else
                    {
                        Teste = Bank
                       .Teste
                       
                       .Where(x => !x.Descricao.ToLower().Contains(pesquisa.ToLower()) && !x.IDTeste.ToString().Contains(pesquisa)).OrderByDescending(x => x.IDTeste).ToList();
                    }
                    break;

                case "DESCRICAO-UP":

                    if (Condicao || pesquisa == "")
                    {
                        Teste = Bank
                       .Teste
                       
                       .Where(x => x.Descricao.ToLower().Contains(pesquisa.ToLower()) || x.IDTeste.ToString().Contains(pesquisa)).OrderBy(x => x.Descricao).ToList();
                    }
                    else
                    {
                        Teste = Bank
                       .Teste
                       
                       .Where(x => !x.Descricao.ToLower().Contains(pesquisa.ToLower()) && !x.IDTeste.ToString().Contains(pesquisa)).OrderBy(x => x.Descricao).ToList();
                    }
                    break;

                case "DESCRICAO-DOWN":

                    if (Condicao || pesquisa == "")
                    {
                        Teste = Bank
                       .Teste
                       
                       .Where(x => x.Descricao.ToLower().Contains(pesquisa.ToLower()) || x.IDTeste.ToString().Contains(pesquisa)).OrderByDescending(x => x.Descricao).ToList();
                    }
                    else
                    {
                        Teste = Bank
                       .Teste
                       
                       .Where(x => !x.Descricao.ToLower().Contains(pesquisa.ToLower()) && !x.IDTeste.ToString().Contains(pesquisa)).OrderByDescending(x => x.Descricao).ToList();
                    }
                    break;

                default:
                    break;
            }

            int Total = Teste.Count; // lista total

            ViewBag.Paginas = Math.Ceiling(Convert.ToDecimal(Convert.ToDouble(Total) / Range)); // conta paginas

            Pagina = Pagina > ViewBag.Paginas ? Convert.ToInt32(ViewBag.Paginas) - 1 : Pagina; // trata a pagina para nn ser maior que as paginas possiveis do obj

            Pagina = Teste.Count < Range ? Pagina * Teste.Count : Pagina * Range; // trata pagina para ser o primeiro registro da pagina

            Range = Range + Pagina > Teste.Count ? Teste.Count - Pagina : Range; // trata o range para nn ser maior que o range do obj

            Teste = Teste.Count < Range ? Teste : Teste.GetRange(Pagina, Range); // monta pagina

            ViewBag.Total = $"Registro(s): {Total} - Exibindo de {(Pagina == 0 ? 1 : Pagina + 1)} a {(Pagina == 0 ? Range : Pagina + Range)} - { ViewBag.Paginas} Página(s)"; // monta string do footer

            return PartialView(Teste);
        }
    }
}