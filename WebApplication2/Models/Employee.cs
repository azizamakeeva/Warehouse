using System;
using System.Collections.Generic;

#nullable disable

namespace WebApplication2.Models
{
    public partial class Employee 
    {
        public Employee()
        {
            ProductSales = new HashSet<ProductSale>();
            Productions = new HashSet<Production>();
            RawMaterialPurchases = new HashSet<RawMaterialPurchase>();
        }

        public int Id { get; set; }
        public string Name { get; set; }
        public int? Positions { get; set; }
        public int? Salary { get; set; }
        public string Address { get; set; }
        public int? Phone { get; set; }

        public virtual Position PositionsNavigation { get; set; }
        public virtual ICollection<ProductSale> ProductSales { get; set; }
        public virtual ICollection<Production> Productions { get; set; }
        public virtual ICollection<RawMaterialPurchase> RawMaterialPurchases { get; set; }
    }
}
