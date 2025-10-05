using static TicketingService.EntityFramework.Enumerations.Enums;

namespace TicketingService.DTOs
{
    public class TicketCreateDto
    {
        public string Title { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public TicketPriority Priority { get; set; }
    }
}
