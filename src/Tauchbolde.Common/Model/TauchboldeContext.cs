using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;

namespace Tauchbolde.Common.Model
{
    public class TauchboldeContext : DbContext
    {
        public DbSet<Event> Events { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(
                @"Server=(localdb)\mssqllocaldb;Database=Tauchbolde;Trusted_Connection=True;");
        }       
    }
}