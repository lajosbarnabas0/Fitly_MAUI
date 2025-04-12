using Fitly.ViewModels;

namespace Fitly.Pages;

public partial class OwnRecipeListPage : ContentPage
{
	public OwnRecipeListPage(OwnRecipeListViewModel vm)
	{
		InitializeComponent();
		BindingContext = vm;
	}
}