using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace ClinicaVeterinaria.Models;

public partial class ProdottiInCassetto
{
    public int IdProdottoInCassetto { get; set; }

    public int? IdProdotto { get; set; }

    public int? IdCassetto { get; set; }

    [Display(Name = "Quantità")]
    [Range(0, int.MaxValue, ErrorMessage = "La quantità deve essere maggiore o uguale a zero.")]
    public int Quantita { get; set; }

    public virtual Cassetti? IdCassettoNavigation { get; set; }

    public virtual Prodotti? IdProdottoNavigation { get; set; }
}
