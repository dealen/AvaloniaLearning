using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace RectangleArrangeApp2.ViewModels
{
    public partial class MainViewModel : ViewModelBase
    {
        [ObservableProperty]
        private string _greeting = "Welcome to Avalonia!";

        [ObservableProperty]
        public int _canvasWidth = 800;

        [ObservableProperty]
        public int _canvasHeight = 600;

        [ObservableProperty]
        public int _newCanvasWidth;

        [ObservableProperty]
        public int _newCanvasHeight;

        public RelayCommand SetCanvasSize => new(() =>
        {
            CanvasWidth = NewCanvasWidth;
            CanvasHeight = NewCanvasHeight;
        });
    }
}
