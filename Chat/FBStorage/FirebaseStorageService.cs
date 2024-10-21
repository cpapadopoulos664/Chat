using Chat.Models;
using Firebase.Auth;
using Firebase.Database;
using Firebase.Database.Query;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Net.Http.Headers;
using static System.Net.Mime.MediaTypeNames;

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

        if (response.IsSuccessStatusCode)
        {
            var jsonResponse = await response.Content.ReadAsStringAsync();
            dynamic result = JsonConvert.DeserializeObject(jsonResponse);
            var RecoverPath = $"https://firebasestorage.googleapis.com/v0/b/mobileapp-1556e.appspot.com/o/pictures%2F{_authClient.User.Uid}%2F{fileName}?alt=media&token={result.downloadTokens}";
            _firebaseClient.Child("Content").Child(_authClient.User.Uid).PostAsync(new Challenge
            {
                PhotoUrl = RecoverPath,
                UID = _authClient.User.Uid,
                Username = currentUser.ToString()
            });
            return result.mediaLink; // Return the download URL of the uploaded file
        }
        var errorContent = await response.Content.ReadAsStringAsync();
        throw new Exception($"File upload to Firebase Storage failed. Error: {errorContent}");
    }

}
