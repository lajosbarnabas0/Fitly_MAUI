using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fitly.Models
{
    public class RecipeResponse
    {
        public string avg_time { get; set; }
        public string title { get; set; }
        public string? image_paths { get; set; }
        public string description { get; set; }
        public string ingredients { get; set; }
    }
}
