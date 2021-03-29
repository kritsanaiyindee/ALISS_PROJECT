using ALISS.HISUpload.Batch.Models;
using ALISS.HISUpload.DTO;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace ALISS.HISUpload.Batch.DataAccess
{
    public class HISDataContext : DbContext
    {
        private static IConfiguration _iconfiguration;
        public DbSet<TRSTGHISFileUploadDetail> TRSTGHISFileUploadDetails { get; set; }
        public DbSet<TRSTGHISFileUploadHeader> TRSTGHISFileUploadHeaders { get; set; }
        public DbSet<TRHISFileUploadSummary> TRHISFileUploadSummarys { get; set; }
        public DbSet<HISUploadDataDTO> HISUploadDataDTOs { get; set; }
        public DbSet<TCParameter> TCParameters { get; set; }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            var builder = new ConfigurationBuilder()
                              .SetBasePath(Directory.GetCurrentDirectory())
                              .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);
            _iconfiguration = builder.Build();

            optionsBuilder.UseSqlServer(_iconfiguration.GetConnectionString("HISFileUploadContext"));
        }
        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.Entity<HISUploadDataDTO>().HasKey(x => x.hfu_id);
            builder.Entity<TCParameter>().HasKey(x => x.prm_id);

            builder.Entity<TRSTGHISFileUploadHeader>().HasKey(x => x.huh_id);
            builder.Entity<TRSTGHISFileUploadHeader>().ToTable("TRSTGHISFileUploadHeader");

            builder.Entity<TRSTGHISFileUploadDetail>().HasKey(x => x.hud_id);
            builder.Entity<TRSTGHISFileUploadDetail>().ToTable("TRSTGHISFileUploadDetail");

            builder.Entity<TRHISFileUploadSummary>().HasKey(x => x.hus_id);
            builder.Entity<TRHISFileUploadSummary>().ToTable("TRHISFileUploadSummary");

            base.OnModelCreating(builder);
        }
    }

}