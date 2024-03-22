namespace ClinicaVeterinaria.Models.ViewModel
{
    public class VisitaViewModel
    {
        public Visite Visita { get; set; }
        public List<Ricettemediche> Ricette { get; set; }

        // Dizionario che mappa ogni IdricettaMedica ai rispettivi Prodotti
        public Dictionary<int, List<Prodotti>> ProdottiPerRicetta { get; set; }

        public VisitaViewModel()
        {
            Ricette = new List<Ricettemediche>();
            ProdottiPerRicetta = new Dictionary<int, List<Prodotti>>();
        }
    }
}
