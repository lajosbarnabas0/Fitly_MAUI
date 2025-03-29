using CommunityToolkit.Maui;
using Fitly.Pages;
using Fitly.ViewModels;
using Microsoft.Extensions.Logging;

namespace Fitly
{
    public static class MauiProgram
    {
        public static MauiApp CreateMauiApp()
        {
            var builder = MauiApp.CreateBuilder();
            builder
                .UseMauiApp<App>()
                .UseMauiCommunityToolkit()
                .ConfigureFonts(fonts =>
                {
                    fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                    fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
                    fonts.AddFont("Montserrat-Regular.ttf", "Montserrat");
                });

            builder.Services.AddSingleton<MainPage>();
            builder.Services.AddSingleton<MainViewModel>();
            builder.Services.AddSingleton<LoginPage>();
            builder.Services.AddSingleton<LoginViewModel>();
            builder.Services.AddSingleton<RegisterPage>();
            builder.Services.AddSingleton<RegisterViewModel>();
            builder.Services.AddSingleton<ProfilePage>();
            builder.Services.AddSingleton<ProfileViewModel>();
            builder.Services.AddSingleton<LetsBeginPage>();
            builder.Services.AddSingleton<LetsBeginViewModel>();
            builder.Services.AddSingleton<MainCommunityPage>();
            builder.Services.AddSingleton<MainCommunityViewModel>();
            builder.Services.AddSingleton<PostListPage>();
            builder.Services.AddSingleton<PostListViewModel>();

            Microsoft.Maui.Handlers.EntryHandler.Mapper.AppendToMapping(nameof(Entry), (handler, view) =>
            {
#if ANDROID
                handler.PlatformView.SetBackgroundColor(Android.Graphics.Color.Transparent);
#endif
            });

            Microsoft.Maui.Handlers.DatePickerHandler.Mapper.AppendToMapping(nameof(DatePicker), (handler, view) =>
            {
#if ANDROID
                handler.PlatformView.SetBackgroundColor(Android.Graphics.Color.Transparent);
#endif
            });

            Microsoft.Maui.Handlers.PickerHandler.Mapper.AppendToMapping(nameof(Picker), (handler, view) =>
            {
#if ANDROID
                handler.PlatformView.SetBackgroundColor(Android.Graphics.Color.Transparent);
#endif
            });

#if DEBUG
            builder.Logging.AddDebug();
#endif

            return builder.Build();
        }
    }
}
