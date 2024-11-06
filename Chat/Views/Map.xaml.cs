namespace Chat.Views;

public partial class Map : ContentPage
{
	public Map()
	{
		InitializeComponent();
        LoadMap();

    }

    private void LoadMap()
    {
        string htmlContent = @"
        <!DOCTYPE html>
        <html>
        <head>
            <meta name='viewport' content='initial-scale=1.0, user-scalable=no' />
            <style>
                #map {
                    height: 100vh; /* Full viewport height */
                    width: 100vw; /* Full viewport width */
                    margin: 0;
                    padding: 0;
                }
            </style>
            <link rel='stylesheet' href='https://unpkg.com/leaflet@1.7.1/dist/leaflet.css' />
            <script src='https://unpkg.com/leaflet@1.7.1/dist/leaflet.js'></script>
        </head>
        <body>
            <div id='map'></div>
            <script>
                var map = L.map('map').setView([51.505, -0.09], 13);
                L.tileLayer('https://tile.openstreetmap.org/{z}/{x}/{y}.png', {
                    maxZoom: 19,
                    attribution: '© OpenStreetMap contributors'
                }).addTo(map);
            </script>
        </body>
        </html>";

        MapWebView.Source = new HtmlWebViewSource { Html = htmlContent };
    }

    private async void OnBackButtonClicked(object sender, EventArgs e)
    {
        await Shell.Current.GoToAsync(nameof(Details));
    }
}