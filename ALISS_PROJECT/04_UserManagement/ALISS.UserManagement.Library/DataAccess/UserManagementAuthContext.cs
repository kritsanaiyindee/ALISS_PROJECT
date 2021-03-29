using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.EntityFrameworkCore;
using ALISS.UserManagement.Library.Models;
using ALISS.UserManagement.DTO;

namespace ALISS.UserManagement.Library.DataAccess
{
    public class UserManagementAuthContext : DbContext
    {
        public DbSet<TCUserLogin> TCUserLogins { get; set; }
        public DbSet<TCUserLoginPermission> TCUserLoginPermissions { get; set; }
        public DbSet<TCUserPasswordHistory> TCUserPasswordHistorys { get; set; }
        public DbSet<UserLoginDTO> UserLoginDTOs { get; set; }
        public DbSet<UserLoginPermissionDTO> UserLoginPermissionDTOs { get; set; }

        public DbSet<LogProcess> LogProcesss { get; set; }

        public UserManagementAuthContext(DbContextOptions<UserManagementAuthContext> options) : base(options)
        {

        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.Entity<TCUserLogin>().HasKey(x => x.usr_id);
            builder.Entity<TCUserLogin>().ToTable("TCUserLogin");

            builder.Entity<TCUserLoginPermission>().HasKey(x => x.usp_id);
            builder.Entity<TCUserLoginPermission>().ToTable("TCUserLoginPermission");

            builder.Entity<TCUserPasswordHistory>().HasKey(x => x.uph_id);
            builder.Entity<TCUserPasswordHistory>().ToTable("TCUserPasswordHistory");

            builder.Entity<UserLoginDTO>().HasKey(x => x.usr_id);

            builder.Entity<UserLoginPermissionDTO>().HasKey(x => x.usp_id);

            builder.Entity<LogProcess>().HasKey(x => x.log_id);
            builder.Entity<LogProcess>().ToTable("XLogProcess");

            base.OnModelCreating(builder);
        }
    }
}
