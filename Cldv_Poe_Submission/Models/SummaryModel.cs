using System.ComponentModel.DataAnnotations;

namespace Cldv_Poe_Submission.Models
{
    public class SummaryModel //Setting up a model that the
                              //HomeController can use to make a summary table 
    {
        [Display(Name = "Specialist Name")] //making use of name attributes to format the text shown to the user (dotnet-bot, 2026)
        public string SpecialistName { get; set; }

        [Display(Name = "Event Name")]
        public string EventName { get; set; }

        [Display(Name = "Venue Name")]
        public string VenueName { get; set; }

        [Display(Name = "Booking Date")]
        public DateTime BookingDate { get; set; }

    }
    /*Reference List
      
    dotnet-bot .2026. DisplayAttribute.Name Property (System.componentModel.DataAnnotations). [source code] Microsoft.com. Available at: <https://learn.microsoft.com/en-us/dotnet/api/system.componentmodel.dataannotations.displayattribute.name?view=net-10.0> [Accessed 8 Apr. 2026].
      
    */
}
