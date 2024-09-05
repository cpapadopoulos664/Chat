namespace Chat.Views;
using Firebase.Auth;


public partial class SignUp : ContentPage
{
    private readonly FirebaseAuthClient _authClient;
    public SignUp(FirebaseAuthClient authClient)
	{
        InitializeComponent();
        _authClient = authClient;

    }
    private async void OnSignUpButtonClicked(object sender, EventArgs e)
    {
        string username = UsernameEntry.Text;
        string email = EmailEntry.Text;
        string password = PasswordEntry.Text;

        if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(email) || string.IsNullOrEmpty(password))
        {
            StatusLabel.Text = "Please fill in all fields.";
        }
        else
        {
            try
            {
                //Create the user using Firebase authentication
                var user = await _authClient.CreateUserWithEmailAndPasswordAsync(email, password, username);
                StatusLabel.TextColor = Colors.Green;
                StatusLabel.Text = "Sign up successful!";
            }
            catch (Exception ex)
            {
                StatusLabel.TextColor = Colors.Red;
                StatusLabel.Text = $"Sign up failed: {ex.Message}";
            }
        }
    }
    private async void OnBackButtonClicked (object sender, EventArgs e)
    {
        await Shell.Current.GoToAsync("..");
    }
}