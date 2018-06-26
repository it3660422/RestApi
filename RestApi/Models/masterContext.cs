using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RestApi.Models
{
    public partial class masterContext : DbContext
    {
        public masterContext(DbContextOptions<masterContext> options) : base(options)
        {

        }

        public virtual DbSet<CheckCard> CheckCards { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<CheckCard>(entity =>
            {
                entity.HasKey(e => e.cnt);
                entity.Property(e => e.cnt).HasColumnName("CNT");
            });
        }
    }
}
