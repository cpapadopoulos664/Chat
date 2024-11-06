using Microsoft.Maui.Controls; // Ensure you have the right namespace
using System.Reflection; // To access embedded resources
using System.IO; // For Stream and StreamReader
using System.Diagnostics; // For debugging purposes

namespace Chat.Views
{
    public partial class Details : ContentPage
    {
        public string PhotoUrl { get; set; }

        public Details()
        {
            InitializeComponent();
            PhotoUrl = Views.Content.PhotoUrl;
            BindingContext = this;
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