using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using ALISS.ANTIBIOGRAM.DTO;
using ALISS.ANTIBIOGRAM.Library.Models;

namespace ALISS.ANTIBIOGRAM.Library.DataAccess
{
    public class ReportContext : DbContext
    {
        public DbSet<AntibiogramDataDTO> DropdownListDTOs { get; set; }
        public DbSet<AntibiogramHospitalTemplateDTO> DropdownHospitalTemplateListDTOs { get; set; }
        public DbSet<AntibiogramAreaHealthTemplateDTO> DropdownAreaHealthTemplateListDTOs { get; set; }
        public DbSet<AntibiogramNationTemplateDTO> DropdownNationTemplateListDTOs { get; set; }
        public DbSet<RPYearlyIsolateListingRISDTO> RPYearlyIsolateListingRISDTOs { get; set; }
        public DbSet<RPYearlyIsolateListingRISDetailDTO> RPYearlyIsolateListingRISDetailDTOs { get; set; }
        public DbSet<RPAntibiogramSurveilOrganism> RPAntibiogramSurveilOrganism { get; set; }
        public DbSet<RPAntibiogramSurveilOrganismDTO> RPAntibiogramSurveilOrganismDTOs { get; set; }
        public DbSet<RPAntibiogramSurveilAntibiotic> RPAntibiogramSurveilAntibiotic { get; set; }
        public DbSet<RPAntibiogramSurveilAntibioticDTO> RPAntibiogramSurveilAntibioticDTOs { get; set; }
        public ReportContext(DbContextOptions<ReportContext> options) : base(options)
        {

        }

        protected override void OnModelCreating(ModelBuilder builder)
        {          
            builder.Entity<AntibiogramDataDTO>().HasNoKey();
            builder.Entity<AntibiogramHospitalTemplateDTO>().HasNoKey();
            builder.Entity<AntibiogramAreaHealthTemplateDTO>().HasNoKey();
            builder.Entity<AntibiogramNationTemplateDTO>().HasNoKey();           

            builder.Entity<RPAntibiogramSurveilOrganism>().HasKey(x => x.id);
            builder.Entity<RPAntibiogramSurveilOrganism>().ToTable("RPAntibiogramSurveilOrganism");
            builder.Entity<RPAntibiogramSurveilOrganismDTO>().HasKey(x => x.id);

            builder.Entity<RPAntibiogramSurveilAntibiotic>().HasKey(x => x.id);
            builder.Entity<RPAntibiogramSurveilAntibiotic>().ToTable("RPAntibiogramSurveilAntibiotic");
            builder.Entity<RPAntibiogramSurveilAntibioticDTO>().HasKey(x => x.id);

            builder.Entity<RPYearlyIsolateListingRISDTO>().HasKey(x => x.id);
            builder.Entity<RPYearlyIsolateListingRISDTO>().ToTable("RPYearlyIsolateListingRIS");

            builder.Entity<RPYearlyIsolateListingRISDetailDTO>().HasKey(x => x.id);
            builder.Entity<RPYearlyIsolateListingRISDetailDTO>().ToTable("RPYearlyIsolateListingRISDetail");


            base.OnModelCreating(builder);
        }

    }
}
