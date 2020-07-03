using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace OZO.ViewModels
{
  public class PartnerViewModel : IValidatableObject
  {
    public int IdPartnera { get; set; }
    [Required]
    [RegularExpression("[OT]")]
    public string TipPartnera { get; set; }
    [Display(Name = "Prezime")]
    public string PrezimeOsobe { get; set; }
    [Display(Name = "Ime")]
    public string ImeOsobe { get; set; }
    [Display(Name = "Matični broj tvrtke")]
    public string MbrTvrtke { get; set; }
    [Display(Name = "Naziv")]
    public string NazivTvrtke { get; set; }
    [Required]
    [RegularExpression("[0-9]{13}", ErrorMessage = "Mbr mora imati 13 znamenki!")]
    [Display(Name = "Mbr")]
    public string Mbr { get; set; }
    [Display(Name = "Adresa")]
    public string AdrPartnera { get; set; }
    [Display(Name = "Adresa isporuke")]
    public string AdrIsporuke { get; set; }

    public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
    {
      if (TipPartnera == "O")
      {
        if (string.IsNullOrWhiteSpace(ImeOsobe)) { yield return new ValidationResult("Potrebno je upisati ime osobe", new[] { nameof(ImeOsobe) }); }
        if (string.IsNullOrWhiteSpace(PrezimeOsobe))
        { //-->yield sad vrati ValidationResult i ostani tu u metodi, pa kada drugi put bude pozvan nastavlja od tu kod
          yield return new ValidationResult("Potrebno je upisati prezime osobe", new[] { nameof(PrezimeOsobe) });
        }
      }
      else if (TipPartnera == "T")
      {
        if (string.IsNullOrWhiteSpace(NazivTvrtke))
        {
          yield return new ValidationResult("Potrebno je upisati naziv tvrtke", new[] { nameof(NazivTvrtke) });
        }
        if (string.IsNullOrWhiteSpace(MbrTvrtke))
        {
          yield return new ValidationResult("Potrebno je upisati matični broj tvrtke", new[] { nameof(MbrTvrtke) });
        }
      }

    }
  }
}
