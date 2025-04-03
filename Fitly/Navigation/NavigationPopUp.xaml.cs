using CommunityToolkit.Maui.Views;
using Fitly.Helper;

namespace Fitly.Navigation;

public partial class NavigationPopUp : Popup
{
	public NavigationPopUp()
	{
		InitializeComponent();
	}

    private async void HomePage_Tapped(object sender, TappedEventArgs e)
    {
        await Shell.Current.GoToAsync("//MainPage");
        Close();
    }
    
    private async void MakeAChangePage_Tapped(object sender, TappedEventArgs e)
    {
        return;
    }
    
    private async void SocialPage_Tapped(object sender, TappedEventArgs e)
    {
        await Shell.Current.GoToAsync("//MainCommunityPage");
        Close(); ;
    }
    
    private async void ProfilePage_Tapped(object sender, TappedEventArgs e)
    {
        string? isLoginSet = SecureStorage.Default.GetAsync("LoginToken").Result;
        if(isLoginSet != null)
        {
            await Shell.Current.GoToAsync("//ProfilePage");
            Close();
        }
        else
        {
            await Shell.Current.GoToAsync("//LetsBeginPage");
            Close();
        }
    }
}