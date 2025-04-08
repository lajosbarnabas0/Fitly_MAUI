using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fitly.Models
{
    public class RecipeResponse
    {
        public string Name { get; set; }
        public string Email { get; set; }
        public string[] Image_paths { get; set; }
        public string Description { get; set; }
        public string Ingredients { get; set; }
    }
}
