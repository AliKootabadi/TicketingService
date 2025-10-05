namespace TicketingService.EntityFramework.Enumerations
{
    public class Enums
    {
        public enum Role { Employee, Admin }
        public enum TicketStatus { Open, InProgress, Closed }
        public enum TicketPriority { Low, Medium, High }
    }
}
