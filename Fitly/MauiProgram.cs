using CommunityToolkit.Maui;
using Fitly.Pages;
using Fitly.ViewModels;
using Microsoft.Extensions.Logging;
using Plugin.Maui.Pedometer;

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
            builder.Services.AddSingleton<RecipeListPage>();
            builder.Services.AddSingleton<RecipeListViewModel>();
            builder.Services.AddSingleton<RecipeDetailPage>();
            builder.Services.AddSingleton<RecipeDetailViewModel>();
            builder.Services.AddSingleton<PostDetailPage>();
            builder.Services.AddSingleton<PostDetailViewModel>();
            builder.Services.AddSingleton<NewRecipePage>();
            builder.Services.AddSingleton<NewRecipeViewModel>();
            builder.Services.AddSingleton<NewPostPage>();
            builder.Services.AddSingleton<NewPostViewModel>();
            builder.Services.AddSingleton<CaloriePage>();
            builder.Services.AddSingleton<CalorieViewModel>();
            builder.Services.AddSingleton<ChangePage>();
            builder.Services.AddSingleton<ChangeViewModel>();
            builder.Services.AddSingleton<NewMealPage>();
            builder.Services.AddSingleton<NewMealViewModel>();
            builder.Services.AddSingleton<OwnPostListPage>();
            builder.Services.AddSingleton<OwnPostListViewModel>();
            builder.Services.AddSingleton<OwnRecipeListPage>();
            builder.Services.AddSingleton<OwnRecipeListViewModel>();
            builder.Services.AddSingleton<StepCounterPage>();
            builder.Services.AddSingleton<StepCounterViewModel>();
            builder.Services.AddSingleton(Pedometer.Default);


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
