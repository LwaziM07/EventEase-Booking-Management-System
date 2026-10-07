using Cldv_Poe_Submission.Models;
using Cldv_Poe_Submission.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Cldv_Poe_Submission.Controllers
{
    public class EventsController : Controller
    {
        private readonly EventEaseDBContext _context;
        private readonly BlobService _blob; //NAMING CONVENTON FOR SINGLETON
        public EventsController(EventEaseDBContext context, BlobService blob)
        {
            _context = context;
            _blob = blob;
        }

        // GET: Events
        public async Task<IActionResult> Index(int? eventTypeID, DateTime? startDate, DateTime? endDate) //Added search method. Filters through customer ID, Start date or End date
        {
            //Pulling all of the event entries saved into the event table (gets passed to drop down list) //(Co., 2022)
            var events = _context.Events.Include(e => e.EventType).AsQueryable();

            if (eventTypeID.HasValue) //checks if there's an event type to select from using LINQ (Troelsen and Japikse, 2022)
            {
                events = events.Where(e => e.EventTypeId == eventTypeID);
            }

            //checks the the user's start date against the ones saved in the table.
            if (startDate.HasValue && endDate.HasValue)
            {
                events = events.Where(e => e.StartDate <= endDate && e.EndDate >= startDate);
            }

            ViewData["EventTypeID"] = new SelectList(_context.EventTypes, "EventTypeId", "EventType1");
            return View(await events.ToListAsync());

        }

        // GET: Events/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var @event = await _context.Events
               .Include(e => e.EventType)          
               .FirstOrDefaultAsync(m => m.EventId == id);
            if (@event == null)
            {
                return NotFound();
            }

            return View(@event);
        }

        // GET: Events/Create
        public IActionResult Create()
        {
            ViewData["EventTypeID"] = new SelectList(_context.EventTypes, "EventTypeId", "EventType1");
            return View();
        }

        // POST: Events/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("EventId,EventName,StartDate,EndDate,EventDescription,EventTypeID")] Event @event, IFormFile imageFile)
        {
            // if file is present and not empty, upload to blob storage (Co., 2022)
            if (imageFile != null && imageFile.Length > 0)
            {
                string[] acceptedfiles = { ".jpg", ".jpeg", ".png" }; //setting up an array of different filetype extensions (Troelsen and Japikse, 2022)

                string fileType = Path.GetExtension(imageFile.FileName).ToLower();//takes the uploaded file type of the uploaded document (dotnet-bot, 2026).
                                                                                  //set that document to lowercase

                if (!acceptedfiles.Contains(fileType)) //compare the document's file type with the rest of the accepted extensions
                {
                    TempData["ErrorMessage"] = "Only image files (.jpg, .jpeg,.png) are allowed"; //setting the error message
                    return View(@event);
                }
                // wait for file to upload to the blob, then get url to where it lives
                string uploadedUrl = await _blob.UploadImageAsync(imageFile);
                @event.EventImageUrl = uploadedUrl;

            }

            ViewData["EventTypeID"] = new SelectList(_context.EventTypes, "EventTypeId", "EventType1", @event.EventTypeId);

            if (ModelState.IsValid)
            {
                _context.Add(@event);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(@event);
        }

        // GET: Events/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var @event = await _context.Events.FindAsync(id);
            if (@event == null)
            {
                return NotFound();
            }
            ViewData["EventTypeID"] = new SelectList(_context.EventTypes, "EventTypeId", "EventType1", @event.EventTypeId); return View(@event);
        }

        // POST: Events/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("EventId,EventName,StartDate,EndDate,EventDescription,EventTypeID")] Event @event, IFormFile imageFile)
        {
            if (id != @event.EventId)
            {
                return NotFound();
            }
             // if file is present and not empty, upload to blob storage (Co., 2022)
            if (imageFile != null && imageFile.Length > 0)
            {
                string[] acceptedfiles = { ".jpg", ".jpeg", ".png" };//setting up an array of different filetype extensions

                string fileType = Path.GetExtension(imageFile.FileName).ToLower(); //takes the uploaded file type of the uploaded document (dotnet-bot, 2026).
                                                                                   //set that document to lowercase

                if (!acceptedfiles.Contains(fileType))//compare the document's file type with the rest of the accepted extensions
                {
                    TempData["ErrorMessage"] = "Only image files (.jpg, .jpeg,.png) are allowed"; //setting the error message if the extension does not match
                    return View(@event);
                }
                // wait for file to upload to the blob, then get url to where it lives
                string uploadedUrl = await _blob.UploadImageAsync(imageFile);
                @event.EventImageUrl = uploadedUrl;

            }
        
           

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(@event);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!EventExists(@event.EventId))
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
            return View(@event);
        }

        // GET: Events/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            //checks the moduleStudent table for any events currently listed by specialists before deleting it.
            bool hasListedEvents = await _context.Bookings.AnyAsync(ms => ms.EventId == id);
            //if there are events, we return an error message
            if (hasListedEvents)
            {
                //write error message
                TempData["ErrorMessage"] = "You cannot delete an event that has associated venues.";
                //return error
                return RedirectToAction(nameof(Index));
            }

            var @event = await _context.Events
               .Include(e => e.EventType)
               .FirstOrDefaultAsync(m => m.EventId == id);
            if (@event == null)
            {
                return NotFound();
            }

            return View(@event);
        }

        // POST: Events/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var @event = await _context.Events.FindAsync(id);
            if (@event != null)
            {
                _context.Events.Remove(@event);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        private bool EventExists(int id)
        {
            return _context.Events.Any(e => e.EventId == id);
        }
    }
}
