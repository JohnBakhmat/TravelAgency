using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using TravelAgency.Models;

namespace TravelAgency.Data
{
    public class ApplicationDbContext : IdentityDbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }
        public DbSet<Vocation> Vocations { get; set; }
        public DbSet<Customer> Customers { get; set; }
        public DbSet<Operator> Operators { get; set; }
        public DbSet<Tour> Tours { get; set; }
        public DbSet<Hotel> Hotels { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<Vocation>()
                .HasOne(o => o.Operator)
                .WithMany(v => v.Vocations)
                .HasForeignKey(v=>v.OperatorId);
            builder.Entity<Vocation>()
                .HasOne(o => o.Customer)
                .WithMany(v => v.Vocations)
                .HasForeignKey(v=>v.CustomerId);

        }
    }
}
