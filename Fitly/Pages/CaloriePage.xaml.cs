using CommunityToolkit.Maui.Views;
using Fitly.Navigation;
using Fitly.ViewModels;

namespace Fitly.Pages;

public partial class CaloriePage : ContentPage
{
	public CaloriePage(CalorieViewModel cv)
	{
		InitializeComponent();
		BindingContext = cv;
	}

    private void NavMenu_Tapped(object sender, TappedEventArgs e)
    {
        this.ShowPopup(new NavigationPopUp());
    }
}