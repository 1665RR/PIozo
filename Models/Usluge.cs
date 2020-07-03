using System;
using System.Collections.Generic;

namespace OZO.Models
{
    public partial class Usluge
    {
        public Usluge()
        {
            Poslovi = new HashSet<Poslovi>();
            UslugePartner = new HashSet<UslugePartner>();
        }

        public int IdUsluge { get; set; }
        public string NazivUsluge { get; set; }
        public decimal? Cijena { get; set; }
        public string Opis { get; set; }
        public int IdReferentniTip { get; set; }
        public DateTime? VremenskiRok { get; set; }

        public virtual ReferentniTip IdReferentniTipNavigation { get; set; }
        public virtual ICollection<Poslovi> Poslovi { get; set; }
        public virtual ICollection<UslugePartner> UslugePartner { get; set; }
    }
}
