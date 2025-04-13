using Fitly.ViewModels;

namespace Fitly.Pages;

public partial class WorkoutsPage : ContentPage
{
	public WorkoutsPage(WorkoutViewModel vm)
	{
		InitializeComponent();
		BindingContext = vm;
	}
}