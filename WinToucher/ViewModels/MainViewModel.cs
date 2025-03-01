using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using WinToucher.Models;
using WinToucher.Services;

namespace WinToucher.ViewModels
{
    public partial class MainViewModel : ObservableObject
    {
        private readonly ITouchInjectionService _touchService;
        private readonly ObservableCollection<TouchPath> _touchPaths = [];

        private CancellationTokenSource? _cts;

        [ObservableProperty]
        private bool _isOverlayActive;

        [ObservableProperty]
        private string _message;

        public bool TopMost => IsOverlayActive && !Debugger.IsAttached; // Disble topmost while debugging

        public ObservableCollection<TouchPath> TouchPaths => _touchPaths;

        public MainViewModel(ITouchInjectionService touchService)
        {
            _touchService = touchService;
            _touchService.Initialize(2);
        }

        public void StartDrawing(Point position, int pointerId)
        {
            _touchPaths.Add(new TouchPath { PointerId = pointerId });
            _touchPaths[^1].AddNow(position);
        }

        public void Draw(Point position)
        {
            _touchPaths[^1].AddNow(position);
        }

        [RelayCommand]
        private void ToggleOverlay()
        {
            _cts?.Cancel(); // Cancel any existing task
            IsOverlayActive = !IsOverlayActive;
        }

        [RelayCommand]
        private void ClearPaths()
        {
            _cts?.Cancel(); // Cancel any existing task
            _touchPaths.Clear();
            OnPropertyChanged(nameof(TouchPaths));
        }

        [RelayCommand]
        private void ExecutePaths()
        {
            _cts?.Cancel(); // Cancel any existing task
            _cts = new CancellationTokenSource();

            Task.Run(async () =>
            {
                try
                {
                    await SimulateTouchesAsync(_cts.Token);
                }
                catch (Exception ex)
                {
                    Message = $"An error occurred: {ex.Message}";
                }
            });

            IsOverlayActive = false;
        }

        [RelayCommand]
        private void Exit()
        {
            Application.Current.Shutdown();
        }

        private async Task SimulateTouchesAsync(CancellationToken token)
        {
            foreach (var path in _touchPaths)
            {
                if (path.Points.Count == 0) continue;

                long startTime = path.Timestamps[0];
                _touchService.InjectTouchDown(path.PointerId, path.Points[0]);

                for (int i = 1; i < path.Points.Count; i++)
                {
                    token.ThrowIfCancellationRequested(); // Allows cancellation

                    long elapsed = path.Timestamps[i] - startTime;
                    long previousElapsed = path.Timestamps[i - 1] - startTime;
                    int sleepTime = (int)(elapsed - previousElapsed);
                    if (sleepTime > 0)
                    {
                        await Task.Delay(sleepTime, token); // Non-blocking sleep
                    }

                    _touchService.InjectTouchMove(path.PointerId, path.Points[i]);
                }

                _touchService.InjectTouchUp(path.PointerId, path.Points[^1]);
            }
        }
    }
}