using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.EntityFrameworkCore;
using ALISS.DropDownList.DTO;

namespace ALISS.DropDownList.Library.DataAccess
{
    public class DropDownListContext : DbContext
    {
        public DbSet<HospitalDataDTO> HospitalDataDTOs { get; set; }
        public DbSet<HospitalLabDataDTO> HospitalLabDataDTOs { get; set; }
        public DbSet<DropDownListDTO> DropDownListDTOs { get; set; }
        public DbSet<ParameterDTO> ParameterDTOs { get; set; }

        public DropDownListContext(DbContextOptions<DropDownListContext> options) : base(options)
        {

        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.Entity<HospitalDataDTO>().HasKey(x => x.hos_id);

            builder.Entity<HospitalLabDataDTO>().HasKey(x => x.hos_id);

            builder.Entity<DropDownListDTO>().HasKey(x => x.data_id);

            builder.Entity<ParameterDTO>().HasKey(x => x.prm_id);

            base.OnModelCreating(builder);
        }
    }
}
