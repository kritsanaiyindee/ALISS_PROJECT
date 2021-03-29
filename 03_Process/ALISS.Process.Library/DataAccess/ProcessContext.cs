using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.EntityFrameworkCore;
using ALISS.Process.Library.Models;
using ALISS.Process.DTO;

namespace ALISS.Process.Library.DataAccess
{
    public class ProcessContext : DbContext
    {
        public DbSet<TRProcessRequest> TRProcessRequests { get; set; }
        public DbSet<TRProcessRequestDetail> TRProcessRequestDetails { get; set; }
        public DbSet<ProcessRequestDTO> ProcessRequestDTOs { get; set; }
        public DbSet<ProcessRequestDetailDTO> ProcessRequestDetailDTOs { get; set; }
        public DbSet<ProcessRequestCheckDetailDTO> ProcessRequestCheckDetailDTOs { get; set; }

        public DbSet<LogProcess> LogProcesss { get; set; }

        public ProcessContext(DbContextOptions<ProcessContext> options) : base(options)
        {

        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.Entity<TRProcessRequest>().HasKey(x => x.pcr_id);
            builder.Entity<TRProcessRequest>().ToTable("TRProcessRequest");

            builder.Entity<TRProcessRequestDetail>().HasKey(x => x.pcd_id);
            builder.Entity<TRProcessRequestDetail>().ToTable("TRProcessRequestDetail");

            builder.Entity<ProcessRequestDTO>().HasKey(x => x.pcr_id);

            builder.Entity<ProcessRequestDetailDTO>().HasKey(x => x.pcd_id);

            builder.Entity<ProcessRequestCheckDetailDTO>().HasKey(x => x.pcd_id);

            builder.Entity<LogProcess>().HasKey(x => x.log_id);
            builder.Entity<LogProcess>().ToTable("XLogProcess");

            base.OnModelCreating(builder);
        }
    }
}
