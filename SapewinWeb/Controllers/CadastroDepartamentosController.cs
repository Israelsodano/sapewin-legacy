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
   
    public class CadastroDepartamentosController : Controller
    {
        Login.Models.LoginModel BankLogin = new Login.Models.LoginModel();
        MyContext Bank = new MyContext();

        [HttpGet]
        [VerificaLogin]
        public ActionResult CadastroDepartamentos_Abrir()
        {            
            Login.Models.LoginSistema UsuarioLogado = BankLogin
                .LoginSistema.AsNoTracking()
                .FirstOrDefault(x => x.IDLoginsistema == Convert.ToInt32(Session["Usuario"].ToString())); // carrega usuario logado

            int IDEmpresa = Bank.Empresas.First(x => Session["Empresa"].ToString().Contains(x.IDEmpresa.ToString("0000"))).IDEmpresa;

            ViewBag.PermissoesIndices = UsuarioLogado.PerfiMaster
                ? Bank.FuncoesdeTelas.AsNoTracking().Select(x => x.IDFuncaoTela).ToList()
                : Bank.PermissoesdeTelas.AsNoTracking().Where(x => x.IDUsuario == UsuarioLogado.IDLoginsistema && Session["Empresa"].ToString().Contains(x.IDEmpresa.ToString("0000"))).Select(x => x.IDFuncaoTela).ToList(); //carreha lista de funçoes de telas qiue o usuario possui

            List<Departamentos> Departamentos = UsuarioLogado.PerfiMaster ? Bank.Departamentos.Where(x => x.IDEmpresa == IDEmpresa).ToList() : Bank.PermissoesdeDepartamentos.Where(x => x.IDUsuario == UsuarioLogado.IDLoginsistema).Select(x => x.Departamento).ToList();

            int Total = Departamentos.Count;

            Departamentos = Departamentos.Count < 10 ? Departamentos : Departamentos.GetRange(0, 10); // cria pagina 

            ViewBag.Paginas = Math.Ceiling(Convert.ToDecimal(Convert.ToDouble(Total) / 10)); //conta paginas

            ViewBag.Total = $"Registro(s): {Total} - Exibindo de {(Departamentos.Count > 0 ? 1 : 0)} a {Departamentos.Count} - {ViewBag.Paginas} Página(s)"; //monta string do footer

           return VerificaLoad.IsAjax(HttpContext.Request) ? PartialView(Departamentos) : (ViewResultBase)View(Departamentos);            
        }

        [HttpGet]
        [VerificaLoad]
        public ActionResult CadastroDepartamentos_Abrir_Grid(String pesquisa, int Range, int Pagina, String Order, bool Condicao)
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

            List<Departamentos> Departamentos = new List<Models.Departamentos>();

            switch (Order.ToUpper())
            {
                case "CODIGO-UP":
                    if (Condicao || pesquisa == "")
                    {
                        Departamentos = UsuarioLogado.PerfiMaster ? Bank.Departamentos.Where(x => x.IDEmpresa == IDEmpresa && (x.IDDepartamento.ToString().StartsWith(pesquisa) || x.Nome.ToLower().StartsWith(pesquisa))).OrderBy(x => x.IDDepartamento).ToList() : Bank.PermissoesdeDepartamentos.Where(x => x.IDUsuario == UsuarioLogado.IDLoginsistema && x.IDEmpresa == IDEmpresa).Select(x => x.Departamento).Where(x => x.IDDepartamento.ToString().StartsWith(pesquisa) || x.Nome.ToLower().StartsWith(pesquisa)).OrderBy(x=>x.IDDepartamento).ToList();
                    }
                    else
                    {
                        Departamentos = UsuarioLogado.PerfiMaster ? Bank.Departamentos.Where(x => x.IDEmpresa == IDEmpresa && (!x.IDDepartamento.ToString().StartsWith(pesquisa) && !x.Nome.ToLower().StartsWith(pesquisa))).OrderBy(x => x.IDDepartamento).ToList() : Bank.PermissoesdeDepartamentos.Where(x => x.IDUsuario == UsuarioLogado.IDLoginsistema && x.IDEmpresa == IDEmpresa).Select(x => x.Departamento).Where(x => !x.IDDepartamento.ToString().StartsWith(pesquisa) && !x.Nome.ToLower().StartsWith(pesquisa)).OrderBy(x => x.IDDepartamento).ToList();
                    }
                    break;


                case "CODIGO-DOWN":

                    if (Condicao || pesquisa == "")
                    {
                        Departamentos = UsuarioLogado.PerfiMaster ? Bank.Departamentos.Where(x => x.IDEmpresa == IDEmpresa && (x.IDDepartamento.ToString().StartsWith(pesquisa) || x.Nome.ToLower().StartsWith(pesquisa))).OrderByDescending(x => x.IDDepartamento).ToList() : Bank.PermissoesdeDepartamentos.Where(x => x.IDUsuario == UsuarioLogado.IDLoginsistema && x.IDEmpresa == IDEmpresa).Select(x => x.Departamento).Where(x => x.IDDepartamento.ToString().StartsWith(pesquisa) || x.Nome.ToLower().StartsWith(pesquisa)).OrderByDescending(x => x.IDDepartamento).ToList();
                    }
                    else
                    {
                        Departamentos = UsuarioLogado.PerfiMaster ? Bank.Departamentos.Where(x => x.IDEmpresa == IDEmpresa && (!x.IDDepartamento.ToString().StartsWith(pesquisa) && !x.Nome.ToLower().StartsWith(pesquisa))).OrderByDescending(x => x.IDDepartamento).ToList() : Bank.PermissoesdeDepartamentos.Where(x => x.IDUsuario == UsuarioLogado.IDLoginsistema && x.IDEmpresa == IDEmpresa).Select(x => x.Departamento).Where(x => !x.IDDepartamento.ToString().StartsWith(pesquisa) && !x.Nome.ToLower().StartsWith(pesquisa)).OrderByDescending(x => x.IDDepartamento).ToList();
                    }
                    break;

                case "DESCRICAO-UP":

                    if (Condicao || pesquisa == "")
                    {
                        Departamentos = UsuarioLogado.PerfiMaster ? Bank.Departamentos.Where(x => x.IDEmpresa == IDEmpresa && (x.IDDepartamento.ToString().StartsWith(pesquisa) || x.Nome.ToLower().StartsWith(pesquisa))).OrderBy(x => x.Nome).ToList() : Bank.PermissoesdeDepartamentos.Where(x => x.IDUsuario == UsuarioLogado.IDLoginsistema && x.IDEmpresa == IDEmpresa).Select(x => x.Departamento).Where(x => x.IDDepartamento.ToString().StartsWith(pesquisa) || x.Nome.ToLower().StartsWith(pesquisa)).OrderBy(x => x.Nome).ToList();
                    }
                    else
                    {
                        Departamentos = UsuarioLogado.PerfiMaster ? Bank.Departamentos.Where(x => x.IDEmpresa == IDEmpresa && (!x.IDDepartamento.ToString().StartsWith(pesquisa) && !x.Nome.ToLower().StartsWith(pesquisa))).OrderBy(x => x.Nome).ToList() : Bank.PermissoesdeDepartamentos.Where(x => x.IDUsuario == UsuarioLogado.IDLoginsistema && x.IDEmpresa == IDEmpresa).Select(x => x.Departamento).Where(x => !x.IDDepartamento.ToString().StartsWith(pesquisa) && !x.Nome.ToLower().StartsWith(pesquisa)).OrderByDescending(x => x.Nome).ToList();
                    }
                    break;

                case "DESCRICAO-DOWN":

                    if (Condicao || pesquisa == "")
                    {
                        Departamentos = UsuarioLogado.PerfiMaster ? Bank.Departamentos.Where(x => x.IDEmpresa == IDEmpresa && (x.IDDepartamento.ToString().StartsWith(pesquisa) || x.Nome.ToLower().StartsWith(pesquisa))).OrderByDescending(x => x.Nome).ToList() : Bank.PermissoesdeDepartamentos.Where(x => x.IDUsuario == UsuarioLogado.IDLoginsistema && x.IDEmpresa == IDEmpresa).Select(x => x.Departamento).Where(x => x.IDDepartamento.ToString().StartsWith(pesquisa) || x.Nome.ToLower().StartsWith(pesquisa)).OrderByDescending(x => x.Nome).ToList();
                    }
                    else
                    {
                        Departamentos = UsuarioLogado.PerfiMaster ? Bank.Departamentos.Where(x => x.IDEmpresa == IDEmpresa && (!x.IDDepartamento.ToString().StartsWith(pesquisa) && !x.Nome.ToLower().StartsWith(pesquisa))).OrderByDescending(x => x.Nome).ToList() : Bank.PermissoesdeDepartamentos.Where(x => x.IDUsuario == UsuarioLogado.IDLoginsistema && x.IDEmpresa == IDEmpresa).Select(x => x.Departamento).Where(x => !x.IDDepartamento.ToString().StartsWith(pesquisa) && !x.Nome.ToLower().StartsWith(pesquisa)).OrderByDescending(x => x.Nome).ToList();
                    }
                    break;

                default:
                    break;
            }      

            int Total = Departamentos.Count; // lista total

            ViewBag.Paginas = Math.Ceiling(Convert.ToDecimal(Convert.ToDouble(Total) / Range)); // conta paginas

            Pagina = Pagina > ViewBag.Paginas ? Convert.ToInt32(ViewBag.Paginas) - 1 : Pagina; // trata a pagina para nn ser maior que as paginas possiveis do obj

            Pagina = Departamentos.Count < Range ? Pagina * Departamentos.Count : Pagina * Range; // trata pagina para ser o primeiro registro da pagina

            Range = Range + Pagina > Departamentos.Count ? Departamentos.Count - Pagina : Range; // trata o range para nn ser maior que o range do obj

            Departamentos = Departamentos.Count < Range ? Departamentos : Departamentos.GetRange(Pagina, Range); // monta pagina

            ViewBag.Total = $"Registro(s): {Total} - Exibindo de {(Pagina == 0 ? 1 : Pagina + 1)} a {(Pagina == 0 ? Range : Pagina + Range)} - { ViewBag.Paginas} Página(s)"; // monta string do footer

            return PartialView(Departamentos);
        }

        public static bool VerificaFuncionarios(long id)
        {
            MyContext Bank = new MyContext();
            return Bank.Departamentos.Include(x => x.Funcionarios).FirstOrDefault(x => x.IDDepartamento == id).Funcionarios.Count == 0;
        }

        [HttpGet]
        [VerificaLogin]
        public ActionResult CadastroDepartamentos_Incluir()
        {
            return PartialView();
        }

        [HttpPost]        
        [VerificaLoad]
        
        public ActionResult CadastroDepartamentos_Incluir_Salvar(long? IDDepartamento, String Nome)
        {
            if (String.IsNullOrEmpty(Nome))
            {
                return Json(new { status = false, msg = "Preencha todos os campos" });
            }
            else
            {
                try
                {
                    if (IDDepartamento.HasValue && Bank.Departamentos.Where(x=>x.IDDepartamento == IDDepartamento.Value && Session["Empresa"].ToString().Contains(x.IDEmpresa.ToString("0000"))).Count() > 0)
                    {
                        throw new Exception("Já existe um Departamento com esse código");
                    }

                    Login.Models.LoginSistema UsuarioLogado = BankLogin
                    .LoginSistema.AsNoTracking()
                    .Include(x => x.Cliente)
                    .ThenInclude(x=>x.LoginSistema).AsNoTracking()
                    .FirstOrDefault(x => x.IDLoginsistema == Convert.ToInt32(Session["Usuario"].ToString())); // carrega usuario logado

                    Departamentos Departamento = new Departamentos
                    {
                        IDDepartamento = IDDepartamento == null ? Bank.Departamentos.Where(x => Session["Empresa"].ToString().Contains(x.IDEmpresa.ToString("0000"))).OrderBy(x => x.IDDepartamento).LastOrDefault() == null ? 1 : Bank.Departamentos.Where(x => Session["Empresa"].ToString().Contains(x.IDEmpresa.ToString("0000"))).OrderBy(x => x.IDDepartamento).LastOrDefault().IDDepartamento + 1 : Convert.ToInt64(IDDepartamento),
                        IDEmpresa = Bank.Empresas.FirstOrDefault(x => Session["Empresa"].ToString().Contains(x.IDEmpresa.ToString("0000"))).IDEmpresa,
                        Empresa = Bank.Empresas.FirstOrDefault(x => Session["Empresa"].ToString().Contains(x.IDEmpresa.ToString("0000"))),
                        Nome = Nome,                                            
                    };

                    foreach (var Usuario in UsuarioLogado.Cliente.LoginSistema.ToList())
                    {
                        Bank.PermissoesdeDepartamentos.Add(new PermissoesdeDepartamentos
                        {
                            IDEmpresa = Departamento.IDEmpresa,
                            Empresa = Departamento.Empresa,
                            IDUsuario = Usuario.IDLoginsistema,
                            Departamento = Departamento,
                            IDDepartamento = Departamento.IDDepartamento
                        });
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
        public ActionResult CadastroDepartamentos_Alterar(int id)
        {
            Login.Models.LoginSistema UsuarioLogado = BankLogin
                      .LoginSistema.AsNoTracking()
                      .Include(x => x.Cliente)
                      .FirstOrDefault(x => x.IDLoginsistema == Convert.ToInt32(Session["Usuario"].ToString())); // carrega usuario logado


            if (!UsuarioLogado.PerfiMaster && Bank.PermissoesdeDepartamentos.Where(x => x.IDDepartamento == id && x.IDUsuario == UsuarioLogado.IDLoginsistema).Count() == 0)
            {
                return new HttpUnauthorizedResult();
            }

            Departamentos Departamento = Bank.Departamentos.AsNoTracking().FirstOrDefault(x => x.IDDepartamento == id && Session["Empresa"].ToString().Contains(x.IDEmpresa.ToString("0000")));
            return PartialView(Departamento); //retorna obj por id e sessao para a view
        }

        [HttpPost]
        
        [VerificaLogin]
        public ActionResult CadastroDepartamentos_Alterar_Salvar(int id, String Nome)
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
                     .Include(x => x.Cliente)
                     .FirstOrDefault(x => x.IDLoginsistema == Convert.ToInt32(Session["Usuario"].ToString())); // carrega usuario logado


                    if (!UsuarioLogado.PerfiMaster && Bank.PermissoesdeDepartamentos.Where(x => x.IDDepartamento == id && x.IDUsuario == UsuarioLogado.IDLoginsistema).Count() == 0)
                    {
                        return new HttpUnauthorizedResult();
                    }

                    Departamentos Departamento = Bank.Departamentos.First(x => x.IDDepartamento == id && Session["Empresa"].ToString().Contains(x.IDEmpresa.ToString("0000")));
                    Departamento.Nome = Nome;
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
        public ActionResult CadastroDepartamentos_Remover(int id)
        {
            Login.Models.LoginSistema UsuarioLogado = BankLogin
                     .LoginSistema.AsNoTracking()
                     .Include(x => x.Cliente)
                     .FirstOrDefault(x => x.IDLoginsistema == Convert.ToInt32(Session["Usuario"].ToString())); // carrega usuario logado


            if (!UsuarioLogado.PerfiMaster && Bank.PermissoesdeDepartamentos.Where(x => x.IDDepartamento == id && x.IDUsuario == UsuarioLogado.IDLoginsistema).Count() == 0)
            {
                return new HttpUnauthorizedResult();
            }

            Departamentos Departamento = Bank.Departamentos.AsNoTracking().Include(x => x.Funcionarios).AsNoTracking().First(x => x.IDDepartamento == id && Session["Empresa"].ToString().Contains(x.IDEmpresa.ToString("0000")));

            if (Departamento.Funcionarios.Count > 0)
            {
                throw new Exception("Html Alterado");
            }

            return PartialView(Departamento); //retorna obj por id e sessao para a view
        }

        [HttpPost]
        
        [VerificaLogin]
        public ActionResult CadastroDepartamentos_Remover_Salvar(int id)
        {
            try
            {
                Login.Models.LoginSistema UsuarioLogado = BankLogin
                     .LoginSistema.AsNoTracking()
                     .Include(x => x.Cliente)
                     .FirstOrDefault(x => x.IDLoginsistema == Convert.ToInt32(Session["Usuario"].ToString())); // carrega usuario logado


                if (!UsuarioLogado.PerfiMaster && Bank.PermissoesdeDepartamentos.Where(x => x.IDDepartamento == id && x.IDUsuario == UsuarioLogado.IDLoginsistema).Count() == 0)
                {
                    return new HttpUnauthorizedResult();
                }

                Departamentos Departamento = Bank.Departamentos.Include(x => x.Funcionarios).AsNoTracking().First(x => x.IDDepartamento == id && Session["Empresa"].ToString().Contains(x.IDEmpresa.ToString("0000")));
                if (Departamento.Funcionarios.Count > 0)
                {
                    throw new Exception("Html Alterado");
                }

                Bank.Departamentos.Remove(Departamento);
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
        public ActionResult CadastroDepartamentos_Remover_Selecao(long[] id)
        {
            Login.Models.LoginSistema UsuarioLogado = BankLogin
                     .LoginSistema.AsNoTracking()
                     .Include(x => x.Cliente)
                     .FirstOrDefault(x => x.IDLoginsistema == Convert.ToInt32(Session["Usuario"].ToString())); // carrega usuario logado


            if (!UsuarioLogado.PerfiMaster && Bank.PermissoesdeDepartamentos.Where(x => id.Contains(x.IDDepartamento) && x.IDUsuario == UsuarioLogado.IDLoginsistema).Count() == 0)
            {
                return new HttpUnauthorizedResult();
            }

            List<Departamentos> Departamentos = Bank.Departamentos.AsNoTracking().Where(x => id.Contains(x.IDDepartamento)).ToList();
            return PartialView(Departamentos);
        }

        [HttpPost]
        
        [VerificaLogin]
        public ActionResult CadastroDepartamentos_Remover_Selecao_Salvar(long[] id)
        {
            try
            {
                Login.Models.LoginSistema UsuarioLogado = BankLogin
                     .LoginSistema.AsNoTracking()
                     .Include(x => x.Cliente)
                     .FirstOrDefault(x => x.IDLoginsistema == Convert.ToInt32(Session["Usuario"].ToString())); // carrega usuario logado


                if (!UsuarioLogado.PerfiMaster && Bank.PermissoesdeDepartamentos.Where(x => id.Contains(x.IDDepartamento) && x.IDUsuario == UsuarioLogado.IDLoginsistema).Count() == 0)
                {
                    return new HttpUnauthorizedResult();
                }

                List<Departamentos> Departamento = Bank.Departamentos.Include(x=>x.Funcionarios).AsNoTracking().Where(x => id.Contains(x.IDDepartamento)).ToList();
                if (Departamento.Select(x=>x.Funcionarios).Count() > 0)
                {
                    throw new Exception("Não é possivel Remover um Departamento que tem Funcionarios");
                }

                Bank.Departamentos.RemoveRange(Departamento);                    
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