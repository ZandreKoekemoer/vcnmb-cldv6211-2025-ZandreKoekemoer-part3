using CLDVPOEMVCEventEase.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CLDVPOEMVCEventEase.Controllers
{
    public class SearchController : Controller
    {
        private readonly ApplicationDbContext _context;
        public SearchController(ApplicationDbContext context)
        {
            _context = context;
        }
        public IActionResult Index()
        {
            return View(new List<Booking>());
        }
        
           //Reference Search Functionality
           // According to Microsoft (2025), this controller allows basic search functionality.
           // I used this method because i was struggling with creating a search functionality
        [HttpPost]
        public async Task<IActionResult> Index(string searchTerm)
        {
            var booking = await _context.Booking
                .Where(i =>
                        i.BookingID.ToString().Contains(searchTerm) ||
                        i.Event.EName.Contains(searchTerm))
                .Include(i => i.Venue)
                .Include(i => i.Event)
                .ToListAsync();
            return View(booking);
        }
    }
}

/*References

Microsoft, 2025. Part 7, add search to an ASP.NET Core MVC app.
(Version 2.0) [Source Code]
https://learn.microsoft.com/en-us/aspnet/core/tutorials/first-mvc-app/search?view=aspnetcore-9.0 
[Accessed 13 May 2025]

*/