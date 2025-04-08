using Fitly.Helper;
using Fitly.ViewModels;

namespace Fitly.Pages;

public partial class NewPostPage : ContentPage
{
	public NewPostPage(NewPostViewModel vm)
	{
		InitializeComponent();
		BindingContext = vm;
	}

    private async void ContentPage_Loaded(object sender, EventArgs e)
    {
        await LoadedAnimation.AnimateElementsOnPage(this);
    }
}