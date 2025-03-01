using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using WinToucher.ViewModels;

namespace WinToucher.Views
{
    public partial class MainWindow : Window
    {
        private MainViewModel ViewModel => DataContext as MainViewModel;
        private bool _isDrawing = false;

        public MainWindow()
        {
            InitializeComponent();
            WindowState = WindowState.Maximized;
        }

        private void OverlayCanvas_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (!ViewModel.IsOverlayActive) return;

            _isDrawing = true;
            ViewModel.StartDrawing(e.GetPosition(OverlayCanvas), MapMouseButton(e.ChangedButton));
            e.Handled = true;
        }

        private void OverlayCanvas_MouseMove(object sender, MouseEventArgs e)
        {
            if (!_isDrawing || !ViewModel.IsOverlayActive) return;

            ViewModel.Draw(e.GetPosition(OverlayCanvas));
            e.Handled = true;
        }

        private void OverlayCanvas_MouseUp(object sender, MouseButtonEventArgs e)
        {
            _isDrawing = false;
            e.Handled = true;
        }

        private static int MapMouseButton(MouseButton button)
        {
            return button switch
            {
                MouseButton.Left => 0,
                MouseButton.Right => 1,
                MouseButton.Middle => 2,
                _ => throw new System.NotImplementedException(),
            };
        }
    }
}