using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Painel
{
    class PainelContext : DbContext
    {
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {                
                optionsBuilder.UseMySql("Server=192.168.0.20;Database=login2;Uid=sa;Pwd=dpa5461*;");
                //optionsBuilder.UseMySql("server=localhost;uid=websape;pwd=Dpa546101;database=login");
            }

            base.OnConfiguring(optionsBuilder);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Funcoes>().HasKey(x => new { x.IDFuncao });
            modelBuilder.Entity<FuncoesDeTelas>().HasKey(x => new { x.IDFuncaoTela });
            modelBuilder.Entity<Telas>().HasKey(x => new { x.IDTela });
            base.OnModelCreating(modelBuilder);
        }

        public virtual DbSet<Funcoes> Funcoes { get; set; }

        public virtual DbSet<FuncoesDeTelas> FuncoesdeTelas { get; set; }

        public virtual DbSet<Telas> Telas { get; set; }
    }
}
