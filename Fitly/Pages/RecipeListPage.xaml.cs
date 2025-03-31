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
		this.BindingContext = vm;
	}

    private async void ContentPage_Loaded(object sender, EventArgs e)
    {
        await LoadedAnimation.AnimateElementsOnPage(this);
    }

    private void NavMenu_Tapped(object sender, TappedEventArgs e)
    {
        this.ShowPopup(new NavigationPopUp());
    }
}