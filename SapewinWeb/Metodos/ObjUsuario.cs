using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Login.Models;
using SapewinWeb.Models;
using Newtonsoft.Json.Linq;
using System.Web.Script.Serialization;
using Microsoft.EntityFrameworkCore;

namespace SapewinWeb.Metodos
{
    public class ObjUsuario
    {
        static public string[] RetornaIDSetor(List<Setores> Setores)
        {  
            return  Setores.Select(x=> $"{x.IDSetor}/{x.IDEmpresa}").ToArray();
        }

        static public string[] RetornaIDDepartamento(List<Departamentos> Departamentos)
        {
            return Departamentos.Select(x=> $"{x.IDDepartamento}/{x.IDEmpresa}").ToArray();
        }

        static public string[] RetornaDepartamentosdoUsuario(int id)
        {
            MyContext Bank = new MyContext();
            var i = Bank.PermissoesdeDepartamentos.Where(x => x.IDUsuario == id).Select(x => $"{x.IDDepartamento}/{x.IDEmpresa}").ToArray();
            return i;
        }

        static public string[] RetornaSetoresdoUsuario(int id)
        {
            MyContext Bank = new MyContext();
            var i = Bank.PermissoesdeSetores.Where(x => x.IDUsuario == id).Select(x => $"{x.IDSetor}/{x.IDEmpresa}").ToArray();
            return i;
        }

        static public List<SapewinWeb.Models.Funcoes> RetornaFuncoes(List<SapewinWeb.Models.FuncoesdeTelas> Funcoesdetela)
        {
            MyContext Bank = new MyContext();
            int[] i = Funcoesdetela.Select(x => x.IDFuncao).ToArray();
            return Bank.Funcoes.Where(x => i.Contains(x.IDFuncao)).ToList();
        }

        public enum Tipo
        {
           Master = 0, Administrador = 1, Supervisor = 2, Usuario = 3, Colaborador = 4
        }

        static public int[] RetornaIDEmpresas(int id)
        {

            MyContext Bank = new MyContext();

            List<Empresas> Empresas = Bank.PermissoesdeEmpresas.Include(x => x.Empresa).Where(x => x.IDUsuario == id).Select(x => x.Empresa).ToList();

            int[] e = Empresas.Select(x => x.IDEmpresa).ToArray();

            return e;
        }

        static public bool ComparaIDEmpresa(int idusuario, int idempresa)
        {
            MyContext Bank = new MyContext();

            return Bank.PermissoesdeEmpresas.Where(x => x.IDUsuario == idusuario && x.IDEmpresa == idempresa).Count() > 0;
        }
    }
}