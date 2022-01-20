using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Login.Models
{
    public class LoginModel : DbContext
    {
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlServer("Server=guiboni.database.windows.net;Database=ControlePonto;User Id=guiboni;Password=123mudar@;");
            }

            base.OnConfiguring(optionsBuilder);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Clientes>().HasKey(x=> new { x.IDCliente });
            modelBuilder.Entity<Funcoes>().HasKey(x => new { x.IDFuncao });
            modelBuilder.Entity<FuncoesDeTelas>().HasKey(x => new { x.IDFuncaoTela, x.IDProduto });
            modelBuilder.Entity<LoginFabRevenda>().HasKey(x => new { x.IDLoginfabrevenda });
            modelBuilder.Entity<LoginSistema>().HasKey(x => new { x.IDLoginsistema });
            //modelBuilder.Entity<LogSistema>().HasKey(x => new { x.IDLogsistema });
            modelBuilder.Entity<Produtos>().HasKey(x => new { x.IDProduto });
            modelBuilder.Entity<ProdutosCliente>().HasKey(x => new { x.IDCliente, x.IDProduto });
            modelBuilder.Entity<ProdutosRevenda>().HasKey(x => new { x.IDRevenda, x.IDProduto });
            modelBuilder.Entity<Revendas>().HasKey(x => new { x.IDRevenda });
            modelBuilder.Entity<Servidores>().HasKey(x => new { x.IDServidor});
            modelBuilder.Entity<ServidoresRevenda>().HasKey(x => new { x.IDServidor, x.IDRevenda });
            modelBuilder.Entity<Telas>().HasKey(x => new { x.IDTela });

            modelBuilder.Entity<Revendas>().HasMany(x => x.Revendedoras).WithOne(op => op.Fabricante).IsRequired(false).HasForeignKey(@"ID_Fabrica");
            modelBuilder.Entity<Revendas>().HasOne(x => x.Fabricante).WithMany(op => op.Revendedoras).IsRequired(false).HasForeignKey(@"ID_Fabrica");
            modelBuilder.Entity<Clientes>().HasMany(x => x.Filiais).WithOne(op => op.Matriz).IsRequired(false).HasForeignKey(@"IDMatriz");
            modelBuilder.Entity<Clientes>().HasOne(x => x.Matriz).WithMany(op => op.Filiais).IsRequired(false).HasForeignKey(@"IDMatriz");

            base.OnModelCreating(modelBuilder);
        }

        public virtual DbSet<Clientes> Clientes { get; set; }

        public virtual DbSet<Funcoes> Funcoes { get; set; }

        public virtual DbSet<FuncoesDeTelas> FuncoesDeTelas { get; set; }

        public virtual DbSet<LoginSistema> LoginSistema { get; set; }

       // public virtual DbSet<LogSistema> LogSistema { get; set; }

        public virtual DbSet<Produtos> Produtos { get; set; }

        public virtual DbSet<ProdutosCliente> ProdutosCliente { get; set; }

        public virtual DbSet<ProdutosRevenda> ProdutosRevenda { get; set; }

        public virtual DbSet<Revendas> Revendas { get; set; }

        public virtual DbSet<Servidores> Servidores { get; set; }

        public virtual DbSet<ServidoresRevenda> ServidoresRevenda { get; set; }

        public virtual DbSet<Telas> Telas { get; set; }

        public virtual DbSet<LoginFabRevenda> LoginFabRevenda { get; set; }
    }
}
