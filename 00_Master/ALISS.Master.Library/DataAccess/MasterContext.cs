using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.EntityFrameworkCore;
using ALISS.Master.DTO;

namespace ALISS.Master.Library.DataAccess
{
    public class MasterContext : DbContext
    {
        public DbSet<MasterDataDTO> DropdownListDTOs { get; set; }
        public DbSet<LogProcessDTO> LogProcessDTOs { get; set; }
        public DbSet<TRNoticeMessageDTO> TRNoticeMessageDTOs { get; set; }

        public MasterContext(DbContextOptions<MasterContext> options) : base(options)
        {

        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.Entity<MasterDataDTO>().HasKey(x => x.ddl_Value);

            builder.Entity<LogProcessDTO>().HasKey(x => x.log_id);

            builder.Entity<TRNoticeMessageDTO>().HasKey(x => x.noti_id);

            base.OnModelCreating(builder);
        }
    }
}
