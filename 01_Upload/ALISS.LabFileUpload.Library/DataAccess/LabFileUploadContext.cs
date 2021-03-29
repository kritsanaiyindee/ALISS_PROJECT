using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using ALISS.LabFileUpload.DTO;
using ALISS.LabFileUpload.Library.Models;


namespace ALISS.LabFileUpload.Library.DataAccess
{
    public class LabFileUploadContext : DbContext
    {
        public DbSet<TRLabFileUpload> TRLabFileUploads { get; set; }
        public DbSet<LabFileUploadDataDTO> LabFileUploadDataDTOs { get; set; }
        public DbSet<LabFileSummaryHeaderListDTO> LabFileSummaryHeaderListDTOs { get; set; }
        public DbSet<LabFileSummaryDetailListDTO> LabFileSummaryDetailListDTOs { get; set; }
        public DbSet<LabFileErrorHeaderListDTO> LabFileErrorHeaderListDTOs { get; set; }
        public DbSet<LabFileErrorDetailListDTO> LabFileErrorDetailListDTOs { get; set; }


        public LabFileUploadContext(DbContextOptions<LabFileUploadContext> options) : base(options)
        {

        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
           
            builder.Entity<LabFileUploadDataDTO>().HasKey(x => x.lfu_id);

            builder.Entity<LabFileSummaryHeaderListDTO>().HasKey(x => x.fsh_id);
            builder.Entity<LabFileSummaryDetailListDTO>().HasKey(x => x.fsd_id);
            builder.Entity<LabFileSummaryHeaderListDTO>()
                .HasMany(h => h.LabFileSummaryDetailLists)
                .WithOne(d => d.LabFileSummaryHeaderList);

            builder.Entity<LabFileSummaryDetailListDTO>()
                .HasOne(d => d.LabFileSummaryHeaderList)
                .WithMany(h => h.LabFileSummaryDetailLists);

            builder.Entity<LabFileErrorHeaderListDTO>().HasKey(x => x.feh_id);
            builder.Entity<LabFileErrorDetailListDTO>().HasKey(x => x.fed_id);

            builder.Entity<TRLabFileUpload>().HasKey(x => x.lfu_id);
            builder.Entity<TRLabFileUpload>().ToTable("TRLabFileUpload");

            base.OnModelCreating(builder);
        }

    }
}
