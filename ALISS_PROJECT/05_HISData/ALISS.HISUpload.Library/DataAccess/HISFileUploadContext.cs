using ALISS.HISUpload.DTO;
using ALISS.HISUpload.Library.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace ALISS.HISUpload.Library.DataAccess
{
    public class HISFileUploadContext : DbContext
    {
        public DbSet<TRHISFileUpload> TRHISFileUploads { get; set; }
        public DbSet<TRHISFileUploadSummary> TRHISFileUploadSummarys { get; set; }
        public DbSet<HISUploadDataDTO> HISFileUploadDataDTOs { get; set; }
        public DbSet<HISFileTemplateDTO> HISFileTemplateDTOs { get; set; }
        public DbSet<HISFileUploadSummaryDTO> HISFileUploadSummaryDTOs { get; set; }
        public DbSet<LabDataWithHISDTO> LabDataWithHISDTOs { get; set; }
        public HISFileUploadContext(DbContextOptions<HISFileUploadContext> options) : base(options)
        {

        }
        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.Entity<TRHISFileUpload>().HasKey(x => x.hfu_id);
            builder.Entity<TRHISFileUpload>().ToTable("TRHISFileUpload");

            builder.Entity<TRHISFileUploadSummary>().HasKey(x => x.hus_id);
            builder.Entity<TRHISFileUploadSummary>().ToTable("TRHISFileUploadSummary");

            builder.Entity<HISFileUploadSummaryDTO>().HasKey(x => x.hus_id);

            builder.Entity<HISUploadDataDTO>().HasKey(x => x.hfu_id);

            builder.Entity<HISFileTemplateDTO>().HasKey(x => x.hft_id);
            //builder.Entity<HISFileTemplateDTO>().ToTable("TCHISFileTemplate");

            builder.Entity<LabDataWithHISDTO>().HasNoKey();

            base.OnModelCreating(builder);
        }

    }
}
