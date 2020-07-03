using System;
using System.Collections.Generic;

namespace OZO.Models
{
    public partial class NatjecajPartner
    {
        public int IdNatjecajPartner { get; set; }
        public int IdPartnera { get; set; }
        public int IdNatječaji { get; set; }

        public virtual Natječaji IdNatječajiNavigation { get; set; }
        public virtual Partner IdPartneraNavigation { get; set; }
    }
}
