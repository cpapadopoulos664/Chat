using Chat.Models;
using Firebase.Database;
using Firebase.Database.Query;
using System.Collections.ObjectModel;

namespace Chat.Views;

public partial class GeneralChat : ContentPage
{
    private readonly FirebaseClient firebaseClient;
    private readonly string idToken;
    private string loggedInUser;
    private string selectedUser;

    public ObservableCollection<Message> Messages { get; set; } = new ObservableCollection<Message>(); // list
    public GeneralChat(FirebaseClient FirebaseClient)
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
        firebaseClient.Child("Message").Child("Gen").PostAsync(new Message
        {
            Id = "Gen",
            SendUser = SignIn.LoggedInUsername,
            ReceivedUser = "Gen",
            Text = Text.Text,
            Date = DateTime.Now,
        });// xriazetai na gini xrisi using firebase query gia na to dexti 
        Text.Text = string.Empty;
    }

    public async Task LoadData()
    {

        firebaseClient.Child("Message").Child("Gen").AsObservable<Message>().Subscribe(
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
        await Shell.Current.GoToAsync(nameof(Views.Navigation));
    }
}