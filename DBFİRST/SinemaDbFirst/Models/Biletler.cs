using System;
using System.Collections.Generic;

namespace dbfirstProjem.Models;

public partial class Biletler
{
    public int BiletId { get; set; }

    public int SeansId { get; set; }

    public int KoltukId { get; set; }

    public DateTime SatisTarihi { get; set; }

    public decimal Fiyat { get; set; }

    public string? UserId { get; set; }

    public virtual Koltuklar Koltuk { get; set; } = null!;

    public virtual Seanslar Seans { get; set; } = null!;
}
