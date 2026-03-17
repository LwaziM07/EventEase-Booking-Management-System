using Cldv_Poe_Submission.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace Cldv_Poe_Submission.Controllers
{
    public class HomeController : Controller
    {
        private readonly EventEaseDBContext _context;
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
                }).ToList();

            return View(tableSummary);
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
