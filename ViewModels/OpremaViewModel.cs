using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace OZO.ViewModels
{
  public class OpremaViewModel
  {
    public int IdOprema  { get; set; }
    public string Naziv  { get; set; }
    public string Status { get; set; }
    public bool Dostupnost { get; set; }
    public string NazivReferentniTip { get; set; }
    public bool ImaSlika { get; set; }
    public int? SlikaChecksum { get; set; }
  }
}

