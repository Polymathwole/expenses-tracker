using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ExpensesTracker.Model
{
    public class AuditRepository : IAuditRepo
    {
        private AppDbContext dbContext;

        public AuditRepository(AppDbContext dbC)
        {
            dbContext = dbC;
        }

        public void AddAuditRecord(Audit a)
        {
            dbContext.Audit.Add(a);
            dbContext.SaveChanges();
        }
    }
}
