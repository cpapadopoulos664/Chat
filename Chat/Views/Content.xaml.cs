using System.Collections.ObjectModel;
using Chat.Models;
using Firebase.Auth;
using Firebase.Database;
using Firebase.Database.Query;

namespace Chat.Views;

public partial class Content : ContentPage
{
    private readonly FirebaseClient firebaseClient;
    private readonly FirebaseAuthClient _authClient;
    public ObservableCollection<Challenge> PhotoItems { get; set; } = new ObservableCollection<Challenge>();
    public static string PhotoUrl { get; set; }
    public static string GPS { get; set; }

    public Content(FirebaseClient FirebaseClient, FirebaseAuthClient firebaseAuthClient)
    {
        InitializeComponent();
        firebaseClient = FirebaseClient;
        _authClient= firebaseAuthClient;
        BindingContext = this; // Set BindingContext to the current page
    }

    protected override async void OnNavigatedTo(NavigatedToEventArgs args)
    {
        base.OnNavigatedTo(args);
        await LoadData();
    }

    public async Task LoadData()
    {
        // Add data to the ObservableCollection
        PhotoItems.Clear();
        var challenges = await firebaseClient
        .Child("Content")
        .OnceAsync<Challenge>();
        foreach (var challenge in challenges)
        {
            PhotoItems.Add(challenge.Object);
        }
    }

    private async void OnPhotoSelected(object sender, SelectionChangedEventArgs e)
    {
        var Challenge = e.CurrentSelection.FirstOrDefault() as Challenge;
        PhotoUrl = Challenge.PhotoUrl;
        GPS = Challenge.GPS;
        await Shell.Current.GoToAsync(nameof(Details));
    }
    private async void OnBackButtonClicked(object sender, EventArgs e)
    {
        await Shell.Current.GoToAsync(nameof(Views.Navigation));
    }
}
