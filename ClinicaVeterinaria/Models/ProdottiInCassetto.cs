using System;
using System.Collections.Generic;

namespace ClinicaVeterinaria.Models;

public partial class ProdottiInCassetto
{
    public int IdProdottoInCassetto { get; set; }

    public int? IdProdotto { get; set; }

    public int? IdCassetto { get; set; }

    public int Quantita { get; set; }

    public virtual Cassetti? IdCassettoNavigation { get; set; }

    public virtual Prodotti? IdProdottoNavigation { get; set; }
}
