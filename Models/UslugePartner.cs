using System;
using System.Collections.Generic;

namespace OZO.Models
{
    public partial class UslugePartner
    {
        public int IdUslugePartner { get; set; }
        public int IdUsluge { get; set; }
        public int IdPartnera { get; set; }

        public virtual Usluge IdUslugeNavigation { get; set; }
    }
}
