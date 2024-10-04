using System.Collections.ObjectModel;
using Chat.Models;
using Firebase.Database;

namespace Chat.Views;

public partial class Content : ContentPage
{
    public ObservableCollection<Challenge> PhotoItems { get; set; } = new ObservableCollection<Challenge>();

    public Content()
    {
        InitializeComponent();
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
        var challenges = new List<Challenge>
        {
            new Challenge { PhotoUrl = "https://example.com/photo4.jpg", InfoText = "Relax by the beach" },
            new Challenge { PhotoUrl = "https://example.com/photo1.jpg", InfoText = "Discover the forest trails" },
            new Challenge { PhotoUrl = "https://example.com/photo3.jpg", InfoText = "Adventure in the desert" },
            new Challenge { PhotoUrl = "https://example.com/photo5.jpg", InfoText = "Adventure in the desert" },
            new Challenge { PhotoUrl = "https://example.com/photo1.jpg", InfoText = "Explore the mountains" }
        };

        foreach (var challenge in challenges)
        {
            PhotoItems.Add(challenge);
        }
    }

    private async void OnBackButtonClicked(object sender, EventArgs e)
    {
        await Shell.Current.GoToAsync(nameof(Views.Navigation));
    }
}
