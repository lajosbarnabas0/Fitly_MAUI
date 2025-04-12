using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Fitly.Models;

namespace Fitly.API
{
    public static class GetData
    {
        public static async Task<User?> GetUserById(string url)
        {
            return await HTTPRequest<User>.Get(url);
        }

        public static async Task<List<Meal>?> GetMeals(string url)
        {
            return await HTTPRequest<List<Meal>>.Get(url);
        }

        public static async Task<ObservableCollection<Comment>?> GetComment(string url)
        {
            var root = await HTTPRequest<CommentRoot>.Get(url);

            return root?.Comments;
        }

    }
}
