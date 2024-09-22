using Microsoft.Maui.Hosting;
using Firebase.Database;
using Firebase.Database.Query;
using Chat.Models;
using System.Collections.ObjectModel;
using System.ComponentModel;
using Firebase.Auth;
using Firebase.Auth.Providers;
using LiteDB;

namespace Chat.Views;

public partial class GroupChat : ContentPage, IQueryAttributable
{
    private readonly FirebaseClient firebaseClient;
    private readonly string idToken;
    private string loggedInUser;
    private string selectedUser;
    private string loggedInUserUID;
    private string selectedUserUID;

    public ObservableCollection<Message> Messages { get; set; } = new ObservableCollection<Message>(); // list
    public GroupChat(FirebaseClient FirebaseClient)
	{
		InitializeComponent();
        firebaseClient = FirebaseClient; // create and asighn
        BindingContext = this;// dese me to ui 
    }

    protected override async void OnNavigatedTo(NavigatedToEventArgs args)
    {
        base.OnNavigatedTo(args);
        await LoadData(); // gia klisi me to pou anixi h forma 
    }

    public void ApplyQueryAttributes(IDictionary<string, object> query)
    {
        if (query.ContainsKey("loggedInUser"))
        {
            loggedInUser = query["loggedInUser"] as string;
        }

        if (query.ContainsKey("selectedUser"))
        {
            selectedUser = query["selectedUser"] as string;
        }

        if (query.ContainsKey("loggedInUserUID"))
        {
            loggedInUserUID = query["loggedInUserUID"] as string;
        }

        if (query.ContainsKey("selectedUserUID"))
        {
            selectedUserUID = query["selectedUserUID"] as string;
        }
    }


    private async void OnSend(object sender, EventArgs e)
    {
        var Id ="";
        if (string.Compare(loggedInUser, selectedUser) > 0)
        {
            Id= $"{loggedInUser}_{selectedUser}";
        }
        else
        {
            Id = $"{selectedUser}_{loggedInUser}";
        }
        firebaseClient.Child("Message").Child(Id).PostAsync(new Message
        {
            Id = Id,
            SendUser = loggedInUser,
            SendUserUID = loggedInUserUID,
            ReceivedUser = selectedUser,
            ReceivedUserUID = selectedUserUID,
            Text = Text.Text,
            Date = DateTime.Now,
        });// xriazetai na gini xrisi using firebase query gia na to dexti 
        Text.Text = string.Empty;
    }

    public async Task LoadData()
    {
        var Id = "";
        if (string.Compare(loggedInUser, selectedUser) > 0)
        {
            Id = $"{loggedInUser}_{selectedUser}";
        }
        else
        {
            Id = $"{selectedUser}_{loggedInUser}";
        }

        firebaseClient.Child("Message").Child(Id).AsObservable<Message>().Subscribe(
            (item) =>
            {
                if (item.Object != null)
                {
                    Messages.Add(item.Object);
                }
            });
    }

    private async void OnBackButtonClicked(object sender, EventArgs e)
    {
        await Shell.Current.GoToAsync(nameof(Lobby));
    }
}