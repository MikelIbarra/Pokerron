using System;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using PokerronBank.Model;

namespace PokerronBank.Data
{
    public sealed class AppDbContext : DbContext
    {


        private readonly string DbPath;


        public AppDbContext(string dbPath)
        {
       
            DbPath = dbPath;
           


        }

        public bool HasUnsavedChanges()
        {
            return ChangeTracker.HasChanges();
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlite($"Filename={DbPath}");
        }
       


       public DbSet<DataBaseVersion> DataBaseVersion { get; set; }
        public DbSet<Jugador> Jugadores { get; set; }
        public DbSet<Ingreso> Ingresos { get; set; }
        public DbSet<Partida> Partidas { get; set; }
        public DbSet<Compra> Compras { get; set; }




    }
}
