using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

#nullable disable

namespace RCS_WebAPI.Models
{
    public partial class RCS_dbContext : DbContext
    {
        public RCS_dbContext()
        {
        }

        public RCS_dbContext(DbContextOptions<RCS_dbContext> options)
            : base(options)
        {
        }

        public virtual DbSet<UserLogin> UserLogins { get; set; }
        public virtual DbSet<UserOtp> UserOtps { get; set; }
        public virtual DbSet<UserCar> UserCars { get; set; }
        public virtual DbSet<Order> Orders { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlServer("Password=Allahisgr8;Persist Security Info=True;User ID=sa;Initial Catalog=RCS_db;Data Source=(local)");
                //optionsBuilder.UseSqlServer(GlobalProperties.DBConnection);
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasAnnotation("Relational:Collation", "SQL_Latin1_General_CP1_CI_AS");

            modelBuilder.Entity<UserLogin>(entity =>
            {
                entity.HasKey(e => e.UserName);

                entity.ToTable("UserLogin");

                entity.Property(e => e.UserName)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.CreatedByIpaddress)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("CreatedByIPAddress");

                entity.Property(e => e.CreatedDateTime).HasDefaultValueSql("(getdate())");

                entity.Property(e => e.Id)
                    .ValueGeneratedOnAdd()
                    .HasColumnName("ID");

                entity.Property(e => e.Landline)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.Mobile)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.ModifiedByIpaddress)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("ModifiedByIPAddress");

                entity.Property(e => e.ModifiedDateTime).HasDefaultValueSql("(getdate())");

                entity.Property(e => e.FirstName)
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.LastName)
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.Password)
                    .IsRequired()
                    .HasMaxLength(500);

                entity.Property(e => e.Status).HasDefaultValueSql("((9))");

                entity.Property(e => e.Type).HasDefaultValueSql("((3))");

                entity.Property(e => e.EmailAlerts).HasDefaultValueSql("(0)");

            });

            modelBuilder.Entity<UserOtp>(entity =>
            {
                entity.HasKey(e => new { e.UserLoginId, e.Otp });

                entity.ToTable("UserOTP");

                entity.Property(e => e.UserLoginId).HasColumnName("UserLoginID");

                entity.Property(e => e.Otp)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("OTP");

                entity.Property(e => e.CreatedDateTime).HasDefaultValueSql("(getdate())");

                entity.Property(e => e.IsExpired).HasDefaultValueSql("(0)");

                entity.Property(e => e.CreatedByIpaddress)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("CreatedByIPAddress");
            });


            modelBuilder.Entity<UserCar>(entity =>
            {
                entity.HasKey(e => new { e.UserLoginId, e.Name });

                entity.ToTable("UserCar");

                entity.Property(e => e.UserLoginId).HasColumnName("UserLoginID");

                entity.Property(e => e.Name)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("Name");

                entity.Property(e => e.CreatedDateTime).HasDefaultValueSql("(getdate())");

                entity.Property(e => e.IsAvailable).HasDefaultValueSql("(0)");

                entity.Property(e => e.CreatedByIpaddress)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("CreatedByIPAddress");
            });

            modelBuilder.Entity<Order>(entity =>
            {
                entity.HasKey(e => new { e.CustomerLoginID, e.Car, e.BookingDateTime, e.CreatedBy });

                entity.ToTable("Order");

                entity.Property(e => e.CreatedByIpaddress)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("CreatedByIPAddress");

                entity.Property(e => e.CreatedDateTime).HasDefaultValueSql("(getdate())");

                entity.Property(e => e.Id)
                    .ValueGeneratedOnAdd()
                    .HasColumnName("ID");
                entity.Property(e => e.ModifiedByIpaddress)
                   .HasMaxLength(50)
                   .IsUnicode(false)
                   .HasColumnName("ModifiedByIPAddress");

                entity.Property(e => e.ModifiedDateTime).HasDefaultValueSql("(getdate())");

                entity.Property(e => e.Status).HasDefaultValueSql("Available");
                
                entity.Property(e => e.CustomerLoginID).HasDefaultValueSql("0");

            });

            ////////////////////////////////////////
            OnModelCreatingPartial(modelBuilder);
            ////////////////////////////////////////
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
