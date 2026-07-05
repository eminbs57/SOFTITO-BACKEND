using System;
using System.Collections.Generic;

namespace dbfirstProjem.Models;

public partial class Koltuklar
{
    public int KoltukId { get; set; }

    public int SalonId { get; set; }

    public string Sira { get; set; } = null!;

    public string Numara { get; set; } = null!;
    public bool DoluMu { get; set; }
    public virtual ICollection<Biletler> Biletlers { get; set; } = new List<Biletler>();

    public virtual Salonlar Salon { get; set; } = null!;
}
