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
    
    public class CadastroEmpresasController : Controller
    {
        Login.Models.LoginModel BankLogin = new Login.Models.LoginModel();
        MyContext Bank = new MyContext();

        [HttpGet]
        [VerificaLogin]
        public ActionResult CadastroEmpresas_Abrir() // Metodo Abrir Com grid
        {
            Login.Models.LoginSistema UsuarioLogado = BankLogin
                .LoginSistema.AsNoTracking()
                .FirstOrDefault(x => x.IDLoginsistema == Convert.ToInt32(Session["Usuario"].ToString())); // Carrega Usuario Logado

            
            ViewBag.PermissoesIndices = UsuarioLogado.PerfiMaster
                ? Bank.FuncoesdeTelas.AsNoTracking().Select(x => x.IDFuncaoTela).ToList()
                : Bank.PermissoesdeTelas.AsNoTracking().Where(x => x.IDUsuario == UsuarioLogado.IDLoginsistema && Session["Empresa"].ToString().Contains(x.IDEmpresa.ToString("0000"))).Select(x => x.IDFuncaoTela).ToList(); // Monta uma lista com as funçoes de tela que o usuario possui

            List<Empresas> Empresas = UsuarioLogado.PerfiMaster ? Bank.Empresas.ToList() : Bank.PermissoesdeEmpresas.Include(x => x.Empresa).Where(x => x.IDUsuario == UsuarioLogado.IDLoginsistema).Select(x => x.Empresa).ToList();

            int Total = Empresas.Count;

            Empresas = Empresas.Count < 10 ? Empresas : Empresas.GetRange(0, 10); // pega apenas a primeira pagina

            ViewBag.Paginas = Math.Ceiling(Convert.ToDecimal(Convert.ToDouble(Total) / 10)); //conta paginas

            ViewBag.Total = $"Registro(s): {Total} - Exibindo de {(Empresas.Count > 0 ? 1 : 0)} a {Empresas.Count} - {ViewBag.Paginas} Página(s)"; // monta string do footer do grid

           
            return VerificaLoad.IsAjax(HttpContext.Request) ? PartialView(Empresas) : (ViewResultBase)View(Empresas);
        }

        [HttpGet]
        [VerificaLoad]
        public ActionResult CadastroEmpresas_Abrir_Grid(String pesquisa, int Range, int Pagina, String Order, bool Condicao)
        {
            if (String.IsNullOrEmpty(pesquisa)) { pesquisa = ""; } // limpa string

            pesquisa = pesquisa.ToLower().Replace('+', ' ');

            Login.Models.LoginSistema UsuarioLogado = BankLogin
                .LoginSistema.AsNoTracking()
                .FirstOrDefault(x => x.IDLoginsistema == Convert.ToInt32(Session["Usuario"].ToString())); // carrega usuario           

            ViewBag.PermissoesIndices = UsuarioLogado.PerfiMaster
             ? Bank.FuncoesdeTelas.AsNoTracking().Select(x => x.IDFuncaoTela).ToList()
             : Bank.PermissoesdeTelas.AsNoTracking().Where(x => x.IDUsuario == UsuarioLogado.IDLoginsistema && Session["Empresa"].ToString().Contains(x.IDEmpresa.ToString("0000"))).Select(x => x.IDFuncaoTela).ToList(); //carreha lista de funçoes de telas qiue o usuario possui

            List<Empresas> Empresas = new List<Models.Empresas>();

            switch (Order.ToUpper())
            {
                case "CODIGO-UP":

                    if (Condicao || pesquisa == "")
                    {
                        Empresas = UsuarioLogado.PerfiMaster ? Bank.Empresas.Where(x => x.IDEmpresa.ToString().StartsWith(x.IDEmpresa.ToString()) || x.Nome.ToLower().Contains(pesquisa) || x.Documento.StartsWith(pesquisa)).OrderBy(x=>x.IDEmpresa).ToList() : Bank.PermissoesdeEmpresas.Where(x => x.IDUsuario == UsuarioLogado.IDLoginsistema).Select(x => x.Empresa).Where(x => x.IDEmpresa.ToString().StartsWith(pesquisa) || x.Nome.ToLower().Contains(pesquisa) || x.Documento.StartsWith(pesquisa)).OrderBy(x=>x.IDEmpresa).ToList();
                    }
                    else
                    {
                        Empresas = UsuarioLogado.PerfiMaster ? Bank.Empresas.Where(x => !x.IDEmpresa.ToString().StartsWith(x.IDEmpresa.ToString()) && !x.Nome.ToLower().Contains(pesquisa) && !x.Documento.StartsWith(pesquisa)).OrderBy(x=>x.IDEmpresa).ToList() : Bank.PermissoesdeEmpresas.Where(x => x.IDUsuario == UsuarioLogado.IDLoginsistema).Select(x => x.Empresa).Where(x => !x.IDEmpresa.ToString().StartsWith(pesquisa) && !x.Nome.ToLower().Contains(pesquisa) && !x.Documento.StartsWith(pesquisa)).OrderBy(x => x.IDEmpresa).ToList();
                    }
                    break;

                case "CODIGO-DOWN":

                    if (Condicao || pesquisa == "")
                    {
                        Empresas = UsuarioLogado.PerfiMaster ? Bank.Empresas.Where(x => x.IDEmpresa.ToString().StartsWith(x.IDEmpresa.ToString()) || x.Nome.ToLower().Contains(pesquisa) || x.Documento.StartsWith(pesquisa)).OrderByDescending(x=>x.IDEmpresa).ToList() : Bank.PermissoesdeEmpresas.Where(x => x.IDUsuario == UsuarioLogado.IDLoginsistema).Select(x => x.Empresa).Where(x => x.IDEmpresa.ToString().StartsWith(pesquisa) || x.Nome.ToLower().Contains(pesquisa) || x.Documento.StartsWith(pesquisa)).OrderByDescending(x => x.IDEmpresa).ToList();
                    }
                    else
                    {
                        Empresas = UsuarioLogado.PerfiMaster ? Bank.Empresas.Where(x => !x.IDEmpresa.ToString().StartsWith(x.IDEmpresa.ToString()) && !x.Nome.ToLower().Contains(pesquisa) && !x.Documento.StartsWith(pesquisa)).OrderByDescending(x=>x.IDEmpresa).ToList() : Bank.PermissoesdeEmpresas.Where(x => x.IDUsuario == UsuarioLogado.IDLoginsistema).Select(x => x.Empresa).Where(x => !x.IDEmpresa.ToString().StartsWith(pesquisa) && !x.Nome.ToLower().Contains(pesquisa) && !x.Documento.StartsWith(pesquisa)).OrderByDescending(x => x.IDEmpresa).ToList();
                    }
                    break;

                case "RAZAO-UP":

                    if (Condicao || pesquisa == "")
                    {
                        Empresas = UsuarioLogado.PerfiMaster ? Bank.Empresas.Where(x => x.IDEmpresa.ToString().StartsWith(x.IDEmpresa.ToString()) || x.Nome.ToLower().Contains(pesquisa) || x.Documento.StartsWith(pesquisa)).OrderBy(x=>x.Nome).ToList() : Bank.PermissoesdeEmpresas.Where(x => x.IDUsuario == UsuarioLogado.IDLoginsistema).Select(x => x.Empresa).Where(x => x.IDEmpresa.ToString().StartsWith(pesquisa) || x.Nome.ToLower().Contains(pesquisa) || x.Documento.StartsWith(pesquisa)).OrderBy(x => x.Nome).ToList();
                    }
                    else
                    {
                        Empresas = UsuarioLogado.PerfiMaster ? Bank.Empresas.Where(x => !x.IDEmpresa.ToString().StartsWith(x.IDEmpresa.ToString()) && !x.Nome.ToLower().Contains(pesquisa) && !x.Documento.StartsWith(pesquisa)).OrderBy(x=>x.Nome).ToList() : Bank.PermissoesdeEmpresas.Where(x => x.IDUsuario == UsuarioLogado.IDLoginsistema).Select(x => x.Empresa).Where(x => !x.IDEmpresa.ToString().StartsWith(pesquisa) && !x.Nome.ToLower().Contains(pesquisa) && !x.Documento.StartsWith(pesquisa)).OrderBy(x => x.Nome).ToList();
                    }
                    break;

                case "RAZAO-DOWN":

                    if (Condicao || pesquisa == "")
                    {
                        Empresas = UsuarioLogado.PerfiMaster ? Bank.Empresas.Where(x => x.IDEmpresa.ToString().StartsWith(x.IDEmpresa.ToString()) || x.Nome.ToLower().Contains(pesquisa) || x.Documento.StartsWith(pesquisa)).OrderByDescending(x=>x.Nome).ToList() : Bank.PermissoesdeEmpresas.Where(x => x.IDUsuario == UsuarioLogado.IDLoginsistema).Select(x => x.Empresa).Where(x => x.IDEmpresa.ToString().StartsWith(pesquisa) || x.Nome.ToLower().Contains(pesquisa) || x.Documento.StartsWith(pesquisa)).OrderByDescending(x => x.Nome).ToList();
                    }
                    else
                    {
                        Empresas = UsuarioLogado.PerfiMaster ? Bank.Empresas.Where(x => !x.IDEmpresa.ToString().StartsWith(x.IDEmpresa.ToString()) && !x.Nome.ToLower().Contains(pesquisa) && !x.Documento.StartsWith(pesquisa)).OrderByDescending(x=>x.Nome).ToList() : Bank.PermissoesdeEmpresas.Where(x => x.IDUsuario == UsuarioLogado.IDLoginsistema).Select(x => x.Empresa).Where(x => !x.IDEmpresa.ToString().StartsWith(pesquisa) && !x.Nome.ToLower().Contains(pesquisa) && !x.Documento.StartsWith(pesquisa)).OrderByDescending(x => x.Nome).ToList();
                    }
                    break;

                case "DOCUMENTO-UP":

                    if (Condicao || pesquisa == "")
                    {
                        Empresas = UsuarioLogado.PerfiMaster ? Bank.Empresas.Where(x => x.IDEmpresa.ToString().StartsWith(x.IDEmpresa.ToString()) || x.Nome.ToLower().Contains(pesquisa) || x.Documento.StartsWith(pesquisa)).OrderBy(x=>x.Documento).ToList() : Bank.PermissoesdeEmpresas.Where(x => x.IDUsuario == UsuarioLogado.IDLoginsistema).Select(x => x.Empresa).Where(x => x.IDEmpresa.ToString().StartsWith(pesquisa) || x.Nome.ToLower().Contains(pesquisa) || x.Documento.StartsWith(pesquisa)).OrderBy(x => x.Documento).ToList();
                    }
                    else
                    {
                        Empresas = UsuarioLogado.PerfiMaster ? Bank.Empresas.Where(x => !x.IDEmpresa.ToString().StartsWith(x.IDEmpresa.ToString()) && !x.Nome.ToLower().Contains(pesquisa) && !x.Documento.StartsWith(pesquisa)).OrderBy(x => x.Documento).ToList() : Bank.PermissoesdeEmpresas.Where(x => x.IDUsuario == UsuarioLogado.IDLoginsistema).Select(x => x.Empresa).Where(x => !x.IDEmpresa.ToString().StartsWith(pesquisa) && !x.Nome.ToLower().Contains(pesquisa) && !x.Documento.StartsWith(pesquisa)).OrderBy(x => x.Documento).ToList();
                    }
                    break;

                case "DOCUMENTO-DOWN":

                    if (Condicao || pesquisa == "")
                    {
                        Empresas = UsuarioLogado.PerfiMaster ? Bank.Empresas.Where(x => x.IDEmpresa.ToString().StartsWith(x.IDEmpresa.ToString()) || x.Nome.ToLower().Contains(pesquisa) || x.Documento.StartsWith(pesquisa)).OrderByDescending(x => x.Documento).ToList() : Bank.PermissoesdeEmpresas.Where(x => x.IDUsuario == UsuarioLogado.IDLoginsistema).Select(x => x.Empresa).Where(x => x.IDEmpresa.ToString().StartsWith(pesquisa) || x.Nome.ToLower().Contains(pesquisa) || x.Documento.StartsWith(pesquisa)).OrderByDescending(x => x.Documento).ToList();
                    }
                    else
                    {
                        Empresas = UsuarioLogado.PerfiMaster ? Bank.Empresas.Where(x => !x.IDEmpresa.ToString().StartsWith(x.IDEmpresa.ToString()) && !x.Nome.ToLower().Contains(pesquisa) && !x.Documento.StartsWith(pesquisa)).OrderByDescending(x => x.Documento).ToList() : Bank.PermissoesdeEmpresas.Where(x => x.IDUsuario == UsuarioLogado.IDLoginsistema).Select(x => x.Empresa).Where(x => !x.IDEmpresa.ToString().StartsWith(pesquisa) && !x.Nome.ToLower().Contains(pesquisa) && !x.Documento.StartsWith(pesquisa)).OrderByDescending(x => x.Documento).ToList();
                    }
                    break;

                default:
                    break;
            }            

            int Total = Empresas.Count;

            ViewBag.Paginas = Math.Ceiling(Convert.ToDecimal(Convert.ToDouble(Total) / Range)); // conta paginas

            Pagina = Pagina > ViewBag.Paginas ? Convert.ToInt32(ViewBag.Paginas) - 1 : Pagina; // set primeiro registro a ser mostrado

            Pagina = Empresas.Count < Range ? Pagina * Empresas.Count : Pagina * Range; // trata para ver se o tamanho de pagina padrao e menor que o numero de registros

            Range = Range + Pagina > Empresas.Count ? Empresas.Count - Pagina : Range; // trata ultimo registro para nao ser maior que o numero de registros

            Empresas = Empresas.Count < Range ? Empresas : Empresas.GetRange(Pagina, Range); // monta pagina

            ViewBag.Total = $"Registro(s): {Total} - Exibindo de {(Pagina == 0 ? 1 : Pagina + 1)} a {(Pagina == 0 ? Range : Pagina + Range)} - { ViewBag.Paginas} Página(s)"; // monta string do footer

            return PartialView(Empresas);
        }

        [HttpGet]
        [VerificaLogin]

        public ActionResult CadastroEmpresas_Alterar(int id)
        {
            return PartialView(Bank.Empresas.AsNoTracking().First(x=>x.IDEmpresa == id)); //passa a empresa para a viw
        }       

        [HttpPost]
        
        [VerificaLogin]
        public ActionResult CadastroEmpresas_Alterar_Salvar(int id, int? IDFolha, String Endereco, String UF, String Cidade, String Cep, String Bairro, String CEI)
        {           
            try
            {
                Empresas Empresa = Bank.Empresas.FirstOrDefault(x => x.IDEmpresa == id);
                Empresa.IDFolha = IDFolha;
                Empresa.Endereco = String.IsNullOrEmpty(Endereco) ? null : Endereco;
                Empresa.UF = String.IsNullOrEmpty(UF) ? null : UF;
                Empresa.Cep = String.IsNullOrEmpty(Cep) ? null : Cep;
                Empresa.Cidade = String.IsNullOrEmpty(Cidade) ? null : Cidade; 
                Empresa.Bairro = String.IsNullOrEmpty(Bairro) ? null : Bairro;
                Empresa.CEI = String.IsNullOrEmpty(CEI) ? null : CEI;
                Bank.SaveChanges(); // SaveChanges as novas informações recebidas no obj
                return Json(new { status = true });
            }
            catch(Exception e)
            {
                return Json(new { status = false, msg = e.Message });
            }
        }
    }
}