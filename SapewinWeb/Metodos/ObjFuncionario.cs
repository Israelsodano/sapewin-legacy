using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using SapewinWeb.Models;
using Microsoft.EntityFrameworkCore;

namespace SapewinWeb.Metodos
{
    public class ObjFuncionario
    {
        public static List<Setores> RetornaSetores()
        {
            MyContext Bank = new MyContext();

            Login.Models.LoginModel BankLogin = new Login.Models.LoginModel();

            Login.Models.LoginSistema UsuarioLogado = BankLogin
                .LoginSistema.AsNoTracking()
                .FirstOrDefault(x => x.IDLoginsistema == Convert.ToInt32(HttpContext.Current.Session["Usuario"].ToString())); // carrega usuario logado
            
            int idempresa = Bank.Empresas.FirstOrDefault(x => HttpContext.Current.Session["Empresa"].ToString().Contains(x.IDEmpresa.ToString("0000"))).IDEmpresa;

            return UsuarioLogado.PerfiMaster ? Bank.Setores.Where(x => x.IDEmpresa == idempresa).ToList() : Bank.PermissoesdeSetores.Where(x => x.IDUsuario == UsuarioLogado.IDLoginsistema && x.IDEmpresa == idempresa).Select(x => x.Setor).ToList();

        }

        public static List<Departamentos> RetornaDepartamentos()
        {
            MyContext Bank = new MyContext();

            Login.Models.LoginModel BankLogin = new Login.Models.LoginModel();

            Login.Models.LoginSistema UsuarioLogado = BankLogin
                .LoginSistema.AsNoTracking()
                .FirstOrDefault(x => x.IDLoginsistema == Convert.ToInt32(HttpContext.Current.Session["Usuario"].ToString())); // carrega usuario logado

            int idempresa = Bank.Empresas.FirstOrDefault(x => HttpContext.Current.Session["Empresa"].ToString().Contains(x.IDEmpresa.ToString("0000"))).IDEmpresa;

            return UsuarioLogado.PerfiMaster ? Bank.Departamentos.Where(x => x.IDEmpresa == idempresa).ToList() : Bank.PermissoesdeDepartamentos.Where(x => x.IDUsuario == UsuarioLogado.IDLoginsistema && x.IDEmpresa == idempresa).Select(x => x.Departamento).ToList();

        }

        public static List<Parametros> RetornaParametros()
        {
            MyContext Bank = new MyContext();            

            int idempresa = Bank.Empresas.FirstOrDefault(x => HttpContext.Current.Session["Empresa"].ToString().Contains(x.IDEmpresa.ToString("0000"))).IDEmpresa;

            return Bank.Parametros.Where(x => x.IDEmpresa == idempresa).ToList();
        }

        public static List<Cargos> RetornaCargos()
        {
            MyContext Bank = new MyContext();            

            return Bank.Cargos.ToList();
        }

        public static List<Escalas> RetornaEscalas()
        {
            MyContext Bank = new MyContext();            

            int idempresa = Bank.Empresas.FirstOrDefault(x => HttpContext.Current.Session["Empresa"].ToString().Contains(x.IDEmpresa.ToString("0000"))).IDEmpresa;

            return Bank.Escalas.Where(x => x.IDEmpresa == idempresa).ToList();
        }

        public static String RetornaCartao(List<CartaoProximidade> ListaCartoes) 
        {
            var Cartao = ListaCartoes.FirstOrDefault(x => x.DataInicial <= DateTime.Now && (x.DataFinal == null || x.DataFinal >= DateTime.Now));

            var txt = Cartao == null ? "" : Cartao.NumerodoCartao.PadLeft(10, '0');

            return txt;
        }

        public static Mensagem RetornaMensagem(MensagensFuncionarios MsgFun)
        {
            MyContext Bank = new MyContext();

            return Bank.Mensagem.FirstOrDefault(x => x.IDMensagem == MsgFun.IDMensagem);
        }

        public static String RetornaCartoesdodia(List<CartaoProximidade> Cartoes)
        {
            var hoje = DateTime.Now.Date;

            var cartao = Cartoes.FirstOrDefault(x => hoje >= x.DataInicial && (x.DataFinal == null || hoje <= x.DataFinal));

            return cartao != null ? $"{cartao.DataInicial.Day.ToString("00")}/{cartao.DataInicial.Month.ToString("00")}/{cartao.DataInicial.Year.ToString("0000")} até {((cartao.DataFinal == null) ? ("Indefinido") : ($"{cartao.DataFinal.Value.Day.ToString("00")}/{cartao.DataFinal.Value.Month.ToString("00")}/{cartao.DataFinal.Value.Year.ToString("0000")}"))} - Código: {cartao.NumerodoCartao.PadLeft(10, '0')}" : "";
        }

        public static String RetornaAfastamentosdodia(List<Afastamentos> Afastamentos)
        {
            var hoje = DateTime.Now.Date;

            var afastamento = Afastamentos.FirstOrDefault(x => hoje >= x.DataInicial && (x.DataFinal == null || hoje <= x.DataFinal));

            return afastamento != null ? $"{afastamento.DataInicial.Day.ToString("00")}/{afastamento.DataInicial.Month.ToString("00")}/{afastamento.DataInicial.Year.ToString("0000")} até {((afastamento.DataFinal == null) ? ("Indefinido") : ($"{afastamento.DataFinal.Value.Day.ToString("00")}/{afastamento.DataFinal.Value.Month.ToString("00")}/{afastamento.DataFinal.Value.Year.ToString("0000")}"))} - Motivo: {afastamento.Abreviacao}" : "Não está afastado hoje";
        }

        public static String RetornaFolgasdodia(List<Folgas> Folgas)
        {
            var hoje = DateTime.Now.Date;

            var folga = Folgas.FirstOrDefault(x => hoje == x.Data);


            return folga != null ? $"Está de Folga hoje" : "Não está de Folga hoje";
        }

        public static String RetornaHorariosdodia(List<HorariosOcasionais> Horarios)
        {
            MyContext Bank = new MyContext();

            var hoje = DateTime.Now.Date;

            var horario = Horarios.FirstOrDefault(x => x.Data == hoje);

            var hr = horario == null ? null : Bank.Horarios.FirstOrDefault(x => x.IDHorario == horario.IDHorario && HttpContext.Current.Session["Empresa"].ToString().Contains(x.IDEmpresa.ToString("0000")));

            return horario != null ? $"Cumprindo o horário: {hr.IDHorario.ToString("0000")} - {hr.Descricao}" : "Cumprindo horário normal";
        }

    }   
}