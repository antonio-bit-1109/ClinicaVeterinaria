namespace ClinicaVeterinaria.Models
{
    public class BackOfficeModel
    {
        public List<Utenti> Utenti { get; set; }
        public List<Animali> Animali { get; set; }
        public List<Armadietti> Armadietti { get; set; }
        public List<Cassetti> Cassetti { get; set; }
        public List<Dittafornitrice> Dittafornitrice { get; set; }
        public List<Prodotti> Prodotti { get; set; }
        public List<ProdottiInCassetto> ProdottiInCassetto { get; set; }
        public List<Ricettemediche> Ricettemediche { get; set; }
        public List<Ricoveri> Ricoveri { get; set; }
        public List<Visite> Visite { get; set; }
        public List<Vendite> Vendite { get; set; }

        public List<Ruoli> Ruoli { get; set; }

    }
}
