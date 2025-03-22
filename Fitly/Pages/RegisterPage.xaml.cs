using CommunityToolkit.Maui.Views;
using Fitly.Helper;
using Fitly.Navigation;
using Fitly.ViewModels;

namespace Fitly.Pages;

public partial class RegisterPage : ContentPage
{
	public RegisterPage(RegisterViewModel vm)
	{
		InitializeComponent();
		this.BindingContext = vm;
	}

    private async void ContentPage_Loaded(object sender, EventArgs e)
    {
        await LoadedAnimation.AnimateElementsOnPage(this);
        gender_PCKR.SelectedItem = null;
    }
    private void NavMenu_Tapped(object sender, TappedEventArgs e)
    {
        this.ShowPopup(new NavigationPopUp());
    }

    private async void Register_Button_Clicked(object sender, EventArgs e)
    {
        await Register_Button.ScaleTo(1.1, 200, Easing.CubicOut);
        await Register_Button.ScaleTo(1.0, 300, Easing.CubicIn);
    }

}