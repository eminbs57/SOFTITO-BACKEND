using System;
using System.Collections.Generic;

namespace dbfirstProjem.Models;

public partial class Filmler
{
    public int FilmId { get; set; }

    public string Ad { get; set; } = null!;

    public int SureDk { get; set; }

    public decimal? Fiyat { get; set; }

    public string? ResimUrl { get; set; }
    public string? Tur { get; set; }

    public virtual ICollection<Seanslar> Seanslars { get; set; } = new List<Seanslar>();
}
