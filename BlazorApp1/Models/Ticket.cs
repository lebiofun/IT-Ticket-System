using System;


namespace TicketApp.Models
{
public class Ticket
{
public int TicketId { get; set; }
public string Title { get; set; } = string.Empty;
public string Description { get; set; } = string.Empty;
public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
public DateTime? UpdatedAt { get; set; }
public DateTime? ClosedAt { get; set; }
public int? UserId { get; set; }
public int? EmployeeId { get; set; }
public int? CategoryId { get; set; }
public int? StatusId { get; set; }
}
}