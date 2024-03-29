﻿using System;
using System.Collections.Generic;

namespace ClinicaVeterinaria.Models;

public partial class Ricettemediche
{
    public int IdricettaMedica { get; set; }

    public int? IdVisita { get; set; }

    public int? IdUtente { get; set; }

    public DateTime DataPrescrizione { get; set; }

    public string Descrizione { get; set; } = null!;

    public virtual Utenti? IdUtenteNavigation { get; set; }

    public virtual Visite? IdVisitaNavigation { get; set; }

    public virtual ICollection<Vendite> Vendites { get; set; } = new List<Vendite>();

    public virtual ICollection<Prodotti> IdProdottos { get; set; } = new List<Prodotti>();
}
