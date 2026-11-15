using System;
using System.Collections.Generic;
using System.Text;

namespace SportsCenterReservation.Infrastructure.Context;

using Microsoft.EntityFrameworkCore;
using SportsCenterReservation.Domain.Entities;

public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {
    }

    public DbSet<SportsArea> SportsAreas => Set<SportsArea>();
    public DbSet<Schedule> Schedules => Set<Schedule>();
    public DbSet<User> Users => Set<User>();
    public DbSet<Reservation> Reservations => Set<Reservation>();
    public DbSet<Payment> Payments => Set<Payment>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<Schedule>()
            .HasOne(s => s.SportsArea)
            .WithMany(sa => sa.Schedules)
            .HasForeignKey(s => s.SportsAreaId)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<Reservation>()
            .HasOne(r => r.SportsArea)
            .WithMany()
            .HasForeignKey(r => r.SportsAreaId)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<Reservation>()
            .HasOne(r => r.Schedule)
            .WithMany()
            .HasForeignKey(r => r.ScheduleId)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<Reservation>()
            .HasOne(r => r.User)
            .WithMany()
            .HasForeignKey(r => r.UserId)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<SportsArea>().Property(sa => sa.HourlyRate).HasPrecision(18, 2);
        modelBuilder.Entity<Reservation>().Property(r => r.TotalAmount).HasPrecision(18, 2);
        modelBuilder.Entity<Payment>().Property(p => p.Amount).HasPrecision(18, 2);
    }
}
