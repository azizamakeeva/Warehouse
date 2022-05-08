using System;
using System.Collections.Generic;

#nullable disable

namespace WebApplication2.Models
{
    public partial class Ingridient
    {
        public int Id { get; set; }
        public int? Products { get; set; }
        public int? RawMaterials { get; set; }
        public int? Count { get; set; }

        public virtual FinishedProduct ProductsNavigation { get; set; }
        public virtual RawMaterial RawMaterialsNavigation { get; set; }
    }
}
