using BlazorApp1.Data;
using BlazorApp1.Models;
using Microsoft.EntityFrameworkCore;

namespace BlazorApp1.Services;

public class ChatState
{
    private readonly IDbContextFactory<ApplicationDbContext> _factory;
    public event Action? OnChange;
    private void Notify() => OnChange?.Invoke();

    public ChatState(IDbContextFactory<ApplicationDbContext> factory)
    {
        _factory = factory;
    }
    public List<ChatMessage> GetMessages(int ticketId)
    {
        using var context = _factory.CreateDbContext();
        return context.ChatMessages.Where(m => m.TicketId == ticketId).OrderBy(m => m.SentAt).ToList();
    }

    public void AddMessage(int ticketId, string senderId, string text)
    {
        using var context = _factory.CreateDbContext();

        var msg = new ChatMessage
        {
            TicketId = ticketId,
            SenderId = senderId,
            Text = text,
            SentAt = DateTime.UtcNow
        };

        context.ChatMessages.Add(msg);
        context.SaveChanges();
        Notify();
    }
}