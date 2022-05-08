using System;
using System.Collections.Generic;

#nullable disable

namespace WebApplication2.Models
{
    public partial class FinishedProduct
    {
        public FinishedProduct()
        {
            Ingridients = new HashSet<Ingridient>();
            ProductSales = new HashSet<ProductSale>();
            Productions = new HashSet<Production>();
        }

        public int Id { get; set; }
        public string Name { get; set; }
        public int? Unit { get; set; }
        public decimal? Sum { get; set; }
        public int? Count { get; set; }

        public virtual Unit UnitNavigation { get; set; }
        public virtual ICollection<Ingridient> Ingridients { get; set; }
        public virtual ICollection<ProductSale> ProductSales { get; set; }
        public virtual ICollection<Production> Productions { get; set; }
    }
}
