using System;
using System.Collections.Generic;
using System.Text;
using SportsCenterReservation.Domain.Core;

namespace SportsCenterReservation.Domain.Entities;

public class User : BaseEntity
{
    public string FullName { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string PhoneNumber { get; set; } = string.Empty;
    public string IdentificationNumber { get; set; } = string.Empty;
    public DateTime RegistrationDate { get; set; } = DateTime.UtcNow;
}
