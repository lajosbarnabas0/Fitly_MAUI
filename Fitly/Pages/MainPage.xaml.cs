using Fitly.ViewModels;
using CommunityToolkit.Maui.Animations;
using Microsoft.Maui.Controls;
using Fitly.Helper;
using CommunityToolkit.Maui.Views;
using Fitly.Navigation;

namespace Fitly.Pages
{
    public partial class MainPage : ContentPage
    {
        public MainPage(MainViewModel vm)
        {
            InitializeComponent();
            this.BindingContext = vm;
        }

        private async void LetsBegin_Button_Clicked(object sender, EventArgs e)
        {
            await letsBegin_Button.ScaleTo(1.1, 200, Easing.CubicOut);
            await letsBegin_Button.ScaleTo(1.0, 300, Easing.CubicIn);
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
}
