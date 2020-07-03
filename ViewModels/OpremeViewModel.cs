using System;
using System.Collections.Generic;

namespace OZO.ViewModels
{
  public class OpremeViewModel
  {
    public IEnumerable<OpremaViewModel> Oprema { get; set; }
    public PagingInfo PagingInfo { get; set; }
  }
}

