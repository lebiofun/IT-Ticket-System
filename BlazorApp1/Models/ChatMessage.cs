using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BlazorApp1.Models;

public class ChatMessage
{
    public int Id { get; set; }
    public int TicketId { get; set; }
    [Required]
    public string SenderId { get; set; } = string.Empty;
    [Required]
    public string Text { get; set; } = string.Empty;
    public DateTime SentAt { get; set; } = DateTime.UtcNow;
}