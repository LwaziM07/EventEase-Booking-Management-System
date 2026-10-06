using Cldv_Poe_Submission.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Transactions;

namespace Cldv_Poe_Submission.Controllers
{
    public class BookingsController : Controller
    {
        private readonly EventEaseDBContext _context;

        public BookingsController(EventEaseDBContext context)
        {
            _context = context;
        }

        // GET: Bookings
        public async Task<IActionResult> Index(string searchString)
        {
            var bookings = _context.Bookings.Include(b => b.Event).Include(b => b.Venue);
            return View(await bookings.ToListAsync());
        }

        // GET: Bookings/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var booking = await _context.Bookings
                .Include(b => b.Event)
                .Include(b => b.Venue)
                .FirstOrDefaultAsync(m => m.BookingId == id);
            if (booking == null)
            {
                return NotFound();
            }

            return View(booking);
        }

        // GET: Bookings/Create
        public IActionResult Create()
        {
            ViewData["EventId"] = new SelectList(_context.Events, "EventId", "EventName");
            ViewData["VenueId"] = new SelectList(_context.Venues, "VenueId", "VenueName");
            return View();
        }

        // POST: Bookings/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("BookingId,EventId,SpecialistName,SpecialistEmail,BookingDate,VenueId")] Booking booking)
        {
            if (ModelState.IsValid)
            {
                var transactionOptions = new TransactionOptions
                {
                    IsolationLevel = IsolationLevel.Serializable
                };
                
                using (var scope = new TransactionScope(TransactionScopeOption.Required, transactionOptions,
                    TransactionScopeAsyncFlowOption.Enabled))
                {
                    var currentEvent = await _context.Events.FirstOrDefaultAsync(e => e.EventId == booking.EventId);

                    //retrieving all of the venue data
                    var venue = await _context.Venues.FirstOrDefaultAsync(v => v.VenueId == booking.VenueId);

                    if (venue == null)
                    {

                        return NotFound();

                    }
                    if (!venue.Availability)//checks if the venue is listed as available.
                    {//if false, an error message is displayed to the user.
                        TempData["ErrorMessage"] = "this venue is not available."; //setting the error message

                        ViewData["EventId"] = new SelectList(_context.Events, "EventId", "EventName", booking.EventId);
                        ViewData["VenueId"] = new SelectList(_context.Venues, "VenueId", "VenueName", booking.VenueId);

                        return View(booking);
                    }

                    bool isDoubleBooked = await _context.Bookings.Where(b => b.VenueId == booking.VenueId).AnyAsync(b => currentEvent.StartDate < b.Event.EndDate && currentEvent.EndDate > b.Event.StartDate);
                    if (isDoubleBooked)
                    {
                        TempData["ErrorMessage"] = "cannot book a venue to two events at the same time."; //setting the error message

                        ViewData["EventId"] = new SelectList(_context.Events, "EventId", "EventName", booking.EventId);
                        ViewData["VenueId"] = new SelectList(_context.Venues, "VenueId", "VenueName", booking.VenueId);

                        return View(booking);
                    }
                    else
                    {
                        _context.Add(booking);
                        await _context.SaveChangesAsync();
                        scope.Complete();
                    }
                    return RedirectToAction(nameof(Index));

                }
                
            }
            ViewData["EventId"] = new SelectList(_context.Events, "EventId", "EventName", booking.EventId);
            ViewData["VenueId"] = new SelectList(_context.Venues, "VenueId", "VenueName", booking.VenueId);
            return View(booking);
        }

        // GET: Bookings/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var booking = await _context.Bookings.FindAsync(id);
            if (booking == null)
            {
                return NotFound();
            }
            ViewData["EventId"] = new SelectList(_context.Events, "EventId", "EventName", booking.EventId);
            ViewData["VenueId"] = new SelectList(_context.Venues, "VenueId", "VenueName", booking.VenueId);
            return View(booking);
        }

        // POST: Bookings/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("BookingId,EventId,SpecialistName,SpecialistEmail,BookingDate,VenueId")] Booking booking)
        {
            if (id != booking.BookingId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(booking);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!BookingExists(booking.BookingId))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            ViewData["EventId"] = new SelectList(_context.Events, "EventId", "EventName", booking.EventId);
            ViewData["VenueId"] = new SelectList(_context.Venues, "VenueId", "VenueName", booking.VenueId);
            return View(booking);
        }

        // GET: Bookings/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var booking = await _context.Bookings
                .Include(b => b.Event)
                .Include(b => b.Venue)
                .FirstOrDefaultAsync(m => m.BookingId == id);
            if (booking == null)
            {
                return NotFound();
            }

            return View(booking);
        }

        // POST: Bookings/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var booking = await _context.Bookings.FindAsync(id);
            if (booking != null)
            {
                _context.Bookings.Remove(booking);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        public async Task<IActionResult> Search(int? searchBookID, string searchString, DateTime? searchBookingDate) //Added search method. Filters through customer ID, Start date or End date
        {
            //CREATE LIST OF Bookings (gets passed to drop down list) //(Co., 2022)
            ViewData["BookingList"] = new SelectList(_context.Bookings, "BookingId", "BookingId", searchBookID);

            ViewData["CurrentSearch"] = searchString;
            ViewData["CurrentBookingDate"] = searchBookingDate?.ToString("yyyy-MM-dd");

            //next we check if user entered a value to search.

            bool hasSearched = searchBookID.HasValue||!string.IsNullOrEmpty(searchString) || searchBookingDate.HasValue;

            if (!hasSearched)
            {
                return View(new List<Booking>());
            }

            //first step of returning required data we etch all of data from database //(Co., 2022).

            var bookings = _context.Bookings.Include(b => b.Event).AsQueryable();

            //did they select a booking or event?
            if (searchBookID.HasValue)
            {

                bookings = bookings.Where(b => b.BookingId == searchBookID.Value);


            }
            
            if (!string.IsNullOrEmpty(searchString))
            {
                var entry=searchString.ToLower();
                
                //using LINQ to search for a booking entry based on what was given ti be used as a search parameter (Troelsen and Japikse, 2022)

                bookings = bookings.Where(b => b.SpecialistName.ToLower().Contains(entry) ||  b.Event.EventName.ToLower().Contains(entry));


            }
            //next, booking date

            if (searchBookingDate.HasValue)
            {

                bookings = bookings.Where(b => b.BookingDate.Date == searchBookingDate.Value.Date);

            }
            return View(await bookings.ToListAsync());
        }
        private bool BookingExists(int id)
        {
            return _context.Bookings.Any(e => e.BookingId == id);
        }
    }
}
