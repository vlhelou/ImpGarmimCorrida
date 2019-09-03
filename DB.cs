using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.EntityFrameworkCore;


namespace ImportacaoGPX
{
    class DB : DbContext
    {
        public DbSet<Corrida> Corrida { get; set; }
        public DbSet<Track> Track { get; set; }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer("server=.;database=corrida;Integrated Security=True;");
        }

    }
}
