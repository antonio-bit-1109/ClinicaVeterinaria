namespace ClinicaVeterinaria.Models;

public partial class Utenti
{
    public int IdUtente { get; set; }

    public string Nome { get; set; } = null!;

    public string Cognome { get; set; } = null!;

    public string Password { get; set; } = null!;

    public string? FotoUtente { get; set; }

    public int? IdRuolo { get; set; }

    public virtual ICollection<Animali> Animalis { get; set; } = new List<Animali>();

    public virtual Ruoli? IdRuoloNavigation { get; set; }

    public virtual ICollection<Ricettemediche> Ricettemediches { get; set; } = new List<Ricettemediche>();

    public virtual ICollection<Vendite> Vendites { get; set; } = new List<Vendite>();
}
