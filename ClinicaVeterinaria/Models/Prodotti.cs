using System.ComponentModel.DataAnnotations;

namespace ClinicaVeterinaria.Models;

public partial class Prodotti
{
    public int IdProdotto { get; set; }

    public string Nomeprodotto { get; set; } = null!;

    public int IdDittaFornitrice { get; set; }

    public bool IsMedicinale { get; set; }

    public string PossibiliUsi { get; set; } = null!;

    public decimal? Prezzo { get; set; }

    public string? FotoProdotto { get; set; }

    [Display(Name = "Ditta")]
    public virtual Dittafornitrice IdDittaFornitriceNavigation { get; set; } = null!;

    public virtual ICollection<ProdottiInCassetto> ProdottiInCassettos { get; set; } = new List<ProdottiInCassetto>();

    public virtual ICollection<Vendite> Vendites { get; set; } = new List<Vendite>();

    public virtual ICollection<Ricettemediche> IdRicettaMedicas { get; set; } = new List<Ricettemediche>();
}
