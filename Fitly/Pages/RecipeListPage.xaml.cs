using CommunityToolkit.Maui.Views;
using Fitly.Helper;
using Fitly.Navigation;
using Fitly.ViewModels;

namespace Fitly.Pages;

public partial class RecipeListPage : ContentPage
{
	public RecipeListPage(RecipeListViewModel vm)
	{
		InitializeComponent();
        BindingContext = vm;
	}

    private void NavMenu_Tapped(object sender, TappedEventArgs e)
    {
        this.ShowPopup(new NavigationPopUp());
    }
}