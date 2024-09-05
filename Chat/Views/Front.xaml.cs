namespace Chat.Views;

public partial class Front : ContentPage
{
	public Front()
	{
		InitializeComponent();
	}

    private async void OnSignUpButtonClicked(object sender, EventArgs e)
    {
        await Shell.Current.GoToAsync(nameof(SignUp));
    }

    private async void OnSignInButtonClicked(object sender, EventArgs e)
    {
        await Shell.Current.GoToAsync(nameof(SignIn));
    }
}