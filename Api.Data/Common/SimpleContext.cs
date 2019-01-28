using Microsoft.EntityFrameworkCore;
using Sisc.Api.Common;

namespace Sisc.Api.Data.Common
{
    public class SimpleContext : DbContext, ISimpleContext
    {
        public SimpleContext(DbContextOptions<SimpleContext> options) : base(options)
        {
        }

        public DbSet<Airline> Airlines { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            builder.Entity<Airline>()
                .HasKey(c => new {c.Id});
            builder.Entity<Airline>()
                .HasIndex(c => new {c.IATACode});
            builder.Entity<Airline>()
                .HasIndex(c => new {c.ICAOCode});

        }
    }
}
