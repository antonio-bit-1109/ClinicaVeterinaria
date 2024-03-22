﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace ClinicaVeterinaria.Models;

public partial class Vendite
{
    public int IdVendita { get; set; }

    public int? IdProdotto { get; set; }

    public int? IdUtente { get; set; }

    [Display(Name = "Codice Fiscale")]
    public string? Cf { get; set; }

    public int? IdricettaMedica { get; set; }

    public virtual Prodotti? IdProdottoNavigation { get; set; }

    public virtual Utenti? IdUtenteNavigation { get; set; }

    public virtual Ricettemediche? IdricettaMedicaNavigation { get; set; }
}
