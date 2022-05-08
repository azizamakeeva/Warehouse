using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApplication2.Models;

namespace WebApplication2.ViewModel
{
    public class IngridientsViewModel 
    {
        public IEnumerable<Ingridient> Ingredients { get; set; }
        public SelectList FinishedProducts { get; set; }
        public int? SelectedProduct { get; set; }
        public string Name { get; set; }
    }
}
