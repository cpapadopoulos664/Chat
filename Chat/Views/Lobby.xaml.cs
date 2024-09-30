using Chat.Models;
using Firebase.Database;
using System.Collections.ObjectModel;

namespace Chat.Views;

public partial class Lobby : ContentPage
{
    private readonly FirebaseClient firebaseClient;

    public ObservableCollection<User> Usernames { get; set; } = new ObservableCollection<User>();

    public static string selectedUser { get; set; }
    public static string selectedUserUID { get; set; }

    public Lobby(FirebaseClient FirebaseClient)
    {
        InitializeComponent();
        firebaseClient = FirebaseClient;
        BindingContext = this;
    }

    protected override async void OnNavigatedTo(NavigatedToEventArgs args)
    {
        base.OnNavigatedTo(args);
        await LoadData();
    }

    public async Task LoadData()
    {
        var users = await firebaseClient.Child("UserNames").OnceAsync<User>();

        foreach (var user in users)
        {
            Usernames.Add(user.Object);
        }

        firebaseClient.Child("UserNames").AsObservable<User>().Subscribe((item) =>
        {
            if (item.Object != null && !Usernames.Any(u => u.Username == item.Object.Username))
            {
                Usernames.Add(item.Object);
            }
        });
    }

    private async void OnUserSelected(object sender, SelectionChangedEventArgs e)
    {
         var User = e.CurrentSelection.FirstOrDefault() as User;
         selectedUser = User.Username;
         selectedUserUID = User.UID;
         await Shell.Current.GoToAsync(nameof(GroupChat));
    }

    private async void OnBackButtonClicked(object sender, EventArgs e)
    {
        await Shell.Current.GoToAsync(nameof(Views.Navigation));
    }
}
