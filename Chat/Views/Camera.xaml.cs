namespace Chat.Views;
using SkiaSharp;
using System.IO;
using Microsoft.Maui.Storage;
using Microsoft.Extensions.DependencyInjection;

public partial class Camera : ContentPage
{
    private readonly FirebaseStorageService _firebaseStorageService;
    private string BackgroundImagePath;
    private string OverlayImagePath;
    private string UniqueFileName;
    public Camera(FirebaseStorageService FirebaseStorageService)
	{
		InitializeComponent();
        _firebaseStorageService = FirebaseStorageService;
        BindingContext = this;
    }

    async void OnCaptureBackgroundImage(object sender, EventArgs e)
    {
        var photo = await CapturePhotoAsync();
        if (photo != null)
        {
            BackgroundImage.Source = ImageSource.FromFile(photo.FullPath);
            BackgroundImagePath = photo.FullPath;
        }
    }

    async void OnCaptureOverlayImage(object sender, EventArgs e)
    {
        var photo = await CapturePhotoAsync();
        if (photo != null)
        {
            OverlayImage.Source = ImageSource.FromFile(photo.FullPath);
            OverlayImagePath = photo.FullPath;
            var overlaidImagePath = await OverlayImagesAndSaveFile(BackgroundImagePath, OverlayImagePath);
            ResultImage.Source = ImageSource.FromFile(overlaidImagePath);
            // Upload the saved file to Firebase Storage
            using var fileStream = File.OpenRead(overlaidImagePath);
            var downloadUrl = await _firebaseStorageService.UploadFileAsync(fileStream, UniqueFileName);
        }
    }

    async Task<FileResult> CapturePhotoAsync()
    {
        try
        {
            // Use MediaPicker to take a photo
            var photo = await MediaPicker.CapturePhotoAsync();
            if (photo == null)
                return null;

            // Save the photo to the app's cache directory
            var newFile = Path.Combine(FileSystem.CacheDirectory, photo.FileName);
            using (var stream = await photo.OpenReadAsync())
            using (var newStream = File.OpenWrite(newFile))
            {
                await stream.CopyToAsync(newStream);
            }

            return photo;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"CapturePhotoAsync THREW: {ex.Message}");
            return null;
        }
    }



    async Task<string> OverlayImagesAndSaveFile(string backgroundFilePath, string overlayFilePath)
    {
        using var backgroundBitmap = SKBitmap.Decode(backgroundFilePath);
        using var overlayBitmap = SKBitmap.Decode(overlayFilePath);

        // Create a new surface
        var info = new SKImageInfo(backgroundBitmap.Width, backgroundBitmap.Height);
        using var surface = SKSurface.Create(info);
        var canvas = surface.Canvas;

        // Draw the background image
        canvas.Clear();
        canvas.DrawBitmap(backgroundBitmap, 0, 0);

        // Draw the overlay image with some transparency
        var paint = new SKPaint
        {
            Color = new SKColor(255, 255, 255, 128) // Adjust alpha for transparency
        };
        canvas.DrawBitmap(overlayBitmap, 0, 0, paint);

        // Create an image from the canvas
        using var image = surface.Snapshot();
        using var data = image.Encode(SKEncodedImageFormat.Png, 100);

        // Save the file to cache directory
        UniqueFileName = $"overlaid_image_{DateTime.Now:yyyyMMddHHmmss}.png";
        var filePath = Path.Combine(FileSystem.CacheDirectory, UniqueFileName);
        using var stream = File.OpenWrite(filePath);
        data.SaveTo(stream);
        return filePath; // Return the path to the saved file
    }

    private async void OnBackButtonClicked(object sender, EventArgs e)
    {
        await Shell.Current.GoToAsync(nameof(Views.Navigation));
    }
}