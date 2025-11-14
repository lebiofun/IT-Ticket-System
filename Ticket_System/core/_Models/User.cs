using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ticket_System.core._Models
{
    public class User
    {
        public int Id { get; set; }
        public string Login { get; set; }
        public string FullName { get; set; }
        public string Department { get; set; }
        public string Role { get; set; } // EndUser, TechSupport, SupportManager
    }
}
