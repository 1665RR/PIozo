using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using OZO.Models;
using Microsoft.Extensions.Options;

// For more information on enabling Web API for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace OZO.Controllers.AutoComplete
{
    [Route("autocomplete/[controller]")]
    public class PosloviController : Controller
    {
        private readonly PI09Context ctx;
        private readonly AppSettings appData;

        public PosloviController(PI09Context ctx, IOptionsSnapshot<AppSettings> options)
        {
            this.ctx = ctx;
            appData = options.Value;
        }
        
        [HttpGet]      
        public IEnumerable<IdLabel> Get(string term)
        {
            var query = ctx.Poslovi
                            .Select(m => new IdLabel
                            {
                                Id = m.IdPoslovi,
                                Label = m.Naziv
                            })
                            .Where(l => l.Label.Contains(term));
          
            var list = query.OrderBy(l => l.Label)
                            .ThenBy(l => l.Id)
                            .Take(appData.AutoCompleteCount)
                            .ToList();           
            return list;
        }       
    }
}
