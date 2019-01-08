using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ExpensesTracker.Model
{
    public class Audit
    {
        public int Id { get; set; }
        public string UserName { get; set; }
        public string Event { get; set; }
        public string UserIP { get; set; }
        public DateTime TimeStamp { get; set; }
    }

    public enum Action
    {
        Create,
        Edit,
        Delete,
        Login,
        Logout
    }
}
