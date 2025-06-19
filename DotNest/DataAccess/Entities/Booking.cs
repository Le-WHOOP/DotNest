using System;
using System.Collections.Generic;

namespace DotNest.DataAccess.Entities;

public partial class Booking
{
    public int Id { get; set; }

    public int UserId { get; set; }

    public int RentalId { get; set; }

    public DateOnly FromDate { get; set; }

    public DateOnly ToDate { get; set; }

    public virtual Rental Rental { get; set; } = null!;

    public virtual User User { get; set; } = null!;
}
