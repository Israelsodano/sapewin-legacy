using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SapewinWeb.Models;
using Login.Models;

namespace SapewinWeb.Metodos
{
    public static class LogGeral 
    {       
        public static void GeraLog(string Funcao, string Tela, string Descricao)
        {
            var Bank = new MyContext();

            var UsuarioLogado = new LoginModel().LoginSistema.First(x => x.IDLoginsistema == Convert.ToInt32(HttpContext.Current.Session["Usuario"]));

            var Log = new SapewinWeb.Models.LogSistema { DataHora = DateTime.Now, Descricao = Descricao, Funcao = Funcao, Tela = Tela, IP = HttpContext.Current.Request.UserHostAddress, IDLog = Bank.LogSistema.OrderBy(x=>x.IDLog).LastOrDefault() == null ? 1 : Bank.LogSistema.OrderBy(x => x.IDLog).LastOrDefault().IDLog + 1, IDUsuario = UsuarioLogado.IDLoginsistema };

            Bank.LogSistema.Add(Log);

            Bank.SaveChanges();
        }      
    }
}