using System;
using System.Collections.Generic;

namespace DotNest.DataAccess.Entities;

public partial class Rental
{
    public int Id { get; set; }

    public int? PictureId { get; set; }

    public int UserId { get; set; }

    public virtual ICollection<Booking> Bookings { get; set; } = new List<Booking>();

    public virtual Picture? Picture { get; set; }

    public virtual User User { get; set; } = null!;
}
