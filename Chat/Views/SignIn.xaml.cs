using Firebase.Auth;

namespace Chat.Views;

public partial class SignIn : ContentPage
{
    private readonly FirebaseAuthClient _authClient;
    public SignIn(FirebaseAuthClient authClient)
	{
		InitializeComponent();
        _authClient = authClient;
    }
    private async void OnSignInButtonClicked(object sender, EventArgs e)
    {
        string email = EmailEntry.Text;
        string password = PasswordEntry.Text;

        if (string.IsNullOrEmpty(email) || string.IsNullOrEmpty(password))
        {
            StatusLabel.Text = "Please enter your email and password.";
        }
        else
        {
            try
            {
                // Sign in the user using Firebase authentication
                var user = await _authClient.SignInWithEmailAndPasswordAsync(email, password);
                StatusLabel.TextColor = Colors.Green;
                StatusLabel.Text = "Sign in successful!";
                await Shell.Current.GoToAsync(nameof(GroupChat));
                // Handle successful sign-in (e.g., navigate to another page)
            }
            catch (Exception ex)
            {
                StatusLabel.TextColor = Colors.Red;
                StatusLabel.Text = $"Sign in failed: {ex.Message}";
            }
        }
    }
    private async void OnBackButtonClicked(object sender, EventArgs e)
    {
        await Shell.Current.GoToAsync("..");
    }
}