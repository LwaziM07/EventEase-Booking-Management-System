using System.ComponentModel.DataAnnotations;

namespace Cldv_Poe_Submission.Models
{
    public class EventTypes
    {
        public int EventTypeID { get; set; }
        
        [Display(Name = "Event Type")] //making use of name attributes to format the text shown to the user (dotnet-bot, 2026)
        public string EventType { get; set; }

    }
}
/*Reference list
 
dotnet-bot.2026. DisplayAttribute.Name Property (System.componentModel.DataAnnotations). [source code] Microsoft.com. Available at: <https://learn.microsoft.com/en-us/dotnet/api/system.componentmodel.dataannotations.displayattribute.name?view=net-10.0> [Accessed 8 Apr. 2026].
 
 */