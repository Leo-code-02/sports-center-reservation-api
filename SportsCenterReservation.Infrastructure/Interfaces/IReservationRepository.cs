using System;
using System.Collections.Generic;
using System.Text;

namespace SportsCenterReservation.Infrastructure.Interfaces;

using SportsCenterReservation.Domain.Entities;
using SportsCenterReservation.Domain.Repository;
using SportsCenterReservation.Infrastructure.Core;

public interface IReservationRepository : IBaseRepository<Reservation>
{
    Task<IEnumerable<Reservation>> GetReservationsWithDetailsAsync();
    Task<bool> IsSlotAvailableAsync(int sportsAreaId, int scheduleId, DateTime date);
}