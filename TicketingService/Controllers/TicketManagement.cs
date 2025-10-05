using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using TicketingService.DTOs;
using TicketingService.EntityFramework;
using TicketingService.EntityFramework.Models;
using static TicketingService.EntityFramework.Enumerations.Enums;

namespace TicketingService.Controllers
{
    [Route("tickets")]
    [ApiController]
    public class TicketManagement : ControllerBase
    {
        private readonly AppDbContext _context;

        public TicketManagement(AppDbContext context)
        {
            _context = context;
        }
        private Guid GetCurrentUserId()
        {
            var userIdClaim = User.FindFirstValue(ClaimTypes.NameIdentifier);
            return userIdClaim != null ? Guid.Parse(userIdClaim) : Guid.Empty;
        }

        [Authorize(Roles = "Employee")]
        [HttpPost]
        public async Task<IActionResult> CreateTicket(TicketCreateDto dto)
        {
            var userId = GetCurrentUserId();

            var ticket = new Ticket
            {
                Title = dto.Title,
                Description = dto.Description,
                Status = TicketStatus.Open,
                Priority = dto.Priority,
                CreatedByUserId = userId,
                CreatedAt = DateTime.Now,
                UpdatedAt = DateTime.Now
            };

            _context.Tickets.Add(ticket);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetTicketById), new { id = ticket.Id }, ticket);
        }

        [Authorize(Roles = "Employee")]
        [HttpGet("my")]
        public async Task<IActionResult> GetMine()
        {
            var userId = GetCurrentUserId();

            var tickets = await _context.Tickets
                .Where(t => t.CreatedByUserId == userId)
                .ToListAsync();

            return Ok(tickets);
        }

        [Authorize(Roles = "Admin")]
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var tickets = await _context.Tickets
               .Include(t => t.CreatedByUser)
               .Include(t => t.AssignedToUser)
               .ToListAsync();

            return Ok(tickets);
        }

        [Authorize(Roles = "Admin")]
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateTicket(Guid id, TicketUpdateDto dto)
        {
            var ticket = await _context.Tickets.FindAsync(id);
            if (ticket == null)
                return NotFound();

            if (dto.AssignedToUserId != null)
            {
                var assignedUser = await _context.Users.FindAsync(dto.AssignedToUserId);
                if (assignedUser == null)
                    return BadRequest("Assigned user does not exist.");

                if (assignedUser.Role != Role.Admin)
                    return BadRequest("Assigned user must be an Admin.");
            }

            ticket.Status = dto.Status;
            ticket.Priority = dto.Priority;
            ticket.AssignedToUserId = dto.AssignedToUserId;
            ticket.UpdatedAt = DateTime.UtcNow;

            await _context.SaveChangesAsync();

            return Ok(ticket);
        }

        [Authorize(Roles = "Admin")]
        [HttpGet("stats")]
        public async Task<IActionResult> GetStats()
        {
            var stats = await _context.Tickets
               .GroupBy(t => t.Status)
               .Select(g => new { Status = g.Key, Count = g.Count() })
               .ToListAsync();

            return Ok(stats);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetTicketById(Guid id)
        {
            var ticket = await _context.Tickets
               .Include(t => t.CreatedByUser)
               .Include(t => t.AssignedToUser)
               .FirstOrDefaultAsync(t => t.Id == id);

            if (ticket == null) return NotFound();

            var currentUserId = GetCurrentUserId();
            var userRole = User.FindFirstValue(ClaimTypes.Role);

            if (ticket.CreatedByUserId != currentUserId &&
                ticket.AssignedToUserId != currentUserId)
            {
                return Forbid();
            }

            return Ok(ticket);
        }

        [Authorize(Roles = "Admin")]
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteTicket(Guid id)
        {
            var ticket = await _context.Tickets.FindAsync(id);
            if (ticket == null) return NotFound();

            _context.Tickets.Remove(ticket);
            await _context.SaveChangesAsync();
            return NoContent();
        }
    }
}
