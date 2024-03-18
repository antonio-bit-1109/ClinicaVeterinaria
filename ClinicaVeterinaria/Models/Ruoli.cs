using System;
using System.Collections.Generic;

namespace ClinicaVeterinaria.Models;

public partial class Ruoli
{
    public int IdRuolo { get; set; }

    public string NomeRuolo { get; set; } = null!;

    public virtual ICollection<Utenti> Utentis { get; set; } = new List<Utenti>();
}
