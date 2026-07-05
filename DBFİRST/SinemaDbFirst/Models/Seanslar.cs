using System;
using System.Collections.Generic;

namespace dbfirstProjem.Models;

public partial class Seanslar
{
    public int SeansId { get; set; }

    public int FilmId { get; set; }

    public int SalonId { get; set; }

    public DateTime BaslamaZamani { get; set; }

    public virtual ICollection<Biletler> Biletlers { get; set; } = new List<Biletler>();

    public virtual Filmler Film { get; set; } = null!;

    public virtual Salonlar Salon { get; set; } = null!;
}
