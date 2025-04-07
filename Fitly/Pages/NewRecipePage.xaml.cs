using Fitly.ViewModels;

namespace Fitly.Pages;

public partial class NewRecipePage : ContentPage
{
	public NewRecipePage(NewRecipeViewModel vm)
	{
		InitializeComponent();
		BindingContext = vm;
	}

    private void ContentPage_Loaded(object sender, EventArgs e)
    {

    }
}