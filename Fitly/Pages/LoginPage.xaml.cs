using Fitly.ViewModels;
using Fitly.Helper;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Maui.Views;
using Fitly.Navigation;

namespace Fitly.Pages;

public partial class LoginPage : ContentPage
{
	public LoginPage(LoginViewModel vm)
	{
		InitializeComponent();
		this.BindingContext = vm;
	}

    private async void ContentPage_Loaded(object sender, EventArgs e)
    {   
        await LoadedAnimation.AnimateElementsOnPage(this);
    }

    private async void Login_Button_Clicked(object sender, EventArgs e)
    {
        await Login_Button.ScaleTo(1.1, 200, Easing.CubicOut);
        await Login_Button.ScaleTo(1.0, 300, Easing.CubicIn);
    }

    private void NavMenu_Tapped(object sender, TappedEventArgs e)
    {
        this.ShowPopup(new NavigationPopUp());
    }
}