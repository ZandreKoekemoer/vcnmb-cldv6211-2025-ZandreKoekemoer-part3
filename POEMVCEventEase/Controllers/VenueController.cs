using CLDVPOEMVCEventEase.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CLDVPOEMVCEventEase.Controllers
{
    public class VenueController : Controller
    {
        private readonly ApplicationDbContext _context;
        public VenueController(ApplicationDbContext context)
        {
            _context = context;
        }
        // Reference Display Availability
        //According to Stack Overflow (2011), a boolean value like Available can be formatted to display "Yes" or "No" for better user readability.
        //I used this method because I was struggling to aply a Yes no availability filter for Venue
        public async Task<IActionResult> Index(bool? onlyAvailable)
        {
            var venueA = _context.Venue.AsQueryable();

            
            if (onlyAvailable == true)
            {
                venueA = venueA.Where(v => v.Available);
            }

            var venues = await venueA.ToListAsync();

            
            ViewBag.OnlyAvailable = onlyAvailable == true;
            return View(venues);
        }
        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Create(Venue venue)
        {
            if (ModelState.IsValid)
            {
                _context.Add(venue);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(venue);
        }
        public async Task<IActionResult> Details(int? id)
        {
            var venue = await _context.Venue.FirstOrDefaultAsync(m => m.VenueID == id);
            if (venue == null)
            {
                return NotFound();
            }
            return View(venue);
        }
        public async Task<IActionResult> Delete(int? id)
        {
            var venue = await _context.Venue.FirstOrDefaultAsync(m => m.VenueID == id);
            if (venue == null)
            {
                return NotFound();
            }
            return View(venue);
        }

        [HttpPost]
        public async Task<IActionResult> Delete(int? id, Venue venue)
        {
            _context.Venue.Remove(venue);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool VenueExists(int id)
        {
            return _context.Venue.Any(e => e.VenueID == id);
        }
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var venue = await _context.Venue.FindAsync(id);
            if (venue == null)
            {
                return NotFound();
            }
            return View(venue);
        }
        [HttpPost]
        public async Task<IActionResult> Edit(int id, Venue venue)
        {
            if (id != venue.VenueID)
            {
                return NotFound();
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
                    if (!VenueExists(venue.VenueID))
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
    }
}

/*
Stack Overflow. 2011. How to generically format a boolean to a Yes/No string (Version 2.0) [Source Code].
Available at:< https://stackoverflow.com/questions/5632920/how-to-generically-format-a-boolean-to-a-yes-no-string>
[Accessed 22 June 2025].

Reece Waving. 2025. Getting started with MVC (Version 2.0) [Source code].
Available at:
<https://www.youtube.com/watch?v=eXGo2-nZnzk&list=PL480DYS-b_kevhFsiTpPIB2RzhKPig4iK&index=4>
[Accessed 16 March 2025]. 
*/

