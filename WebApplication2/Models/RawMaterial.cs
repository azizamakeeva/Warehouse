using System;
using System.Collections.Generic;

#nullable disable

namespace WebApplication2.Models
{
    public partial class RawMaterial
    {
        public RawMaterial()
        {
            Ingridients = new HashSet<Ingridient>();
            RawMaterialPurchases = new HashSet<RawMaterialPurchase>();
        }

        public int Id { get; set; }
        public string Name { get; set; }
        public int? Unit { get; set; }
        public int? Sum { get; set; }
        public int? Count { get; set; }

        public virtual Unit UnitNavigation { get; set; }
        public virtual ICollection<Ingridient> Ingridients { get; set; }
        public virtual ICollection<RawMaterialPurchase> RawMaterialPurchases { get; set; }
    }
}
