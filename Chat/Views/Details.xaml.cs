using Chat.Models;

namespace Chat.Views;

public partial class Details : ContentPage
{
	public string PhotoUrl { get; set; }	

    public Details()
	{
		InitializeComponent();
        PhotoUrl = Views.Content.PhotoUrl;
        BindingContext = this;
    }

    private async void OnBackButtonClicked(object sender, EventArgs e)
    {
        await Shell.Current.GoToAsync(nameof(Views.Content));
    }


}