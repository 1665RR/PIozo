using OZO.Models;
using System.Collections.Generic;

namespace OZO.ViewModels
{
    public class PartneriViewModel
    {
        public IEnumerable<ViewPartner> Partneri { get; set; }
        public PagingInfo PagingInfo { get; set; }       
    }
}