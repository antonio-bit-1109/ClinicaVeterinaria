using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace ClinicaVeterinaria.Models;

public partial class Prodotti
{
    public int IdProdotto { get; set; }

    [Display(Name = "Prodotto")]
    public string Nomeprodotto { get; set; } = null!;

    [Display(Name = "Ditta")]
    public int IdDittaFornitrice { get; set; }

    [Display(Name = "Medicinale")]
    public bool IsMedicinale { get; set; }

    [Display(Name = "Possibili Usi")]
    public string PossibiliUsi { get; set; } = null!;

    public decimal? Prezzo { get; set; }

    [Display(Name = "Ditta")]
    public virtual Dittafornitrice IdDittaFornitriceNavigation { get; set; } = null!;

    public virtual ICollection<ProdottiInCassetto> ProdottiInCassettos { get; set; } = new List<ProdottiInCassetto>();

    public virtual ICollection<Vendite> Vendites { get; set; } = new List<Vendite>();

    public virtual ICollection<Ricettemediche> IdRicettaMedicas { get; set; } = new List<Ricettemediche>();
}
