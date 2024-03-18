using System;
using System.Collections.Generic;

namespace ClinicaVeterinaria.Models;

public partial class Cassetti
{
    public int IdCassetto { get; set; }

    public int? IdArmadietto { get; set; }

    public string Descrizione { get; set; } = null!;

    public virtual Armadietti? IdArmadiettoNavigation { get; set; }

    public virtual ICollection<ProdottiInCassetto> ProdottiInCassettos { get; set; } = new List<ProdottiInCassetto>();
}
