﻿using System;
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
        public virtual DbSet<CarImage> CarImage { get; set; }
        public virtual DbSet<Rent> Rent { get; set; }
        public virtual DbSet<Review> Review { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasAnnotation("ProductVersion", "2.2.1-servicing-10028");

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

            modelBuilder.Entity<CarImage>(entity =>
            {
                entity.ToTable("CarImage", "CarRent");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.CarId).HasColumnName("carId");

                entity.Property(e => e.ImgUrl)
                    .IsRequired()
                    .HasColumnName("imgUrl")
                    .HasMaxLength(100);

                entity.HasOne(d => d.Car)
                    .WithMany(p => p.CarImage)
                    .HasForeignKey(d => d.CarId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__CarImage__carId__75A278F5");
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
