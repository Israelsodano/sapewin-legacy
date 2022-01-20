using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SapewinWeb.Metodos;
using Login.Models;
using SapewinWeb.Models;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json.Linq;
using System.Web.Script.Serialization;

namespace SapewinWeb.Controllers
{
    public class CadastroUsuariosController : Controller
    {
        LoginModel BankLogin = new LoginModel();

        MyContext Bank = new MyContext();


        [HttpGet]
        [VerificaLogin]
        public ActionResult CadastroUsuarios_Abrir()
        {
            LoginSistema UsuarioLogado = BankLogin
                .LoginSistema
                .FirstOrDefault(x => x.IDLoginsistema == Convert.ToInt32(Session["Usuario"].ToString())); // carrega usuario logado

            ViewBag.PermissoesIndices = UsuarioLogado.PerfiMaster
              ? Bank.FuncoesdeTelas.Select(x => x.IDFuncaoTela).ToList()
              : Bank.PermissoesdeTelas.Where(x => x.IDUsuario == UsuarioLogado.IDLoginsistema && Session["Empresa"].ToString().Contains(x.IDEmpresa.ToString("0000"))).Select(x => x.IDFuncaoTela).ToList(); //carreha lista de funçoes de telas qiue o usuario possui
            
            List<LoginSistema> Usuarios = BankLogin.LoginSistema.AsNoTracking().Where(x => !x.PerfiMaster && x.IDCliente == UsuarioLogado.IDCliente && x.Tipo > UsuarioLogado.Tipo).ToList(); // Carrega Lista de Usuarios do sistema

            int Total = Usuarios.Count;

            Usuarios = Usuarios.Count < 10 ? Usuarios : Usuarios.GetRange(0, 10); // cria pagina 

            ViewBag.Paginas = Math.Ceiling(Convert.ToDecimal(Convert.ToDouble(Total) / 10)); //conta paginas

            ViewBag.Total = $"Registro(s): {Total} - Exibindo de {(Usuarios.Count > 0 ? 1 : 0)} a {Usuarios.Count} - {ViewBag.Paginas} Página(s)"; //monta string do footer

            return VerificaLoad.IsAjax(HttpContext.Request) ? PartialView(Usuarios) : (ViewResultBase)View(Usuarios);

        }

        [HttpGet]
        [VerificaLogin]
        public ActionResult CadastroUsuarios_Abrir_Grid(String pesquisa, int Range, int Pagina, String Order, bool Condicao)
        {
            if (String.IsNullOrEmpty(pesquisa)) { pesquisa = ""; }

            pesquisa = pesquisa.ToLower().Replace('+', ' ');

            LoginSistema UsuarioLogado = BankLogin
                .LoginSistema
                .FirstOrDefault(x => x.IDLoginsistema == Convert.ToInt32(Session["Usuario"].ToString())); // carrega usuario logado 

            ViewBag.PermissoesIndices = UsuarioLogado.PerfiMaster
             ? Bank.FuncoesdeTelas.AsNoTracking().Select(x => x.IDFuncaoTela).ToList()
             : Bank.PermissoesdeTelas.AsNoTracking().Where(x => x.IDUsuario == UsuarioLogado.IDLoginsistema && Session["Empresa"].ToString().Contains(x.IDEmpresa.ToString("0000"))).Select(x => x.IDFuncaoTela).ToList(); //carreha lista de funçoes de telas qiue o usuario possui

            List<LoginSistema> Usuarios = new List<LoginSistema>();

            switch (Order.ToUpper())
            {
                case "DESCRICAO-UP":

                    if (Condicao || pesquisa == "")
                    {
                        Usuarios = BankLogin
                       .LoginSistema.AsNoTracking()
                       .Where(x => (x.Nome.ToLower().Contains(pesquisa.ToLower()) || x.Login.ToLower().Contains(pesquisa.ToLower()) || x.IDLoginsistema.ToString().Contains(pesquisa)) && x.IDCliente == UsuarioLogado.IDCliente && !x.PerfiMaster && x.Tipo > UsuarioLogado.Tipo).OrderBy(x => x.Login).ToList();
                    }
                    else
                    {
                        Usuarios = BankLogin
                       .LoginSistema.AsNoTracking()
                       .Where(x => (!x.Nome.ToLower().Contains(pesquisa.ToLower()) && x.Login.ToLower().Contains(pesquisa.ToLower()) && !x.IDLoginsistema.ToString().Contains(pesquisa)) && x.IDCliente == UsuarioLogado.IDCliente && !x.PerfiMaster && x.Tipo > UsuarioLogado.Tipo).OrderBy(x => x.Login).ToList();
                    }
                    break;

                case "DESCRICAO-DOWN":

                    if (Condicao || pesquisa == "")
                    {
                        Usuarios = BankLogin
                       .LoginSistema.AsNoTracking()
                       .Where(x => (x.Login.ToLower().Contains(pesquisa.ToLower()) || x.Nome.ToLower().Contains(pesquisa.ToLower()) || x.IDLoginsistema.ToString().Contains(pesquisa)) && x.IDCliente == UsuarioLogado.IDCliente && !x.PerfiMaster && x.Tipo > UsuarioLogado.Tipo).OrderByDescending(x => x.Login).ToList();
                    }
                    else
                    {
                        Usuarios = BankLogin
                       .LoginSistema.AsNoTracking()
                       .Where(x => (!x.Nome.ToLower().Contains(pesquisa.ToLower()) && !x.Login.ToLower().Contains(pesquisa.ToLower()) && !x.IDLoginsistema.ToString().Contains(pesquisa)) && x.IDCliente == UsuarioLogado.IDCliente && !x.PerfiMaster && x.Tipo > UsuarioLogado.Tipo).OrderByDescending(x => x.Login).ToList();
                    }
                    break;

                case "TIPO-UP":

                    if (Condicao || pesquisa == "")
                    {
                        Usuarios = BankLogin
                       .LoginSistema.AsNoTracking()
                       .Where(x => (x.Nome.ToLower().Contains(pesquisa.ToLower()) || x.Login.ToLower().Contains(pesquisa.ToLower()) || x.IDLoginsistema.ToString().Contains(pesquisa)) && x.IDCliente == UsuarioLogado.IDCliente && !x.PerfiMaster && x.Tipo > UsuarioLogado.Tipo).OrderBy(x => x.Tipo).ToList();
                    }
                    else
                    {
                        Usuarios = BankLogin
                       .LoginSistema.AsNoTracking()
                       .Where(x => (!x.Nome.ToLower().Contains(pesquisa.ToLower()) && !x.Login.ToLower().Contains(pesquisa.ToLower()) && !x.IDLoginsistema.ToString().Contains(pesquisa)) && x.IDCliente == UsuarioLogado.IDCliente && !x.PerfiMaster && x.Tipo > UsuarioLogado.Tipo).OrderBy(x => x.Tipo).ToList();
                    }
                    break;

                case "TIPO-DOWN":

                    if (Condicao || pesquisa == "")
                    {
                        Usuarios = BankLogin
                       .LoginSistema.AsNoTracking()
                       .Where(x => (x.Nome.ToLower().Contains(pesquisa.ToLower()) || x.Login.ToLower().Contains(pesquisa.ToLower()) || x.IDLoginsistema.ToString().Contains(pesquisa)) && x.IDCliente == UsuarioLogado.IDCliente && !x.PerfiMaster && x.Tipo > UsuarioLogado.Tipo).OrderByDescending(x => x.Tipo).ToList();
                    }
                    else
                    {
                        Usuarios = BankLogin
                       .LoginSistema.AsNoTracking()
                       .Where(x => (!x.Nome.ToLower().Contains(pesquisa.ToLower()) && x.Login.ToLower().Contains(pesquisa.ToLower()) && !x.IDLoginsistema.ToString().Contains(pesquisa)) && x.IDCliente == UsuarioLogado.IDCliente && !x.PerfiMaster && x.Tipo > UsuarioLogado.Tipo).OrderByDescending(x => x.Tipo).ToList();
                    }
                    break;

                case "NOME-UP":

                    if (Condicao || pesquisa == "")
                    {
                        Usuarios = BankLogin
                       .LoginSistema.AsNoTracking()
                       .Where(x => (x.Nome.ToLower().Contains(pesquisa.ToLower()) || x.Login.ToLower().Contains(pesquisa.ToLower()) || x.IDLoginsistema.ToString().Contains(pesquisa)) && x.IDCliente == UsuarioLogado.IDCliente && !x.PerfiMaster && x.Tipo > UsuarioLogado.Tipo).OrderBy(x => x.Nome).ToList();
                    }
                    else
                    {
                        Usuarios = BankLogin
                       .LoginSistema.AsNoTracking()
                       .Where(x => (!x.Nome.ToLower().Contains(pesquisa.ToLower()) && !x.Login.ToLower().Contains(pesquisa.ToLower()) && !x.IDLoginsistema.ToString().Contains(pesquisa)) && x.IDCliente == UsuarioLogado.IDCliente && !x.PerfiMaster && x.Tipo > UsuarioLogado.Tipo).OrderBy(x => x.Nome).ToList();
                    }
                    break;

                case "NOME-DOWN":

                    if (Condicao || pesquisa == "")
                    {
                        Usuarios = BankLogin
                       .LoginSistema.AsNoTracking()
                       .Where(x => (x.Login.ToLower().Contains(pesquisa.ToLower()) || x.Nome.ToLower().Contains(pesquisa.ToLower()) || x.IDLoginsistema.ToString().Contains(pesquisa)) && x.IDCliente == UsuarioLogado.IDCliente && !x.PerfiMaster && x.Tipo > UsuarioLogado.Tipo).OrderByDescending(x => x.Nome).ToList();
                    }
                    else
                    {
                        Usuarios = BankLogin
                       .LoginSistema.AsNoTracking()
                       .Where(x => (!x.Nome.ToLower().Contains(pesquisa.ToLower()) && !x.Login.ToLower().Contains(pesquisa.ToLower()) && !x.IDLoginsistema.ToString().Contains(pesquisa)) && x.IDCliente == UsuarioLogado.IDCliente && !x.PerfiMaster && x.Tipo > UsuarioLogado.Tipo).OrderByDescending(x => x.Nome).ToList();
                    }
                    break;


                default:
                    break;
            }            

            int Total = Usuarios.Count; // lista total

            ViewBag.Paginas = Math.Ceiling(Convert.ToDecimal(Convert.ToDouble(Total) / Range)); // conta paginas

            Pagina = Pagina > ViewBag.Paginas ? Convert.ToInt32(ViewBag.Paginas) - 1 : Pagina; // trata a pagina para nn ser maior que as paginas possiveis do obj

            Pagina = Usuarios.Count < Range ? Pagina * Usuarios.Count : Pagina * Range; // trata pagina para ser o primeiro registro da pagina

            Range = Range + Pagina > Usuarios.Count ? Usuarios.Count - Pagina : Range; // trata o range para nn ser maior que o range do obj

            Usuarios = Usuarios.Count < Range ? Usuarios : Usuarios.GetRange(Pagina, Range); // monta pagina

            ViewBag.Total = $"Registro(s): {Total} - Exibindo de {(Pagina == 0 ? 1 : Pagina + 1)} a {(Pagina == 0 ? Range : Pagina + Range)} - { ViewBag.Paginas} Página(s)"; // monta string do footer

            return PartialView(Usuarios);
        }

        [HttpGet]
        [VerificaLogin]
        public ActionResult CadastroUsuarios_Incluir()
        {
            Login.Models.LoginSistema UsuarioLogado = BankLogin
                 .LoginSistema.AsNoTracking()
                 .FirstOrDefault(x => x.IDLoginsistema == Convert.ToInt32(Session["Usuario"].ToString())); // Carrega Usuario Logado

            return PartialView(UsuarioLogado);
        }

        [HttpGet]
        [VerificaLogin]
        public ActionResult CadastroUsuarios_Alterar(int id)
        {
            LoginSistema Usuario = BankLogin.LoginSistema.First(x => x.IDLoginsistema == id);
            return PartialView(Usuario);
        }

        [HttpGet]
        [VerificaLogin]
        public ActionResult CadastroUsuarios_Remover(int id)
        {
            if (id == Convert.ToInt32(Session["Usuario"]))
            {
                throw new Exception("Código Html Alterado");
            }

            LoginSistema Usuario = BankLogin.LoginSistema.FirstOrDefault(x => x.IDLoginsistema == id);            
            
            return PartialView(Usuario);
        }

        [HttpPost]
        
        [VerificaLogin]
        public ActionResult CadastroUsuarios_Remover_Salvar(int id)
        {

            try
            {
                if (id == Convert.ToInt32(Session["Usuario"]))
                {
                    throw new Exception("Código Html Alterado");
                }

                LoginSistema Usuario = BankLogin.LoginSistema.FirstOrDefault(x => x.IDLoginsistema == id);

                List<PermissoesdeTelas> PermissoesTelas = Bank.PermissoesdeTelas.Where(x => x.IDUsuario == Usuario.IDLoginsistema).ToList();
                Bank.PermissoesdeTelas.RemoveRange(PermissoesTelas);

                List<PermissoesdeSetores> PermissoesSetores = Bank.PermissoesdeSetores.Where(x => x.IDUsuario == Usuario.IDLoginsistema).ToList();
                Bank.PermissoesdeSetores.RemoveRange(PermissoesSetores);

                List<PermissoesdeDepartamentos> PermissoesDepartamentos = Bank.PermissoesdeDepartamentos.Where(x => x.IDUsuario == Usuario.IDLoginsistema).ToList();
                Bank.PermissoesdeDepartamentos.RemoveRange(PermissoesDepartamentos);

                List<PermissoesdeEmpresas> PermissoesEmpresas = Bank.PermissoesdeEmpresas.Where(x => x.IDUsuario == Usuario.IDLoginsistema).ToList();
                Bank.PermissoesdeEmpresas.RemoveRange(PermissoesEmpresas);

                List<PermissoesdeFuncionarios> PermissoesFuncionarios = Bank.PermissoesdeFuncionarios.Where(x => x.IDUsuario == Usuario.IDLoginsistema).ToList();
                Bank.PermissoesdeFuncionarios.RemoveRange(PermissoesFuncionarios);


                BankLogin.LoginSistema.Remove(Usuario);                
                
                BankLogin.SaveChanges();

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
        public ActionResult CadastroUsuarios_Remover_Selecao(int[] id)
        {
            if (id.Contains(Convert.ToInt32(Session["Usuario"])))
            {
                throw new Exception("Código Html Alterado");
            }

            List<LoginSistema> Usuario = BankLogin.LoginSistema.Where(x => id.Contains(x.IDLoginsistema)).ToList();
            return PartialView(Usuario);
        }

        [HttpPost]
        
        [VerificaLogin]
        public ActionResult CadastroUsuarios_Remover_Selecao_Salvar(int[] id)
        {
            try
            {
                if (id.Contains(Convert.ToInt32(Session["Usuario"])))
                {
                    throw new Exception("Código Html Alterado");
                }

                List<LoginSistema> Usuario = BankLogin.LoginSistema.Where(x => id.Contains(x.IDLoginsistema)).ToList();
                int[] ids = Usuario.Select(x => x.IDLoginsistema).Distinct().ToArray();

                List<PermissoesdeTelas> PermissoesTelas = Bank.PermissoesdeTelas.Where(x => ids.Contains(x.IDUsuario)).ToList();
                Bank.PermissoesdeTelas.RemoveRange(PermissoesTelas);

                List<PermissoesdeSetores> PermissoesSetores = Bank.PermissoesdeSetores.Where(x => ids.Contains(x.IDUsuario)).ToList();
                Bank.PermissoesdeSetores.RemoveRange(PermissoesSetores);

                List<PermissoesdeDepartamentos> PermissoesDepartamentos = Bank.PermissoesdeDepartamentos.Where(x => ids.Contains(x.IDUsuario)).ToList();
                Bank.PermissoesdeDepartamentos.RemoveRange(PermissoesDepartamentos);

                List<PermissoesdeEmpresas> PermissoesEmpresas = Bank.PermissoesdeEmpresas.Where(x => ids.Contains(x.IDUsuario)).ToList();
                Bank.PermissoesdeEmpresas.RemoveRange(PermissoesEmpresas);

                List<PermissoesdeFuncionarios> PermissoesFuncionarios = Bank.PermissoesdeFuncionarios.Where(x => ids.Contains(x.IDUsuario)).ToList();
                Bank.PermissoesdeFuncionarios.RemoveRange(PermissoesFuncionarios);

                BankLogin.LoginSistema.RemoveRange(Usuario);
                BankLogin.SaveChanges();
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
        public ActionResult SelecionaEmpresas(int? id)
        {
            Login.Models.LoginSistema UsuarioLogado = BankLogin
                 .LoginSistema.AsNoTracking()
                 .FirstOrDefault(x => x.IDLoginsistema == Convert.ToInt32(Session["Usuario"].ToString())); // Carrega Usuario Logado

            ViewBag.Usuario = id.HasValue ? BankLogin.LoginSistema.First(x => x.IDLoginsistema == id.Value) : null;

            List<Empresas> Empresas = UsuarioLogado.PerfiMaster ? Bank.Empresas.ToList() : Bank.PermissoesdeEmpresas.Include(x => x.Empresa).Where(x => x.IDUsuario == UsuarioLogado.IDLoginsistema).Select(x => x.Empresa).ToList();
                       
            return PartialView(Empresas);
        }

        [HttpGet]
        [VerificaLoad]
        public ActionResult Permissoes(int[] Empresas, int? id)
        {

            Login.Models.LoginSistema UsuarioLogado = BankLogin
                 .LoginSistema.AsNoTracking()
                 .FirstOrDefault(x => x.IDLoginsistema == Convert.ToInt32(Session["Usuario"].ToString())); // Carrega Usuario Logado

            ViewBag.Usuario = id.HasValue ? id : null;

            List<Empresas> Empresa = UsuarioLogado.PerfiMaster ? Bank.Empresas.Where(x=> Empresas.Contains(x.IDEmpresa)).ToList() : Bank.PermissoesdeEmpresas.Include(x => x.Empresa).Where(x => x.IDUsuario == UsuarioLogado.IDLoginsistema && Empresas.Contains(x.IDEmpresa)).Select(x => x.Empresa).ToList();

            return PartialView(Empresa);
        }

        [HttpGet]
        [VerificaLoad]
        public ActionResult GridsPermissoes(int id, int? usuario)
        {

            Login.Models.LoginSistema UsuarioLogado = BankLogin
                .LoginSistema.AsNoTracking()
                .FirstOrDefault(x => x.IDLoginsistema == Convert.ToInt32(Session["Usuario"].ToString())); // Carrega Usuario Logado

            if (!UsuarioLogado.PerfiMaster && Bank.PermissoesdeEmpresas.Where(x=>x.IDEmpresa == id && x.IDUsuario == UsuarioLogado.IDLoginsistema).Count() == 0)
            {
                throw new Exception("Usuario não possui permissão para essa empresa");
            }
                      
            ViewBag.Usuario = !usuario.HasValue ? null : usuario;
            

            Empresas Empresa = Bank.Empresas.Include(x=>x.Setores).Include(x=>x.Departamentos).First(x => x.IDEmpresa == id);
            return PartialView(Empresa);
        }
       
        public ActionResult GridSetores(int? id, string pesquisa)
        {
            pesquisa = pesquisa.ToLower().Replace('+', ' ');

            Login.Models.LoginSistema UsuarioLogado = BankLogin
                 .LoginSistema.AsNoTracking()
                 .FirstOrDefault(x => x.IDLoginsistema == Convert.ToInt32(Session["Usuario"].ToString())); // Carrega Usuario Logado

            List<Setores> Setores = UsuarioLogado.PerfiMaster ? Bank.Setores.Where(x => x.IDEmpresa == id && (x.IDSetor.ToString().StartsWith(pesquisa) || x.Nome.Contains(pesquisa))).ToList() : Bank.PermissoesdeSetores.Include(x => x.Setor).Where(x => x.IDEmpresa == id && x.IDUsuario == UsuarioLogado.IDLoginsistema).Select(x => x.Setor).Where(x=> (x.IDSetor.ToString().StartsWith(pesquisa) || x.Nome.Contains(pesquisa))).ToList();
            
            return PartialView(Setores);
        }

       
        public ActionResult GridDepartamentos(int? id, string pesquisa)
        {
            pesquisa = pesquisa.ToLower().Replace('+', ' ');

            Login.Models.LoginSistema UsuarioLogado = BankLogin
                 .LoginSistema.AsNoTracking()
                 .FirstOrDefault(x => x.IDLoginsistema == Convert.ToInt32(Session["Usuario"].ToString())); // Carrega Usuario Logado

            List<Departamentos> Departamentos = UsuarioLogado.PerfiMaster ? Bank.Departamentos.Where(x => x.IDEmpresa == id && (x.IDDepartamento.ToString().StartsWith(pesquisa) || x.Nome.Contains(pesquisa))).ToList() : Bank.PermissoesdeDepartamentos.Include(x => x.Departamento).Where(x => x.IDEmpresa == id && x.IDUsuario == UsuarioLogado.IDLoginsistema).Select(x => x.Departamento).Where(x=> (x.IDDepartamento.ToString().StartsWith(pesquisa) || x.Nome.Contains(pesquisa))).ToList();

            return PartialView(Departamentos);
        }
                
        
        [VerificaLoad]
        public ActionResult GridFuncionarios(int? id, string[] Setores, string[] Departamentos, string pesquisa)
        {
            try
            {
                if (String.IsNullOrEmpty(pesquisa))
                {
                    pesquisa = "";
                }

                pesquisa = pesquisa.ToLower().Replace('+', ' ');

                Login.Models.LoginSistema UsuarioLogado = BankLogin
                    .LoginSistema.AsNoTracking()
                    .FirstOrDefault(x => x.IDLoginsistema == Convert.ToInt32(Session["Usuario"].ToString())); // Carrega Usuario Logado

                Setores = Setores == null ? new string[] { } : Setores;

                Departamentos = Departamentos == null ? new string[] { } : Departamentos;

                List<Funcionarios> Funcionario = UsuarioLogado.PerfiMaster ? Bank.Funcionarios.Where(x => x.IDEmpresa == id && (Setores.Contains($"{x.IDSetor}/{x.IDEmpresa}") && Departamentos.Contains($"{x.IDDepartamento}/{x.IDEmpresa}")) && (x.IDFuncionario.ToString().StartsWith(pesquisa) || x.Nome.Contains(pesquisa))).ToList() : Bank.PermissoesdeFuncionarios.Include(x => x.Funcionario).Select(x => x.Funcionario).Where(x => x.IDEmpresa == id && (Setores.Contains($"{x.IDSetor}/{x.IDEmpresa}") && Departamentos.Contains($"{x.IDDepartamento}/{x.IDEmpresa}")) && (x.IDFuncionario.ToString().StartsWith(pesquisa) || x.Nome.Contains(pesquisa))).ToList();

                return PartialView(Funcionario);
            }
            catch(Exception e)
            {
                return new HttpUnauthorizedResult(e.Message);
            }
        }
        
        [HttpGet]
        [VerificaLoad]
        public ActionResult GridsPermissoesdeTelas(int? id)
        {

            ViewBag.Usuario = id.HasValue ? id : null;

            return PartialView();
        }

        [HttpGet]
        [VerificaLoad]
        public ActionResult GridCadastros()
        {
            Login.Models.LoginSistema UsuarioLogado = BankLogin
                 .LoginSistema.AsNoTracking()
                 .FirstOrDefault(x => x.IDLoginsistema == Convert.ToInt32(Session["Usuario"].ToString())); // Carrega Usuario Logado

            var ids = UsuarioLogado.PerfiMaster ? new int[] { } : Bank.PermissoesdeTelas.Include(x => x.FuncaodeTela).Where(x => x.IDUsuario == UsuarioLogado.IDLoginsistema).Select(x => x.FuncaodeTela).Select(x => x.IDTela).ToArray();

            List<SapewinWeb.Models.Telas> Telas = UsuarioLogado.PerfiMaster ? Bank.Telas.Include(x=>x.FuncoesdeTelas).Where(x => x.Nome.ToLower().Contains("cadastro")).ToList() : Bank.Telas.Include(x=>x.FuncoesdeTelas).Where(x=> ids.Contains(x.IDTela) && x.Nome.ToLower().Contains("cadastro")).ToList();

            return PartialView(Telas);
        }

        [HttpGet]
        [VerificaLoad]
        public ActionResult GridFerramentas()
        {
            Login.Models.LoginSistema UsuarioLogado = BankLogin
                 .LoginSistema.AsNoTracking()
                 .FirstOrDefault(x => x.IDLoginsistema == Convert.ToInt32(Session["Usuario"].ToString())); // Carrega Usuario Logado

            var ids = UsuarioLogado.PerfiMaster ? new int[] { } : Bank.PermissoesdeTelas.Include(x => x.FuncaodeTela).Where(x => x.IDUsuario == UsuarioLogado.IDLoginsistema).Select(x => x.FuncaodeTela).Select(x => x.IDTela).ToArray();

            List<SapewinWeb.Models.Telas> Telas = UsuarioLogado.PerfiMaster ? Bank.Telas.Include(x => x.FuncoesdeTelas).Where(x => x.Nome.ToLower().Contains("ferramenta")).ToList() : Bank.Telas.Include(x => x.FuncoesdeTelas).Where(x => ids.Contains(x.IDTela) && x.Nome.ToLower().Contains("ferramenta")).ToList();

            return PartialView(Telas);
        }

        [HttpGet]
        [VerificaLoad]
        public ActionResult GridUtils()
        {
            Login.Models.LoginSistema UsuarioLogado = BankLogin
                 .LoginSistema.AsNoTracking()
                 .FirstOrDefault(x => x.IDLoginsistema == Convert.ToInt32(Session["Usuario"].ToString())); // Carrega Usuario Logado

            var ids = UsuarioLogado.PerfiMaster ? new int[] { } : Bank.PermissoesdeTelas.Include(x => x.FuncaodeTela).Where(x => x.IDUsuario == UsuarioLogado.IDLoginsistema).Select(x => x.FuncaodeTela).Select(x => x.IDTela).ToArray();

            List<SapewinWeb.Models.Telas> Telas = UsuarioLogado.PerfiMaster ? Bank.Telas.Include(x => x.FuncoesdeTelas).Where(x => x.Nome.ToLower().Contains("util")).ToList() : Bank.Telas.Include(x => x.FuncoesdeTelas).Where(x => ids.Contains(x.IDTela) && x.Nome.ToLower().Contains("util")).ToList();

            return PartialView(Telas);
        }

        [HttpGet]
        [VerificaLoad]
        public ActionResult GridRelatorios()
        {
            Login.Models.LoginSistema UsuarioLogado = BankLogin
                 .LoginSistema.AsNoTracking()
                 .FirstOrDefault(x => x.IDLoginsistema == Convert.ToInt32(Session["Usuario"].ToString())); // Carrega Usuario Logado

            var ids = UsuarioLogado.PerfiMaster ? new int[] { } : Bank.PermissoesdeTelas.Include(x => x.FuncaodeTela).Where(x => x.IDUsuario == UsuarioLogado.IDLoginsistema).Select(x => x.FuncaodeTela).Select(x => x.IDTela).ToArray();

            List<SapewinWeb.Models.Telas> Telas = UsuarioLogado.PerfiMaster ? Bank.Telas.Include(x => x.FuncoesdeTelas).Where(x => x.Nome.ToLower().Contains("relatorio")).ToList() : Bank.Telas.Include(x => x.FuncoesdeTelas).Where(x => ids.Contains(x.IDTela) && x.Nome.ToLower().Contains("relatorio")).ToList();

            return PartialView(Telas);
        }

        public void AdicionaDadosUsuario(int[] Empresas, string[] Setores, string[] Departamentos, string[] Funcionarios, string[] PermissoesdeTelas, LoginSistema Usuario)
        {

            for (int i = 0; i < Empresas.LongLength; i++)
            {
                Bank.PermissoesdeEmpresas.Add(new PermissoesdeEmpresas
                {
                    IDEmpresa = Empresas[i],
                    IDUsuario = Usuario.IDLoginsistema
                });

                for (int x = 0; x < PermissoesdeTelas.LongLength; x++)
                {
                    Bank.PermissoesdeTelas.Add(new PermissoesdeTelas
                    {
                        IDEmpresa = Empresas[i],
                        IDFuncaoTela = PermissoesdeTelas[x],
                        IDUsuario = Usuario.IDLoginsistema
                    });
                }
            }

            for (int i = 0; i < Setores.LongLength; i++)
            {
                Bank.PermissoesdeSetores.Add(new PermissoesdeSetores
                {
                    IDEmpresa = Convert.ToInt32(Setores[i].Split('/')[1]),
                    IDSetor = Convert.ToInt32(Setores[i].Split('/')[0]),
                    IDUsuario = Usuario.IDLoginsistema,
                });
            }

            for (int i = 0; i < Departamentos.LongLength; i++)
            {
                Bank.PermissoesdeDepartamentos.Add(new PermissoesdeDepartamentos
                {
                    IDEmpresa = Convert.ToInt32(Departamentos[i].Split('/')[1]),
                    IDDepartamento = Convert.ToInt32(Departamentos[i].Split('/')[0]),
                    IDUsuario = Usuario.IDLoginsistema,
                });
            }

            for (int i = 0; i < Funcionarios.LongLength; i++)
            {
                Bank.PermissoesdeFuncionarios.Add(new PermissoesdeFuncionarios
                {
                    IDEmpresa = Convert.ToInt32(Funcionarios[i].Split('/')[1]),
                    IDFuncionario = Convert.ToInt32(Funcionarios[i].Split('/')[0]),
                    IDUsuario = Usuario.IDLoginsistema
                });
            }            

            Bank.SaveChanges();
        }

        [HttpPost]
        
        [VerificaLogin]
        public ActionResult CadastroUsuarios_Incluir_Salvar([Bind(Include = "Nome,Login,Email,Telefone")] LoginSistema Usuario, bool Personalizado, string ConfEmail, int? Privilegio, int[] Empresas, string[] Setores, string[] Departamentos, string[] Funcionarios, string[] PermissoesdeTelas)
        {
            try
            { 
                if (String.IsNullOrEmpty(Usuario.Nome) || String.IsNullOrEmpty(Usuario.Email) || String.IsNullOrEmpty(Usuario.Login))
                {
                    throw new Exception("Preencha todos os campos");
                }

                if (Usuario.Email != ConfEmail)
                {
                    throw new Exception("Confirme o Email novamente");
                }

                if (!Privilegio.HasValue)
                {
                    throw new Exception("Selecione um Nível de Privilégio para o Usuário");
                }

                Login.Models.LoginSistema UsuarioLogado = BankLogin
                  .LoginSistema.AsNoTracking()
                  .FirstOrDefault(x => x.IDLoginsistema == Convert.ToInt32(Session["Usuario"].ToString())); // Carrega Usuario Logado

                if (BankLogin.LoginSistema.Where(x=>x.IDCliente == UsuarioLogado.IDCliente).Select(x=>x.Login).Contains(Usuario.Login))
                {
                    throw new Exception("Esse Login Já esta em uso");
                }

                Usuario.IDCliente = UsuarioLogado.IDCliente;
                Usuario.PerfiMaster = false;
                Usuario.Ativo = true;
                Usuario.ForcaAlteracao = true;
                Usuario.Senha = Criptografia.HashValue(Usuario.Login);
                Usuario.Tipo = Privilegio.Value;
                Usuario.Personalizado = Personalizado;
                
                Usuario.IDLoginsistema = BankLogin.LoginSistema.OrderBy(x => x.IDLoginsistema).LastOrDefault() != null ? BankLogin.LoginSistema.OrderBy(x => x.IDLoginsistema).LastOrDefault().IDLoginsistema + 1 : 1; 

                BankLogin.LoginSistema.Add(Usuario);

                if (Empresas == null && Personalizado)
                {
                    throw new Exception("Nenhuma Empresa Selecionada");
                }

                if (PermissoesdeTelas == null && Personalizado)
                {
                    throw new Exception("Nenhuma Permissão Tela Selecionada");
                }

                if (Personalizado)
                {
                    Funcionarios = Funcionarios == null ? new string[] { } : Funcionarios;

                    Setores = Setores == null ? new string[] { } : Setores;

                    Departamentos = Departamentos == null ? new string[] { } : Departamentos;

                    AdicionaDadosUsuario(Empresas, Setores, Departamentos, Funcionarios, PermissoesdeTelas, Usuario);
                }
                else
                {
                    SincronizaFuncoes.Sincroniza.Go();

                    if (Usuario.Tipo == 1 || Usuario.Tipo == 2)
                    {
                        Empresas = UsuarioLogado.PerfiMaster ? Bank.Empresas.Select(x => x.IDEmpresa).ToArray() : Bank.PermissoesdeEmpresas.Where(x => x.IDUsuario == UsuarioLogado.IDLoginsistema).Select(x => x.IDEmpresa).ToArray();

                        Setores = UsuarioLogado.PerfiMaster ? Bank.Setores.Select(x => $"{x.IDSetor}/{x.IDEmpresa}").ToArray() : Bank.PermissoesdeSetores.Where(x => x.IDUsuario == UsuarioLogado.IDLoginsistema && Empresas.Contains(x.IDEmpresa)).Select(x => $"{x.IDSetor}/{x.IDEmpresa}").ToArray();

                        Departamentos = UsuarioLogado.PerfiMaster ? Bank.Departamentos.Select(x => $"{x.IDDepartamento}/{x.IDEmpresa}").ToArray() : Bank.PermissoesdeDepartamentos.Where(x => x.IDUsuario == UsuarioLogado.IDLoginsistema && Empresas.Contains(x.IDEmpresa)).Select(x => $"{x.IDDepartamento}/{x.IDEmpresa}").ToArray();

                        Funcionarios = UsuarioLogado.PerfiMaster ? Bank.Funcionarios.Select(x => $"{x.IDFuncionario}/{x.IDEmpresa}").ToArray() : Bank.PermissoesdeFuncionarios.Where(x => x.IDUsuario == UsuarioLogado.IDLoginsistema && Empresas.Contains(x.IDEmpresa)).Select(x => $"{x.IDFuncionario}/{x.IDEmpresa}").ToArray();

                        PermissoesdeTelas = BankLogin.FuncoesDeTelas.Where(x=>x.IDProduto == 1).Select(x=>x.IDFuncaoTela).ToArray();                       

                       

                        AdicionaDadosUsuario(Empresas, Setores, Departamentos, Funcionarios, PermissoesdeTelas, Usuario);

                    } else if (Usuario.Tipo == 3)
                    {

                        Empresas = UsuarioLogado.PerfiMaster ? Bank.Empresas.Select(x => x.IDEmpresa).ToArray() : Bank.PermissoesdeEmpresas.Where(x => x.IDUsuario == UsuarioLogado.IDLoginsistema).Select(x => x.IDEmpresa).ToArray();

                        Setores = UsuarioLogado.PerfiMaster ? Bank.Setores.Select(x => $"{x.IDSetor}/{x.IDEmpresa}").ToArray() : Bank.PermissoesdeSetores.Where(x => x.IDUsuario == UsuarioLogado.IDLoginsistema && Empresas.Contains(x.IDEmpresa)).Select(x => $"{x.IDSetor}/{x.IDEmpresa}").ToArray();

                        Departamentos = UsuarioLogado.PerfiMaster ? Bank.Departamentos.Select(x => $"{x.IDDepartamento}/{x.IDEmpresa}").ToArray() : Bank.PermissoesdeDepartamentos.Where(x => x.IDUsuario == UsuarioLogado.IDLoginsistema && Empresas.Contains(x.IDEmpresa)).Select(x => $"{x.IDDepartamento}/{x.IDEmpresa}").ToArray();

                        Funcionarios = UsuarioLogado.PerfiMaster ? Bank.Funcionarios.Select(x => $"{x.IDFuncionario}/{x.IDEmpresa}").ToArray() : Bank.PermissoesdeFuncionarios.Where(x => x.IDUsuario == UsuarioLogado.IDLoginsistema && Empresas.Contains(x.IDEmpresa)).Select(x => $"{x.IDFuncionario}/{x.IDEmpresa}").ToArray();

                        PermissoesdeTelas = BankLogin.FuncoesDeTelas.Where(x => x.IDProduto == 1).Select(x => x.IDFuncaoTela).ToArray();

                        AdicionaDadosUsuario(Empresas, Setores, Departamentos, Funcionarios, PermissoesdeTelas, Usuario);
                    }
                }

                BankLogin.SaveChanges();

                return Json(new { status = true });

            }
            catch(Exception e)
            {
                return Json(new { status = false, msg = e.Message });
            }
        }

        [HttpPost]
        
        [VerificaLogin]
        public ActionResult CadastroUsuarios_Alterar_Salvar([Bind(Include = "Nome,Login,Email,Telefone")] LoginSistema Usuario, int id, bool Personalizado, string ConfEmail, int Privilegio, int[] Empresas, string[] Setores, string[] Departamentos, string[] Funcionarios, string[] PermissoesdeTelas)
        {
            try
            {
                if (String.IsNullOrEmpty(Usuario.Nome) || String.IsNullOrEmpty(Usuario.Email) || String.IsNullOrEmpty(Usuario.Login))
                {
                    throw new Exception("Preencha todos os campos");
                }

                if (Usuario.Email != ConfEmail)
                {
                    throw new Exception("Confirme o Email novamente");
                }

                Login.Models.LoginSistema UsuarioLogado = BankLogin
                   .LoginSistema.AsNoTracking()
                   .FirstOrDefault(x => x.IDLoginsistema == Convert.ToInt32(Session["Usuario"].ToString())); // Carrega Usuario Logado

                LoginSistema UsuarioPadrao = BankLogin.LoginSistema.FirstOrDefault(x => x.IDLoginsistema == id);

                if (BankLogin.LoginSistema.Where(x => x.IDCliente == UsuarioLogado.IDCliente && x.IDLoginsistema != UsuarioPadrao.IDLoginsistema).Select(x => x.Login).Contains(Usuario.Login))
                {
                    throw new Exception("Esse Login Já esta em uso");
                }              

                UsuarioPadrao.Nome = Usuario.Nome;
                UsuarioPadrao.Login = Usuario.Login;
                UsuarioPadrao.Email = Usuario.Email;
                UsuarioPadrao.Telefone = UsuarioPadrao.Telefone;
                UsuarioPadrao.Personalizado = Personalizado;
                UsuarioPadrao.Tipo = Privilegio;

                List<PermissoesdeEmpresas> PermEmpUsuariopd = Bank.PermissoesdeEmpresas.Where(x => x.IDUsuario == UsuarioPadrao.IDLoginsistema).ToList();
                Bank.PermissoesdeEmpresas.RemoveRange(PermEmpUsuariopd);

                List<PermissoesdeSetores> PermSetUsuariopd = Bank.PermissoesdeSetores.Where(x => x.IDUsuario == UsuarioPadrao.IDLoginsistema).ToList();
                Bank.PermissoesdeSetores.RemoveRange(PermSetUsuariopd);

                List<PermissoesdeDepartamentos> PermDepUsuariopd = Bank.PermissoesdeDepartamentos.Where(x => x.IDUsuario == UsuarioPadrao.IDLoginsistema).ToList();
                Bank.PermissoesdeDepartamentos.RemoveRange(PermDepUsuariopd);

                List<PermissoesdeFuncionarios> PermFuncUsuariopd = Bank.PermissoesdeFuncionarios.Where(x => x.IDUsuario == UsuarioPadrao.IDLoginsistema).ToList();
                Bank.PermissoesdeFuncionarios.RemoveRange(PermFuncUsuariopd);

                List<PermissoesdeTelas> PermTelaUsuariopd = Bank.PermissoesdeTelas.Where(x => x.IDUsuario == UsuarioPadrao.IDLoginsistema).ToList();
                Bank.PermissoesdeTelas.RemoveRange(PermTelaUsuariopd);

                if (Empresas == null && Personalizado)
                {
                    throw new Exception("Nenhuma Empresa Selecionada");
                }

                if (PermissoesdeTelas == null && Personalizado)
                {
                    throw new Exception("Nenhuma Permissão Tela Selecionada");
                }

                Bank.SaveChanges();

                if (Personalizado)
                {
                    Funcionarios = Funcionarios == null ? new string[] { } : Funcionarios;

                    Setores = Setores == null ? new string[] { } : Setores;

                    Departamentos = Departamentos == null ? new string[] { } : Departamentos;                   

                    AdicionaDadosUsuario(Empresas, Setores, Departamentos, Funcionarios, PermissoesdeTelas, UsuarioPadrao);
                }
                else
                {
                    SincronizaFuncoes.Sincroniza.Go();

                    if (UsuarioPadrao.Tipo == 1 || UsuarioPadrao.Tipo == 2)
                    {
                        Empresas = UsuarioLogado.PerfiMaster ? Bank.Empresas.Select(x => x.IDEmpresa).ToArray() : Bank.PermissoesdeEmpresas.Where(x => x.IDUsuario == UsuarioLogado.IDLoginsistema).Select(x => x.IDEmpresa).ToArray();

                        Setores = UsuarioLogado.PerfiMaster ? Bank.Setores.Select(x => $"{x.IDSetor}/{x.IDEmpresa}").ToArray() : Bank.PermissoesdeSetores.Where(x => x.IDUsuario == UsuarioLogado.IDLoginsistema && Empresas.Contains(x.IDEmpresa)).Select(x => $"{x.IDSetor}/{x.IDEmpresa}").ToArray();

                        Departamentos = UsuarioLogado.PerfiMaster ? Bank.Departamentos.Select(x => $"{x.IDDepartamento}/{x.IDEmpresa}").ToArray() : Bank.PermissoesdeDepartamentos.Where(x => x.IDUsuario == UsuarioLogado.IDLoginsistema && Empresas.Contains(x.IDEmpresa)).Select(x => $"{x.IDDepartamento}/{x.IDEmpresa}").ToArray();

                        Funcionarios = UsuarioLogado.PerfiMaster ? Bank.Funcionarios.Select(x => $"{x.IDFuncionario}/{x.IDEmpresa}").ToArray() : Bank.PermissoesdeFuncionarios.Where(x => x.IDUsuario == UsuarioLogado.IDLoginsistema && Empresas.Contains(x.IDEmpresa)).Select(x => $"{x.IDFuncionario}/{x.IDEmpresa}").ToArray();

                        PermissoesdeTelas = BankLogin.FuncoesDeTelas.Where(x => x.IDProduto == 1).Select(x => x.IDFuncaoTela).ToArray();

                        AdicionaDadosUsuario(Empresas, Setores, Departamentos, Funcionarios, PermissoesdeTelas, UsuarioPadrao);

                    }
                    else if (UsuarioPadrao.Tipo == 3)
                    {

                        Empresas = UsuarioLogado.PerfiMaster ? Bank.Empresas.Select(x => x.IDEmpresa).ToArray() : Bank.PermissoesdeEmpresas.Where(x => x.IDUsuario == UsuarioLogado.IDLoginsistema).Select(x => x.IDEmpresa).ToArray();

                        Setores = UsuarioLogado.PerfiMaster ? Bank.Setores.Select(x => $"{x.IDSetor}/{x.IDEmpresa}").ToArray() : Bank.PermissoesdeSetores.Where(x => x.IDUsuario == UsuarioLogado.IDLoginsistema && Empresas.Contains(x.IDEmpresa)).Select(x => $"{x.IDSetor}/{x.IDEmpresa}").ToArray();

                        Departamentos = UsuarioLogado.PerfiMaster ? Bank.Departamentos.Select(x => $"{x.IDDepartamento}/{x.IDEmpresa}").ToArray() : Bank.PermissoesdeDepartamentos.Where(x => x.IDUsuario == UsuarioLogado.IDLoginsistema && Empresas.Contains(x.IDEmpresa)).Select(x => $"{x.IDDepartamento}/{x.IDEmpresa}").ToArray();

                        Funcionarios = UsuarioLogado.PerfiMaster ? Bank.Funcionarios.Select(x => $"{x.IDFuncionario}/{x.IDEmpresa}").ToArray() : Bank.PermissoesdeFuncionarios.Where(x => x.IDUsuario == UsuarioLogado.IDLoginsistema && Empresas.Contains(x.IDEmpresa)).Select(x => $"{x.IDFuncionario}/{x.IDEmpresa}").ToArray();

                        PermissoesdeTelas = new string[Bank.Telas.Count() * Bank.Funcoes.Count()];

                        PermissoesdeTelas = BankLogin.FuncoesDeTelas.Where(x => x.IDProduto == 1).Select(x => x.IDFuncaoTela).ToArray();

                        AdicionaDadosUsuario(Empresas, Setores, Departamentos, Funcionarios, PermissoesdeTelas, UsuarioPadrao);
                    }
                }

                BankLogin.SaveChanges();

                return Json(new { status = true });

            }
            catch (Exception e)
            {
                return Json(new { status = false, msg = e.Message });
            }

        }

        [HttpPost]
        [VerificaLoad]
        public ActionResult RetornaArrays(int[] Empresas, int ? id)
        {
            try
            {
                LoginSistema Usuario = null;

                if (id.HasValue)
                {
                    Usuario = BankLogin.LoginSistema.FirstOrDefault(x => x.IDLoginsistema == id);
                }

                Login.Models.LoginSistema UsuarioLogado = BankLogin
                .LoginSistema.AsNoTracking()
                .FirstOrDefault(x => x.IDLoginsistema == Convert.ToInt32(Session["Usuario"].ToString())); // Carrega Usuario Logado

                Empresas = Empresas == null ? new int[] { } : Empresas;

                var Setores = Usuario == null ? UsuarioLogado.PerfiMaster ? Bank.Setores.Where(x => Empresas.Contains(x.IDEmpresa)).Select(x => $"{x.IDSetor}/{x.IDEmpresa}").Distinct().ToArray() : Bank.PermissoesdeSetores.Include(x => x.Setor).Where(x => x.IDUsuario == UsuarioLogado.IDLoginsistema).Select(x => x.Setor).Where(x => Empresas.Contains(x.IDEmpresa)).Select(x => $"{x.IDSetor}/{x.IDEmpresa}").Distinct().ToArray() : Bank.PermissoesdeSetores.Where(x=> Empresas.Contains(x.IDEmpresa) && (x.IDUsuario == Usuario.IDLoginsistema && (UsuarioLogado.PerfiMaster || x.IDUsuario == UsuarioLogado.IDLoginsistema))).Select(x=> $"{x.IDSetor}/{x.IDEmpresa}").ToArray();

                var Departamentos = Usuario == null ? UsuarioLogado.PerfiMaster ? Bank.Departamentos.Where(x => Empresas.Contains(x.IDEmpresa)).Select(x => $"{x.IDDepartamento}/{x.IDEmpresa}").Distinct().ToArray() : Bank.PermissoesdeDepartamentos.Include(x => x.Departamento).Where(x => x.IDUsuario == UsuarioLogado.IDLoginsistema).Select(x => x.Departamento).Where(x => Empresas.Contains(x.IDEmpresa)).Select(x => $"{x.IDDepartamento}/{x.IDEmpresa}").Distinct().ToArray() : Bank.PermissoesdeDepartamentos.Where(x => Empresas.Contains(x.IDEmpresa) && (x.IDUsuario == Usuario.IDLoginsistema && (UsuarioLogado.PerfiMaster || x.IDUsuario == UsuarioLogado.IDLoginsistema))).Select(x => $"{x.IDDepartamento}/{x.IDEmpresa}").ToArray();

                var Funcionarios = Usuario == null ? UsuarioLogado.PerfiMaster ? Bank.Funcionarios.Where(x => Empresas.Contains(x.IDEmpresa)).Select(x => $"{x.IDFuncionario}/{x.IDEmpresa}").Distinct().ToArray() : Bank.PermissoesdeFuncionarios.Include(x => x.Funcionario).Where(x => x.IDUsuario == UsuarioLogado.IDLoginsistema).Select(x => x.Funcionario).Where(x => Empresas.Contains(x.IDEmpresa)).Select(x => $"{x.IDFuncionario}/{x.IDEmpresa}").Distinct().ToArray() : Bank.PermissoesdeFuncionarios.Where(x => Empresas.Contains(x.IDEmpresa) && (x.IDUsuario == Usuario.IDLoginsistema && (UsuarioLogado.PerfiMaster || x.IDUsuario == UsuarioLogado.IDLoginsistema))).Select(x => $"{x.IDFuncionario}/{x.IDEmpresa}").ToArray();

                var FuncionariosNaoslc = Usuario == null ? new string[] { } : Bank.Funcionarios.Where(x=> !Funcionarios.Contains($"{x.IDFuncionario}/{x.IDEmpresa}") && Setores.Contains($"{x.IDSetor}/{x.IDEmpresa}") && Departamentos.Contains($"{x.IDDepartamento}/{x.IDEmpresa}")).Select(x=> $"{x.IDFuncionario}/{x.IDEmpresa}").ToArray();

                

                return Json(new { status = true, Setores = Setores, Departamentos = Departamentos, Funcionarios = Funcionarios, FuncionariosNaoslc = FuncionariosNaoslc });
            }
            catch(Exception e)
            {
                return Json(new { status = false, msg = e.Message });
            }           
        }

        [HttpPost]
        [VerificaLoad]
        public ActionResult RetornaPermissoesdeTela(int id)
        {
            var PermissoesdeTelas = Bank.PermissoesdeTelas.Where(x => x.IDUsuario == id).Select(x=>x.IDFuncaoTela).Distinct().ToArray();
            return Json(new { status = true, Permissoes = PermissoesdeTelas });
        }
    }
}
