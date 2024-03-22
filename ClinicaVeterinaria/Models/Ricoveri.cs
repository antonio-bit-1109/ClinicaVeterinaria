using System;
using System.Collections.Generic;

namespace ClinicaVeterinaria.Models;

public partial class Ricoveri
{
    public int IdRicovero { get; set; }

    public DateTime Dataregistrazionericovero { get; set; }

    public int? IdAnimale { get; set; }

    public DateTime DataInizioRicovero { get; set; }

    public DateTime? DataFinericovero { get; set; }

    public decimal PrezzoGiornalieroRicovero { get; set; }

    public bool IsRicoveroAttivo { get; set; }

    public decimal? PrezzoTotaleRicovero { get; set; }

    public virtual Animali? IdAnimaleNavigation { get; set; }
}
