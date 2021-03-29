using Microsoft.EntityFrameworkCore;
using ALISS.GLASS.DTO;
using System;
using System.Collections.Generic;
using System.Text;
using ALISS.DropDownList.DTO;

namespace ALISS.GLASS.Library.DataAccess
{
    public class GlassContext : DbContext
    {
        public DbSet<GlassFileListDTO> DropdownGlassListDTOs { get; set; }
        public DbSet<GlassInfectOriginOverviewDTO> DropdownGlassOverviewListDTOs { get; set; }
        public DbSet<GlassPathogenNSDTO> DropdownGlassPathogenNSListDTOs { get; set; }
        public DbSet<GlassInfectSpecimenDTO> DropdownGlassInfectSpecimenDTOs { get; set; }
        public DbSet<GlassInfectPathAntiCombineDTO> DropdownGlassInfectPathAntiCombineDTOs { get; set; }
        public DbSet<ParameterDTO> ParameterDTOs { get; set; }
        public GlassContext(DbContextOptions<GlassContext> options) : base(options)
        {

        }
        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.Entity<GlassFileListDTO>().HasNoKey();
            builder.Entity<GlassInfectOriginOverviewDTO>().HasNoKey();
            builder.Entity<GlassPathogenNSDTO>().HasNoKey();
            builder.Entity<GlassInfectSpecimenDTO>().HasNoKey();
            builder.Entity<GlassInfectPathAntiCombineDTO>().HasNoKey();
            builder.Entity<ParameterDTO>().HasNoKey();

            base.OnModelCreating(builder);
        }
    }
}
