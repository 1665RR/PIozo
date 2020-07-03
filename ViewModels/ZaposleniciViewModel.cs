using System.Collections.Generic;

namespace OZO.ViewModels
{
  public class ZaposleniciViewModel
  {
    public IEnumerable<ZaposlenikViewModel> Zaposlenici { get; set; }
    public PagingInfo PagingInfo { get; set; }
  }
}
