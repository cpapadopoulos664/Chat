using Firebase.Database;
using Chat.Models;
using System.Collections.ObjectModel;
using Firebase.Database.Query; // For debugging purposes

namespace Chat.Views
{
    public partial class Details : ContentPage
    {
        private readonly FirebaseClient firebaseClient;

        public ObservableCollection<Challenge> PhotoItems { get; set; } = new ObservableCollection<Challenge>();
        public  string PhotoUrl { get; set; }
        public static string Type { get; set; }
        public static string Link { get; set; }
        public Details(FirebaseClient FirebaseClient)
        {
            InitializeComponent();
            PhotoUrl = Views.Content.PhotoUrl;
            Link = PhotoUrl;
            BindingContext = this;
            firebaseClient = FirebaseClient;
            LoadData();
        }

        public async Task LoadData()
        {
            // Add data to the ObservableCollection
            PhotoItems.Clear();
            try
            {
                var challenges = await firebaseClient
                                .Child("Responce")
                                .OnceAsync<Challenge>();



                foreach (var challenge in challenges)
                {
                    if (challenge.Object.Link == Link)
                    {
                        PhotoItems.Add(challenge.Object);
                    }
                }

            }
            catch (Exception ex) 
            {
                // Log or handle exception
                Console.WriteLine($"Error retrieving photo: {ex.Message}");
            }
           
        }
        private async void OnSolve(object sender, EventArgs e)
        {
            Type = "Responce";
            await Shell.Current.GoToAsync(nameof(Camera));
        }
        private async void OnMapButtonClicked(object sender, EventArgs e)
        {
            await Shell.Current.GoToAsync(nameof(Map));
        }

        private async void OnBackButtonClicked(object sender, EventArgs e)
        {
            await Shell.Current.GoToAsync(nameof(Views.Content));
        }
    }
}