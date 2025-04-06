using Fitly.ViewModels;

namespace Fitly.Pages;

public partial class RecipeDetailPage : ContentPage
{
	public RecipeDetailPage(RecipeDetailViewModel vm)
	{
		InitializeComponent();
		BindingContext = vm;
	}
}