using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.EntityFrameworkCore;
using ALISS.AUTH.Library.Models;
using ALISS.AUTH.DTO;

namespace ALISS.AUTH.Library.DataAccess
{
    public class AuthContext : DbContext
    {
        public DbSet<TCMenu> TCMenus { get; set; }
        public DbSet<TBConfig> TBConfigs { get; set; }
        public DbSet<TCRole> TCRoles { get; set; }
        public DbSet<TCRolePermission> TCRolePermissions { get; set; }

        public DbSet<ColumnConfigDTO> ColumnConfigDTOs { get; set; }
        public DbSet<RoleDTO> RoleDTOs { get; set; }
        public DbSet<RolePermissionDTO> RolePermissionDTOs { get; set; }
        public DbSet<LogUserLoginDTO> LogUserLoginDTOs { get; set; }

        public DbSet<TCPasswordConfig> TCPasswordConfigs { get; set; }
        public DbSet<LogProcess> LogProcesss { get; set; }

        public AuthContext(DbContextOptions<AuthContext> options) : base(options)
        {

        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.Entity<TCMenu>().HasKey(x => x.mnu_id);
            builder.Entity<TCMenu>().ToTable("TCMenu");

            builder.Entity<TBConfig>().HasKey(x => x.tbc_id);
            builder.Entity<TBConfig>().ToTable("TBConfig");

            builder.Entity<TCRole>().HasKey(x => x.rol_id);
            builder.Entity<TCRole>().ToTable("TCRole");

            builder.Entity<TCRolePermission>().HasKey(x => x.rop_id);
            builder.Entity<TCRolePermission>().ToTable("TCRolePermission");

            builder.Entity<RoleDTO>().HasKey(x => x.rol_id);
            builder.Entity<RoleDTO>().ToTable("RoleDTO");

            builder.Entity<ColumnConfigDTO>().HasKey(x => x.tbc_id);

            builder.Entity<RolePermissionDTO>().HasKey(x => x.mnu_id);

            builder.Entity<LogUserLoginDTO>().HasKey(x => x.log_id);
            builder.Entity<LogUserLoginDTO>().ToTable("LogUserLoginDTO");

            builder.Entity<TCPasswordConfig>().HasKey(x => x.pwc_id);
            builder.Entity<TCPasswordConfig>().ToTable("TCPasswordConfig");

            builder.Entity<LogProcess>().HasKey(x => x.log_id);
            builder.Entity<LogProcess>().ToTable("XLogProcess");

            base.OnModelCreating(builder);
        }
    }
}
