using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Fitly.API;
using Windows.Services.Maps;

namespace Fitly.ViewModels
{
    public partial class LoginViewModel : ObservableObject
    {
        private readonly HTTPRequest _apiService = new HTTPRequest();

        [RelayCommand]
        async Task IfNoAccountRegistered()
        {
            await Shell.Current.GoToAsync("//RegisterPage");
        }

        [ObservableProperty]
        private string name;

        [ObservableProperty]
        private int age;

        [ObservableProperty]
        private string responseMessage;

        public MyViewModel()
        {
            SendDataCommand = new AsyncRelayCommand(SendDataAsync);
        }

        public IAsyncRelayCommand SendDataCommand { get; }

        private async Task SendDataAsync()
        {
            var data = new { Name, Age };
            var result = await ApiService.Post<ResponseModel>("https://example.com/api/data", data);

            ResponseMessage = result != null ? $"Siker: {result.Message}" : "Sikertelen küldés!";
        }
    }

    public class ResponseModel
    {
        public string Message { get; set; }
    }
}
}
