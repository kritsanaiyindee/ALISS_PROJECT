using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.EntityFrameworkCore;
using ALISS.LoginManagement.DTO;
using ALISS.LoginManagement.Library.Models;

namespace ALISS.LoginManagement.Library.DataAccess
{
    public class AuthContext : DbContext
    {
        public DbSet<LogUserLogin> LogUserLogins { get; set; }
        public DbSet<TCUserLogin> TCUserLogins { get; set; }

        public DbSet<LoginUserDTO> LoginUserDTOs { get; set; }
        public DbSet<LoginUserPermissionDTO> LoginUserPermissionDTOs { get; set; }
        public DbSet<LoginUserRolePermissionDTO> LoginUserRolePermissionDTOs { get; set; }
        public DbSet<MenuData> MenuDatas { get; set; }

        public AuthContext(DbContextOptions<AuthContext> options) : base(options)
        {

        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.Entity<LogUserLogin>().HasKey(x => x.log_id);
            builder.Entity<LogUserLogin>().ToTable("XLogUserLogin");

            builder.Entity<LoginUserDTO>().HasKey(x => x.usr_id);
            builder.Entity<LoginUserPermissionDTO>().HasKey(x => x.usp_id);
            builder.Entity<LoginUserRolePermissionDTO>().HasKey(x => x.rop_id);

            builder.Entity<TCUserLogin>().HasKey(x => x.usr_id);
            builder.Entity<TCUserLogin>().ToTable("TCUserLogin");

            builder.Entity<MenuData>().HasKey(x => x.mnu_id);

            base.OnModelCreating(builder);
        }
    }
}
