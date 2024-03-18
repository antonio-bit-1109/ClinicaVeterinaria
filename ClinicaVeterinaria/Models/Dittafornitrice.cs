using System;
using System.Collections.Generic;

namespace ClinicaVeterinaria.Models;

public partial class Dittafornitrice
{
    public int IdDittaFornitrice { get; set; }

    public string NomeDitta { get; set; } = null!;

    public string RecapitoDitta { get; set; } = null!;

    public string Indirizzo { get; set; } = null!;

    public virtual ICollection<Prodotti> Prodottis { get; set; } = new List<Prodotti>();
}
