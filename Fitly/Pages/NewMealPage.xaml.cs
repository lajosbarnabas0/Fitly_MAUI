using Fitly.ViewModels;

namespace Fitly.Pages;

public partial class NewMealPage : ContentPage
{
	public NewMealPage(NewMealViewModel vm)
	{
		InitializeComponent();
		BindingContext = vm;
	}
}