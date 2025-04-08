using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Fitly.Models;

namespace Fitly.API
{
    public class SendData
    {
        public static async Task<UpdateProfileResponse> UpdateProfile(string url, object data)
        {
            return await HTTPRequest<UpdateProfileResponse>.Put(url, data);
        }

        public static async Task<Recipe> SendRecipe(string url, object data)
        {
            return await HTTPRequest<Recipe>.Post(url, data);
        }

        public static async Task<PostRequest> SendPost(string url, object data)
        {
            return await HTTPRequest<PostRequest>.Post(url, data);
        }
    }
}
