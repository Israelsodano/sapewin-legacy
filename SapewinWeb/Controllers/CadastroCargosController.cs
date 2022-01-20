using Microsoft.EntityFrameworkCore;
using SapewinWeb.Metodos;
using SapewinWeb.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SapewinWeb.Controllers
{
   
    
    public class CadastroCargosController : Controller
    {
        Login.Models.LoginModel BankLogin = new Login.Models.LoginModel();
        MyContext Bank = new MyContext();

        [HttpGet]
        [VerificaLogin]
        public ActionResult CadastroCargos_Abrir()
        {
            Login.Models.LoginSistema UsuarioLogado = BankLogin
                .LoginSistema
                .FirstOrDefault(x => x.IDLoginsistema == Convert.ToInt32(Session["Usuario"].ToString())); // carrega usuario logado

            ViewBag.PermissoesIndices = UsuarioLogado.PerfiMaster
              ? Bank.FuncoesdeTelas.Select(x => x.IDFuncaoTela).ToList()
              : Bank.PermissoesdeTelas.Where(x => x.IDUsuario == UsuarioLogado.IDLoginsistema && Session["Empresa"].ToString().Contains(x.IDEmpresa.ToString("0000"))).Select(x => x.IDFuncaoTela).ToList(); //carreha lista de funçoes de telas qiue o usuario possui

            List<Cargos> Cargos = Bank.Cargos.AsNoTracking().Include(x => x.Funcionarios).ToList(); // Carrega Lista de Cargos do sistema

            int Total = Cargos.Count;

            Cargos = Cargos.Count < 10 ? Cargos : Cargos.GetRange(0, 10); // cria pagina 

            ViewBag.Paginas = Math.Ceiling(Convert.ToDecimal(Convert.ToDouble(Total) / 10)); //conta paginas

            ViewBag.Total = $"Registro(s): {Total} - Exibindo de {(Cargos.Count > 0 ? 1 : 0)} a {Cargos.Count} - {ViewBag.Paginas} Página(s)"; //monta string do footer

            return VerificaLoad.IsAjax(HttpContext.Request) ? PartialView(Cargos) : (ViewResultBase)View(Cargos);

        }

        [HttpGet]  
        [VerificaLoad]      
        public ActionResult CadastroCargos_Abrir_Grid(String pesquisa, int Range, int Pagina, String Order, bool Condicao)
        {         
            if (String.IsNullOrEmpty(pesquisa)) { pesquisa = ""; }

            pesquisa = pesquisa.ToLower().Replace('+', ' ');

            Login.Models.LoginSistema UsuarioLogado = BankLogin
                .LoginSistema
                .FirstOrDefault(x => x.IDLoginsistema == Convert.ToInt32(Session["Usuario"].ToString())); // carrega usuario logado 

            ViewBag.PermissoesIndices = UsuarioLogado.PerfiMaster
             ? Bank.FuncoesdeTelas.AsNoTracking().Select(x => x.IDFuncaoTela).ToList()
             : Bank.PermissoesdeTelas.AsNoTracking().Where(x => x.IDUsuario == UsuarioLogado.IDLoginsistema && Session["Empresa"].ToString().Contains(x.IDEmpresa.ToString("0000"))).Select(x => x.IDFuncaoTela).ToList(); //carreha lista de funçoes de telas qiue o usuario possui

            List<Cargos> Cargos = new List<Models.Cargos>();

            switch (Order.ToUpper())
            {
                case "CODIGO-UP":

                    if (Condicao || pesquisa == "")
                    {
                        Cargos = Bank
                       .Cargos
                       .Include(x => x.Funcionarios).AsNoTracking()
                       .Where(x => x.Nome.ToLower().Contains(pesquisa.ToLower()) || x.IDCargo.ToString().Contains(pesquisa)).OrderBy(x => x.IDCargo).ToList();
                    }
                    else
                    {
                        Cargos = Bank
                       .Cargos
                       .Include(x => x.Funcionarios).AsNoTracking()
                       .Where(x => !x.Nome.ToLower().Contains(pesquisa.ToLower()) && !x.IDCargo.ToString().Contains(pesquisa)).OrderBy(x => x.IDCargo).ToList();
                    }
                    break;

                case "CODIGO-DOWN":

                    if (Condicao || pesquisa == "")
                    {
                        Cargos = Bank
                       .Cargos
                       .Include(x => x.Funcionarios).AsNoTracking()
                       .Where(x => x.Nome.ToLower().Contains(pesquisa.ToLower()) || x.IDCargo.ToString().Contains(pesquisa)).OrderByDescending(x => x.IDCargo).ToList();
                    }
                    else
                    {
                        Cargos = Bank
                       .Cargos
                       .Include(x => x.Funcionarios).AsNoTracking()
                       .Where(x => !x.Nome.ToLower().Contains(pesquisa.ToLower()) && !x.IDCargo.ToString().Contains(pesquisa)).OrderByDescending(x => x.IDCargo).ToList();
                    }
                    break;

                case "DESCRICAO-UP":

                    if (Condicao || pesquisa == "")
                    {
                        Cargos = Bank
                       .Cargos
                       .Include(x => x.Funcionarios).AsNoTracking()
                       .Where(x => x.Nome.ToLower().Contains(pesquisa.ToLower()) || x.IDCargo.ToString().Contains(pesquisa)).OrderBy(x => x.Nome).ToList();
                    }
                    else
                    {
                        Cargos = Bank
                       .Cargos
                       .Include(x => x.Funcionarios).AsNoTracking()
                       .Where(x => !x.Nome.ToLower().Contains(pesquisa.ToLower()) && !x.IDCargo.ToString().Contains(pesquisa)).OrderBy(x => x.Nome).ToList();
                    }
                    break;

                case "DESCRICAO-DOWN":

                    if (Condicao || pesquisa == "")
                    {
                        Cargos = Bank
                       .Cargos
                       .Include(x => x.Funcionarios).AsNoTracking()
                       .Where(x => x.Nome.ToLower().Contains(pesquisa.ToLower()) || x.IDCargo.ToString().Contains(pesquisa)).OrderByDescending(x => x.Nome).ToList();
                    }
                    else
                    {
                        Cargos = Bank
                       .Cargos
                       .Include(x => x.Funcionarios).AsNoTracking()
                       .Where(x => !x.Nome.ToLower().Contains(pesquisa.ToLower()) && !x.IDCargo.ToString().Contains(pesquisa)).OrderByDescending(x => x.Nome).ToList();
                    }
                    break;

                default:
                    break;
            }

            int Total = Cargos.Count; // lista total

            ViewBag.Paginas = Math.Ceiling(Convert.ToDecimal(Convert.ToDouble(Total) / Range)); // conta paginas

            Pagina = Pagina > ViewBag.Paginas ? Convert.ToInt32(ViewBag.Paginas) - 1 : Pagina; // trata a pagina para nn ser maior que as paginas possiveis do obj

            Pagina = Cargos.Count < Range ? Pagina * Cargos.Count : Pagina * Range; // trata pagina para ser o primeiro registro da pagina

            Range = Range + Pagina > Cargos.Count ? Cargos.Count - Pagina : Range; // trata o range para nn ser maior que o range do obj

            Cargos = Cargos.Count < Range ? Cargos : Cargos.GetRange(Pagina, Range); // monta pagina

            ViewBag.Total = $"Registro(s): {Total} - Exibindo de {(Pagina == 0 ? 1 : Pagina + 1)} a {(Pagina == 0 ? Range : Pagina + Range)} - { ViewBag.Paginas} Página(s)"; // monta string do footer

            return PartialView(Cargos);
        }

        [HttpGet]
        [VerificaLogin]
        public ActionResult CadastroCargos_Incluir()
        {
            return PartialView();
        }

        [HttpPost]
        
        [VerificaLogin]
        public ActionResult CadastroCargos_Incluir_Salvar(int? IDCargo, String Nome)
        {
            if (String.IsNullOrEmpty(Nome))
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

                    if (IDCargo.HasValue && Bank.Cargos.Where(x=>x.IDCargo == IDCargo.Value).Count() > 0)
                    {
                        throw new Exception("Já existe um Cargo com esse código");
                    }

                    Cargos Cargo = new Cargos
                    {
                        IDCargo = IDCargo == null ? Bank.Cargos.AsNoTracking().OrderBy(x => x.IDCargo).LastOrDefault() == null ? 1 : Bank.Cargos.AsNoTracking().OrderBy(x => x.IDCargo).LastOrDefault().IDCargo + 1 : Convert.ToInt32(IDCargo),
                        Nome = Nome
                    };

                    Bank.Cargos.Add(Cargo);
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
        public ActionResult CadastroCargos_Alterar(int id)
        {
            Cargos Cargo = Bank.Cargos.AsNoTracking().FirstOrDefault(x => x.IDCargo == id);
            return PartialView(Cargo); //retorna obj por id e sessao para a view
        }

        [HttpPost]
        
        [VerificaLogin]
        public ActionResult CadastroCargos_Alterar_Salvar(int id, String Nome)
        {
            if (String.IsNullOrEmpty(Nome))
            {
                return Json(new { status = false, msg = "Preencha todos os campos" });
            }
            else
            {
                try
                {
                    Cargos Cargo = Bank.Cargos.First(x => x.IDCargo == id);
                    Cargo.Nome = Nome;
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
        public ActionResult CadastroCargos_Remover(int id)
        {
            Cargos Cargo = Bank.Cargos.AsNoTracking().Include(x => x.Funcionarios).AsNoTracking().First(x => x.IDCargo == id);

            if (Cargo.Funcionarios.Count > 0)
            {
                throw new Exception("Html Alterado");
            }

            return PartialView(Cargo); //retorna obj por id e sessao para a view
        }

        [HttpPost]
        
        [VerificaLogin]       
        public ActionResult CadastroCargos_Remover_Salvar(int id)
        {
            try
            {
                Cargos Cargo = Bank.Cargos.Include(x => x.Funcionarios).AsNoTracking().Include(x=>x.Funcionarios).First(x => x.IDCargo == id);

                if (Cargo.Funcionarios.Count > 0)
                {
                    throw new Exception("Html Alterado");
                }

                Bank.Cargos.Remove(Cargo);
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
        public ActionResult CadastroCargos_Remover_Selecao(int[] id)
        {
            List<Cargos> Cargos = Bank.Cargos.AsNoTracking().Where(x => id.Contains(x.IDCargo)).ToList();
            return PartialView(Cargos);
        }

        [HttpPost]
        
        [VerificaLogin]
        
        public ActionResult CadastroCargos_Remover_Selecao_Salvar(int[] id)
        {
            try
            {
               
                List<Cargos> Cargo = Bank.Cargos.Include(x => x.Funcionarios).AsNoTracking().Where(x => id.Contains(x.IDCargo)).ToList();
                
                if (Cargo.Where(x => x.Funcionarios.Count > 0).Count() > 0)
                {
                    throw new Exception("Não é possivel Remover um Cargo que tem Funcionarios...");
                }

                Bank.Cargos.RemoveRange(Cargo);                    
                
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