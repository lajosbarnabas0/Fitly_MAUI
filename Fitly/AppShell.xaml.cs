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
            Routing.RegisterRoute(nameof(NewRecipePage), typeof(NewRecipePage));
            Routing.RegisterRoute(nameof(NewPostPage), typeof(NewPostPage));
            Routing.RegisterRoute(nameof(CaloriePage), typeof(CaloriePage));
            Routing.RegisterRoute(nameof(ChangePage), typeof(ChangePage));
            Routing.RegisterRoute(nameof(NewMealPage), typeof(NewMealPage));
            Routing.RegisterRoute(nameof(OwnPostListPage), typeof(OwnPostListPage));
            Routing.RegisterRoute(nameof(OwnRecipeListPage), typeof(OwnRecipeListPage));
        }
    }
}
