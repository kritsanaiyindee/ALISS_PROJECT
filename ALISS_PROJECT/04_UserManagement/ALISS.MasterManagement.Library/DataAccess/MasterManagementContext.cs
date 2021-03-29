using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.EntityFrameworkCore;
using ALISS.MasterManagement.Library.Models;
using ALISS.MasterManagement.DTO;

namespace ALISS.MasterManagement.Library.DataAccess
{
    public class MasterManagementContext : DbContext
    {
        public DbSet<TCHospital> TCHospitals { get; set; }

        public DbSet<MasterHospitalDTO> MasterHospitalDTOs { get; set; }

        public DbSet<TRHospital> TRHospitals { get; set; }

        public DbSet<LogProcess> LogProcesss { get; set; }

        public DbSet<TCMasterTemplate> TCMasterTemplates { get; set; }

        public DbSet<TCAntibiotic> TCAntibiotics { get; set; }
        public DbSet<AntibioticDTO> AntibioticDTOs { get; set; }

        public DbSet<TCExpertRule> TCExpertRoles { get; set; }
        public DbSet<ExpertRuleDTO> ExpertRuleDTOs { get; set; }

        public DbSet<TCOrganism> TCOrganisms { get; set; }

        public DbSet<OrganismDTO> OrganismDTOs { get; set; }

        public DbSet<TCQCRange> TCQCRanges { get; set; }
        public DbSet<QCRangeDTO> QCRangeDTOs { get; set; }

        public DbSet<TCSpecimen> TCSpecimens { get; set; }

        public DbSet<TCWardType> TCWardTypes { get; set; }
        public DbSet<TCWHONETColumn> TCWHONETColumns { get; set; }

        public MasterManagementContext(DbContextOptions<MasterManagementContext> options) : base(options)
        {

        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.Entity<TCHospital>().HasKey(x => x.hos_id);
            builder.Entity<TCHospital>().ToTable("TCHospital");

            builder.Entity<MasterHospitalDTO>().HasKey(x => x.hos_id);

            builder.Entity<TRHospital>().HasKey(x => x.hos_id);
            builder.Entity<TRHospital>().ToTable("TRHospital");

            builder.Entity<LogProcess>().HasKey(x => x.log_id);
            builder.Entity<LogProcess>().ToTable("XLogProcess");

            builder.Entity<TCMasterTemplate>().HasKey(x => x.mst_id);
            builder.Entity<TCMasterTemplate>().ToTable("TCMasterTemplate");

            builder.Entity<TCAntibiotic>().HasKey(x => x.ant_id);
            builder.Entity<TCAntibiotic>().ToTable("TCAntibiotic");

            builder.Entity<AntibioticDTO>().HasKey(x => x.ant_id);

            builder.Entity<TCExpertRule>().HasKey(x => x.exr_id);
            builder.Entity<TCExpertRule>().ToTable("TCExpertRule");
            builder.Entity<ExpertRuleDTO>().HasKey(x => x.exr_id);

            builder.Entity<TCOrganism>().HasKey(x => x.org_id);
            builder.Entity<TCOrganism>().ToTable("TCOrganism");

            builder.Entity<OrganismDTO>().HasKey(x => x.org_id);

            builder.Entity<TCQCRange>().HasKey(x => x.qcr_id);
            builder.Entity<TCQCRange>().ToTable("TCQCRange");
            builder.Entity<QCRangeDTO>().HasKey(x => x.qcr_id);

            builder.Entity<TCSpecimen>().HasKey(x => x.spc_id);
            builder.Entity<TCSpecimen>().ToTable("TCSpecimen");

            builder.Entity<TCWardType>().HasKey(x => x.wrd_id);
            builder.Entity<TCWardType>().ToTable("TCWardType");

            builder.Entity<TCWHONETColumn>().HasKey(x => x.wnc_id);
            builder.Entity<TCWHONETColumn>().ToTable("TCWHONETColumn");

            base.OnModelCreating(builder);
        }
    }
}
