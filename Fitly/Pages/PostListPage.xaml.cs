 using CommunityToolkit.Maui.Views;
using CommunityToolkit.Mvvm.ComponentModel;
using Fitly.Helper;
using Fitly.Models;
using Fitly.Navigation;
using Fitly.ViewModels;
using Microsoft.Maui.Controls;

namespace Fitly.Pages;

public partial class PostListPage : ContentPage
{
	public PostListPage(PostListViewModel vm)
	{
		InitializeComponent();
        BindingContext = vm;
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