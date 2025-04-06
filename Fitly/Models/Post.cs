using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using CommunityToolkit.Mvvm.ComponentModel;

namespace Fitly.Models
{
    public class Post 
    {
        public int id { get; set; }
        public int user_id { get; set; }
        public string? Title { get; set; }
        public string? Content { get; set; }
        public string[]? image_path { get; set; }
        public DateTime created_at { get; set; }
        public DateTime updated_at { get; set; }
        public User? User { get; set; }

        private string _author;
        public string Author
        {
            get => User?.Name ?? _author;
            set => _author = value;
        }
    }
}
