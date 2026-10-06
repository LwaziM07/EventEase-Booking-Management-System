using Cldv_Poe_Submission.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace Cldv_Poe_Submission.Controllers
{
    public class HomeController : Controller
    {
        private readonly EventEaseDBContext _context;//loads up the context with data from
                                                     //the database (Co., 2022).
        public HomeController(EventEaseDBContext context)
        {
            _context = context;
        }
        public IActionResult Index()
        {
            var tableSummary = _context.Bookings.Select
                (b => new SummaryModel
                {
                    SpecialistName = b.SpecialistName,
                    EventName = b.Event.EventName,
                    VenueName = b.Venue.VenueName,
                    BookingDate = b.BookingDate
                }).ToList();//using LINQ to create a table of information,
                            //consisting the specialist, event name, venue name and booking date (Troelsen and Japikse, 2022)

            return View(tableSummary); //setting up a method to return a summarised listed of attributes
        }
        
        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
/*Reference List
 
Co., T. (2022). Cloud Computing Technology. Springer Nature.
 
Troelsen, A. and Japikse, P. (2022). Pro C# 10 with .NET 6. Apress.
 
 */