using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.ComponentModel;
using Fitly.Models;

namespace Fitly.ViewModels
{
    public partial class PostListViewModel : ObservableObject
    {
        public ObservableCollection<Post> Posts { get; set; } = new ObservableCollection<Post>();

        [ObservableProperty]
        User user;

        [ObservableProperty]
        string title;

        [ObservableProperty]
        string content;

        public PostListViewModel()
        {
            // Példa adatok helyes inicializálása
            Posts.Add(new Post
            {
                Title = "Szép tájkép",
                User = new User { Name = "John Doe" },
                Content = "Ez egy szép tájkép leírása."
            });

            Posts.Add(new Post
            {
                Title = "Naplemente",
                User = new User { Name = "Jane Smith" },
                Content = "Egy gyönyörű naplemente képe."
            });

            Posts.Add(new Post
            {
                Title = "Hegyek",
                User = new User { Name = "Alice Brown" },
                Content = "Hegyek és völgyek csodálatos látványa."
            });
        }
    }
}
