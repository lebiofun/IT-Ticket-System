using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ticket_System.core._Models
{
    public class ALog
    {
        public int Id { get; set; }
        public int TicketId { get; set; }
        public int UserId { get; set; }

        public int OldStatusId { get; set; }
        public int NewStatusId { get; set; }
        public DateTime ChangedAt { get; set; }
    }
}
