using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using ALISS.Mapping.DTO;
using ALISS.Mapping.Library.Models;
using ALISS.MasterManagement.Library.Models;

namespace ALISS.Mapping.Library.DataAccess
{
    public class MappingContext : DbContext
    {
        public DbSet<LogProcess> LogProcesss { get; set; }
        public DbSet<TRMapping> TRMappings { get; set; }
        public DbSet<MappingListsDTO> MappingListDTOs { get; set; }
        public DbSet<MappingDataDTO> MappingDataDTOs { get; set; }

        public DbSet<TRWHONetMapping> TRWHONetMappings { get; set; }
        public DbSet<WHONetMappingListsDTO> WHONetMappingListsDTOs { get; set; }
        public DbSet<WHONetMappingDataDTO> WHONetMappingDataDTOs { get; set; }

        public DbSet<TRSpecimenMapping> TRSpecimenMappings { get; set; }
        public DbSet<SpecimenMappingListsDTO> SpecimenMappingListsDTOs { get; set; }
        public DbSet<SpecimenMappingDataDTO> SpecimenMappingDataDTOs { get; set; }


        public DbSet<TROrganismMapping> TROrganismMappings { get; set; }
        public DbSet<OrganismMappingListsDTO> OrganismMappingListsDTOs { get; set; }
        public DbSet<OrganismMappingDataDTO> OrganismMappingDataDTOs { get; set; }

        public DbSet<TRWardTypeMapping> TRWardTypeMappings { get; set; }
        public DbSet<WardTypeMappingListsDTO> WardTypeMappingListsDTOs { get; set; }
        public DbSet<WardTypeMappingDataDTO> WardTypeMappingDataDTOs { get; set; }
        public MappingContext(DbContextOptions<MappingContext> options) : base(options)
        {

        }
        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.Entity<MappingListsDTO>().HasKey(x => x.mp_id);
            builder.Entity<MappingDataDTO>().HasKey(x => x.mp_id);

            builder.Entity<TRMapping>().HasKey(x => x.mp_id);
            builder.Entity<TRMapping>().ToTable("TRMapping");


            builder.Entity<WHONetMappingListsDTO>().HasKey(x => x.wnm_id);
            builder.Entity<WHONetMappingDataDTO>().HasKey(x => x.wnm_id);

            builder.Entity<TRWHONetMapping>().HasKey(x => x.wnm_id);
            builder.Entity<TRWHONetMapping>().ToTable("TRWHONetMapping");


            builder.Entity<SpecimenMappingListsDTO>().HasKey(x => x.spm_id);
            builder.Entity<SpecimenMappingDataDTO>().HasKey(x => x.spm_id);

            builder.Entity<TRSpecimenMapping>().HasKey(x => x.spm_id);
            builder.Entity<TRSpecimenMapping>().ToTable("TRSpecimenMapping");

            builder.Entity<OrganismMappingListsDTO>().HasKey(x => x.ogm_id);
            builder.Entity<OrganismMappingDataDTO>().HasKey(x => x.ogm_id);

            builder.Entity<TROrganismMapping>().HasKey(x => x.ogm_id);
            builder.Entity<TROrganismMapping>().ToTable("TROrganismMapping");


            builder.Entity<WardTypeMappingListsDTO>().HasKey(x => x.wdm_id);
            builder.Entity<WardTypeMappingDataDTO>().HasKey(x => x.wdm_id);

            builder.Entity<TRWardTypeMapping>().HasKey(x => x.wdm_id);
            builder.Entity<TRWardTypeMapping>().ToTable("TRWardTypeMapping");


            builder.Entity<LogProcess>().HasKey(x => x.log_id);
            builder.Entity<LogProcess>().ToTable("XLogProcess");


            base.OnModelCreating(builder);
        }

    }
}
