namespace Chat.Views;

public partial class Map : ContentPage
{
    public string GPS { get; set; }
    public Map()
	{
		InitializeComponent();
        GPS = Views.Content.GPS;
        LoadMap();

    }

    private void LoadMap()
    {
        // Assuming GPS is in the format "Latitude: 37.421998333333335, Longitude: -122.084"
        // Remove the words "Latitude:" and "Longitude:"
        string cleanedGPS = GPS.Replace("Latitude:", "").Replace("Longitude:", "").Trim();

        // Split the coordinates based on comma
        string[] gpsCoordinates = cleanedGPS.Split(',');
        string latitude = gpsCoordinates[0].Trim();
        string longitude = gpsCoordinates[1].Trim();

        // Use the cleaned latitude and longitude values in the HTML
        string htmlContent = $@"
    <!DOCTYPE html>
    <html>
    <head>
        <meta name='viewport' content='initial-scale=1.0, user-scalable=no' />
        <style>
            #map {{
                height: 100vh;
                width: 100vw;
                margin: 0;
                padding: 0;
            }}
        </style>
        <link rel='stylesheet' href='https://unpkg.com/leaflet@1.7.1/dist/leaflet.css' />
        <script src='https://unpkg.com/leaflet@1.7.1/dist/leaflet.js'></script>
    </head>
    <body>
        <div id='map'></div>
        <script>
            document.addEventListener('DOMContentLoaded', function () {{
                try {{
                    var map = L.map('map').setView([{latitude}, {longitude}], 13);
                    L.tileLayer('https://tile.openstreetmap.org/{{z}}/{{x}}/{{y}}.png', {{
                        maxZoom: 19,
                        attribution: '© OpenStreetMap contributors'
                    }}).addTo(map);

                    // Add a marker at the GPS coordinates
                    L.marker([{latitude}, {longitude}]).addTo(map)
                        .bindPopup('You are here')
                        .openPopup();
                }} catch (error) {{
                    document.body.innerHTML = '<p>Map loading error: ' + error.message + '</p>';
                }}
            }});
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