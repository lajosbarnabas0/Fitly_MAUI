using Fitly.ViewModels;

namespace Fitly.Pages;

public partial class NewPostPage : ContentPage
{
	public NewPostPage(NewPostViewModel vm)
	{
		InitializeComponent();
		BindingContext = vm;
	}

    private void ContentPage_Loaded(object sender, EventArgs e)
    {

    }
}