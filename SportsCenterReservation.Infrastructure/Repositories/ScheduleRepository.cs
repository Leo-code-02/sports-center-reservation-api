using System;
using System.Collections.Generic;
using System.Text;

namespace SportsCenterReservation.Infrastructure.Repositories;

using Microsoft.EntityFrameworkCore;
using SportsCenterReservation.Domain.Entities;
using SportsCenterReservation.Infrastructure.Context;
using SportsCenterReservation.Infrastructure.Core;
using SportsCenterReservation.Infrastructure.Interfaces;

public class ScheduleRepository : BaseRepository<Schedule>, IScheduleRepository
{
    public ScheduleRepository(ApplicationDbContext context) : base(context)
    {
    }

    public async Task<IEnumerable<Schedule>> GetBySportsAreaIdAsync(int sportsAreaId)
    {
        return await _dbSet
            .Where(s => s.SportsAreaId == sportsAreaId)
            .AsNoTracking()
            .ToListAsync();
    }
}