using OZO.Extensions;
using OZO.Models;
using OZO.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using System;
using System.Linq;

namespace OZO.Controllers
{
  public class ZaposleniciController : Controller
  {
    private readonly PI09Context ctx;
    private readonly AppSettings appData;
   
    public ZaposleniciController(PI09Context ctx, IOptionsSnapshot<AppSettings> options)
    {
      this.ctx = ctx;
      appData = options.Value;
    }

    public IActionResult Index(int page = 1, int sort = 1, bool ascending = true)
    {      
      int pagesize = appData.PageSize;
      var query = ctx.Zaposlenici.AsNoTracking();
      int count = query.Count();

      var pagingInfo = new PagingInfo
      {
        CurrentPage = page,
        Sort = sort,
        Ascending = ascending,
        ItemsPerPage = pagesize,
        TotalItems = count
      };
      if (page < 1)
      {
        page = 1;
      }
      else if (page > pagingInfo.TotalPages)
      {
        return RedirectToAction(nameof(Index), new { page = pagingInfo.TotalPages, sort = sort, ascending = ascending });
      }

      System.Linq.Expressions.Expression<Func<Zaposlenici, object>> orderSelector = null;
      switch (sort)
      {
        case 1:
          orderSelector = m => m.IdZaposlenici;
          break;
        case 2:
          orderSelector = m => m.Ime;
          break;
        case 3:
          orderSelector = m => m.Prezime;
          break;
        case 4:
          orderSelector = m => m.DatumRođenja;
          break;
        case 5:
          orderSelector = m => m.TrošakZaposlenika;
          break;
        case 6:
          orderSelector = m => m.IdPosloviNavigation.Naziv;
          break;
      }
      if (orderSelector != null)
      {
        query = ascending ?
               query.OrderBy(orderSelector) :
               query.OrderByDescending(orderSelector);
      }

      var zaposlenici = query
                  .Select(m => new ZaposlenikViewModel
                  {
                    IdZaposlenici = m.IdZaposlenici,
                    Ime = m.Ime,
                    Prezime = m.Prezime,
                    DatumRođenja = m.DatumRođenja,
                    TrošakZaposlenika = m.TrošakZaposlenika,
                    Naziv = m.IdPosloviNavigation.Naziv
                  })
                  .Skip((page - 1) * pagesize)
                  .Take(pagesize)
                  .ToList();
      var model = new ZaposleniciViewModel
      {
        Zaposlenici = zaposlenici,
        PagingInfo = pagingInfo
      };

      return View(model);
    }

    [HttpGet]
    public IActionResult Edit(int id, int page = 1, int sort = 1, bool ascending = true)
    {
      ViewBag.Page = page;
      ViewBag.Sort = sort;
      ViewBag.Ascending = ascending;

      var zaposlenici = ctx.Zaposlenici
                       .AsNoTracking()
                       .Where(m => m.IdZaposlenici == id)
                       .SingleOrDefault();
      if (zaposlenici != null)
      {
        PrepareDropDownLists();
        return View(zaposlenici);
      }
      else
      {
        return NotFound($"Neispravan id zaposlenika: {id}");
      }
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult Edit(Zaposlenici zaposlenici, int page = 1, int sort = 1, bool ascending = true)
    {
      if (zaposlenici == null)
      {
        return NotFound("Nema poslanih podataka");
      }
      bool checkId = ctx.Zaposlenici.Any(m => m.IdZaposlenici == zaposlenici.IdZaposlenici);
      if (!checkId)
      {
        return NotFound($"Neispravan id zaposlenika: {zaposlenici?.IdZaposlenici}");
      }

      PrepareDropDownLists();
      if (ModelState.IsValid)
      {
        try
        {
          ctx.Update(zaposlenici);
          ctx.SaveChanges();

          TempData[Constants.Message] = "Zaposlenici ažurirano.";
          TempData[Constants.ErrorOccurred] = false;
          return RedirectToAction(nameof(Index), new { page, sort, ascending });          
        }
        catch (Exception exc)
        {
          ModelState.AddModelError(string.Empty, exc.CompleteExceptionMessage());
          return View(zaposlenici);
        }
      }
      else
      {
        return View(zaposlenici);
      }
    }

    public IActionResult Row(int id)
    {
      var zaposlenici = ctx.Zaposlenici                       
                       .Where(m => m.IdZaposlenici == id)
                       .Select(m => new ZaposlenikViewModel
                       {
                           IdZaposlenici = m.IdZaposlenici,
                            Ime = m.Ime,
                            Prezime = m.Prezime,
                            DatumRođenja = m.DatumRođenja,
                            TrošakZaposlenika = m.TrošakZaposlenika,
                            Naziv = m.IdPosloviNavigation.Naziv
                       })
                       .SingleOrDefault();
      if (zaposlenici != null)
      {
        return PartialView(zaposlenici);
      }
      else
      {
        //vratiti prazan sadržaj?
        return NoContent();
      }
    }

    [HttpGet]
    public IActionResult Create()
    {
      PrepareDropDownLists();
      return View();
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult Create(Zaposlenici zaposlenici)
    {
      if (ModelState.IsValid)
      {
        try
        {
          ctx.Add(zaposlenici);
          ctx.SaveChanges();

          TempData[Constants.Message] = $"Zaposlenici {zaposlenici.Ime} {zaposlenici.Prezime} dodano. Id zaposlenika = {zaposlenici.IdZaposlenici}";
          TempData[Constants.ErrorOccurred] = false;
          return RedirectToAction(nameof(Index));

        }
        catch (Exception exc)
        {
          ModelState.AddModelError(string.Empty, exc.CompleteExceptionMessage());
          PrepareDropDownLists();
          return View(zaposlenici);
        }
      }
      else
      {
        PrepareDropDownLists();
        return View(zaposlenici);
      }
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult Delete(int id)
    {
      var zaposlenici = ctx.Zaposlenici
                       .AsNoTracking() //ima utjecaj samo za Update, za brisanje možemo staviti AsNoTracking
                       .Where(m => m.IdZaposlenici == id)
                       .SingleOrDefault();
      if (zaposlenici != null)
      {
        try
        {
          string naziv = zaposlenici.Prezime;
          ctx.Remove(zaposlenici);          
          ctx.SaveChanges();
          var result = new
          {
            message = $"Zaposlenik {naziv} sa šifrom {id} obrisano.",
            successful = true
          };
          return Json(result);
        }
        catch (Exception exc)
        {
          var result = new
          {
            message = "Pogreška prilikom brisanja zaposlenika: " + exc.CompleteExceptionMessage(),
            successful = false
          };
          return Json(result);
        }
      }
      else
      {
        return NotFound($"Zaposlenici sa šifrom {id} ne postoji");
      }
    }

    private void PrepareDropDownLists()
    {
        var poslovi = ctx.Poslovi                    
                            .OrderBy(d => d.IdPoslovi)
                            .Select(d => new { d.IdPoslovi, d.Naziv })
                            .ToList();     
          
        ViewBag.Poslovi = new SelectList(poslovi, nameof(Poslovi.IdPoslovi), nameof(Poslovi.Naziv));
    }
  }
}
