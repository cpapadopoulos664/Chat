using Chat.Models;
using Firebase.Database;
using Firebase.Database.Query;
using System.Collections.ObjectModel;

namespace Chat.Views;

public partial class ChallengesOnMap : ContentPage
{
    private readonly FirebaseClient firebaseClient;

    public ObservableCollection<Models.Challenge> GpsLocations { get; set; } = new ObservableCollection<Models.Challenge>();
    public ChallengesOnMap(FirebaseClient FirebaseClient)
	{
		InitializeComponent();
        firebaseClient = FirebaseClient;
        BindingContext = this;
    }

    protected override async void OnNavigatedTo(NavigatedToEventArgs args)
    {
        base.OnNavigatedTo(args);
        await LoadData(); // gia klisi me to pou anixi h forma 
    }

    public async Task LoadData()
    {
        var GpsData = await firebaseClient
        .Child("Content")
        .OnceAsync<Challenge>();

        foreach (var Data in GpsData)
        {
            GpsLocations.Add(Data.Object);
        }

        int count = 0;
        var off = 0;
        string HtmlPoints = string.Empty;
        string HtmlPoint = string.Empty;
        var Centerlatitude =0.0;
        var Centerlongitude = 0.0;
        foreach (var Data in GpsLocations)
        {
            var cleanedGPS = Data.GPS.ToString();
            cleanedGPS = cleanedGPS.Replace("Latitude:", "").Replace("Longitude:", "").Trim();
            string[] gpsCoordinates = cleanedGPS.Split(',');
            string latitude = gpsCoordinates[0].Trim();
            string longitude = gpsCoordinates[1].Trim();
            Centerlatitude = Convert.ToDouble(latitude) + Centerlatitude;
            Centerlongitude = Convert.ToDouble(longitude) + Centerlongitude;
            count = count + 1;
            HtmlPoint = $@" var marker{count} =  L.marker([{latitude}, {longitude}]).addTo(map)
                    .bindPopup('Point{count}')
                    .openPopup();";
            HtmlPoints = HtmlPoint + "\n" + HtmlPoints;
        }

        Centerlatitude = Centerlatitude / count;
        Centerlongitude = Centerlongitude / count;


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
                var map = L.map('map').setView([{Centerlatitude}, {Centerlongitude}], 5);
                L.tileLayer('https://tile.openstreetmap.org/{{z}}/{{x}}/{{y}}.png', {{
                    maxZoom: 19,
                    attribution: '© OpenStreetMap contributors'
                }}).addTo(map);
                {HtmlPoints}
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
        await Shell.Current.GoToAsync(nameof(Views.Navigation));
    }
}