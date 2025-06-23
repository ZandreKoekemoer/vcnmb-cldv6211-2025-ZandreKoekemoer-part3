using Microsoft.EntityFrameworkCore;
using CLDVPOEMVCEventEase.Models;

namespace CLDVPOEMVCEventEase.Models
{
    public class ApplicationDbContext :DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) 
        { 
        }
        public DbSet<Venue> Venue { get; set; }
        public DbSet<Event> Event { get; set; }
        public DbSet<Booking> Booking { get; set; }
        public DbSet<EventType> EventType { get; set; }
    }
}
/*
Reece Waving. 2025. Getting started with MVC (Version 2.0) [Source code].
Available at:
<https://www.youtube.com/watch?v=FaAVkUH2qSw&list=PL480DYS-b_kevhFsiTpPIB2RzhKPig4iK&index=3>
[Accessed 16 March 2025]. 
*/