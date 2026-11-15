using System;
using System.Collections.Generic;
using System.Text;

namespace SportsCenterReservation.Infrastructure.Repositories;

using Microsoft.EntityFrameworkCore;
using SportsCenterReservation.Domain.Entities;
using SportsCenterReservation.Infrastructure.Context;
using SportsCenterReservation.Infrastructure.Core;
using SportsCenterReservation.Infrastructure.Interfaces;

public class ReservationRepository : BaseRepository<Reservation>, IReservationRepository
{
    public ReservationRepository(ApplicationDbContext context) : base(context)
    {
    }

    public async Task<IEnumerable<Reservation>> GetReservationsWithDetailsAsync()
    {
        return await _dbSet
            .Include(r => r.User)
            .Include(r => r.SportsArea)
            .Include(r => r.Schedule)
            .AsNoTracking()
            .ToListAsync();
    }

    public async Task<bool> IsSlotAvailableAsync(int sportsAreaId, int scheduleId, DateTime date)
    {
        return !await _dbSet.AnyAsync(r =>
            r.SportsAreaId == sportsAreaId &&
            r.ScheduleId == scheduleId &&
            r.ReservationDate.Date == date.Date &&
            r.Status != ReservationStatus.Cancelled);
    }
}