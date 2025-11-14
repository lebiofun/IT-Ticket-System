using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ticket_System.core._Models
{
    public class Ticket
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public int CategoryId { get; set; }
        public int StatusId { get; set; }
        public int? AssignedEmployeeId { get; set; }

        public string Title { get; set; }
        public string Description { get; set; }

        public DateTime CreatedAt { get; set; }
        public DateTime? ClosedAt { get; set; }
    }
}
