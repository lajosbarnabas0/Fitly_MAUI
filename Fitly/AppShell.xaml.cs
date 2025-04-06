using Fitly.Pages;

namespace Fitly
{
    public partial class AppShell : Shell
    {
        public AppShell()
        {
            InitializeComponent();

            Routing.RegisterRoute(nameof(RecipeDetailPage), typeof(RecipeDetailPage));
            Routing.RegisterRoute(nameof(PostDetailPage), typeof(PostDetailPage));
            Routing.RegisterRoute(nameof(RecipeListPage), typeof(RecipeListPage));
            Routing.RegisterRoute(nameof(PostListPage), typeof(PostListPage));
        }
    }
}
