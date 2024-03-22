using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace ClinicaVeterinaria.Models
{
    public partial class Ricoveri
    {
        public int IdRicovero { get; set; }

        [Display(Name = "Data di Registrazione del Ricovero")]
        public DateTime Dataregistrazionericovero { get; set; }

        public int? Idanimale { get; set; }

        [Display(Name = "Data di Inizio del Ricovero")]
        public DateTime DataInizioRicovero { get; set; }

        [Display(Name = "Data di Fine del Ricovero")]
        public DateTime? DataFinericovero { get; set; }

        [Display(Name = "Prezzo Giornaliero del Ricovero")]
        public decimal PrezzoGiornalieroRicovero { get; set; }

        [Display(Name = "Stato del Ricovero")]
        public bool IsRicoveroAttivo { get; set; }

        [Display(Name = "Prezzo Totale del Ricovero")]
        public decimal? PrezzoTotaleRicovero { get; set; }

        public virtual Animali? IdanimaleNavigation { get; set; }
    }
}
