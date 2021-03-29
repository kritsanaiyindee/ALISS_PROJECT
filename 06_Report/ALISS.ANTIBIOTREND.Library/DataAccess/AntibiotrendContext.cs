using ALISS.ANTIBIOTREND.DTO;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace ALISS.ANTIBIOTREND.Library.DataAccess
{
    public class AntibiotrendContext : DbContext
    {
        public DbSet<SP_AntimicrobialResistanceDTO> DropdownAMRListDTOs { get; set; }
        public DbSet<NationHealthStrategyDTO> AMRNationHealthStrategyListDTOs { get; set; }
        public DbSet<AntibiotrendAMRStrategyDTO> AntibiotrendAMRStrategyListDTOs { get; set; }
        public AntibiotrendContext(DbContextOptions<AntibiotrendContext> options) : base(options)
        {

        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.Entity<SP_AntimicrobialResistanceDTO>().HasNoKey();
            builder.Entity<NationHealthStrategyDTO>().HasNoKey();
            builder.Entity<AntibiotrendAMRStrategyDTO>().HasNoKey();

            base.OnModelCreating(builder);
        }

    }
}
