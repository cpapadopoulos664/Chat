using Firebase.Auth.Providers;
using Firebase.Auth;
using Microsoft.Extensions.Logging;
using Chat.Views;
using Firebase.Database;

namespace Chat
{
    public static class MauiProgram
    {
        public static MauiApp CreateMauiApp()
        {
            var builder = MauiApp.CreateBuilder();
            builder
                .UseMauiApp<App>()
                .ConfigureFonts(fonts =>
                {
                    fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                    fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
                });

#if DEBUG
    		builder.Logging.AddDebug();
#endif
            builder.Services.AddSingleton(new FirebaseAuthClient(new FirebaseAuthConfig()  
            {
                ApiKey = "AIzaSyC_n48HmdN-eIyyEbZ-JbY1Dx4x_0khJy8",
                AuthDomain = "mobileapp-1556e.firebaseapp.com",
                Providers = [new EmailProvider()]

            }));

            builder.Services.AddSingleton<FirebaseClient>(serviceProvider =>
            {
                // Resolve the auth client
                var authClient = serviceProvider.GetRequiredService<FirebaseAuthClient>();

                // Fetch the ID token from the signed-in user
                return new FirebaseClient("https://mobileapp-1556e-default-rtdb.europe-west1.firebasedatabase.app/",
                    new FirebaseOptions
                    {
                        AuthTokenAsyncFactory = async () =>
                        {
                            // Assume user is signed in and get their ID token
                            var currentUser = authClient.User;
                            if (currentUser != null)
                            {
                                return await currentUser.GetIdTokenAsync();
                            }
                            return null;
                        }
                    });
            });

            builder.Services.AddSingleton<FirebaseStorageService>(serviceProvider =>
            {
                var authClient = serviceProvider.GetRequiredService<FirebaseAuthClient>();

                return new FirebaseStorageService(authClient);
            });

            builder.Services.AddTransient<SignUp>();
            builder.Services.AddTransient<SignIn>();
            builder.Services.AddTransient<GroupChat>();
            builder.Services.AddTransient<Lobby>();
            builder.Services.AddTransient<Navigation>();
            builder.Services.AddTransient<GeneralChat>();
            builder.Services.AddTransient<Camera>();
            builder.Services.AddTransient<Content>();
            return builder.Build();
        }
    }
}
