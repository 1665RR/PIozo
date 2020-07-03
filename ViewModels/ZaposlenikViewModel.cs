using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace OZO.ViewModels
{
  public class ZaposlenikViewModel
  {
        public int IdZaposlenici { get; set; }
        public string Ime { get; set; }
        public string Prezime { get; set; }
        public DateTime? DatumRođenja { get; set; }
        public decimal? TrošakZaposlenika { get; set; }
        public string Naziv{ get; set; }
  }
}
