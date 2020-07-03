using System;
using System.Collections.Generic;

namespace OZO.Models
{
    public partial class Tvrtka
    {
        public Tvrtka()
        {
            Partner = new HashSet<Partner>();
        }

        public int IdTvrtke { get; set; }
        public string MbrTvrtke { get; set; }
        public string NazivTvrtke { get; set; }

        public virtual ICollection<Partner> Partner { get; set; }
    }
}
