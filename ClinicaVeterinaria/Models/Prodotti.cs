using System;
using System.Collections.Generic;

namespace ClinicaVeterinaria.Models
{
    public partial class Prodotti
    {
        public int IdProdotto { get; set; }

        [Display(Name = "Nome Prodotto")]
        [Required(ErrorMessage = "Il nome del prodotto è obbligatorio.")]
        public string Nomeprodotto { get; set; } = null!;

        [Display(Name = "ID Ditta Fornitrice")]
        [Required(ErrorMessage = "L'ID della ditta fornitrice è obbligatorio.")]
        public int IdDittaFornitrice { get; set; }

        [Display(Name = "Medicinale")]
        public bool IsMedicinale { get; set; }

        [Display(Name = "Possibili Usi")]
        [Required(ErrorMessage = "I possibili usi del prodotto sono obbligatori.")]
        public string PossibiliUsi { get; set; } = null!;

        [Display(Name = "Prezzo")]
        [Range(0, double.MaxValue, ErrorMessage = "Il prezzo deve essere maggiore o uguale a zero.")]
        public decimal? Prezzo { get; set; }

        [Display(Name = "Foto Prodotto")]
        public string? FotoProdotto { get; set; }

        public virtual Dittafornitrice IdDittaFornitriceNavigation { get; set; } = null!;

        public virtual ICollection<ProdottiInCassetto> ProdottiInCassettos { get; set; } = new List<ProdottiInCassetto>();

        public virtual ICollection<Vendite> Vendites { get; set; } = new List<Vendite>();

        public virtual ICollection<Ricettemediche> IdRicettaMedicas { get; set; } = new List<Ricettemediche>();
    }
}
