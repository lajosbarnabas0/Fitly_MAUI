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

}