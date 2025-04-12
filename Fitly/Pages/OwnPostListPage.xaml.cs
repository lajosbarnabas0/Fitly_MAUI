using Fitly.ViewModels;

namespace Fitly.Pages;

public partial class OwnPostListPage : ContentPage
{
	public OwnPostListPage(OwnPostListViewModel vm)
	{
		InitializeComponent();
		BindingContext = vm;
	}
}