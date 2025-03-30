namespace Fitly.Cards;

using CommunityToolkit.Mvvm.ComponentModel;
using Fitly.Models;
using Microsoft.Maui.Controls;

public partial class PostCard : ContentView
{
    public static readonly BindableProperty TitleProperty =
        BindableProperty.Create(nameof(Title), typeof(string), typeof(PostCard), defaultValue: "");

    public static readonly BindableProperty AuthorProperty =
        BindableProperty.Create(nameof(Author), typeof(string), typeof(PostCard), defaultValue: "");

    public static readonly BindableProperty ContentProperty =
        BindableProperty.Create(nameof(Content), typeof(string), typeof(PostCard), defaultValue: "");

    public string Title
    {
        get => (string)GetValue(TitleProperty);
        set => SetValue(TitleProperty, value);
    }

    public string Author
    {
        get => (string)GetValue(AuthorProperty);
        set => SetValue(AuthorProperty, value);
    }

    public string Description
    {
        get => (string)GetValue(ContentProperty);
        set => SetValue(ContentProperty, value);
    }

    public PostCard()
    {
        InitializeComponent();
        BindingContext = this;
        
    }
}