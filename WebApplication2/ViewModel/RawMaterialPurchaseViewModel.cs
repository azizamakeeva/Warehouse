using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;

namespace WebApplication2.ViewModel
{
    public class RawMaterialPurchaseViewModel 
    {
        public SelectList RawMaterials { get; set; }
        public SelectList Employees { get; set; }
        public int? SelectRaw { get; set; }
        public int? SelectEmployee { get; set; }
        [Required]
        public int? quan { get; set; }
        [Required]
        public int? sum { get; set; }
        public string errorText { get; set; }

    }
}
