using System;
using System.Collections.Generic;

#nullable disable

namespace WebApplication2.Models
{
    public partial class Budget
    {
        public int Id { get; set; }
        public int BudgetAmount { get; set; }
        public int BudgetPercent { get; set; }
    }
}
