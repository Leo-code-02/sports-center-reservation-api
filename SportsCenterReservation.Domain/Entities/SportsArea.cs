using System;
using System.Collections.Generic;
using System.Text;
using SportsCenterReservation.Domain.Core;

namespace SportsCenterReservation.Domain.Entities
{
    public class SportsArea : BaseEntity
    {
        public string Name { get; set; } = string.Empty;
        public string SportType { get; set; } = string.Empty;
        public int MaxCapacity { get; set; }
        public decimal HourlyRate { get; set; }

        public ICollection<Schedule> Schedules { get; set; } = new List<Schedule>();
    }
}
