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
            // Load the map HTML file into the WebView
            LoadMap();
        }



        private void LoadMap()
        {
            // Adjust the resource name if necessary
            string htmlFileName = "Chat.Resources.Map.Map.html";
            var assembly = typeof(Details).GetTypeInfo().Assembly;

            using (Stream stream = assembly.GetManifestResourceStream(htmlFileName))
            {
                if (stream == null)
                {
                    throw new FileNotFoundException("HTML resource not found: " + htmlFileName);
                }

                using (StreamReader reader = new StreamReader(stream))
                {
                    string htmlContent = reader.ReadToEnd();
                    MapWebView.Source = new HtmlWebViewSource { Html = htmlContent };
                }
            }
        }

        private async void OnBackButtonClicked(object sender, EventArgs e)
        {
            await Shell.Current.GoToAsync(nameof(Views.Content));
        }
    }
}