using System;
using System.Collections.Generic;

namespace ClinicaVeterinaria.Models;

public partial class Armadietti
{
    public int IdArmadietto { get; set; }

    public string Descrizione { get; set; } = null!;

    public virtual ICollection<Cassetti> Cassettis { get; set; } = new List<Cassetti>();
}
