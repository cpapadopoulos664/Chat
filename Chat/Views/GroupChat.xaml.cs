using Microsoft.Maui.Hosting;
using Firebase.Database;
using Firebase.Database.Query;
using Chat.Models;
using System.Collections.ObjectModel;
using System.ComponentModel;
using Firebase.Auth;
using Firebase.Auth.Providers;

namespace Chat.Views;

public partial class GroupChat : ContentPage
{
    private readonly FirebaseClient firebaseClient;
    private readonly string idToken;

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

    private async void OnSend(object sender, EventArgs e)
    {
        firebaseClient.Child("Message").PostAsync(new Message
        {
            Name = Name.Text, // enter data 
            Text = Text.Text,
        });// xriazetai na gini xrisi using firebase query gia na to dexti 
        Name.Text = string.Empty;
        Text.Text = string.Empty;
    }

    public async Task LoadData()
    {
        firebaseClient.Child("Message").AsObservable<Message>().Subscribe(
            (item) =>
            {
                if (item.Object != null)
                {
                    Messages.Add(item.Object);
                }
            });
    }
}