namespace Chat.Views;
using Chat.Models;
using Firebase.Auth;
using Firebase.Database;
using Firebase.Database.Query;
using System.Collections.ObjectModel;
using static System.Net.Mime.MediaTypeNames;
using User = Models.User;

public partial class SignUp : ContentPage
{
    private readonly FirebaseAuthClient _authClient;
    private readonly FirebaseClient _firebaseClient;
    public List <User> Usernames { get; set; } = new List <User> ();    
    public SignUp(FirebaseAuthClient authClient, FirebaseClient firebaseClient)
	{
        InitializeComponent();
        _authClient = authClient;
        _firebaseClient = firebaseClient; // create and asighn
        BindingContext = this;// dese me to ui 

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
                // Fetch existing users from Firebase
                var existingUsers = await _firebaseClient.Child("User")
                                        .OnceAsync<Models.User>();

                // Check if the username already exists in Firebase
                if (!existingUsers.Any(u => u.Object.Username == username))
                {
                    // Create the user using Firebase authentication
                    var user = await _authClient.CreateUserWithEmailAndPasswordAsync(email, password, username);

                    // Add the new user to Firebase
                    await _firebaseClient.Child("User").PostAsync(new Models.User
                    {
                        Username = username
                    });

                    StatusLabel.TextColor = Colors.Green;
                    StatusLabel.Text = "Sign up successful!";
                }
                else
                {
                    StatusLabel.TextColor = Colors.Red;
                    StatusLabel.Text = "Username already exists.";
                }
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