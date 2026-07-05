using System;
using System.Collections.Generic;

namespace dbfirstProjem.Models;

public partial class Salonlar
{
    public int SalonId { get; set; }

    public string Ad { get; set; } = null!;

    public int Kapasite { get; set; }

    public virtual ICollection<Koltuklar> Koltuklars { get; set; } = new List<Koltuklar>();

    public virtual ICollection<Seanslar> Seanslars { get; set; } = new List<Seanslar>();
}
