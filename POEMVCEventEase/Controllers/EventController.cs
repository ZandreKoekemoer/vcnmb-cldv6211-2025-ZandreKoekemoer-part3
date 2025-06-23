using CLDVPOEMVCEventEase.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CLDVPOEMVCEventEase.Controllers
{
    public class EventController : Controller
    {
        private readonly ApplicationDbContext _context;
        public EventController(ApplicationDbContext context)
        {
            _context = context;
        }
        // Reference Filter by Event Type
        // According to Patrick God (2023), filtering records using a dropdown selection and LINQ queries in the controller allows for clean separation of concerns and dynamic query buildin
        // I applied this approach to filter events by type, as I found it easier to handle conditional filtering logic in the controller instead of the view.
        public async Task<IActionResult> Index(string selectedType)
        {
            var types = await _context.EventType.ToListAsync();
            ViewBag.Types = types;

            
            var events = _context.Event.Include(e => e.EventType).AsQueryable();

            if (!string.IsNullOrEmpty(selectedType))
            {
                events = events.Where(e => e.EventType.EType == selectedType);
            }

            return View(await events.ToListAsync());
        }
        public async Task<IActionResult> Create()
        {
            ViewBag.EventTypes = await _context.EventType.ToListAsync();
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Create(Event eventC)
        {
            if (ModelState.IsValid)
            {
                _context.Add(eventC);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(eventC);
        }
        public async Task<IActionResult> Details(int? id)
        {
            var eventC = await _context.Event.FirstOrDefaultAsync(m => m.EventID == id);
            if (eventC == null)
            {
                return NotFound();
            }
            return View(eventC);
        }

        public async Task<IActionResult> Delete(int? id)
        {
            var eventC = await _context.Event.FirstOrDefaultAsync(m => m.EventID == id);
            if (eventC == null)
            {
                return NotFound();
            }
            return View(eventC);
        }

        [HttpPost]
        public async Task<IActionResult> Delete(int? id, Event eventC)
        {
            _context.Event.Remove(eventC);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool EventExists(int id)
        {
            return _context.Event.Any(e => e.EventID == id);
        }
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return NotFound();

            var eventC = await _context.Event.FindAsync(id);
            if (eventC == null) return NotFound();

            ViewBag.EventTypes = await _context.EventType.ToListAsync();
            return View(eventC);
        }
        [HttpPost]
        public async Task<IActionResult> Edit(int id, Event eventC)
        {
            if(id != eventC.EventID)
            {
                return NotFound();
            }

            if(ModelState.IsValid)
            {
                try
                {
                    _context.Update(eventC);
                    await _context.SaveChangesAsync();
                }
                catch(DbUpdateConcurrencyException)
                {
                    if(!EventExists(eventC.EventID))
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
            return View(eventC);
        }
    }
}
/*
/*
Patrick God. 2023. ASP.NET Core MVC Tutorial: Filtering Data in Views (Version 2.0) [Source Code].
Available at:< https://www.youtube.com/watch?v=8mj1qShv9GI>
[Accessed 20 June 2025]. 

Reece Waving. 2025. Getting started with MVC (Version 2.0) [Source code].
Available at:
<https://www.youtube.com/watch?v=eXGo2-nZnzk&list=PL480DYS-b_kevhFsiTpPIB2RzhKPig4iK&index=4>
[Accessed 16 March 2025]. 
*/

