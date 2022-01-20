using Microsoft.EntityFrameworkCore;
using SapewinWeb.Metodos;
using SapewinWeb.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace SapewinWeb.Controllers
{
    
    public class CadastroSetoresController : Controller
    {
        Login.Models.LoginModel BankLogin = new Login.Models.LoginModel();
        MyContext Bank = new MyContext();       

        [HttpGet]        
        [VerificaLogin]        
        public ActionResult CadastroSetores_Abrir()
        {
            Login.Models.LoginSistema UsuarioLogado = BankLogin
                .LoginSistema.AsNoTracking()
                .FirstOrDefault(x => x.IDLoginsistema == Convert.ToInt32(Session["Usuario"].ToString())); //carrega usuario
           
            ViewBag.PermissoesIndices = UsuarioLogado.PerfiMaster 
                ? Bank.FuncoesdeTelas.Select(x => x.IDFuncaoTela).ToList() 
                : Bank.PermissoesdeTelas.Where(x => x.IDUsuario == UsuarioLogado.IDLoginsistema && Session["Empresa"].ToString().Contains(x.IDEmpresa.ToString("0000"))).Select(x => x.IDFuncaoTela).ToList(); // carrega a lista de funçoes de telas que o usuario possui

            int IDEmpresa = Bank.Empresas.First(x => Session["Empresa"].ToString().Contains(x.IDEmpresa.ToString("0000"))).IDEmpresa;

            List<Setores> Setores = UsuarioLogado.PerfiMaster ? Bank.Setores.Where(x => x.IDEmpresa == IDEmpresa).OrderBy(x => x.IDSetor).ToList() : Bank.PermissoesdeSetores.Where(x => x.IDUsuario == UsuarioLogado.IDLoginsistema && x.IDEmpresa == IDEmpresa).Select(x => x.Setor).OrderBy(x=>x.IDSetor).ToList();

            int Total = Setores.Count;           

            Setores = Setores.Count < 10 ? Setores : Setores.GetRange(0, 10); // monta pagina

            ViewBag.Paginas = Math.Ceiling(Convert.ToDecimal(Convert.ToDouble(Total) / 10)); // conta paginas

            ViewBag.Total = $"Registro(s): {Total} - Exibindo de {(Setores.Count > 0 ? 1 : 0)} a {Setores.Count} - {ViewBag.Paginas} Página(s)"; //monta string do footer

            return VerificaLoad.IsAjax(HttpContext.Request) ? PartialView(Setores) : (ViewResultBase)View(Setores);
        }

        [HttpGet]
        [VerificaLoad]
        public ActionResult CadastroSetores_Abrir_Grid(String pesquisa, int Range, int Pagina, String Order, bool Condicao)
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

            List<Setores> Setores = new List<Models.Setores>();

            switch (Order.ToUpper())
            {
                case "CODIGO-UP":

                    if (Condicao || pesquisa == "")
                    {
                        Setores = UsuarioLogado.PerfiMaster ? Bank.Setores.AsNoTracking().AsNoTracking().Where(x => x.IDEmpresa == IDEmpresa && (x.IDSetor.ToString().StartsWith(pesquisa) || x.Nome.ToLower().StartsWith(pesquisa))).OrderBy(x=>x.IDSetor).ToList() : Bank.PermissoesdeSetores.Where(x => x.IDUsuario == UsuarioLogado.IDLoginsistema && x.IDEmpresa == IDEmpresa).Select(x => x.Setor).Where(x => x.IDSetor.ToString().StartsWith(pesquisa) || x.Nome.ToLower().Contains(pesquisa)).OrderBy(x=> x.IDSetor).ToList();
                    }
                    else
                    {
                        Setores = UsuarioLogado.PerfiMaster ? Bank.Setores.AsNoTracking().AsNoTracking().Where(x => x.IDEmpresa == IDEmpresa && (!x.IDSetor.ToString().StartsWith(pesquisa) && !x.Nome.ToLower().StartsWith(pesquisa))).OrderBy(x=>x.IDSetor).ToList() : Bank.PermissoesdeSetores.Where(x => x.IDUsuario == UsuarioLogado.IDLoginsistema && x.IDEmpresa == IDEmpresa).Select(x => x.Setor).Where(x => !x.IDSetor.ToString().StartsWith(pesquisa) && !x.Nome.ToLower().Contains(pesquisa)).OrderBy(x => x.IDSetor).ToList();
                    }
                    break;

                case "CODIGO-DOWN":

                    if (Condicao || pesquisa == "")
                    {
                        Setores = UsuarioLogado.PerfiMaster ? Bank.Setores.AsNoTracking().AsNoTracking().Where(x => x.IDEmpresa == IDEmpresa && (x.IDSetor.ToString().StartsWith(pesquisa) || x.Nome.ToLower().StartsWith(pesquisa))).OrderByDescending(x=>x.IDSetor).ToList() : Bank.PermissoesdeSetores.Where(x => x.IDUsuario == UsuarioLogado.IDLoginsistema && x.IDEmpresa == IDEmpresa).Select(x => x.Setor).Where(x => x.IDSetor.ToString().StartsWith(pesquisa) || x.Nome.ToLower().Contains(pesquisa)).OrderByDescending(x => x.IDSetor).ToList();
                    }
                    else
                    {
                        Setores = UsuarioLogado.PerfiMaster ? Bank.Setores.AsNoTracking().AsNoTracking().Where(x => x.IDEmpresa == IDEmpresa && (!x.IDSetor.ToString().StartsWith(pesquisa) && !x.Nome.ToLower().StartsWith(pesquisa))).OrderByDescending(x=>x.IDSetor).ToList() : Bank.PermissoesdeSetores.Where(x => x.IDUsuario == UsuarioLogado.IDLoginsistema && x.IDEmpresa == IDEmpresa).Select(x => x.Setor).Where(x => !x.IDSetor.ToString().StartsWith(pesquisa) && !x.Nome.ToLower().Contains(pesquisa)).OrderByDescending(x => x.IDSetor).ToList();
                    }
                    break;

                case "DESCRICAO-UP":

                    if (Condicao || pesquisa == "")
                    {
                        Setores = UsuarioLogado.PerfiMaster ? Bank.Setores.AsNoTracking().AsNoTracking().Where(x => x.IDEmpresa == IDEmpresa && (x.IDSetor.ToString().StartsWith(pesquisa) || x.Nome.ToLower().StartsWith(pesquisa))).OrderBy(x=>x.Nome).ToList() : Bank.PermissoesdeSetores.Where(x => x.IDUsuario == UsuarioLogado.IDLoginsistema && x.IDEmpresa == IDEmpresa).Select(x => x.Setor).Where(x => x.IDSetor.ToString().StartsWith(pesquisa) || x.Nome.ToLower().Contains(pesquisa)).OrderBy(x => x.Nome).ToList();
                    }
                    else
                    {
                        Setores = UsuarioLogado.PerfiMaster ? Bank.Setores.AsNoTracking().AsNoTracking().Where(x => x.IDEmpresa == IDEmpresa && (!x.IDSetor.ToString().StartsWith(pesquisa) && !x.Nome.ToLower().StartsWith(pesquisa))).OrderBy(x=>x.Nome).ToList() : Bank.PermissoesdeSetores.Where(x => x.IDUsuario == UsuarioLogado.IDLoginsistema && x.IDEmpresa == IDEmpresa).Select(x => x.Setor).Where(x => !x.IDSetor.ToString().StartsWith(pesquisa) && !x.Nome.ToLower().Contains(pesquisa)).OrderBy(x => x.Nome).ToList();
                    }
                    break;

                case "DESCRICAO-DOWN":

                    if (Condicao || pesquisa == "")
                    {
                        Setores = UsuarioLogado.PerfiMaster ? Bank.Setores.AsNoTracking().AsNoTracking().Where(x => x.IDEmpresa == IDEmpresa && (x.IDSetor.ToString().StartsWith(pesquisa) || x.Nome.ToLower().StartsWith(pesquisa))).OrderByDescending(x=>x.Nome).ToList() : Bank.PermissoesdeSetores.Where(x => x.IDUsuario == UsuarioLogado.IDLoginsistema && x.IDEmpresa == IDEmpresa).Select(x => x.Setor).Where(x => x.IDSetor.ToString().StartsWith(pesquisa) || x.Nome.ToLower().Contains(pesquisa)).OrderByDescending(x => x.Nome).ToList();
                    }
                    else
                    {
                        Setores = UsuarioLogado.PerfiMaster ? Bank.Setores.AsNoTracking().AsNoTracking().Where(x => x.IDEmpresa == IDEmpresa && (!x.IDSetor.ToString().StartsWith(pesquisa) && !x.Nome.ToLower().StartsWith(pesquisa))).OrderByDescending(x => x.Nome).ToList() : Bank.PermissoesdeSetores.Where(x => x.IDUsuario == UsuarioLogado.IDLoginsistema && x.IDEmpresa == IDEmpresa).Select(x => x.Setor).Where(x => !x.IDSetor.ToString().StartsWith(pesquisa) && !x.Nome.ToLower().Contains(pesquisa)).OrderByDescending(x => x.Nome).ToList();
                    }
                    break;                

                default:
                    break;
            }            

            int Total = Setores.Count;

            ViewBag.Paginas = Math.Ceiling(Convert.ToDecimal(Convert.ToDouble(Total) / Range)); // conta paginas 

            Pagina = Pagina > ViewBag.Paginas ? Convert.ToInt32(ViewBag.Paginas) - 1 : Pagina; // trata pagina para nn ser maior qu o numero maximop possivel de paginas dentro da lista de objs

            Pagina = Setores.Count < Range ? Pagina * Setores.Count : Pagina * Range; // trata pagina para ser o endereço do primeiro registro da pagina do obj

            Range = Range + Pagina > Setores.Count ? Setores.Count - Pagina : Range; // trata range para que nn seja maior que o tamanho do obj 

            Setores = Setores.Count < Range ? Setores : Setores.GetRange(Pagina, Range); // monta pagina

            ViewBag.Total = $"Registro(s): {Total} - Exibindo de {(Pagina == 0 ? 1 : Pagina + 1)} a {(Pagina == 0 ? Range : Pagina + Range)} - { ViewBag.Paginas} Página(s)"; // monta string do footer

            return PartialView(Setores);
        }

        public static bool VerificaFuncionarios(long id)
        {
            MyContext Bank = new MyContext();
            return Bank.Setores.Include(x => x.Funcionarios).FirstOrDefault(x => x.IDSetor == id).Funcionarios.Count == 0;
        }

        [HttpGet]
        [VerificaLogin]
        public ActionResult CadastroSetores_Incluir()
        {
            return PartialView();
        }

        [HttpPost]
        
        [VerificaLogin]
        public ActionResult CadastroSetores_Incluir_Salvar(long? IDSetor, String Nome)
        {
            if (String.IsNullOrEmpty(Nome))
            {
                return Json(new { status = false, msg = "Preencha todos os campos" });
            }
            else
            {
                try
                {
                    if (IDSetor.HasValue && Bank.Setores.Where(x=>x.IDSetor == IDSetor.Value && Session["Empresa"].ToString().Contains(x.IDEmpresa.ToString("0000"))).Count() > 0)
                    {
                        throw new Exception("Já existe um Setor com esse código");
                    }


                    Login.Models.LoginSistema UsuarioLogado = BankLogin
                    .LoginSistema.AsNoTracking()
                    .Include(x => x.Cliente)
                    .ThenInclude(x => x.LoginSistema).AsNoTracking()
                    .FirstOrDefault(x => x.IDLoginsistema == Convert.ToInt32(Session["Usuario"].ToString())); // carrega usuario logado

                    Setores Setor = new Setores
                    {
                        IDSetor = IDSetor == null ? Bank.Setores.Where(x=> Session["Empresa"].ToString().Contains(x.IDEmpresa.ToString("0000"))).OrderBy(x=>x.IDSetor).LastOrDefault() == null ? 1 : Bank.Setores.Where(x => Session["Empresa"].ToString().Contains(x.IDEmpresa.ToString("0000"))).OrderBy(x => x.IDSetor).LastOrDefault().IDSetor + 1 : Convert.ToInt64(IDSetor),
                        IDEmpresa = Bank.Empresas.FirstOrDefault(x => Session["Empresa"].ToString().Contains(x.IDEmpresa.ToString("0000"))).IDEmpresa,
                        Empresa = Bank.Empresas.FirstOrDefault(x => Session["Empresa"].ToString().Contains(x.IDEmpresa.ToString("0000"))),
                        Nome = Nome,
                    };

                    foreach (var Usuario in UsuarioLogado.Cliente.LoginSistema.ToList())
                    {
                        Bank.PermissoesdeSetores.Add(new PermissoesdeSetores
                        {
                            IDEmpresa = Setor.IDEmpresa,
                            Empresa = Setor.Empresa,
                            IDUsuario = Usuario.IDLoginsistema,
                            Setor = Setor,
                            IDSetor = Setor.IDSetor
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
        public ActionResult CadastroSetores_Alterar(int id)
        {
            Login.Models.LoginSistema UsuarioLogado = BankLogin
                       .LoginSistema.AsNoTracking()
                       .Include(x => x.Cliente)
                       .FirstOrDefault(x => x.IDLoginsistema == Convert.ToInt32(Session["Usuario"].ToString())); // carrega usuario logado


            if (!UsuarioLogado.PerfiMaster && Bank.PermissoesdeSetores.Where(x => x.IDSetor == id && x.IDUsuario == UsuarioLogado.IDLoginsistema).Count() == 0)
            {
                return new HttpUnauthorizedResult();
            }

            return PartialView(Bank.Setores.AsNoTracking().First(x=>x.IDSetor == id && Session["Empresa"].ToString().Contains(x.IDEmpresa.ToString("0000")))); //carrega obj por id e sessão na view 
        }

        [HttpPost]
        
        [VerificaLogin]
        public ActionResult CadastroSetores_Alterar_Salvar(int id, String Nome)
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


                    if (!UsuarioLogado.PerfiMaster && Bank.PermissoesdeSetores.Where(x => x.IDSetor == id && x.IDUsuario == UsuarioLogado.IDLoginsistema).Count() == 0)
                    {
                        return new HttpUnauthorizedResult();
                    }

                    Setores Setor = Bank.Setores.First(x=>x.IDSetor == id && Session["Empresa"].ToString().Contains(x.IDEmpresa.ToString("0000")));
                    Setor.Nome = Nome;
                    Bank.SaveChanges();// SaveChanges novas infrmações no obj
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
        public ActionResult CadastroSetores_Remover(int id)
        {
            Login.Models.LoginSistema UsuarioLogado = BankLogin
                       .LoginSistema.AsNoTracking()
                       .Include(x => x.Cliente)
                       .FirstOrDefault(x => x.IDLoginsistema == Convert.ToInt32(Session["Usuario"].ToString())); // carrega usuario logado


            if (!UsuarioLogado.PerfiMaster && Bank.PermissoesdeSetores.Where(x => x.IDSetor == id && x.IDUsuario == UsuarioLogado.IDLoginsistema).Count() == 0)
            {
                return new HttpUnauthorizedResult();
            }

            Setores Setor = Bank.Setores.AsNoTracking().Include(x => x.Funcionarios).AsNoTracking().First(x => x.IDSetor == id && Session["Empresa"].ToString().Contains(x.IDEmpresa.ToString("0000")));

            if (Setor.Funcionarios.Count > 0)
            {
                throw new Exception("Html Alterado");
            }
            return PartialView(Setor);// carrega obj por id e sessão
        }

        [HttpPost]
        
        [VerificaLogin]
        public ActionResult CadastroSetores_Remover_Salvar(int id)
        {
            try
            {
                Login.Models.LoginSistema UsuarioLogado = BankLogin
                       .LoginSistema.AsNoTracking()
                       .Include(x => x.Cliente)
                       .FirstOrDefault(x => x.IDLoginsistema == Convert.ToInt32(Session["Usuario"].ToString())); // carrega usuario logado


                if (!UsuarioLogado.PerfiMaster && Bank.PermissoesdeSetores.Where(x => x.IDSetor == id && x.IDUsuario == UsuarioLogado.IDLoginsistema).Count() == 0)
                {
                    return new HttpUnauthorizedResult();
                }

                Setores Setor = Bank.Setores.Include(x => x.Funcionarios).AsNoTracking().First(x => x.IDSetor == id && Session["Empresa"].ToString().Contains(x.IDEmpresa.ToString("0000")));
                if (Setor.Funcionarios.Count > 0)
                {
                    throw new Exception("Html Alterado");
                }
                Bank.Setores.Remove(Setor);
                Bank.SaveChanges();// Remove obj por id e sessão 
                return Json(new { status = true });
            }
            catch (Exception e)
            {
                return Json(new { status = false, msg = e.Message });
            }
        }
                
        [HttpGet]
        [VerificaLogin]
        public ActionResult CadastroSetores_Remover_Selecao(long[] id)
        {
            Login.Models.LoginSistema UsuarioLogado = BankLogin
                       .LoginSistema.AsNoTracking()
                       .Include(x => x.Cliente)
                       .FirstOrDefault(x => x.IDLoginsistema == Convert.ToInt32(Session["Usuario"].ToString())); // carrega usuario logado


            if (!UsuarioLogado.PerfiMaster && Bank.PermissoesdeSetores.Where(x => id.Contains(x.IDSetor) && x.IDUsuario == UsuarioLogado.IDLoginsistema).Count() == 0)
            {
                return new HttpUnauthorizedResult();
            }

            List<Setores> Setores = Bank.Setores.AsNoTracking().Where(x => id.Contains(x.IDSetor) && Session["Empresa"].ToString().Contains(x.IDEmpresa.ToString("0000"))).ToList();
            return PartialView(Setores);
        }      

        [HttpPost]
        
        [VerificaLogin]
        public ActionResult CadastroSetores_Remover_Selecao_Salvar(long[] id)
        {
            try
            {
                Login.Models.LoginSistema UsuarioLogado = BankLogin
                      .LoginSistema.AsNoTracking()
                      .Include(x => x.Cliente)
                      .FirstOrDefault(x => x.IDLoginsistema == Convert.ToInt32(Session["Usuario"].ToString())); // carrega usuario logado


                if (!UsuarioLogado.PerfiMaster && Bank.PermissoesdeSetores.Where(x => id.Contains(x.IDSetor) && x.IDUsuario == UsuarioLogado.IDLoginsistema).Count() == 0)
                {
                    return new HttpUnauthorizedResult();
                }

                List<Setores> Setores = Bank.Setores.Include(x => x.Funcionarios).AsNoTracking().Where(x => id.Contains(x.IDSetor) && Session["Empresa"].ToString().Contains(x.IDEmpresa.ToString("0000"))).ToList();

                if (Setores.Select(x=>x.Funcionarios).Count() > 0)
                {
                    throw new Exception("Não é possivel Remover um Setor que tem Funcionarios");
                }

                Bank.Setores.RemoveRange(Setores);
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