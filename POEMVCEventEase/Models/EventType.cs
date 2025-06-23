using System.ComponentModel.DataAnnotations;

namespace CLDVPOEMVCEventEase.Models
{
    public class EventType
    {
        [Key] public int ID { get; set; }
        [Required] public string? EType { get; set; }
    }
}
