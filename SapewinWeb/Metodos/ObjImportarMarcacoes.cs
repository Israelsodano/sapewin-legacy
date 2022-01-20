using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SapewinWeb.Metodos
{
    public class ObjImportarMarcacoes
    {

        public static PermssoesImportacoes[] RetornaPermissoesUsuario()
        {

            var usuariologado = new Login.Models.LoginModel().LoginSistema.FirstOrDefault(x => x.IDLoginsistema == Convert.ToInt32(HttpContext.Current.Session["Usuario"].ToString()));

            var permissoesusuario = new SapewinWeb.Models.MyContext().PermissoesdeTelas.Where(x => x.IDUsuario == usuariologado.IDLoginsistema && HttpContext.Current.Session["Empresa"].ToString().Contains(x.IDEmpresa.ToString("0000"))).Select(x=>x.IDFuncaoTela).ToArray();

            var permissoes = usuariologado.PerfiMaster ? new PermssoesImportacoes[] { PermssoesImportacoes.Normal, PermssoesImportacoes.Recalcular, PermssoesImportacoes.Reanalisar } : new PermssoesImportacoes[] { permissoesusuario.Contains("FerramentaImportarMarcacoes-ImportarNormal") ? PermssoesImportacoes.Normal : PermssoesImportacoes.None, permissoesusuario.Contains("FerramentaImportarMarcacoes-ImportarRecalcular") ? PermssoesImportacoes.Recalcular : PermssoesImportacoes.None, permissoesusuario.Contains("FerramentaImportarMarcacoes-ImportarReanalisar") ? PermssoesImportacoes.Reanalisar : PermssoesImportacoes.None };

            permissoes.ToList().RemoveAll(x => x == PermssoesImportacoes.None);

            return permissoes.ToArray();
        }

        public enum PermssoesImportacoes
        {
            Normal,Recalcular,Reanalisar,None
        }
    }
}