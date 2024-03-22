using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace ClinicaVeterinaria.Models
{
    public partial class Animali
    {
        public int IdAnimale { get; set; }

        [Display(Name = "Data di Registrazione")]
        public DateTime Dataregistrazione { get; set; }

        [Display(Name = "Nome dell'Animale")]
        [Required(ErrorMessage = "Il nome dell'animale è obbligatorio.")]
        public string NomeAnimale { get; set; } = null!;

        [Display(Name = "Tipologia")]
        [Required(ErrorMessage = "La tipologia dell'animale è obbligatoria.")]
        public string Tipologia { get; set; } = null!;

        [Display(Name = "Colore del Mantello")]
        [Required(ErrorMessage = "Il colore del mantello è obbligatorio.")]
        public string ColoreMantello { get; set; } = null!;

        [Display(Name = "Data di Nascita")]
        [DataType(DataType.Date)]
        public DateTime? Datanascita { get; set; }

        [Display(Name = "Microchip presente?")]
        public bool HasMicrochip { get; set; }

        [Display(Name = "Numero Microchip")]
        public string? NumMicrochip { get; set; }

        [Display(Name = "Foto dell'Animale")]
        public string? FotoAnimale { get; set; } = null!;

        [Display(Name = "Proprietario presente?")]
        public bool HasProprietario { get; set; }

        public int? IdUtente { get; set; }

        [Display(Name = "Utente")]
        public virtual Utenti? IdUtenteNavigation { get; set; }

        public virtual ICollection<Ricoveri> Ricoveris { get; set; } = new List<Ricoveri>();

        public virtual ICollection<Visite> Visites { get; set; } = new List<Visite>();
    }
}
