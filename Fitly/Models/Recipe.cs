using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Fitly.API;

namespace Fitly.Models
{
    public class Recipe
    {
        public int id { get; set; }
        public string Title { get; set; }
        public string[] Image_paths { get; set; }
        public string Ingredients { get; set; }
        public string Description { get; set; }
        public string Avg_Time { get; set; }
        public int user_id { get; set; }
        public DateTime created_at { get; set; }
        public DateTime updated_at { get; set; }
        public User User { get; set; }

        private string _author;
        public string Author
        {
            get => User?.Name ?? _author;
            set => _author = value;
        }
    }
}
