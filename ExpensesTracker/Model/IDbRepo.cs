using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ExpensesTracker.Model
{
    public interface IAuditRepo
    {
        void AddAuditRecord(Audit aud);
    }
}
