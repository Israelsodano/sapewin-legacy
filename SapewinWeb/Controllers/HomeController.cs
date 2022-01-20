using System;
using System.Linq;
using System.Web.Mvc;
using SapewinWeb.Models;
using SincronizaFuncoes;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using SapewinWeb.Metodos;

namespace SapewinWeb.Controllers
{
    
    public class HomeController : Controller
    {

        Login.Models.LoginModel BankLogin = new Login.Models.LoginModel();
        MyContext Bank = new MyContext();        

        [HttpGet]
        [VerificaLogin]               
        public ActionResult Index()
        {           
            
            Login.Models.LoginSistema UsuarioLogado = BankLogin
                .LoginSistema.AsNoTracking()
                .FirstOrDefault(x => x.IDLoginsistema == Convert.ToInt32(Session["Usuario"].ToString())); // Carrega usuario logado

            //Sincroniza.Go();

            ViewBag.PermissoesIndices = UsuarioLogado.PerfiMaster
              ? Bank.FuncoesdeTelas.AsNoTracking().Select(x => x.IDFuncaoTela).ToList()
              : Bank.PermissoesdeTelas.AsNoTracking().Where(x => x.IDUsuario == UsuarioLogado.IDLoginsistema && Session["Empresa"].ToString().Contains(x.IDEmpresa.ToString("0000"))).Select(x => x.IDFuncaoTela).ToList(); //carreha lista de funçoes de telas qiue o usuario possui


            //List<string> telas = new List<string>
            //{
            //    "CadastroEmpresas",
            //    "CadastroSetores",
            //    "CadastroDepartamentos",
            //    "CadastroCargos",
            //    "CadastroParametros",
            //    "CadastroFeriadosGerais",
            //    "CadastroFeriadosEspecificos",
            //    "CadastroHorarios",
            //    "CadastroEscalas",
            //    "CadastroMotivosdeAbono",
            //    "CadastroUsuarios",
            //    "CadastroFuncionarios",
            //    "CadastroMensagens",
            //    "FerramentaImportarMarcacoes",

            //};

            //List<string> funcoes = new List<string>
            //{
            //    "Abrir",
            //    "Incluir",
            //    "Remover",
            //    "Alterar"
            //};

            //List<Telas> telaslist = telas.Select(x => new Telas() { Nome = x, IDTela = telas.IndexOf(x) + 1 }).ToList();

            //List<Funcoes> funcoeslist = funcoes.Select(x => new Funcoes() { Nome = x, IDFuncao = funcoes.IndexOf(x) + 1 }).ToList();

            //Bank.Funcoes.AddRange(funcoeslist);

            //Bank.Telas.AddRange(telaslist);

            //Bank.SaveChanges();

            //List<FuncoesdeTelas> funcTelas = new List<FuncoesdeTelas>();

            //for (int i = 0; i < telaslist.Count; i++)
            //{
            //    for (int j = 0; j < funcoeslist.Count; j++)
            //    {
            //        funcTelas.Add(new FuncoesdeTelas
            //        {
            //            IDFuncao = funcoeslist[j].IDFuncao,
            //            IDTela = telaslist[i].IDTela,
            //            IDFuncaoTela = telaslist[i].Nome + "-" + funcoeslist[j].Nome,
            //        });
            //    }
            //}

            //Bank.FuncoesdeTelas.AddRange(funcTelas);

            //Bank.SaveChanges();




            //int contador = 1;

            //foreach (var item in Bank.Setores)
            //{
            //    for (int i = 0; i <= 10; i++)
            //    {
            //        Bank.Funcionarios.Add(new Funcionarios { IDFuncionario = contador, IDUsuario = 1, IDDepartamento = item.IDSetor, Nome = "Funcionario - " + contador, IDSetor = item.IDSetor, IDEmpresa = 1, Pis = "sasa", Cpf = "asasa", IDFeriado = 1, Endereco = "asasa" });
            //        contador++;
            //    }

            //}

            //Bank.SaveChanges();


            List<String> PermissoesUsuario = new List<string>();

            List<int> EmpresasdoUsuarioLogado = Bank.PermissoesdeEmpresas.AsNoTracking().Where(x => x.IDUsuario == UsuarioLogado.IDLoginsistema).Select(x => x.IDEmpresa).ToList();  // carrega ids do obj que o usuario tem permissao          

            if (Session["Empresa"] == null)
            {
                SapewinWeb.Models.Empresas Empresa = Bank.Empresas.AsNoTracking().First(x => EmpresasdoUsuarioLogado.Contains(x.IDEmpresa) || UsuarioLogado.PerfiMaster);
                Session["Empresa"] = Empresa.IDEmpresa.ToString("0000") + " - " + Empresa.Nome; // carrega a sessão da empresa 
            }

            if (UsuarioLogado.PerfiMaster)
            {
                PermissoesUsuario = Bank.FuncoesdeTelas.AsNoTracking().Select(x => x.IDFuncaoTela).ToList();
            }
            else
            {
                PermissoesUsuario = Bank.PermissoesdeTelas.AsNoTracking().Where(x => x.IDUsuario == UsuarioLogado.IDLoginsistema && Session["Empresa"].ToString().Contains(x.IDEmpresa.ToString("0000"))).Select(x => x.IDFuncaoTela).ToList();
            }
            ViewBag.PermissoesIndices = PermissoesUsuario;
            
            return PartialView();
        }

        [VerificaLoad]
        public ActionResult Home()
        {
            return PartialView();
        }        

        [VerificaLoad]
        public ActionResult AtualizaNav()
        {
            Login.Models.LoginSistema UsuarioLogado = BankLogin
                .LoginSistema.AsNoTracking()
                .FirstOrDefault(x => x.IDLoginsistema == Convert.ToInt32(Session["Usuario"].ToString())); // Carrega usuario logado

            Sincroniza.Go();

            List<String> PermissoesUsuario = new List<string>();

            if (UsuarioLogado.PerfiMaster)
            {
                PermissoesUsuario = Bank.FuncoesdeTelas.AsNoTracking().Select(x => x.IDFuncaoTela).ToList();
            }
            else
            {
                PermissoesUsuario = Bank.PermissoesdeTelas.AsNoTracking().Where(x => x.IDUsuario == UsuarioLogado.IDLoginsistema && Session["Empresa"].ToString().Contains(x.IDEmpresa.ToString("0000"))).Select(x => x.IDFuncaoTela).ToList();
            }

            ViewBag.PermissoesIndices = PermissoesUsuario;

            return PartialView("_Nav");
        }

        [HttpGet]        
        [VerificaLoad]
        public ActionResult SelecionaEmpresa()
        {
            Login.Models.LoginSistema UsuarioLogado = BankLogin
                .LoginSistema.AsNoTracking()
                .FirstOrDefault(x => x.IDLoginsistema == Convert.ToInt32(Session["Usuario"].ToString())); //carrega usuario            

            List<Empresas> Empresas = UsuarioLogado.PerfiMaster ? Bank.Empresas.ToList() : Bank.PermissoesdeEmpresas.Where(x => x.IDUsuario == UsuarioLogado.IDLoginsistema).Select(x => x.Empresa).ToList();

            int Total = Empresas.Count;

            Empresas = Empresas.Count < 10 ? Empresas : Empresas.GetRange(0, 10); // forma pagina

            ViewBag.Paginas = Math.Ceiling(Convert.ToDecimal(Convert.ToDouble(Total) / 10));     // conta paginas       

            ViewBag.Total = $"Registro(s): {Total} - Exibindo de {(Empresas.Count > 0 ? 1 : 0)} a {Empresas.Count} - {ViewBag.Paginas} Página(s)"; // cria string do footer

            return PartialView(Empresas);
        }

        [HttpGet]        
        [VerificaLoad]
        public ActionResult SelecionaEmpresa_Grid(String pesquisa, int Pagina, String Order, bool Condicao)
        {
            if (string.IsNullOrEmpty(pesquisa)) { pesquisa = ""; }

            pesquisa = pesquisa.ToLower().Replace('+', ' ');

            Login.Models.LoginSistema UsuarioLogado = BankLogin
                .LoginSistema.AsNoTracking()
                .FirstOrDefault(x => x.IDLoginsistema == Convert.ToInt32(Session["Usuario"].ToString())); // carrega usuario logado

            ViewBag.PermissoesIndices = UsuarioLogado.PerfiMaster
             ? Bank.FuncoesdeTelas.AsNoTracking().Select(x => x.IDFuncaoTela).ToList()
             : Bank.PermissoesdeTelas.AsNoTracking().Where(x => x.IDUsuario == UsuarioLogado.IDLoginsistema && Session["Empresa"].ToString().Contains(x.IDEmpresa.ToString("0000"))).Select(x => x.IDFuncaoTela).ToList(); //carreha lista de funçoes de telas qiue o usuario possui

            List<Empresas> Empresas = new List<Models.Empresas>();

            switch (Order.ToUpper())
            {
                case "CODIGO-UP":

                    if (Condicao || pesquisa == "")
                    {
                        Empresas = UsuarioLogado.PerfiMaster ? Bank.Empresas.Where(x => x.IDEmpresa.ToString().StartsWith(x.IDEmpresa.ToString()) || x.Nome.ToLower().Contains(pesquisa) || x.Documento.StartsWith(pesquisa)).OrderBy(x => x.IDEmpresa).ToList() : Bank.PermissoesdeEmpresas.Where(x => x.IDUsuario == UsuarioLogado.IDLoginsistema).Select(x => x.Empresa).Where(x => x.IDEmpresa.ToString().StartsWith(pesquisa) || x.Nome.ToLower().Contains(pesquisa) || x.Documento.StartsWith(pesquisa)).OrderBy(x => x.IDEmpresa).ToList();
                    }
                    else
                    {
                        Empresas = UsuarioLogado.PerfiMaster ? Bank.Empresas.Where(x => !x.IDEmpresa.ToString().StartsWith(x.IDEmpresa.ToString()) && !x.Nome.ToLower().Contains(pesquisa) && !x.Documento.StartsWith(pesquisa)).OrderBy(x => x.IDEmpresa).ToList() : Bank.PermissoesdeEmpresas.Where(x => x.IDUsuario == UsuarioLogado.IDLoginsistema).Select(x => x.Empresa).Where(x => !x.IDEmpresa.ToString().StartsWith(pesquisa) && !x.Nome.ToLower().Contains(pesquisa) && !x.Documento.StartsWith(pesquisa)).OrderBy(x => x.IDEmpresa).ToList();
                    }
                    break;

                case "CODIGO-DOWN":

                    if (Condicao || pesquisa == "")
                    {
                        Empresas = UsuarioLogado.PerfiMaster ? Bank.Empresas.Where(x => x.IDEmpresa.ToString().StartsWith(x.IDEmpresa.ToString()) || x.Nome.ToLower().Contains(pesquisa) || x.Documento.StartsWith(pesquisa)).OrderByDescending(x => x.IDEmpresa).ToList() : Bank.PermissoesdeEmpresas.Where(x => x.IDUsuario == UsuarioLogado.IDLoginsistema).Select(x => x.Empresa).Where(x => x.IDEmpresa.ToString().StartsWith(pesquisa) || x.Nome.ToLower().Contains(pesquisa) || x.Documento.StartsWith(pesquisa)).OrderByDescending(x => x.IDEmpresa).ToList();
                    }
                    else
                    {
                        Empresas = UsuarioLogado.PerfiMaster ? Bank.Empresas.Where(x => !x.IDEmpresa.ToString().StartsWith(x.IDEmpresa.ToString()) && !x.Nome.ToLower().Contains(pesquisa) && !x.Documento.StartsWith(pesquisa)).OrderByDescending(x => x.IDEmpresa).ToList() : Bank.PermissoesdeEmpresas.Where(x => x.IDUsuario == UsuarioLogado.IDLoginsistema).Select(x => x.Empresa).Where(x => !x.IDEmpresa.ToString().StartsWith(pesquisa) && !x.Nome.ToLower().Contains(pesquisa) && !x.Documento.StartsWith(pesquisa)).OrderByDescending(x => x.IDEmpresa).ToList();
                    }
                    break;

                case "RAZAO-UP":

                    if (Condicao || pesquisa == "")
                    {
                        Empresas = UsuarioLogado.PerfiMaster ? Bank.Empresas.Where(x => x.IDEmpresa.ToString().StartsWith(x.IDEmpresa.ToString()) || x.Nome.ToLower().Contains(pesquisa) || x.Documento.StartsWith(pesquisa)).OrderBy(x => x.Nome).ToList() : Bank.PermissoesdeEmpresas.Where(x => x.IDUsuario == UsuarioLogado.IDLoginsistema).Select(x => x.Empresa).Where(x => x.IDEmpresa.ToString().StartsWith(pesquisa) || x.Nome.ToLower().Contains(pesquisa) || x.Documento.StartsWith(pesquisa)).OrderBy(x => x.Nome).ToList();
                    }
                    else
                    {
                        Empresas = UsuarioLogado.PerfiMaster ? Bank.Empresas.Where(x => !x.IDEmpresa.ToString().StartsWith(x.IDEmpresa.ToString()) && !x.Nome.ToLower().Contains(pesquisa) && !x.Documento.StartsWith(pesquisa)).OrderBy(x => x.Nome).ToList() : Bank.PermissoesdeEmpresas.Where(x => x.IDUsuario == UsuarioLogado.IDLoginsistema).Select(x => x.Empresa).Where(x => !x.IDEmpresa.ToString().StartsWith(pesquisa) && !x.Nome.ToLower().Contains(pesquisa) && !x.Documento.StartsWith(pesquisa)).OrderBy(x => x.Nome).ToList();
                    }
                    break;

                case "RAZAO-DOWN":

                    if (Condicao || pesquisa == "")
                    {
                        Empresas = UsuarioLogado.PerfiMaster ? Bank.Empresas.Where(x => x.IDEmpresa.ToString().StartsWith(x.IDEmpresa.ToString()) || x.Nome.ToLower().Contains(pesquisa) || x.Documento.StartsWith(pesquisa)).OrderByDescending(x => x.Nome).ToList() : Bank.PermissoesdeEmpresas.Where(x => x.IDUsuario == UsuarioLogado.IDLoginsistema).Select(x => x.Empresa).Where(x => x.IDEmpresa.ToString().StartsWith(pesquisa) || x.Nome.ToLower().Contains(pesquisa) || x.Documento.StartsWith(pesquisa)).OrderByDescending(x => x.Nome).ToList();
                    }
                    else
                    {
                        Empresas = UsuarioLogado.PerfiMaster ? Bank.Empresas.Where(x => !x.IDEmpresa.ToString().StartsWith(x.IDEmpresa.ToString()) && !x.Nome.ToLower().Contains(pesquisa) && !x.Documento.StartsWith(pesquisa)).OrderByDescending(x => x.Nome).ToList() : Bank.PermissoesdeEmpresas.Where(x => x.IDUsuario == UsuarioLogado.IDLoginsistema).Select(x => x.Empresa).Where(x => !x.IDEmpresa.ToString().StartsWith(pesquisa) && !x.Nome.ToLower().Contains(pesquisa) && !x.Documento.StartsWith(pesquisa)).OrderByDescending(x => x.Nome).ToList();
                    }
                    break;

                case "DOCUMENTO-UP":

                    if (Condicao || pesquisa == "")
                    {
                        Empresas = UsuarioLogado.PerfiMaster ? Bank.Empresas.Where(x => x.IDEmpresa.ToString().StartsWith(x.IDEmpresa.ToString()) || x.Nome.ToLower().Contains(pesquisa) || x.Documento.StartsWith(pesquisa)).OrderBy(x => x.Documento).ToList() : Bank.PermissoesdeEmpresas.Where(x => x.IDUsuario == UsuarioLogado.IDLoginsistema).Select(x => x.Empresa).Where(x => x.IDEmpresa.ToString().StartsWith(pesquisa) || x.Nome.ToLower().Contains(pesquisa) || x.Documento.StartsWith(pesquisa)).OrderBy(x => x.Documento).ToList();
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

            int Range = 10;   // set tamanho de pagina fixo        

            ViewBag.Paginas = Math.Ceiling(Convert.ToDecimal(Convert.ToDouble(Total) / Range)); // conta paginas

            Pagina = Pagina > ViewBag.Paginas ? Convert.ToInt32(ViewBag.Paginas) - 1 : Pagina; // trata paginas para o tamnho corret de acordo cm o obj

            Pagina = Empresas.Count < Range ? Pagina * Empresas.Count : Pagina * Range; // nn deixa a pagina virtual ser maior que a pagina fisica 

            Range = Range + Pagina > Empresas.Count ? Empresas.Count - Pagina : Range; // nao deixa o Range ser maior que o numero de objs
             
            Empresas = Empresas.Count < Range ? Empresas : Empresas.GetRange(Pagina, Range); // Forma pagina

            ViewBag.Total = $"Registro(s): {Total} - Exibindo de {(Pagina == 0 ? 1 : Pagina + 1)} a {(Pagina == 0 ? Range : Pagina + Range)} - { ViewBag.Paginas} Página(s)"; // forma string do footer

            return PartialView(Empresas);
        }

        [HttpPost]
        [VerificaLoad]        
        public ActionResult SelecionaEmpresa_Post(int id)
        {
            try
            {
                Empresas empresa = Bank.Empresas.First(x=>x.IDEmpresa == id);
                Session["Empresa"] = empresa.IDEmpresa.ToString("0000") + " - " + empresa.Nome.ToString(); // seleciona uma nova empresa
                return Json(new { status = true, session = Session["Empresa"].ToString() }); // passa o novo nome via Json
            }
            catch (Exception e)
            {

                return Json(new { status = false, msg = e.Message });
            }
        }
        [HttpPost]
        public ActionResult Atualizafront()
        {
            //List<string> funcoes = new List<string>();

            //Metodos.RealTime.ObjVerificador Funcs = Metodos.RealTime.ListaEmpresas.FirstOrDefault(x => x.Documento == BankLogin.LoginSistema.Include(z => z.Cliente).First(z => z.ID == Convert.ToInt32(Session["Usuario"].ToString())).Cliente.Documento && !x.IDUsuariosRealTime.Contains(Convert.ToInt32(Session["Usuario"].ToString())));

            //if (Funcs != null)
            //{
            //    Metodos.RealTime.ListaEmpresas.FirstOrDefault(x => x.Documento == BankLogin.LoginSistema.Include(z => z.Cliente).First(z => z.ID == Convert.ToInt32(Session["Usuario"].ToString())).Cliente.Documento && !x.IDUsuariosRealTime.Contains(Convert.ToInt32(Session["Usuario"].ToString()))).IDUsuariosRealTime.Add(Convert.ToInt32(Session["Usuario"].ToString()));
            //    return Json(new { funcs = Funcs.FuncTela.ToArray() });
            //}else
            //{
            //    return Json(new { funcs = new int[] { } });
            //}     
            
            return Json(new { i = 0});

        }

        [HttpGet]
        public ActionResult Manutencao()
        {
            return PartialView();
        }
    }
}