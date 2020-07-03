using System;
using System.Collections.Generic;

namespace OZO.Models
{
    public partial class Partner
    {
        public int IdPartnera { get; set; }
        public string TipPartnera { get; set; }
        public string Mbr { get; set; }
        public string AdrPartnera { get; set; }
        public string AdrIsporuke { get; set; }
        public int IdTvrtke { get; set; }

        public virtual Tvrtka IdTvrtkeNavigation { get; set; }
        public virtual Osoba Osoba { get; set; }
        public virtual ICollection<NatjecajPartner> NatjecajPartner { get; set; }
    }
}
