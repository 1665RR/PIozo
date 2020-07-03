using System;
using System.Collections.Generic;

namespace OZO.Models
{
    public partial class ViewPartner
    {
        public int IdPartnera { get; set; }
        public string TipPartnera { get; set; }
        public string Mbr { get; set; }
        public string Naziv { get; set; }
          public string TipPartneraText
        {
            get
            {
                if (TipPartnera == "O")
                {
                    return "Osoba";
                }
                else
                {
                    return "Tvrtka";
                }
            }
        }  
    }
}
