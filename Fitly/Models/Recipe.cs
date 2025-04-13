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
        public string title { get; set; }
        public dynamic? image_paths { get; set; }
        public string ingredients { get; set; }
        public string description { get; set; }
        public string avg_time { get; set; }
        public int user_id { get; set; }
        public DateTime? created_at { get; set; }
        public DateTime? updated_at { get; set; }
        public string[]? image_urls { get; set; }

        public string? avatar { get; set; }

        public User? user { get; set; }

        private string? _author;
        public string? Author
        {
            get => user?.Name ?? _author;
            set => _author = value;
        }
    }

}
