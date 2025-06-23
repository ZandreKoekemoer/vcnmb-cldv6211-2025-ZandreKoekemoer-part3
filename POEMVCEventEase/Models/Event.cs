using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CLDVPOEMVCEventEase.Models
{
    public class Event
    {
        [Key]
        public int EventID { get; set; }

        public string EName { get; set; }
        public DateTime EDate { get; set; }
        public string EDescription { get; set; }
        public string? URL { get; set; }
        [Required] 
        public int ID { get; set; }

        
        [ForeignKey("ID")]
        public EventType? EventType { get; set; }

        public List<Booking> Booking { get; set; } = new();
        
    }
}

