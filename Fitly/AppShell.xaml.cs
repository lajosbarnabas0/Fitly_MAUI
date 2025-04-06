using Fitly.Pages;

namespace Fitly
{
    public partial class AppShell : Shell
    {
        public AppShell()
        {
            InitializeComponent();

            Routing.RegisterRoute("RecipeDetailPage", typeof(RecipeDetailPage));
            //Routing.RegisterRoute("PostListPage", typeof(PostListPage));
        }
    }
}
