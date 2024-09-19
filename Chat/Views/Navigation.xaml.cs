namespace Chat.Views;

public partial class Navigation : ContentPage
{
	public Navigation()
	{
		InitializeComponent();
	}

    private async void OnChat(object sender, EventArgs e)
	{
        await Shell.Current.GoToAsync(nameof(Lobby));
    }

    private async void OnGeneralChat(object sender, EventArgs e)
    {
        await Shell.Current.GoToAsync(nameof(Views.GeneralChat));
    }
    private async void OnCamera(object sender, EventArgs e)
    {
        await Shell.Current.GoToAsync(nameof(Camera));
    }
}