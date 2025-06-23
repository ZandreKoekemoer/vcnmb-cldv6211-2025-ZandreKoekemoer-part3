using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CLDVPOEMVCEventEase.Models
{
    public class Venue
    {
        [Key]
        public int VenueID { get; set; }

        public string VName { get; set; }
        public string VLocation { get; set; }
        public int Capacity { get; set; }
        public bool Available { get; set; }
        public string? URL { get; set; }
        public List<Booking> Booking { get; set; } = new();
    }
}

