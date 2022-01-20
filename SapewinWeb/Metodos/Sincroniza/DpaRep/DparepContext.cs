
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Painel.Models;
using System.Web;

namespace DpaRep
{
    class DparepContext : DbContext
    {
        LoginModel Bank = new LoginModel();
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                LoginSistema UsuarioLogado = Bank.LoginSistema.Include(x => x.Cliente).ThenInclude(x => x.Servidor).FirstOrDefault(x => x.IDLoginsistema == Convert.ToInt32(HttpContext.Current.Session["Usuario"].ToString()));

                if (UsuarioLogado.Cliente.Servidor.Tipo.ToUpper() == "MYSQL")
                {
                    optionsBuilder.UseMySql($"Server={UsuarioLogado.Cliente.Servidor.CamihoBancoAtual};Database=DpaRep_{UsuarioLogado.Cliente.Documento};Uid={UsuarioLogado.Cliente.Servidor.Usuario};Pwd={UsuarioLogado.Cliente.Servidor.Senha};");
                }
                else
                {
                    optionsBuilder.UseSqlServer($"Server={UsuarioLogado.Cliente.Servidor.CamihoBancoAtual};Database=DpaRep_{UsuarioLogado.Cliente.Documento};User Id={UsuarioLogado.Cliente.Servidor.Usuario};Password={UsuarioLogado.Cliente.Servidor.Senha};");
                }
            }

            base.OnConfiguring(optionsBuilder);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {


            modelBuilder.Entity<Funcoes>().HasKey(x => new { x.IDFuncao });
            modelBuilder.Entity<FuncoesdeTelas>().HasKey(x => new { x.IDFuncaoTela });
            modelBuilder.Entity<Telas>().HasKey(x => new { x.IDTela });
            modelBuilder.Entity<Empresas>().HasKey(x => new { x.IDEmpresa });
            modelBuilder.Entity<PermissoesdeTelas>().HasKey(x => new { x.IDUsuario, x.IDFuncaoTela, x.IDEmpresa });

            base.OnModelCreating(modelBuilder);
        }

        public virtual DbSet<Funcoes> Funcoes { get; set; }

        public virtual DbSet<FuncoesdeTelas> FuncoesdeTelas { get; set; }

        public virtual DbSet<Empresas> Empresas { get; set; }

        public virtual DbSet<Telas> Telas { get; set; }

        public virtual DbSet<PermissoesdeTelas> PermissoesdeTelas { get; set; }

    }
}
