using ALISS.TC.WHONET_Antibiotics.Models;
using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System.IO;

namespace ALISS.TC.WHONET_Antibiotics.DataAccess
{
    public class ALISSContext : DbContext
    {
        private static IConfiguration _iconfiguration;

        public DbSet<TCWHONET_Antibiotics> TCWHONET_AntibioticsModel { get; set; }

        //public ALISSContext(DbContextOptions<ALISSContext> options) : base(options)
        //{

        //}

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            var builder = new ConfigurationBuilder()
                               .SetBasePath(Directory.GetCurrentDirectory())
                               .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);
            _iconfiguration = builder.Build();

            optionsBuilder.UseSqlServer(_iconfiguration.GetConnectionString("ALISSContext"));
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.Entity<TCWHONET_Antibiotics>().HasKey(x => x.who_ant_id);
            builder.Entity<TCWHONET_Antibiotics>().ToTable("TCWHONET_Antibiotics");

            base.OnModelCreating(builder);
        }
    }
}
