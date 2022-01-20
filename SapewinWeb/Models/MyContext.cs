using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
//using Login.Models;

namespace SapewinWeb.Models
{
    public class MyContext : DbContext
    {
        //LoginModel Bank = new LoginModel();

        public MyContext()
        {
            //Database.EnsureCreated();
        }
        
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            //LoginSistema UsuarioLogado = Bank.LoginSistema.Include(x => x.Cliente).ThenInclude(x => x.Servidor).FirstOrDefault(x => x.IDLoginsistema == Convert.ToInt32(HttpContext.Current.Session["Usuario"].ToString()));
            if (!optionsBuilder.IsConfigured)
            {
                //optionsBuilder.UseSqlServer("Server=192.168.0.20;Database=Sapewin_25213339000105;User Id=sa;Password=dpa5461*");
                //if (UsuarioLogado.Cliente.Servidor.Tipo.ToUpper() == "MYSQL")
                //{
                //    ConfigurationManager.AppSettings.Set("MySql", "true");
                //    optionsBuilder.UseMySql($"Server={UsuarioLogado.Cliente.Servidor.CamihoBancoAtual};Database=Sapewin_{UsuarioLogado.Cliente.Documento};Uid={UsuarioLogado.Cliente.Servidor.Usuario};Pwd={UsuarioLogado.Cliente.Servidor.Senha};");
                //}
                //else
                //{
                //    ConfigurationManager.AppSettings.Set("MySql", "false");
                //    optionsBuilder.UseSqlServer($"Server={UsuarioLogado.Cliente.Servidor.CamihoBancoAtual};Database=Sapewin_{UsuarioLogado.Cliente.Documento};User Id={UsuarioLogado.Cliente.Servidor.Usuario};Password={UsuarioLogado.Cliente.Servidor.Senha};");
                //}

                if (!optionsBuilder.IsConfigured)
                {
                    optionsBuilder.UseSqlServer("Server=localhost;Database=sapewin;User Id=sa;Password=dpa5461*;");
                }

            }

            base.OnConfiguring(optionsBuilder);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<PermissoesdeDepartamentos>().HasOne(x => x.Departamento).WithMany(x => x.PermissoesdeDepartamentos).OnDelete(Microsoft.EntityFrameworkCore.Metadata.DeleteBehavior.Cascade);
            modelBuilder.Entity<PermissoesdeDepartamentos>().HasKey(x => new { x.IDUsuario, x.IDEmpresa, x.IDDepartamento });

            modelBuilder.Entity<PermissoesdeFuncionarios>().HasKey(x => new { x.IDUsuario, x.IDEmpresa, x.IDFuncionario });
            modelBuilder.Entity<PermissoesdeSetores>().HasKey(x => new { x.IDUsuario, x.IDEmpresa, x.IDSetor });            
            modelBuilder.Entity<PermissoesdeEmpresas>().HasKey(x => new { x.IDUsuario, x.IDEmpresa });            
            modelBuilder.Entity<PermissoesdeTelas>().HasKey(x => new { x.IDUsuario, x.IDEmpresa, x.IDFuncaoTela });
            modelBuilder.Entity<Cargos>().HasKey(x => new { x.IDCargo });
            modelBuilder.Entity<Departamentos>().HasKey(x => new { x.IDDepartamento, x.IDEmpresa });
            modelBuilder.Entity<Empresas>().HasKey(x => new { x.IDEmpresa });
            modelBuilder.Entity<Funcionarios>().HasKey(x => new { x.IDFuncionario, x.IDEmpresa });
            modelBuilder.Entity<Funcoes>().HasKey(x => new { x.IDFuncao });
            modelBuilder.Entity<FuncoesdeTelas>().HasKey(x => new { x.IDFuncaoTela });
            modelBuilder.Entity<Setores>().HasKey(x => new { x.IDSetor, x.IDEmpresa });
            modelBuilder.Entity<Telas>().HasKey(x => new { x.IDTela });
            modelBuilder.Entity<FeriadosGerais>().HasKey(x => new { x.IDFeriado });
            modelBuilder.Entity<GrupodeFeriados>().HasKey(x => new { x.IDFeriado });
            modelBuilder.Entity<FeriadosEspecificos>().HasKey(x=> new { x.IDFeriado, x.Dia, x.Mes, x.Ano});
            modelBuilder.Entity<MotivosdeAbono>().HasKey(x => new { x.IDEmpresa, x.Abreviacao });
            modelBuilder.Entity<Horarios>().HasKey(x => new { x.IDHorario });
            modelBuilder.Entity<Escalas>().HasKey(x => new { x.IDEmpresa, x.IDEscala });
            modelBuilder.Entity<EscalasHorarios>().HasKey(x => new { x.IDEmpresa, x.IDEscala, x.IDHorario, x.Ordem });
            modelBuilder.Entity<IntervalosAuxiliares>().HasKey(x => new { x.IDEmpresa, x.IDHorario, x.IDIntervalo });
            modelBuilder.Entity<Parametros>().HasKey(x => new {  x.IDParametro, x.IDEmpresa });
            modelBuilder.Entity<EscalonamentodeHoraExtra>().HasKey(x => new { x.IDParametro, x.IDEmpresa, x.Tipo, x.Horas, x.Porcentagem });
            modelBuilder.Entity<CartaoProximidade>().HasKey(x => new { x.IDCartao, x.IDFuncionario, x.IDEmpresa });
            modelBuilder.Entity<Mensagem>().HasKey(x => new { x.IDMensagem });
            modelBuilder.Entity<MensagensFuncionarios>().HasKey(x => new { x.IDMensagem, x.IDFuncionario, x.IDEmpresa });
            modelBuilder.Entity<Afastamentos>().HasKey(x => new { x.IDAfastamento, x.IDFuncionario, x.IDEmpresa, x.Abreviacao, x.DataInicial });
            modelBuilder.Entity<Folgas>().HasKey(x => new { x.IDFolga, x.IDFuncionario, x.Data, x.IDEmpresa });
            modelBuilder.Entity<HorariosOcasionais>().HasKey(x => new { x.IDHorario, x.IDFuncionario, x.Data, x.IDHorarioOcasional, x.IDEmpresa });
            modelBuilder.Entity<LogSistema>().HasKey(x => new { x.IDLog });
            modelBuilder.Entity<Teste>().HasKey(x => new { x.IDTeste });


            base.OnModelCreating(modelBuilder);
        }

        public virtual DbSet<Teste> Teste { get; set; }

        public virtual DbSet<LogSistema> LogSistema { get; set; }

        public virtual DbSet<HorariosOcasionais> HorariosOcasionais { get; set; }

        public virtual DbSet<Folgas> Folgas { get; set; }

        public virtual DbSet<Afastamentos> Afastamentos { get; set; }

        public virtual DbSet<MensagensFuncionarios> MensagensFuncionarios { get; set; }

        public virtual DbSet<Mensagem> Mensagem { get; set; }

        public virtual DbSet<CartaoProximidade> CartaoProximidade { get; set; }

        public virtual DbSet<EscalonamentodeHoraExtra> EscalonamentodeHoraExtra { get; set; }

        public virtual DbSet<Parametros> Parametros { get; set; }

        public virtual DbSet<IntervalosAuxiliares> IntervalosAuxiliares { get; set; }

        public virtual DbSet<EscalasHorarios> EscalasHorarios { get; set; }

        public virtual DbSet<Escalas> Escalas { get; set; }

        public virtual DbSet<Horarios> Horarios { get; set; }

        public virtual DbSet<MotivosdeAbono> MotivosdeAbono { get; set; }

        public virtual DbSet<Cargos> Cargos { get; set; }

        public virtual DbSet<Departamentos> Departamentos { get; set; }

        public virtual DbSet<Empresas> Empresas { get; set; }

        public virtual DbSet<Funcionarios> Funcionarios { get; set; }

        public virtual DbSet<Funcoes> Funcoes { get; set; }

        public virtual DbSet<FuncoesdeTelas> FuncoesdeTelas { get; set; }

        public virtual DbSet<PermissoesdeDepartamentos> PermissoesdeDepartamentos { get; set; }

        public virtual DbSet<PermissoesdeEmpresas> PermissoesdeEmpresas { get; set; }

        public virtual DbSet<PermissoesdeFuncionarios> PermissoesdeFuncionarios { get; set; }

        public virtual DbSet<PermissoesdeSetores> PermissoesdeSetores { get; set; }

        public virtual DbSet<PermissoesdeTelas> PermissoesdeTelas { get; set; }

        public virtual DbSet<Setores> Setores { get; set; }

        public virtual DbSet<Telas> Telas { get; set; }

        public virtual DbSet<FeriadosGerais> FeriadosGerais { get; set; }

        public virtual DbSet<GrupodeFeriados> GrupodeFeriados { get; set; }

        public virtual DbSet<FeriadosEspecificos> FeriadosEspecificos { get; set; }

    }
}