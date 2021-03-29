using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.EntityFrameworkCore;
using ALISS.Master.DTO;
using ALISS.Master.Library.Models;

namespace ALISS.Master.Library.DataAccess
{
    public class AuthContext : DbContext
    {
        public DbSet<MasterDataDTO> DropdownListDTOs { get; set; }
        public DbSet<LogProcessDTO> LogProcessDTOs { get; set; }
        public DbSet<TBConfigDTO> TBConfigDTOs { get; set; }
        public DbSet<MenuData> MenuDatas { get; set; }
        //public DbSet<UserLoginDTO> UserLoginDTOs { get; set; }
        //public DbSet<UserLoginPermissionDTO> UserLoginPermissionDTOs { get; set; }

        public AuthContext(DbContextOptions<AuthContext> options) : base(options)
        {

        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.Entity<MasterDataDTO>().HasKey(x => x.ddl_Value);

            builder.Entity<LogProcessDTO>().HasKey(x => x.log_id);

            builder.Entity<TBConfigDTO>().HasKey(x => x.tbc_id);

            builder.Entity<MenuData>().HasKey(x => x.mnu_id);

            //builder.Entity<UserLoginDTO>().HasKey(x => x.usr_id);

            //builder.Entity<UserLoginPermissionDTO>().HasKey(x => x.usp_id);

            base.OnModelCreating(builder);
        }
    }
}
