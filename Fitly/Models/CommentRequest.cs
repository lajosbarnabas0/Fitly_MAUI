using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fitly.Models
{
    public class CommentRequest
    {
        public int user_id { get; set; }
        public string? content { get; set; }
    }
}
