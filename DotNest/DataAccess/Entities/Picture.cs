using System;
using System.Collections.Generic;

namespace DotNest.DataAccess.Entities;

public partial class Picture
{
    public int Id { get; set; }

    public string Base64 { get; set; } = null!;

    public virtual ICollection<Rental> Rentals { get; set; } = new List<Rental>();
}
