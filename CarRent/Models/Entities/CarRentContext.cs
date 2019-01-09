using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace CarRent.Models.Entities
{
    public partial class CarRentContext : DbContext
    {
        public CarRentContext()
        {
        }

        public CarRentContext(DbContextOptions<CarRentContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Car> Car { get; set; }
        public virtual DbSet<Rent> Rent { get; set; }
        public virtual DbSet<Review> Review { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
//#warning To protect potentially sensitive information in your connection string, you should move it out of source code. See http://go.microsoft.com/fwlink/?LinkId=723263 for guidance on storing connection strings.
                optionsBuilder.UseSqlServer("Server=carrentacademy.database.windows.net;Initial Catalog=CarRentDb;Persist Security Info=False;User Id=Adameasten;Password=Pennskrin1;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;", x => x.UseNetTopologySuite());
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasAnnotation("ProductVersion", "2.2.0-rtm-35687");

            modelBuilder.Entity<Car>(entity =>
            {
                entity.ToTable("Car", "CarRent");

                entity.Property(e => e.Description).HasMaxLength(300);

                entity.Property(e => e.Doors).HasColumnName("doors");

                entity.Property(e => e.Fuel)
                    .IsRequired()
                    .HasMaxLength(100);

                entity.Property(e => e.Gear)
                    .IsRequired()
                    .HasMaxLength(100);

                entity.Property(e => e.ImgUrl).HasColumnName("imgUrl");

                entity.Property(e => e.Model)
                    .IsRequired()
                    .HasMaxLength(100);

                entity.Property(e => e.OwnerId)
                    .IsRequired()
                    .HasColumnName("ownerId")
                    .HasMaxLength(450);

                entity.Property(e => e.Price).HasColumnType("money");

                entity.Property(e => e.Type)
                    .IsRequired()
                    .HasMaxLength(100);
            });

            modelBuilder.Entity<Rent>(entity =>
            {
                entity.ToTable("rent", "CarRent");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.CustomerId)
                    .IsRequired()
                    .HasMaxLength(450);

                entity.Property(e => e.DateEnd).HasColumnType("datetime");

                entity.Property(e => e.Datestart).HasColumnType("datetime");

                entity.HasOne(d => d.Car)
                    .WithMany(p => p.Rent)
                    .HasForeignKey(d => d.CarId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__rent__CarId__6477ECF3");
            });

            modelBuilder.Entity<Review>(entity =>
            {
                entity.ToTable("Review", "CarRent");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.DateCreated).HasColumnType("datetime");

                entity.Property(e => e.Rating).HasColumnName("rating");

                entity.Property(e => e.Review1)
                    .HasColumnName("review")
                    .HasMaxLength(600);

                entity.HasOne(d => d.Rent)
                    .WithMany(p => p.Review)
                    .HasForeignKey(d => d.RentId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__Review__RentId__68487DD7");
            });
        }
    }
}
