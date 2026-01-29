using BlazorApp1.Models;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace BlazorApp1.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options)
            : base(options) { }

        public DbSet<Ticket> Tickets { get; set; }
    }
}
