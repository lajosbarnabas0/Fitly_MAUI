using Fitly.ViewModels;

namespace Fitly.Pages;

public partial class CaloriePage : ContentPage
{
	public CaloriePage(CalorieViewModel cv)
	{
		InitializeComponent();
		BindingContext = cv;
	}
}