using System;
using System.Collections.Generic;

#nullable disable

namespace WebApplication2.Models
{
    public partial class Production
    {
        public int Id { get; set; }
        public int? Product { get; set; }
        public int? Count { get; set; }
        public DateTime? Date { get; set; }
        public int? Employee { get; set; }

        public virtual Employee EmployeeNavigation { get; set; }
        public virtual FinishedProduct ProductNavigation { get; set; }
    }
}
