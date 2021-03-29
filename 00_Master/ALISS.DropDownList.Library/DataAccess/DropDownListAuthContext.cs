using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.EntityFrameworkCore;
using ALISS.DropDownList.DTO;

namespace ALISS.DropDownList.Library.DataAccess
{
    public class DropDownListAuthContext : DbContext
    {
        public DbSet<DropDownListDTO> DropDownListDTOs { get; set; }

        public DropDownListAuthContext(DbContextOptions<DropDownListAuthContext> options) : base(options)
        {

        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.Entity<DropDownListDTO>().HasKey(x => x.data_id);

            base.OnModelCreating(builder);
        }
    }
}
