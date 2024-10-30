using Chat.Models;
using Chat.Views;
using Firebase.Auth;
using Firebase.Database;
using Firebase.Database.Query;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Net.Http.Headers;
using static System.Net.Mime.MediaTypeNames;
using Microsoft.Maui.Devices.Sensors;


public class FirebaseStorageService
{
    private readonly FirebaseAuthClient _authClient;
    private readonly FirebaseClient _firebaseClient;
    private const string FirebaseStorageBaseUrl = "https://firebasestorage.googleapis.com/v0/b/mobileapp-1556e.appspot.com/o";

    public FirebaseStorageService(FirebaseAuthClient authClient,FirebaseClient firebaseClient)
    {
        _authClient = authClient;
        _firebaseClient = firebaseClient;
    }

    // Upload File to Firebase Storage
    public async Task<string> UploadFileAsync(Stream fileStream, string fileName, string mimeType = "application/octet-stream")
    {
        var currentUser = _authClient.User;
        if (currentUser == null)
        {
            throw new UnauthorizedAccessException("User is not authenticated.");
        }

        // Fetch ID token for authenticated user
        var idToken = await currentUser.GetIdTokenAsync();

        using var httpClient = new HttpClient();
        var memoryStream = new MemoryStream();
        await fileStream.CopyToAsync(memoryStream);
        var fileBytes = memoryStream.ToArray();
        // Ensure the file is uploaded to the /pictures directory
        // Clean up file name and folder path
        fileName = fileName.Trim().Trim('/'); // Ensure no leading or trailing slashes in the file name
        string firebaseStoragePath = $"pictures/{_authClient.User.Uid}/{fileName}";  // Path within 'pictures' folder
        // Prepare the content as raw byte array
        var byteContent = new ByteArrayContent(fileBytes);
        byteContent.Headers.ContentType = new MediaTypeHeaderValue(mimeType); // Set MIME type of the file

        // Add Firebase Authentication token
        httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", idToken);

        // Send POST request to upload file to the 'pictures' folder
        var response = await httpClient.PostAsync($"{FirebaseStorageBaseUrl}?uploadType=media&name={firebaseStoragePath}", byteContent);

        string Location = await GetGpsLocationAsync();

        if (response.IsSuccessStatusCode)
        {
            var jsonResponse = await response.Content.ReadAsStringAsync();
            dynamic result = JsonConvert.DeserializeObject(jsonResponse);
            var RecoverPath = $"https://firebasestorage.googleapis.com/v0/b/mobileapp-1556e.appspot.com/o/pictures%2F{_authClient.User.Uid}%2F{fileName}?alt=media&token={result.downloadTokens}";
            _firebaseClient.Child("Content").PostAsync(new Challenge
            {
                PhotoUrl = RecoverPath,
                UID = _authClient.User.Uid,
                Username = SignIn.LoggedInUsername,
                GPS = Location
            });
            return result.mediaLink; // Return the download URL of the uploaded file
        }
        var errorContent = await response.Content.ReadAsStringAsync();
        throw new Exception($"File upload to Firebase Storage failed. Error: {errorContent}");
    }

    public async Task<string> GetGpsLocationAsync()
    {
        try
        {
            var location = await Geolocation.GetLocationAsync(new GeolocationRequest
            {
                DesiredAccuracy = GeolocationAccuracy.Best,
                Timeout = TimeSpan.FromSeconds(10)
            });

            if (location != null)
            {
                string locationString = $"Latitude: {location.Latitude}, Longitude: {location.Longitude}";
                return locationString;
            }
        }
        catch (FeatureNotSupportedException)
        {
            return "Location feature not supported on this device.";
        }
        catch (FeatureNotEnabledException)
        {
            return "Location services are not enabled.";
        }
        catch (PermissionException)
        {
            return "Location permission was denied.";
        }
        catch (Exception)
        {
            return "Unable to retrieve location.";
        }

        return "Location not available.";
    }
}
