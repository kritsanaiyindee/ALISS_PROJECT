using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.EntityFrameworkCore;
using ALISS.UserManagement.Library.Models;
using ALISS.UserManagement.DTO;

namespace ALISS.UserManagement.Library.DataAccess
{
    public class UserManagementContext : DbContext
    {
        public DbSet<TRHospital> TRHospitals { get; set; }
        public DbSet<TRHospitalLab> TRHospitalLabs { get; set; }
        public DbSet<HospitalDTO> HospitalDTOs { get; set; }

        public DbSet<LogProcess> LogProcesss { get; set; }

        public UserManagementContext(DbContextOptions<UserManagementContext> options) : base(options)
        {

        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.Entity<TRHospital>().HasKey(x => x.hos_id);
            builder.Entity<TRHospital>().ToTable("TRHospital");

            builder.Entity<TRHospitalLab>().HasKey(x => x.lab_id);
            builder.Entity<TRHospitalLab>().ToTable("TRHospitalLab");

            builder.Entity<HospitalDTO>().HasKey(x => x.hos_id);

            builder.Entity<LogProcess>().HasKey(x => x.log_id);
            builder.Entity<LogProcess>().ToTable("XLogProcess");

            base.OnModelCreating(builder);
        }
    }
}
