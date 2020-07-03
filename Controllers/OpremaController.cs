using System;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using System.IO;
using OZO.Extensions;
using OZO.Models;
using OZO.ViewModels;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Text.Json;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace OZO.Controllers
{
    public class OpremaController : Controller
    {
        // GET: /<controller>/
        private readonly PI09Context ctx;
        private readonly AppSettings appSettings;
        private readonly ILogger<OpremaController> logger;

        public OpremaController (PI09Context ctx, IOptionsSnapshot<AppSettings> optionsSnapshot, ILogger<OpremaController> logger) //shapshot ako se konfigurancijska dat. promijeni pri novom istanciranju promijenit će se i ovo-povezana s appsetting.json natjestamo u Startup
        {
            this.ctx = ctx;
            appSettings=optionsSnapshot.Value;
            this.logger = logger;  
        }
        public IActionResult Index(int page = 1, int sort = 1, bool ascending = true){ //preslikva link koji pokazuje kada ucitamo određenu stranicu, ako nedostaje neki od podataka ovaj se broj uzima
             int pagesize = appSettings.PageSize;
              var query = ctx.Oprema //upit na bazu
                    .AsNoTracking();
                int count = query.Count(); //broj zapisa
                  if (count == 0)
                {
                    logger.LogInformation("Ne postoji oprema");
                    TempData[Constants.Message] = "Ne postoji oprema.";
                    TempData[Constants.ErrorOccurred] = false;
                //    return RedirectToAction(nameof(Create));
                }
                
                    
            
             System.Linq.Expressions.Expression<Func<Oprema, object>> orderSelector = null;
                switch (sort)
                {   
                    case 1:
                    orderSelector = d => d.SlikaOpreme;
                    break;
                    case 2:
                    orderSelector = d => d.IdOprema;
                    break;
                    case 3:
                    orderSelector = d => d.Naziv;
                    break;
                    case 4:
                    orderSelector = d => d.Dostupnost;
                    break;
                    case 5:
                    orderSelector = d => d.IdReferentniTip ;
                    break;
                }
             if (orderSelector != null)
                {
                    query = ascending ? //određuje je li order bi uzlazno ili silazno
                        query.OrderBy(orderSelector) :
                        query.OrderByDescending(orderSelector);
                }
             var oprema =  query
                         .Select(m => new OpremaViewModel
                  {
                    IdOprema = m.IdOprema,
                    Naziv = m.Naziv,
                    Status = m.Status,
                    Dostupnost= m.Dostupnost,
                    NazivReferentniTip = m.IdReferentniTipNavigation.Naziv,
                    ImaSlika = m.SlikaOpreme  != null,
                    SlikaChecksum= m.SlikaChecksum
                  })
                        .Skip((page - 1) * pagesize ) //  koliko podataka preskočiti, na 7.str. preskočit ćemo 6*vel.stranice
                        .Take(pagesize) //dohvaćamo elemente
                        .ToList(); //dobijemo listu
             var pagingInfo = new PagingInfo //ujedinjujemo sve informacije koje smo primili sa strane 
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
                else if (page > pagingInfo.TotalPages) //kada korisnik dode do zadnje stranice, radimo redirekcija na neku akciju-referencirat ćemo se na imena metoda nameof nam osigurava da se promjenom imena ne naruši ova stranica (prilikom kompajliranja javlja grešku ili otkrije stranicu)
                    {
                         return RedirectToAction(nameof(Index), new { page = pagingInfo.TotalPages, sort, ascending }); //new-anonimna klasa, formira se link na novu akciju 
                     }
                 var model = new OpremeViewModel
                    {
                        Oprema = oprema,
                        PagingInfo = pagingInfo
                    };

                    return View(model);
            }
            
             public FileContentResult GetImage(int id)
            {
            byte[] image = ctx.Oprema
                                .Where(a => a.IdOprema == id)
                                .Select(a => a.SlikaOpreme)
                                .SingleOrDefault();
            if (image != null)
                return File(image, "image/jpeg");
            else
                return null;
            }
            [HttpPost]
            [ValidateAntiForgeryToken]
            public IActionResult Delete(int id)
            {
            var oprema = ctx.Oprema.Find(id);
            if (oprema != null)
            {
                try
                {
                string naziv = oprema.Naziv;
                ctx.Remove(oprema);
                ctx.SaveChanges();
                    var result = new
                    {
                        message = $"Oprema {naziv} sa šifrom {id} uspješno obrisan.",
                        successful = true
                    };
                    return Json(result);
                }
                catch (Exception exc)
                {
                        var result = new
                    {
                        message = "Pogreška prilikom brisanja opreme: " + exc.CompleteExceptionMessage(),
                        successful = false
                    };
                    return Json(result);
                }
            }
            else
            {
                return NotFound($"Oprema sa šifrom {id} ne postoji");
            }
            }
            private void PrepareDropDownLists()
            {
            var RT = ctx.ReferentniTip                     
                            .OrderBy(d => d.IdReferentniTip )
                            .Select(d => new { d.IdReferentniTip , d.Naziv })
                            .ToList();      
            ViewBag.ReferentniTip = new SelectList (RT, nameof(ReferentniTip.IdReferentniTip), nameof(ReferentniTip.Naziv));
            }
            [HttpGet]
            public IActionResult Create()
            {
            PrepareDropDownLists();
            return View();
            }
        
            [HttpPost]
            [ValidateAntiForgeryToken]
            public async Task<IActionResult> Create(Oprema oprema, IFormFile slika)
                    {
                    
                        //provjeri jedinstvenost šifre artikla
                        bool exists = await ctx.Oprema.AnyAsync(a => a.IdOprema == oprema.IdOprema);
                        if (exists)
                        {
                        ModelState.AddModelError(nameof(Oprema.IdOprema), "Artikl s navedenom šifrom već postoji");
                        }
                    
                    if (ModelState.IsValid)
                    {
                        try
                        {
                        if (slika != null && slika.Length > 0)
                        {
                            using (MemoryStream stream = new MemoryStream())
                            {
                            await slika.CopyToAsync(stream);
                            oprema.SlikaOpreme = stream.ToArray();
                            }
                        }
                        ctx.Add(oprema);
                        await ctx.SaveChangesAsync();

                        TempData[Constants.Message] = $"Oprema  {oprema.IdOprema} - {oprema.Naziv} dodan";
                        TempData[Constants.ErrorOccurred] = false;
                        return RedirectToAction(nameof(Index));

                        }
                        catch (Exception exc)
                        {
                        ModelState.AddModelError(string.Empty, exc.CompleteExceptionMessage());
                        PrepareDropDownLists();
                        return View(oprema);
                        }
                    }
                    else
                    {
                        PrepareDropDownLists();
                        return View(oprema);
                    }
                    }    
            

              public PartialViewResult Row(int id)
                    {
                    var oprema = ctx.Oprema
                       .AsNoTracking()
                       .Where(a => a.IdOprema == id)
                       .Select(a => new OpremaViewModel
                       {
                         IdOprema = a.IdOprema,
                         Naziv = a.Naziv,
                         Status = a.Status,
                         Dostupnost = a.Dostupnost,
                         NazivReferentniTip = a.IdReferentniTipNavigation.Naziv,
                         ImaSlika = a.SlikaOpreme != null,
                         SlikaChecksum = a.SlikaChecksum
                       })
                       .SingleOrDefault();
      if (oprema != null)
      {
        return PartialView(oprema);
      }
      else
      {
        //vratiti prazan sadržaj?
        return PartialView("ErrorMessageRow", $"Neispravna je šifra opreme: {id}");
      }
    }

    [HttpGet]
    public IActionResult Edit(int id)
    {
      var oprema = ctx.Oprema.Find(id);
      if (oprema != null)
      {
        PrepareDropDownLists();
        return PartialView(oprema);
      }
      else
      {
        return NotFound($"Neispravna šifra artikla: {id}");
      }
    }
    
    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult Edit(Oprema oprema, IFormFile slika, bool obrisisliku)
    {
      if (oprema == null)
      {
        return NotFound("Nema poslanih podataka");
      }
      Oprema dbOprema = ctx.Oprema.FirstOrDefault(a => a.IdOprema == oprema.IdOprema);
      if (dbOprema == null)
      {
        return NotFound($"Neispravna sifra artikla: {oprema.IdOprema}");
      }

      if (ModelState.IsValid)
      {
        try
        {
          //ne možemo ići na varijantu ctx.Update(artikl), jer nismo prenosili sliku, pa bi bila obrisana
          dbOprema.Naziv = oprema.Naziv;
          dbOprema.Status = oprema.Status;
          dbOprema.Dostupnost = oprema.Dostupnost;
          dbOprema.IdReferentniTip= oprema.IdReferentniTip;
          

          if (slika != null && slika.Length > 0)
          {
            using (MemoryStream stream = new MemoryStream())
            {
              slika.CopyTo(stream);
              dbOprema.SlikaOpreme = stream.ToArray();
            }
          }
          else if (obrisisliku)
          {
            oprema.SlikaOpreme = null;
          }

          ctx.SaveChanges();
          return StatusCode(302, Url.Action(nameof(Row), new { id = oprema.IdOprema }));
           
        }
        catch (Exception exc)
        {
          ModelState.AddModelError(string.Empty, exc.CompleteExceptionMessage());
          return PartialView(oprema);
        }
      }
      else
      {
        return PartialView(oprema);
      }
    }

}
}
