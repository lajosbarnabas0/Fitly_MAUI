using Fitly.ViewModels;

namespace Fitly.Pages;

public partial class PostListPage : ContentPage
{
	public PostListPage(PostListViewModel vm)
	{
		InitializeComponent();
		this.BindingContext = vm;
	}
}