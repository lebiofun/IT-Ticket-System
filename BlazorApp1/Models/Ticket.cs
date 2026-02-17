namespace BlazorApp1.Models;

public class Ticket
{
    public int TicketId { get; set; }

    public string Title { get; set; } = string.Empty;

    public string Description { get; set; } = string.Empty;

    public int? StatusId { get; set; }

    public string? UserId { get; set; }

    public string? EmployeeId { get; set; }

    public string Department { get; set; } = string.Empty;

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    public DateTime? UpdatedAt { get; set; }

    public DateTime? ClosedAt { get; set; }
}
