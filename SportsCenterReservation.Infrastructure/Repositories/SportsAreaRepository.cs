using System;
using System.Collections.Generic;
using System.Text;

namespace SportsCenterReservation.Infrastructure.Repositories;

using Microsoft.EntityFrameworkCore;
using SportsCenterReservation.Domain.Entities;
using SportsCenterReservation.Infrastructure.Context;
using SportsCenterReservation.Infrastructure.Core;
using SportsCenterReservation.Infrastructure.Interfaces;

public class SportsAreaRepository : BaseRepository<SportsArea>, ISportsAreaRepository
{
    public SportsAreaRepository(ApplicationDbContext context) : base(context)
    {
    }

    public async Task<IEnumerable<SportsArea>> GetActiveAreasWithSchedulesAsync()
    {
        return await _dbSet
            .Include(a => a.Schedules)
            .Where(a => a.IsActive)
            .AsNoTracking()
            .ToListAsync();
    }
}