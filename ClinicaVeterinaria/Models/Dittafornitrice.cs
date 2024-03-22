using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace ClinicaVeterinaria.Models
{
    public partial class Dittafornitrice
    {
        public int IdDittaFornitrice { get; set; }

        [Display(Name = "Nome Ditta")]
        [Required(ErrorMessage = "Il nome della ditta è obbligatorio.")]
        public string NomeDitta { get; set; } = null!;

        [Display(Name = "Recapito Ditta")]
        [Required(ErrorMessage = "Il recapito della ditta è obbligatorio.")]
        public string RecapitoDitta { get; set; } = null!;

        [Display(Name = "Indirizzo")]
        [Required(ErrorMessage = "L'indirizzo è obbligatorio.")]
        public string Indirizzo { get; set; } = null!;

        public virtual ICollection<Prodotti> Prodottis { get; set; } = new List<Prodotti>();
    }
}
