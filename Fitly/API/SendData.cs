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
            // Explicit típusmegadás
            return await HTTPRequest<UpdateProfileResponse>.Put(url, data);
        }
        
        public static async Task<Meal> SendMeal(string url, object data)
        {
            return await HTTPRequest<Meal>.Post(url, data);
        }
        
        public static async Task<CommentRequest> SendComment(string url, object data)
        {
            // Explicit típusmegadás
            return await HTTPRequest<CommentRequest>.Post(url, data);
        }

        public static async Task<DeleteResponse> DeletePost(string url)
        {
            return await HTTPRequest<DeleteResponse>.Delete(url);
        }
    }
}
