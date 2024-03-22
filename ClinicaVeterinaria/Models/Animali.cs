using System;
using System.Collections.Generic;

namespace ClinicaVeterinaria.Models;

public partial class Animali
{
    public int IdAnimale { get; set; }

    public DateTime Dataregistrazione { get; set; }

    public string NomeAnimale { get; set; } = null!;

    public string Tipologia { get; set; } = null!;

    public string ColoreMantello { get; set; } = null!;

    public DateTime? Datanascita { get; set; }

    public bool HasMicrochip { get; set; }

    public string? NumMicrochip { get; set; }

    public string? FotoAnimale { get; set; }

    public bool HasProprietario { get; set; }

    public int? IdUtente { get; set; }

    public virtual Utenti? IdUtenteNavigation { get; set; }

    public virtual ICollection<Ricoveri> Ricoveris { get; set; } = new List<Ricoveri>();

    public virtual ICollection<Visite> Visites { get; set; } = new List<Visite>();
}
