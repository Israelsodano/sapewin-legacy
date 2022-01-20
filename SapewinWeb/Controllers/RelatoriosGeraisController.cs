using Microsoft.EntityFrameworkCore;
using SapewinWeb.Models;
using System;
using System.IO;
using System.Linq;
using System.Web.Mvc;
using Login.Models;
using SapewinWeb.Metodos;

namespace SapewinWeb.Controllers
{
    public class RelatoriosGeraisController : Controller
    {
        MyContext Bank = new MyContext();
        LoginModel Banklogin = new LoginModel();
        private string Html = string.Empty;
        
        [HttpPost]
        public ActionResult GerarPDF(Tipo CategoriaRelatorio, string Ordem, int[] Ids)
        {
            try
            {
                if (Ids == null)
                {
                    throw new Exception("Selecione ao menos uma das empresas");
                }

                var Empresa = Bank.Empresas.FirstOrDefault(x => Session["Empresa"].ToString().Contains(x.IDEmpresa.ToString("0000")));
                
                ViewBag.NomeEmpresa = Empresa.Nome;
                
                ViewBag.Documento = Empresa.Documento.CnpjeValido() ? Empresa.Documento.ToCnpj() : Empresa.Documento.ToCpf();

                ViewBag.Rua = string.IsNullOrEmpty(Empresa.Endereco) ?  string.Empty : Empresa.Endereco;

                ViewBag.Estado = string.IsNullOrEmpty(Empresa.UF) ? string.Empty : Empresa.UF;

                var UsuarioLogado = Banklogin.LoginSistema.Include(x => x.Cliente).FirstOrDefault(x => x.IDLoginsistema == Convert.ToInt32(Session["Usuario"].ToString()));

                switch (CategoriaRelatorio)
                {
                    case Tipo.Cargos:
                        PdfCargos(Ordem, Ids);
                        break;
                    case Tipo.Empresas:
                        PdfEmpresa(Ordem, Ids, UsuarioLogado);
                        break;
                    default:
                        break;
                }

                return Json(new { status = true, file = Convert.ToBase64String(PdfSharpConvert(Html)), html = Html});
            }
            catch (Exception e)
            {
                return Json(new { status = false, msg = e.Message });   
            }
        }      

        public enum Tipo
        {
            Cargos,
            Empresas
        }

        private static byte[] PdfSharpConvert(string html)
        {
            byte[] res = null;
            using (MemoryStream ms = new MemoryStream())
            {
                var pdf = TheArtOfDev.HtmlRenderer.PdfSharp.PdfGenerator.GeneratePdf(html, PdfSharp.PageSize.A4);                
                pdf.Save(ms);
                res = ms.ToArray();
            }
            return res;
        }

        protected virtual string RenderPartialViewToString(string viewName, object model)
        {
            if (string.IsNullOrEmpty(viewName))
            {
                throw new Exception("Nome da View não pode ser nulo");
            }              

            ViewData.Model = model;

            using (StringWriter sw = new StringWriter())
            {
                ViewEngineResult viewResult = ViewEngines.Engines.FindPartialView(ControllerContext, viewName);
                ViewContext viewContext = new ViewContext(ControllerContext, viewResult.View, ViewData, TempData, sw);
                viewResult.View.Render(viewContext, sw);

                return sw.GetStringBuilder().ToString();
            }
        }

        #region Renderizar PDF

        private string PdfCargos(string Ordem, int[] Ids)
        {
            switch (Ordem.ToUpper())
            {
                case "CODIGO-UP":
                    Html = RenderPartialViewToString("GerarPDFCargos",
                    Bank.Cargos.Include(x => x.Funcionarios)
                     .Where(y => Ids.Contains(y.IDCargo))
                     .OrderBy(z => z.IDCargo).ToList());
                    break;

                case "CODIGO-DOWN":
                    Html = RenderPartialViewToString("GerarPDFCargos",
                    Bank.Cargos.Include(x => x.Funcionarios)
                    .Where(y => Ids.Contains(y.IDCargo))
                    .OrderByDescending(z => z.IDCargo).ToList());
                    break;

                case "DESCRICAO-UP":
                    Html = RenderPartialViewToString("GerarPDFCargos",
                    Bank.Cargos.Include(x => x.Funcionarios)
                    .Where(y => Ids.Contains(y.IDCargo))
                    .OrderBy(z => z.Nome).ToList());
                    break;

                case "DESCRICAO-DOWN":
                    Html = RenderPartialViewToString("GerarPDFCargos",
                    Bank.Cargos.Include(x => x.Funcionarios)
                    .Where(y => Ids.Contains(y.IDCargo))
                    .OrderByDescending(z => z.Nome).ToList());
                    break;

                default:
                    break;
            }

            return Html;
        }

        private string PdfEmpresa(string Ordem, int[] Ids, LoginSistema Usuario)
        {
            
            switch (Ordem.ToUpper())
            {
                case "CODIGO-UP":
                    Html = RenderPartialViewToString("GerarPDFEmpresas",
                        Usuario.PerfiMaster ? Bank.Empresas.ToList() : Bank.PermissoesdeEmpresas
                        .Where(x => x.IDUsuario == Usuario.IDLoginsistema)
                        .Select(y => y.Empresa)
                        .Where(x => Ids.Contains(x.IDEmpresa))
                        .OrderBy(z => z.IDEmpresa).ToList());
                    break;

                case "CODIGO-DOWN":
                    Html = RenderPartialViewToString("GerarPDFEmpresas",
                        Usuario.PerfiMaster ? Bank.Empresas.ToList() : Bank.PermissoesdeEmpresas
                        .Where(x => x.IDUsuario == Usuario.IDLoginsistema)
                        .Select(y => y.Empresa)
                        .Where(x => Ids.Contains(x.IDEmpresa))
                        .OrderByDescending(z => z.IDEmpresa).ToList());
                    break;

                case "RAZAO-UP":
                    Html = RenderPartialViewToString("GerarPDFEmpresas",
                        Usuario.PerfiMaster ? Bank.Empresas.ToList() : Bank.PermissoesdeEmpresas
                        .Where(x => x.IDUsuario == Usuario.IDLoginsistema)
                        .Select(y => y.Empresa)
                        .Where(x => Ids.Contains(x.IDEmpresa))
                        .OrderBy(z => z.Nome).ToList());
                    break;

                case "RAZAO-DOWN":
                    Html = RenderPartialViewToString("GerarPDFEmpresas",
                        Usuario.PerfiMaster ? Bank.Empresas.ToList() : Bank.PermissoesdeEmpresas/*.Include(x => x.Empresa)*/
                        .Where(x => x.IDUsuario == Usuario.IDLoginsistema)
                        .Select(y => y.Empresa)
                        .Where(x => Ids.Contains(x.IDEmpresa))
                        .OrderByDescending(z => z.Nome).ToList());
                    break;
                case "DOCUMENTO-UP":
                    Html = RenderPartialViewToString("GerarPDFEmpresas",
                       Usuario.PerfiMaster ? Bank.Empresas.ToList() : Bank.PermissoesdeEmpresas
                       .Where(x => x.IDUsuario == Usuario.IDLoginsistema)
                       .Select(y => y.Empresa)
                       .Where(x => Ids.Contains(x.IDEmpresa))
                       .OrderBy(z => z.Documento).ToList());
                    break;
                case "DOCUMENTO-DOWN":
                    Html = RenderPartialViewToString("GerarPDFEmpresas",
                        Usuario.PerfiMaster ? Bank.Empresas.ToList() : Bank.PermissoesdeEmpresas
                        .Where(x => x.IDUsuario == Usuario.IDLoginsistema)
                        .Select(y => y.Empresa)
                        .Where(x => Ids.Contains(x.IDEmpresa))
                        .OrderByDescending(z => z.Documento).ToList());
                    break;
                default:
                    break;
            }
            return Html;
        }

        private string PdfSetores(string Ordem, int[] Ids, LoginSistema Usuario)
        {
            return Html;
        }

        private string PdfParametros(string Ordem, int[] Ids)
        {
            return Html;
        }

        private string PdfFeriados(string Ordem, int[] Ids)
        {
            return Html;
        }

        private string PdfHorarios(string Ordem, int[] Ids)
        {
            return Html;
        }

        private string PdfEscalas(string Ordem, int[] Ids)
        {
            return Html;
        }

        private string PdfMotivosAbono(string Ordem, int[] Ids)
        {
            return Html;
        }

        private string PdfFuncionarios(string Ordem, int[] Ids, LoginSistema Usuario)
        {
            return Html;
        }

        private string PdfMapaFuncoes(string Ordem, int[] Ids)
        {
            return Html;
        }

        #endregion
    }
}