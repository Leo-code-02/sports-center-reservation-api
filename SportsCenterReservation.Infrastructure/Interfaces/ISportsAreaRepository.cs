using System;
using System.Collections.Generic;
using System.Text;

namespace SportsCenterReservation.Infrastructure.Interfaces;

using SportsCenterReservation.Domain.Entities;
using SportsCenterReservation.Domain.Repository;
using SportsCenterReservation.Infrastructure.Core;

public interface ISportsAreaRepository : IBaseRepository<SportsArea>
{
    Task<IEnumerable<SportsArea>> GetActiveAreasWithSchedulesAsync();
}