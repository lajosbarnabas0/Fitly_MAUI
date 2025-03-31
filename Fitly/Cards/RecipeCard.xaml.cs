
namespace Fitly.Cards;

public partial class RecipeCard : ContentView
{
    public static readonly BindableProperty TitleProperty =
        BindableProperty.Create(nameof(Title), typeof(string), typeof(PostCard), defaultValue: "");

    public static readonly BindableProperty IngredientsProperty =
        BindableProperty.Create(nameof(Ingredients), typeof(string[]), typeof(PostCard), defaultValue: "");

    public static readonly BindableProperty DescriptionProperty =
        BindableProperty.Create(nameof(Description), typeof(string), typeof(PostCard), defaultValue: "");

    public static readonly BindableProperty TimeToMakeProperty =
        BindableProperty.Create(nameof(TimeToMake), typeof(string), typeof(PostCard), defaultValue: "");

    public static readonly BindableProperty AuthorProperty =
        BindableProperty.Create(nameof(Author), typeof(string), typeof(PostCard), defaultValue: "");


    public string Title
    {
        get => (string)GetValue(TitleProperty);
        set => SetValue(TitleProperty, value);
    }

    public string[] Ingredients
    {
        get => (string[])GetValue(DescriptionProperty);
        set => SetValue(DescriptionProperty, value);
    }

    public string Description
    {
        get => (string)GetValue(TimeToMakeProperty);
        set => SetValue(TimeToMakeProperty, value);
    }

    public string TimeToMake
    {
        get => (string)GetValue(AuthorProperty);
        set => SetValue(AuthorProperty, value);
    }

    public string Author
    {
        get => (string)GetValue(IngredientsProperty);
        set => SetValue(IngredientsProperty, value);
    }



    public RecipeCard()
	{
		InitializeComponent();
        BindingContext = this;
	}
}