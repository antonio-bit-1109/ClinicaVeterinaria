using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace ClinicaVeterinaria.Models;

public partial class Visite
{
    public int IdVisita { get; set; }

    [Display(Name = "Data Visita")]
    public DateTime DataVisita { get; set; }
    public string Anamnesi { get; set; } = null!;

    [Display(Name = "Descrizione Cura")]
    public string DescrizioneCura { get; set; } = null!;

    [Display(Name = "ID Animale")]
    public int? IdAnimale { get; set; }

    [Display(Name = "Prezzo Visita")]
    public decimal? PrezzoVisita { get; set; }

    public virtual Animali? IdAnimaleNavigation { get; set; }

    public virtual ICollection<Ricettemediche> Ricettemediches { get; set; } = new List<Ricettemediche>();
}
