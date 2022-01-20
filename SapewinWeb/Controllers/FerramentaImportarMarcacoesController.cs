using Login.Models;
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
    public class FerramentaImportarMarcacoesController : Controller    {

        Login.Models.LoginModel BankLogin = new Login.Models.LoginModel();
        MyContext Bank = new MyContext();

        [HttpGet]
        [VerificaLogin]
        public ActionResult FerramentaImportarMarcacoes_Abrir()
        {
            return VerificaLoad.IsAjax(HttpContext.Request) ? PartialView() : (ViewResultBase)View();
        }

        [HttpGet]
        [VerificaLoad]
        public ActionResult UltimoProcessamento()
        {
            return PartialView();
        }

        [HttpGet]
        [VerificaLoad]
        public ActionResult GridFuncionarios(TipodeGrid Tipo)
        {
            return PartialView(Tipo);
        }

        [HttpGet]
        [VerificaLoad]
        public ActionResult TabFuncionarios(TipodeGrid Tipo, String pesquisa)
        {
            Login.Models.LoginSistema UsuarioLogado = BankLogin
             .LoginSistema
             .FirstOrDefault(x => x.IDLoginsistema == Convert.ToInt32(Session["Usuario"].ToString())); // carrega usuario logado

            pesquisa = pesquisa.Replace('+', ' ');

            var IDEmpresa = Bank.Empresas.FirstOrDefault(x => Session["Empresa"].ToString().Contains(x.IDEmpresa.ToString("0000"))).IDEmpresa;

            var Funcionarios = UsuarioLogado.PerfiMaster ? Bank.Funcionarios.Where(x => x.IDEmpresa == IDEmpresa && (x.Nome.ToLower().Contains(pesquisa.ToLower()) || x.IDFuncionario.ToString().StartsWith(pesquisa))).ToList() : Bank.PermissoesdeFuncionarios.Include(x => x.Funcionario).Where(x => x.IDUsuario == UsuarioLogado.IDLoginsistema && x.IDEmpresa == IDEmpresa).Select(x => x.Funcionario).Where(x=>x.Nome.Contains(pesquisa) || x.IDFuncionario.ToString().StartsWith(pesquisa)).ToList();

            ViewBag.Ini = Tipo == TipodeGrid.Inicio ? "true" : "false";

            return PartialView(Funcionarios);
        }

        [HttpGet]
        [VerificaLoad]
        public ActionResult GridSetores(TipodeGrid Tipo)
        {
            return PartialView(Tipo);
        }

        [HttpGet]
        [VerificaLoad]
        public ActionResult TabSetores(TipodeGrid Tipo, String pesquisa)
        {
            Login.Models.LoginSistema UsuarioLogado = BankLogin
                .LoginSistema
                .FirstOrDefault(x => x.IDLoginsistema == Convert.ToInt32(Session["Usuario"].ToString())); // carrega usuario logado

            pesquisa = pesquisa.Replace('+', ' ');

            var IDEmpresa = Bank.Empresas.FirstOrDefault(x => Session["Empresa"].ToString().Contains(x.IDEmpresa.ToString("0000"))).IDEmpresa;

            var Setores = UsuarioLogado.PerfiMaster ? Bank.Setores.Where(x => x.IDEmpresa == IDEmpresa && (x.Nome.ToLower().Contains(pesquisa.ToLower()) || x.IDSetor.ToString().StartsWith(pesquisa))).ToList() : Bank.PermissoesdeSetores.Include(x => x.Setor).Where(x => x.IDUsuario == UsuarioLogado.IDLoginsistema && x.IDEmpresa == IDEmpresa).Select(x => x.Setor).Where(x=>x.Nome.ToLower().Contains(pesquisa.ToLower()) || x.IDSetor.ToString().StartsWith(pesquisa)).ToList();

            ViewBag.Ini = Tipo == TipodeGrid.Inicio ? "true" : "false";

            return PartialView(Setores);
        }

        [HttpGet]
        [VerificaLoad]
        public ActionResult GridDepartamentos(TipodeGrid Tipo)
        {           
            return PartialView(Tipo);
        }

        [HttpGet]
        [VerificaLoad]
        public ActionResult TabDepartamentos(TipodeGrid Tipo, String pesquisa)
        {
            Login.Models.LoginSistema UsuarioLogado = BankLogin
                .LoginSistema
                .FirstOrDefault(x => x.IDLoginsistema == Convert.ToInt32(Session["Usuario"].ToString())); // carrega usuario logado

            pesquisa = pesquisa.Replace('+', ' ');

            var IDEmpresa = Bank.Empresas.FirstOrDefault(x => Session["Empresa"].ToString().Contains(x.IDEmpresa.ToString("0000"))).IDEmpresa;

            var Departamentos = UsuarioLogado.PerfiMaster ? Bank.Departamentos.Where(x => x.IDEmpresa == IDEmpresa && (x.Nome.ToLower().Contains(pesquisa.ToLower()) || x.IDDepartamento.ToString().StartsWith(pesquisa))).ToList() : Bank.PermissoesdeDepartamentos.Include(x => x.Departamento).Where(x => x.IDUsuario == UsuarioLogado.IDLoginsistema && x.IDEmpresa == IDEmpresa).Select(x => x.Departamento).Where(x=>x.Nome.ToLower().Contains(pesquisa.ToLower()) || x.IDDepartamento.ToString().StartsWith(pesquisa)).ToList();

            ViewBag.Ini = Tipo == TipodeGrid.Inicio ? "true" : "false";

            return PartialView(Departamentos);
        }

        public enum TipodeGrid
        {
            Inicio = 1, Fim = 2
        }        
    }
}