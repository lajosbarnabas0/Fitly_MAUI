using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fitly.Models
{
    public class Meal
    {
        public int id { get; set; }
        public string name { get; set; }
        public float? kcal { get; set; }
        public float? fat { get; set; }
        public float? carb { get; set; }
        public float? protein { get; set; }
        public float? salt { get; set; }
        public float? sugar { get; set; }
    }
}
