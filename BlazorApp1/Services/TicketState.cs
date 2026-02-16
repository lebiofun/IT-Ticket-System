using BlazorApp1.Models;
using BlazorApp1.Data;
using Microsoft.EntityFrameworkCore;

namespace BlazorApp1.Services
{
    public class TicketState
    {
        private readonly ApplicationDbContext _context;

        public event Action? OnChange;
        private void Notify() => OnChange?.Invoke();

        public TicketState(ApplicationDbContext context)
        {
            _context = context;
        }

        public IEnumerable<Ticket> GetAll() =>
            _context.Tickets
                .OrderByDescending(t => t.CreatedAt)
                .ToList();

        public IEnumerable<Ticket> GetByUser(string userId) =>
            _context.Tickets
                .Where(t => t.UserId == userId)
                .ToList();

        public Ticket? GetById(int id) =>
            _context.Tickets.FirstOrDefault(t => t.TicketId == id);

        public Ticket Create(Ticket t)
        {
            t.CreatedAt = DateTime.UtcNow;
            _context.Tickets.Add(t);
            _context.SaveChanges();
            Notify();
            return t;
        }

        public void Update(Ticket t)
        {
            t.UpdatedAt = DateTime.UtcNow;
            _context.Tickets.Update(t);
            _context.SaveChanges();
            Notify();
        }

        public void ChangeStatus(int ticketId, int statusId)
        {
            var t = GetById(ticketId);
            if (t == null) return;

            t.StatusId = statusId;
            if (statusId == 3)
                t.ClosedAt = DateTime.UtcNow;

            _context.SaveChanges();
            Notify();
        }

        public void AssignEmployee(int ticketId, int employeeId)
        {
            var t = GetById(ticketId);
            if (t == null) return;

            t.EmployeeId = employeeId;
            _context.SaveChanges();
            Notify();
        }
    }
}
