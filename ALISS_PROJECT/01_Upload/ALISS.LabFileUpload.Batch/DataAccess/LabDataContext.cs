using ALISS.LabFileUpload.Batch.Models;
using ALISS.LabFileUpload.DTO;
using ALISS.Mapping.DTO;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace ALISS.LabFileUpload.Batch.DataAccess
{
    public class LabDataContext : DbContext
    {
        private static IConfiguration _iconfiguration;

        public DbSet<MappingDataDTO> MappingDataDTOs { get; set; }
        public DbSet<WHONetMappingListsDTO> WHONetMappingListsDTOs { get; set; }
        public DbSet<TRSTGLabFileDataDetail> TRSTGLabFileDataDetails { get; set; }
        public DbSet<TRSTGLabFileDataHeader> TRSTGLabFileDataHeaders { get; set; }
        public DbSet<LabFileUploadDataDTO> LabFileUploadDataDTOs { get; set; }
        public DbSet<TCParameter> TCParameters { get; set; }
        public DbSet<TRLabFileErrorHeader> TRLabFileErrorHeaders { get; set; }
        public DbSet<TRLabFileErrorDetail> TRLabFileErrorDetails { get; set; }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            var builder = new ConfigurationBuilder()
                             //  .SetBasePath(Directory.GetCurrentDirectory())
                               .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);
            _iconfiguration = builder.Build();

            optionsBuilder.UseSqlServer(_iconfiguration.GetConnectionString("LabFileUploadContext"));
        }
        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.Entity<LabFileUploadDataDTO>().HasKey(x => x.lfu_id);
            builder.Entity<WHONetMappingListsDTO>().HasKey(x => x.wnm_id);
            builder.Entity<MappingDataDTO>().HasKey(x => x.mp_id);
            builder.Entity<TCParameter>().HasKey(x => x.prm_id);

            builder.Entity<TRSTGLabFileDataHeader>().HasKey(x => x.ldh_id);
            builder.Entity<TRSTGLabFileDataHeader>().ToTable("TRSTGLabFileDataHeader");

            builder.Entity<TRSTGLabFileDataDetail>().HasKey(x => x.ldd_id);
            builder.Entity<TRSTGLabFileDataDetail>().ToTable("TRSTGLabFileDataDetail");

            builder.Entity<TRLabFileErrorHeader>().HasKey(x => x.feh_id);
            builder.Entity<TRLabFileErrorHeader>().ToTable("TRLabFileErrorHeader");

            builder.Entity<TRLabFileErrorDetail>().HasKey(x => x.fed_id);
            builder.Entity<TRLabFileErrorDetail>().ToTable("TRLabFileErrorDetail");

            base.OnModelCreating(builder);
        }
    }
}
