using System.ComponentModel.DataAnnotations;

namespace CLDVPOEMVCEventEase.Models
{
    public class Booking
    {
        [Key]
        public int BookingID { get; set; }

        public DateTime Bdate { get; set; }
        public int VenueID  { get; set; }
        public required Venue Venue { get; set; }
        public int EventID { get; set; }
        public required Event Event { get; set; }
    }
}


