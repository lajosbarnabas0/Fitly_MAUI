using System.Diagnostics;
using CommunityToolkit.Maui.Views;
using Fitly.Navigation;
using Fitly.ViewModels;

namespace Fitly.Pages;

public partial class ChangePage : ContentPage
{
	public ChangePage(ChangeViewModel vm)
	{
		InitializeComponent();
	}

    private void NavMenu_Tapped(object sender, TappedEventArgs e)
    {
        this.ShowPopup(new NavigationPopUp());
    }

    private void CaloriePage_Tapped(object sender, TappedEventArgs e)
    {
		try
		{
			Shell.Current.GoToAsync("CaloriePage");
		}
		catch (Exception ex)
		{
            Debug.WriteLine($"Navigációs hiba: {ex.Message}");
            Debug.WriteLine($"Részletek: {ex.StackTrace}");
        }
    }}