using Fitly.ViewModels;

namespace Fitly.Pages;

public partial class PostDetailPage : ContentPage
{
	public PostDetailPage(PostDetailViewModel vm)
	{
		InitializeComponent();
		BindingContext = vm;
	}
}