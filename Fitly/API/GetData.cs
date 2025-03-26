using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Fitly.Models;

namespace Fitly.API
{
    public static class GetData
    {
        public static async Task<User> GetUserById(string url)
        {
            return await HTTPRequest<User>.Get(url);
        }
    }
}
