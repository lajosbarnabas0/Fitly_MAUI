using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fitly.Models
{
    public class Comment
    {
        public int id { get; set; }
        public int post_id { get; set; }
        public int user_id { get; set; }
        public string content { get; set; }
        public DateTime? created_at { get; set; }
        public DateTime? updated_at { get; set; }
        public User user { get; set; }

    }
}
