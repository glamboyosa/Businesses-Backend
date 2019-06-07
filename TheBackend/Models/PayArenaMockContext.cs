using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.Extensions.Configuration;
using TheBackend.Models;

namespace TheBackend.Models
{
    public  class PayArenaMockContext : DbContext
    {
        public PayArenaMockContext()
        {
        }
        private readonly IConfiguration _config;
        public PayArenaMockContext(DbContextOptions options)
            : base(options)
        {
            
        }

        public virtual DbSet<BusinessListing> BusinessListing { get; set; }
        public virtual DbSet<Categories> Categories { get; set; }
        public virtual DbSet<User> User { get; set; }
        public DbSet<Roles> Roles { get; set; }
       

       /* protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. See http://go.microsoft.com/fwlink/?LinkId=723263 for guidance on storing connection strings.
                optionsBuilder.UseNpgsql("Host=localhost;Database=PayArenaMock;Username=postgres;Password=tim");
            }
        }*/

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasAnnotation("ProductVersion", "2.2.2-servicing-10034");

            modelBuilder.Entity<BusinessListing>(entity =>
            {
                entity.Property(e => e.Address).IsRequired();

                entity.Property(e => e.BusinessName).IsRequired();

                entity.Property(e => e.City).IsRequired();

                entity.Property(e => e.CustomerName).IsRequired();

                entity.Property(e => e.Email).IsRequired();

                entity.Property(e => e.Lga).IsRequired();

                entity.Property(e => e.Url).IsRequired();

                entity.HasOne(d => d.CategoryNameNav)
                    .WithMany(p => p.BusinessListing)
                    .HasForeignKey(d => d.CategoryId)
                    .HasConstraintName("BusinessListing_CategoryId_fkey");
            });

            modelBuilder.Entity<Categories>(entity =>
            {
                //entity.HasKey(e => e.CategoryName)
                //    .HasName("Categories_pkey");

                entity.Property(e => e.CategoryName)
                    .HasColumnName("Category_Name")
                    .ValueGeneratedNever();

                entity.HasKey(e => e.Id).HasName("Categories_pkey");

                entity.Property(e => e.Id).ValueGeneratedOnAdd();
            });

            modelBuilder.Entity<User>(entity =>
            {
                entity.Property(e => e.FirstName).IsRequired();

                entity.Property(e => e.LastName).IsRequired();

                entity.Property(e => e.Password).IsRequired();

                entity.Property(e => e.Role).IsRequired();
                entity.Property(e => e.Email).IsRequired();

                //entity.Property(e => e.Token).IsRequired();

                entity.Property(e => e.UserName).IsRequired();
                entity.Property(e => e.Salt).IsRequired();
            });
            
        }

       /* protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. See http://go.microsoft.com/fwlink/?LinkId=723263 for guidance on storing connection strings.
                optionsBuilder.UseNpgsql("Host=localhost;Database=PayArenaMock;Username=postgres;Password=tim");
            }
        }*/

       
    }
}
