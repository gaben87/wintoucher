using Microsoft.Extensions.DependencyInjection;
using System;
using System.Windows;
using WinToucher.ViewModels;

namespace WinToucher.Locators
{
    public class ViewModelLocator
    {
        private IServiceProvider ServiceProvider => ((App)Application.Current).ServiceProvider
            ?? throw new InvalidOperationException("ServiceProvider not initialized in App.");

        public MainViewModel MainViewModel => ServiceProvider.GetRequiredService<MainViewModel>();
    }
}