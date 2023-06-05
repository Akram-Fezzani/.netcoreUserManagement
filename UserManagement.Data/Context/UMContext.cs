using UserManagement.Domain.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;
using Role = UserManagement.Domain.Models.Role;

namespace UserManagement.Data.Context
{
    public class UMContext : DbContext
    {
        public UMContext(DbContextOptions<UMContext> options)
            : base(options)
        { }


        public DbSet<Role> Roles { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<ChefCentre> ChefCentres { get; set; }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>().HasKey(sc => new { sc.UserId });
            modelBuilder.Entity<Role>().HasKey(sc => new { sc.RoleId });

            // modelBuilder.Entity<ChefCentre>().HasKey(sc => new { sc.UserId });

            /*  modelBuilder.Entity<User>()
                     .HasOne(ba => ba.role)
                     .WithMany(ba => ba.users)
                     .HasForeignKey("RoleId");
            modelBuilder.Entity<User>()
        .HasDiscriminator(b => b.);

            modelBuilder.Entity<User>()
                .Property(e => e.BlogType)
                .HasMaxLength(200)
                .HasColumnName("blog_type");
            */
            modelBuilder.Entity<User>()
      .HasDiscriminator<string>("user_type")
      .HasValue<User>("User")
      .HasValue<ChefCentre>("ChefCentre");
            modelBuilder.Entity<ChefCentre>();
        }
    }
}
