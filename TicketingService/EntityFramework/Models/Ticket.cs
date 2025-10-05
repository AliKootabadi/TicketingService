using static TicketingService.EntityFramework.Enumerations.Enums;

namespace TicketingService.EntityFramework.Models
{
    public class Ticket
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public string Title { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public TicketStatus Status { get; set; } = TicketStatus.Closed;
        public TicketPriority Priority { get; set; } = TicketPriority.Low;
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        public Guid CreatedByUserId { get; set; }
        public User CreatedByUser { get; set; } = null!;
        public Guid? AssignedToUserId { get; set; }
        public User? AssignedToUser { get; set; }
    }
}
