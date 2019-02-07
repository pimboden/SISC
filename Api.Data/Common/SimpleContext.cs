﻿using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
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

            builder.Entity<Airline>().ToTable("Airline").HasKey(c => new {c.Id});
            builder.Entity<Airline>().Property(c => c.Id).HasColumnName("Id");
            builder.Entity<Airline>().Property(c => c.Name).HasColumnName("Name");
            builder.Entity<Airline>().Property(c => c.IataCode).HasColumnName("IATACode");
            builder.Entity<Airline>().Property(c => c.IcaoCode).HasColumnName("ICAOCode");



        }
    }
}
