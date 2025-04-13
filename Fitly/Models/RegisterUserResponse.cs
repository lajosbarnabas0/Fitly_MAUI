using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fitly.Models
{
    public class RegisterUserResponse
    {
        public User user { get; set; }
        public string token { get; set; }

    }
}
