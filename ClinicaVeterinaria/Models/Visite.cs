using System;
using System.Collections.Generic;

namespace ClinicaVeterinaria.Models;

public partial class Visite
{
    public int IdVisita { get; set; }

    public DateTime DataVisita { get; set; }

    public string Anamnesi { get; set; } = null!;

    public string DescrizioneCura { get; set; } = null!;

    public int? IdAnimale { get; set; }

    public decimal? PrezzoVisita { get; set; }

    public virtual Animali? IdAnimaleNavigation { get; set; }

    public virtual ICollection<Ricettemediche> Ricettemediches { get; set; } = new List<Ricettemediche>();
}
