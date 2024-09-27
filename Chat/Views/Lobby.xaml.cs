using Chat.Models;
using Firebase.Database;
using System.Collections.ObjectModel;

namespace Chat.Views;

public partial class Lobby : ContentPage
{
    private readonly FirebaseClient firebaseClient;

    public ObservableCollection<User> Usernames { get; set; } = new ObservableCollection<User>();

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
        var selectedUser = e.CurrentSelection.FirstOrDefault() as User;
        if (selectedUser != null)
        {
            var route = $"{nameof(GroupChat)}?loggedInUser={SignIn.LoggedInUsername}&selectedUser={selectedUser.Username}&loggedInUserUID={SignIn.LoggedInUsernameUID}&selectedUserUID={selectedUser.UID}";
            await Shell.Current.GoToAsync(route);
        }
    }

    private async void OnBackButtonClicked(object sender, EventArgs e)
    {
        await Shell.Current.GoToAsync(nameof(Views.Navigation));
    }
}
