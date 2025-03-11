using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fitly.Models
{
    public class LoginUserResponse
    {
        public string token { get; set; }
        public User user { get; set; }

    }
}
