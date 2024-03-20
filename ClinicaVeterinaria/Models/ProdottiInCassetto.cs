using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace ClinicaVeterinaria.Models;

public partial class ProdottiInCassetto
{
    public int IdProdottoInCassetto { get; set; }

    [Display(Name = "Prodotto")]
    public int? IdProdotto { get; set; }

    [Display(Name = "Cassetto")]
    public int? IdCassetto { get; set; }

    public int Quantita { get; set; }

    public virtual Cassetti? IdCassettoNavigation { get; set; }

    public virtual Prodotti? IdProdottoNavigation { get; set; }
}
