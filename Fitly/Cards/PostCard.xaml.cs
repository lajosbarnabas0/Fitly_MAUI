namespace Fitly.Cards;

using CommunityToolkit.Mvvm.ComponentModel;
using Microsoft.Maui.Controls;

public partial class PostCard : ContentView
{

    public string Title
    {
        get
		{
			return title_LBL.Text;
        }
		set
		{
			title_LBL.Text = value;
		}
	}

    public string Author
    {
        get
        {
            return author_LBL.Text;
        }
        set
        {
            author_LBL.Text = value;
        }
    }

    public string Content
    {
        get
        {
            return content_LBL.Text;
        }
        set
        {
            content_LBL.Text = value;
        }
    }

    public PostCard()
	{
		InitializeComponent();
	}

}