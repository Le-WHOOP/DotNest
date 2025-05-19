using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace DotNest.DataAccess.Entities;

public partial class DotNestContext : DbContext
{
    public DotNestContext()
    {
    }

    public DotNestContext(DbContextOptions<DotNestContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Booking> Bookings { get; set; }

    public virtual DbSet<Picture> Pictures { get; set; }

    public virtual DbSet<Rental> Rentals { get; set; }

    public virtual DbSet<User> Users { get; set; }

//    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
//#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
//        => optionsBuilder.UseSqlServer("Data Source=.\\SQLEXPRESS;Initial Catalog=DotNest;TrustServerCertificate=True;Trusted_Connection=True;");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Booking>(entity =>
        {
            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.FromDate)
                .HasColumnType("datetime")
                .HasColumnName("fromDate");
            entity.Property(e => e.RentalId).HasColumnName("rentalId");
            entity.Property(e => e.ToDate)
                .HasColumnType("datetime")
                .HasColumnName("toDate");
            entity.Property(e => e.UserId).HasColumnName("userId");

            entity.HasOne(d => d.Rental).WithMany(p => p.Bookings)
                .HasForeignKey(d => d.RentalId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Bookings_Rentals");

            entity.HasOne(d => d.User).WithMany(p => p.Bookings)
                .HasForeignKey(d => d.UserId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Bookings_Users");
        });

        modelBuilder.Entity<Picture>(entity =>
        {
            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Base64)
                .HasColumnType("text")
                .HasColumnName("base64");
        });

        modelBuilder.Entity<Rental>(entity =>
        {
            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.PictureId).HasColumnName("pictureId");
            entity.Property(e => e.UserId).HasColumnName("userId");

            entity.HasOne(d => d.Picture).WithMany(p => p.Rentals)
                .HasForeignKey(d => d.PictureId)
                .HasConstraintName("FK_Rentals_Pictures");

            entity.HasOne(d => d.User).WithMany(p => p.Rentals)
                .HasForeignKey(d => d.UserId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Rentals_Users");
        });

        modelBuilder.Entity<User>(entity =>
        {
            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Email)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("email");
            entity.Property(e => e.HashedPassword)
                .HasMaxLength(256)
                .IsUnicode(false)
                .HasColumnName("hashedPassword");
            entity.Property(e => e.PasswordSalt)
                .HasMaxLength(256)
                .IsUnicode(false)
                .HasColumnName("passwordSalt");
            entity.Property(e => e.Username)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("username");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
