using CLDVPOEMVCEventEase.Models;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace CLDVPOEMVCEventEase.Controllers
{
    public class BookingController : Controller
    {


        private readonly ApplicationDbContext _context;


        public BookingController(ApplicationDbContext context)
        {
            _context = context;
        }
        //Reference Date Filter
        //According to Stack Overflow (2013), using LINQ to filter lists based on DateTime ranges is efficient and accurate when comparing against user inputted dates
        //I applied this in my booking controller to allow filtering bookings between two selected dates.
        public async Task<IActionResult> Index(DateTime? startDate, DateTime? endDate)
        {
            var bookingQuery = _context.Booking
                .Include(b => b.Venue)
                .Include(b => b.Event)
                .AsQueryable();

            
            if (startDate.HasValue)
            {
                
                bookingQuery = bookingQuery.Where(b => b.Bdate >= startDate.Value);
            }

           
            if (endDate.HasValue)
            {
                
                var nextDay = endDate.Value.Date.AddDays(1);
                bookingQuery = bookingQuery.Where(b => b.Bdate < nextDay);
            }

            
            var filtered = await bookingQuery.ToListAsync();

            
            ViewBag.StartDate = startDate?.ToString("yyyy-MM-dd");
            ViewBag.EndDate = endDate?.ToString("yyyy-MM-dd");

            return View(filtered);
        }
        public IActionResult Create()
        {
            ViewBag.Venue = _context.Venue.ToList();
            ViewBag.Event = _context.Event.ToList();
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Create(Booking booking)
        {
            
            ViewBag.Venue = _context.Venue.ToList();
            ViewBag.Event = _context.Event.ToList();
            if (ModelState.IsValid)
            {
                foreach (var state in ModelState)
                {
                    foreach (var err in state.Value.Errors)
                    {
                        Console.WriteLine($"{state.Key}: {err.ErrorMessage}");
                    }
                }
                return View(booking);   
            }

            bool bookingExists = await _context.Booking.AnyAsync(b =>
                    b.VenueID == booking.VenueID &&
                    b.Bdate == booking.Bdate);
            //Reference: Microsoft Learn.2024. Working with Data in ASP.NET Core – Entity Framework Core.
            // According to Microsoft Learn (2024), validation logic can be handled in controllers using LINQ to query the database context.
            // I used this method to prevent double bookings by checking if a booking with the same venue, event, and date already exists in the database.
            if (bookingExists)
            {
                ModelState.AddModelError("BookingError", "This Booking already exists");
                return View(booking);
            }

            try
            {
                _context.Add(booking);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            catch(DbUpdateException ex)
            {
                ModelState.AddModelError("", "This Booking already exists");
                return View(booking);
            }

        }
        public async Task<IActionResult> Details(int? id)
        {
            var booking = await _context.Booking
                .Include(i => i.Venue)
                .Include(i => i.Event)
                .FirstOrDefaultAsync(i => i.BookingID == id);
            if (booking == null)
            {
                return NotFound();
            }
            return View(booking);
        }
        public async Task<IActionResult> Delete(int? id)
        {
            var booking = await _context.Booking.FirstOrDefaultAsync(m => m.BookingID == id);
            if (booking == null)
            {
                return NotFound();
            }
            return View(booking);
        }

        [HttpPost]
        public async Task<IActionResult> Delete(int? id, Booking booking)
        {
            _context.Booking.Remove(booking);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool BookingExists(int id)
        {
            return _context.Booking.Any(e => e.BookingID == id);
        }
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var booking = await _context.Booking.FindAsync(id);
            if (booking == null)
            {
                return NotFound();
            }
            return View(booking);
        }
        [HttpPost]
        public async Task<IActionResult> Edit(int id, Booking booking)
        {
            if (id != booking.BookingID)
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
                    if (!BookingExists(booking.BookingID))
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
            return View(booking);
        }

        [HttpGet]
        public async Task<IActionResult> Search()
        {
            var view = await _context.Booking
                                     .Include(b => b.Venue)
                                     .Include(b => b.Event)
                                     .ToListAsync();
            return View(view);
        }

        [HttpPost]
        public async Task<IActionResult> Search(string searchTerm)
        {
            var bookingSearch = await _context.Booking
                .Include(i => i.Venue)
                .Include(i => i.Event)
                .ToListAsync();

            if (!string.IsNullOrWhiteSpace(searchTerm))
            {
                if (int.TryParse(searchTerm, out var id))
                {
                    bookingSearch = bookingSearch
                        .Where(b => b.BookingID == id)
                        .ToList();
                }
                else
                {
                    bookingSearch = bookingSearch
                        .Where(b => 
                                b.Event.EName.Contains(searchTerm,StringComparison.OrdinalIgnoreCase))
                        .ToList();
                }
            }

            return View(bookingSearch);
        }

    }
}

/*Microsoft. 2024. Working with Data in ASP.NET Core – Entity Framework Core. (Version 2.0) [Source code]. Available at: <https://learn.microsoft.com/en-us/ef/core/querying/> [Accessed 13 May 2025].Microsoft. 2024. Working with Data in ASP.NET Core – Entity Framework Core. (Version 2.0) [Source code]. Available at: <https://learn.microsoft.com/en-us/ef/core/querying/> [Accessed 13 May 2025].
 
Reece Waving. 2025. Getting started with MVC (Version 2.0) [Source code].
Available at:
<https://www.youtube.com/watch?v=eXGo2-nZnzk&list=PL480DYS-b_kevhFsiTpPIB2RzhKPig4iK&index=4>
[Accessed 16 March 2025].
 
Stack Overflow. 2013. Filter large list based on DateTime (Version 2.0) [Souce code].
Available at:<https://stackoverflow.com/questions/18375752/filter-large-list-based-on-date-time>
[Accessed 22 June 2025].
 */