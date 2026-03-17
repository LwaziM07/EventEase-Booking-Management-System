using System.ComponentModel.DataAnnotations;

namespace Cldv_Poe_Submission.Models
{
    public class SummaryModel
    {
        [Display(Name = "Specialist Name")]
        public string SpecialistName { get; set; }

        [Display(Name = "Event Name")]
        public string EventName { get; set; }
       
        [Display(Name = "Venue Name")]
        public string VenueName { get; set; }
        
        [Display(Name = "Booking Date")]
        public DateTime BookingDate { get; set; }

    }
}
