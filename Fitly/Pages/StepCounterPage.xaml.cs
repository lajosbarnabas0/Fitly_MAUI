using Fitly.ViewModels;

namespace Fitly.Pages;

public partial class StepCounterPage : ContentPage
{
	public StepCounterPage(StepCounterViewModel vm)
	{
		InitializeComponent();
		BindingContext = vm;
	}
}