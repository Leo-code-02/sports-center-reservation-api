using System;
using System.Collections.Generic;
using System.Text;

namespace SportsCenterReservation.Infrastructure.Repositories;

using Microsoft.EntityFrameworkCore;
using SportsCenterReservation.Domain.Entities;
using SportsCenterReservation.Infrastructure.Context;
using SportsCenterReservation.Infrastructure.Core;
using SportsCenterReservation.Infrastructure.Interfaces;

public class UserRepository : BaseRepository<User>, IUserRepository
{
    public UserRepository(ApplicationDbContext context) : base(context)
    {
    }

    public async Task<User?> GetByEmailAsync(string email)
    {
        return await _dbSet
            .AsNoTracking()
            .FirstOrDefaultAsync(u => u.Email == email);
    }
}