namespace Chat.Views;

public partial class Map : ContentPage
{
    public string GPS { get; set; }

    private readonly FirebaseStorageService _storageService;
    public Map(FirebaseStorageService storageService)
	{
		InitializeComponent();
        GPS = Views.Content.GPS;
        _storageService = storageService;
        LoadMap();

    }

    private async void LoadMap()
    {
        // Assuming GPS is in the format "Latitude: 37.421998333333335, Longitude: -122.084"
        string cleanedGPS = GPS.Replace("Latitude:", "").Replace("Longitude:", "").Trim();

        // Split the coordinates based on comma
        string[] gpsCoordinates = cleanedGPS.Split(',');
        string latitude = gpsCoordinates[0].Trim();
        string longitude = gpsCoordinates[1].Trim();
        //  user locatio 
        string UserLocation = await _storageService.GetGpsLocationAsync();
        cleanedGPS = UserLocation.Replace("Latitude:", "").Replace("Longitude:", "").Trim();
        string[]  UsergpsCoordinates = cleanedGPS.Split(',');
        string Userlatitude = UsergpsCoordinates[0].Trim();
        string Userlongitude = UsergpsCoordinates[1].Trim();
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
                // Initialize the map and set the view to the GPS coordinates
                var map = L.map('map').setView([{latitude}, {longitude}], 13);
                L.tileLayer('https://tile.openstreetmap.org/{{z}}/{{x}}/{{y}}.png', {{
                    maxZoom: 19,
                    attribution: '© OpenStreetMap contributors'
                }}).addTo(map);

                

                // Initialize the second marker for the user's location
                var marker2 =  L.marker([{Userlatitude}, {Userlongitude}]).addTo(map)
                    .bindPopup('User Location')
                    .openPopup();
                // Add a marker at the initial GPS coordinates (Location 1)
                var marker1 = L.marker([{latitude}, {longitude}]).addTo(map)
                    .bindPopup('Challenge')
                    .openPopup();

                // Check if geolocation is available and update the user's location
                if (navigator.geolocation) {{
                    navigator.geolocation.watchPosition(function (position) {{
                        var userLat = position.coords.latitude;
                        var userLon = position.coords.longitude;

                        // Log the user's updated position for debugging
                        console.log('User location updated:', userLat, userLon);

                        // Update the second marker's position
                        marker2.setLatLng([userLat, userLon]);

                        // Optionally, update the popup for the user marker
                        marker2.bindPopup('Your current location').openPopup();

                        // Center the map on the user's location
                        map.setView([userLat, userLon], 13);
                    }}, function (error) {{
                        console.error('Error getting user location:', error);
                    }});
                }} else {{
                    alert('Geolocation is not supported by this browser.');
                }}
            }} catch (error) {{
                document.body.innerHTML = '<p>Map loading error: ' + error.message + '</p>';
            }}
        }});
    </script>
</body>
</html>";

        // Set the WebView's source to the generated HTML content
        MapWebView.Source = new HtmlWebViewSource { Html = htmlContent };
    }
    private async void OnBackButtonClicked(object sender, EventArgs e)
    {
        await Shell.Current.GoToAsync(nameof(Details));
    }
}