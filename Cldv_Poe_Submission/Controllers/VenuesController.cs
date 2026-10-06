using Cldv_Poe_Submission.Models;
using Cldv_Poe_Submission.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata;
using System.Threading.Tasks;

namespace Cldv_Poe_Submission.Controllers
{
    public class VenuesController : Controller
    {
        private readonly EventEaseDBContext _context;
        private readonly BlobService _blob; //NAMING CONVENTON FOR SINGLETON

        //adding blob service call for our controller, ensures singleton is created when class is called
       
        public VenuesController(EventEaseDBContext context, BlobService blob)
        {
            _context = context;
            _blob = blob;
        }

        // GET: Venues
        public async Task<IActionResult> Index(bool? OnlyAvailable) //Added search method. Filters through customer ID, Start date or End date
        {
            //Pulling all of the venue entries saved into the venue table (gets passed to drop down list) //(Co., 2022)
            var venues = _context.Venues.AsQueryable();

            if (OnlyAvailable == true) //if the flag is true, select all entries where availability is true
            {
                venues = venues.Where(v => v.Availability); //using LINQ to search for all table entries where availabilitu is true (Troelsen and Japikse, 2022)
            }


            return View(await venues.ToListAsync());
        }

        // GET: Venues/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var venue = await _context.Venues
                .FirstOrDefaultAsync(m => m.VenueId == id);
            if (venue == null)
            {
                return NotFound();
            }

            return View(venue);
        }

        // GET: Venues/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Venues/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("VenueId,VenueName,VenueLocation,Availability,Capacity")] Venue venue, IFormFile imageFile)
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
                    return View(venue);
                }
                // wait for file to upload to the blob, then get url to where it lives (Co., 2022)
                string uploadedUrl = await _blob.UploadImageAsync(imageFile);
                venue.ImageUrl = uploadedUrl;

            }

            if (ModelState.IsValid)
            {
                _context.Add(venue);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(venue);
        }

        // GET: Venues/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var venue = await _context.Venues.FindAsync(id);
            if (venue == null)
            {
                return NotFound();
            }
            return View(venue);
        }

        // POST: Venues/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("VenueId,VenueName,VenueLocation,Availability,Capacity,ImageUrl")] Venue venue, IFormFile imageFile)
        {
            if (id != venue.VenueId)
            {
                return NotFound();
            }

            // if file is present and not empty, upload to blob storage (Co., 2022)
            if (imageFile != null && imageFile.Length > 0)
            {
                string[] acceptedfiles = { ".jpg", ".jpeg", ".png" }; //setting up an array of different filetype extensions (Troelsen and Japikse, 2022)

                string fileType = Path.GetExtension(imageFile.FileName).ToLower();//takes the uploaded file type of the uploaded document (dotnet-bot, 2026).
                                                                                  //set that document to lowercase

                if (!acceptedfiles.Contains(fileType)) //compare the document's file type with the rest of the accepted extensions
                {
                    TempData["ErrorMessage"] = "Only image files (.jpg, .jpeg,.png) are allowed"; //setting the error message
                    return View(venue);
                }
                // wait for file to upload to the blob, then get url to where it lives (Co., 2022)
                string uploadedUrl = await _blob.UploadImageAsync(imageFile);
                venue.ImageUrl = uploadedUrl;

            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(venue);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!VenueExists(venue.VenueId))
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
            return View(venue);
        }

        // GET: Venues/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            //checks the venues table for any events currently listed by specialists before deleting it.
            bool haslistedVenues = await _context.Bookings.AnyAsync(ms => ms.VenueId == id);
            //if there are venues, we return an error message
            if (haslistedVenues)
            {
                //write error message
                TempData["ErrorMessage"] = "You cannot delete a venue that has associated events."; //setting the error message
                //return error
                return RedirectToAction(nameof(Index));
            }

            var venue = await _context.Venues
                .FirstOrDefaultAsync(m => m.VenueId == id);
            if (venue == null)
            {
                return NotFound();
            }

            return View(venue);
        }

        // POST: Venues/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var venue = await _context.Venues.FindAsync(id);
            if (venue != null)
            {
                _context.Venues.Remove(venue);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        private bool VenueExists(int id)
        {
            return _context.Venues.Any(e => e.VenueId == id);
        }
    }
}
/*Reference List
 
Co., T. (2022). Cloud Computing Technology. Springer Nature.
 
 
 */