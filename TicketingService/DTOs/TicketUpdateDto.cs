using static TicketingService.EntityFramework.Enumerations.Enums;

namespace TicketingService.DTOs
{
    public class TicketUpdateDto
    {
        public TicketStatus Status { get; set; }
        public TicketPriority Priority { get; set; }
        public Guid? AssignedToUserId { get; set; }
    }
}
