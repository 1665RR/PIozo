using System;
using System.Collections.Generic;

namespace OZO.Models
{
    public partial class NatječajPartner
    {
        public int IdNatječajPartner { get; set; }
        public int IdNatječaja { get; set; }
        public int IdPartnera { get; set; }

        public virtual Natječaji IdNatječajaNavigation { get; set; }
    }
}
