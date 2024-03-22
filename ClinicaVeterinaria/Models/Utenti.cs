using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace ClinicaVeterinaria.Models;

public partial class Utenti
{
    public int IdUtente { get; set; }

    [Required(ErrorMessage = "Il campo Nome è obbligatorio.")]
    [Display(Name = "Nome")]
    public string Nome { get; set; }

    [Required(ErrorMessage = "Il campo Cognome è obbligatorio.")]
    [Display(Name = "Cognome")]
    public string Cognome { get; set; }

    [Required(ErrorMessage = "Il campo Password è obbligatorio.")]
    [DataType(DataType.Password)]
    [Display(Name = "Password")]
    public string Password { get; set; }

    [Display(Name = "Foto Utente")]
    public string? FotoUtente { get; set; }

    public int? IdRuolo { get; set; }

    public virtual ICollection<Animali> Animalis { get; set; } = new List<Animali>();

    public virtual Ruoli? IdRuoloNavigation { get; set; }

    public virtual ICollection<Ricettemediche> Ricettemediches { get; set; } = new List<Ricettemediche>();

    public virtual ICollection<Vendite> Vendites { get; set; } = new List<Vendite>();
}
