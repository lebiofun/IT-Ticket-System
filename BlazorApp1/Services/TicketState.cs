using System;
using System.Collections.Generic;
using System.Linq;
using TicketApp.Models;


namespace TicketApp.Services
{
    public class TicketState
    {
        private readonly List<Ticket> _tickets = new();
        private int _nextId = 1;

        public int CurrentUserId { get; set; } = 1;
        public event Action? OnChange;
        private void Notify() => OnChange?.Invoke();

        public IEnumerable<Ticket> GetAll() => _tickets.OrderByDescending(t => t.CreatedAt);
        public IEnumerable<Ticket> GetByUser(int userId) => _tickets.Where(t => t.UserId == userId);
        public Ticket? GetById(int id) => _tickets.FirstOrDefault(t => t.TicketId == id);

        public Ticket Create(Ticket t)
        {
            t.TicketId = _nextId++;
            t.CreatedAt = DateTime.UtcNow;
            _tickets.Add(t);
            Notify();
            return t;
        }


        public void Update(Ticket t)
        {
            var ex = GetById(t.TicketId);
            if (ex == null) return;
            ex.Title = t.Title;
            ex.Description = t.Description;
            ex.UpdatedAt = DateTime.UtcNow;
            ex.StatusId = t.StatusId;
            ex.EmployeeId = t.EmployeeId;
            Notify();
        }


        public void ChangeStatus(int ticketId, int statusId)
        {
            var t = GetById(ticketId);
            if (t == null) return;
            t.StatusId = statusId;
            if (statusId == 3) // example: 3 = Closed
            t.ClosedAt = DateTime.UtcNow;
            Notify();
        }
        public void AssignEmployee(int ticketId, int employeeId)
        {
            var t = GetById(ticketId);
            if (t == null) return;
            t.EmployeeId = employeeId;
            Notify();
        }
        public void SeedSample()
        {
            if (_tickets.Any()) return;
            Create(new Ticket { Title = "Sample ticket 1", Description = "Sample description", UserId = 1, StatusId = 1 });
            Create(new Ticket { Title = "Network issue", Description = "Can't connect to VPN.", UserId = 2, StatusId = 2 });
        }
    }
}