using System;
using System.Collections.Generic;
using System.Linq;
using BlazorApp1.Data;
using BlazorApp1.Models;
using Microsoft.EntityFrameworkCore;

namespace BlazorApp1.Services
{
    public class TicketState
    {
        private readonly IDbContextFactory<ApplicationDbContext> _factory;

        public event Action? OnChange;
        private void Notify() => OnChange?.Invoke();

        public TicketState(IDbContextFactory<ApplicationDbContext> factory)
        {
            _factory = factory;
        }

        // Get all tickets
        public IEnumerable<Ticket> GetAll()
        {
            using var context = _factory.CreateDbContext();
            return context.Tickets.OrderByDescending(t => t.CreatedAt).ToList();
        }

        // Get tickets for a specific user
        public IEnumerable<Ticket> GetByUser(string userId)
        {
            using var context = _factory.CreateDbContext();
            return context.Tickets
                          .Where(t => t.UserId == userId).OrderBy(t => t.StatusId)
                          .ThenByDescending(t => t.CreatedAt)
                          .ToList();
        }

        // Get ticket by ID
        public Ticket? GetById(int id)
        {
            using var context = _factory.CreateDbContext();
            return context.Tickets.FirstOrDefault(t => t.TicketId == id);

        }

        // Create a new ticket
        public Ticket Create(Ticket t)
        {
            t.CreatedAt = DateTime.UtcNow;

            using var context = _factory.CreateDbContext();
            context.Tickets.Add(t);
            context.SaveChanges();
            Notify();

            return t;
        }
        public void Delete(int id)
        {
            using var context = _factory.CreateDbContext();

            var ticket = context.Tickets.FirstOrDefault(t => t.TicketId == id);
            if (ticket == null) return;

            context.Tickets.Remove(ticket);
            context.SaveChanges();

            Notify();
        }


        // Update an existing ticket
        public void Update(Ticket t)
        {
            t.UpdatedAt = DateTime.UtcNow;

            using var context = _factory.CreateDbContext();
            var existing = context.Tickets.FirstOrDefault(x => x.TicketId == t.TicketId);
            if (existing == null) return;

            existing.Title = t.Title;
            existing.Description = t.Description;
            existing.StatusId = t.StatusId;
            existing.EmployeeId = t.EmployeeId;
            existing.UpdatedAt = t.UpdatedAt;

            context.SaveChanges();
            Notify();
        }

        // Change ticket status
        public void ChangeStatus(int ticketId, int statusId)
        {
            using var context = _factory.CreateDbContext();
            var t = context.Tickets.FirstOrDefault(x => x.TicketId == ticketId);
            if (t == null) return;

            t.StatusId = statusId;
            if (statusId == 3) // Closed
                t.ClosedAt = DateTime.UtcNow;

            context.SaveChanges();
            Notify();
        }

        // Assign employee by string ID
        public void AssignEmployee(int ticketId, string employeeId)
        {
            using var context = _factory.CreateDbContext();
            var t = context.Tickets.FirstOrDefault(x => x.TicketId == ticketId);
            if (t == null) return;

            t.EmployeeId = employeeId;
            context.SaveChanges();
            Notify();
        }
        public IEnumerable<Ticket> GetByUserOrEmployee(string userId)
        {
            using var context = _factory.CreateDbContext();
            return context.Tickets
                          .Where(t => t.UserId == userId || t.EmployeeId == userId)
                          .OrderByDescending(t => t.CreatedAt)
                          .ToList();
        }

    }
}
