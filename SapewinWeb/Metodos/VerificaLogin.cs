using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Login.Models;
using Microsoft.EntityFrameworkCore;
using SapewinWeb.Models;

namespace SapewinWeb.Metodos
{
    public class VerificaLogin : ActionFilterAttribute
    {  
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            filterContext.HttpContext.Session["Usuario"] = 1;
            if (filterContext.HttpContext.Session["Usuario"] != null) {
                    
                LoginModel bank = new LoginModel();              
                LoginSistema UsuarioLogado = bank.LoginSistema.First(x=>x.IDLoginsistema == Convert.ToInt32(filterContext.HttpContext.Session["Usuario"].ToString()));
                MyContext Bank = new MyContext();

                if (UsuarioLogado == null)
                {
                    filterContext.Result = new RedirectResult("http://192.168.0.20/");
                }
                else
                {
                    SapewinWeb.Models.MyContext b = new Models.MyContext();
                    List<int> EmpresasdoUsuarioLogado = b.PermissoesdeEmpresas.Where(x => x.IDUsuario == UsuarioLogado.IDLoginsistema).Select(x => x.IDEmpresa).ToList();

                    if (filterContext.HttpContext.Session["Empresa"] == null)
                    {
                        SapewinWeb.Models.Empresas Empresa = b.Empresas.First(x => EmpresasdoUsuarioLogado.Contains(x.IDEmpresa) || UsuarioLogado.PerfiMaster);
                        filterContext.HttpContext.Session["Empresa"] = Empresa.IDEmpresa.ToString("0000") + " - " + Empresa.Nome;
                    }

                    if (bank.ProdutosCliente.Where(x => x.IDCliente == UsuarioLogado.IDCliente && x.IDProduto == 1).Count() > 0)
                    {
                        if ((filterContext.HttpContext.Request.Headers.AllKeys.Contains("X-Requested-With") && filterContext.HttpContext.Request.Headers.GetValues("X-Requested-With").Where(x => x == "XMLHttpRequest").Count() > 0) || filterContext.ActionDescriptor.ActionName.Contains("Abrir") || filterContext.ActionDescriptor.ActionName.Contains("Index"))
                        {
                            List<string> Permissoes = Bank.PermissoesdeTelas.Where(x=>x.IDUsuario == UsuarioLogado.IDLoginsistema).Select(x => x.IDFuncaoTela.ToLower()).Distinct().ToList();
                            if (Permissoes.Contains(filterContext.ActionDescriptor.ActionName.Replace("_Salvar", "").Replace("_Selecao", "").Replace("_", "-").ToLower()) || UsuarioLogado.PerfiMaster || filterContext.ActionDescriptor.ActionName.Contains("Index")) 
                            {
                                return;
                            }
                            else
                            {
                                filterContext.Result = new HttpUnauthorizedResult();
                            }
                        }
                        else
                        {
                            filterContext.Result = new HttpUnauthorizedResult();
                        }
                    }
                    else
                    {
                        filterContext.Result = new RedirectResult("http://192.168.0.20/");
                    }                       
                }
            }else
            {
                filterContext.Result = new RedirectResult("http://192.168.0.20/");
            }
        }
    }
}