using Firebase.Auth;
using Firebase.Database;
using Firebase.Database.Query;

namespace Chat.Views;

public partial class SignIn : ContentPage
{
    private readonly FirebaseAuthClient _authClient;
    private readonly FirebaseClient _firebaseClient;
    public static string LoggedInUsername { get; set; }
    public static string LoggedInUsernameUID { get; set; }
    public SignIn(FirebaseAuthClient authClient, FirebaseClient firebaseClient)
	{
		InitializeComponent();
        _authClient = authClient;
        _firebaseClient = firebaseClient;
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
                var uid = user.User.Uid;
                StatusLabel.TextColor = Colors.Green;
                StatusLabel.Text = "Sign in successful!";
                //user data
                var foundUser = await _firebaseClient
                 .Child("User")
                 .Child(uid)  // Query using the UID directly as the key
                 .OnceSingleAsync<Models.User>();
                // Fetch existing users from Firebase
                //var existingUsers = await _firebaseClient.Child("UserNames")
                                        //.OnceAsync<Models.User>();
                //var foundUser = existingUsers.FirstOrDefault(u => u.Object.Email == email); // find the user name 
                if (foundUser != null)
                {
                    LoggedInUsername = foundUser.Username; // Access  Username 
                    LoggedInUsernameUID = uid;
                }
                else
                {
                    LoggedInUsername = null;
                    LoggedInUsernameUID = null;
                }
                await Shell.Current.GoToAsync(nameof(Views.Navigation));
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