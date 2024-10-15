using CommunityToolkit.Mvvm.ComponentModel;

namespace RectangleArrangeApp2.ViewModels
{
    public partial class MainViewModel : ViewModelBase
    {
        [ObservableProperty]
        private string _greeting = "Welcome to Avalonia!";
    }
}
