using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fitly.Models
{

    public class RegisterUserRequest
    {
        public string name { get; set; }
        public string email { get; set; }
        public string gender { get; set; }
        public string birthday { get; set; }
        public string password { get; set; }
        public string password_confirmation { get; set; }
    }

}
