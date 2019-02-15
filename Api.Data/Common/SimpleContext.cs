using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Sisc.Api.Common;

namespace Sisc.Api.Data.Common
{
    public class SimpleContext : DbContext, ISimpleContext
    {
        public SimpleContext(DbContextOptions<SimpleContext> options) : base(options)
        {
        }

        public DbSet<Country> Countries { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<Country>().ToTable("Country").HasKey(c => new {c.Id});
            builder.Entity<Country>().Property(c => c.Id).HasColumnName("Id");
            builder.Entity<Country>().Property(c => c.Name).HasColumnName("Name").HasMaxLength(256);
            builder.Entity<Country>().Property(c => c.IsoCode).HasColumnName("IsoCode").HasMaxLength(2);


        }
    }
}
