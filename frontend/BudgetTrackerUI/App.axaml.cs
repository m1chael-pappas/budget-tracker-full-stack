using Avalonia;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Markup.Xaml;
using BudgetTrackerUI.ViewModels;
using BudgetTrackerUI.Views;
using System;

namespace BudgetTrackerUI
{
    public partial class App : Application
    {
        private MainWindowViewModel? _mainViewModel;

        public override void Initialize()
        {
            AvaloniaXamlLoader.Load(this);
        }

        public override void OnFrameworkInitializationCompleted()
        {
            if (ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
            {
                // Create the view model
                _mainViewModel = new MainWindowViewModel();

                // Set up main window
                desktop.MainWindow = new MainWindow
                {
                    DataContext = _mainViewModel,
                };

                // Register for shutdown
                desktop.ShutdownRequested += OnShutdownRequested;
                desktop.Exit += OnExit;
            }

            base.OnFrameworkInitializationCompleted();
        }

        private void OnShutdownRequested(object? sender, ShutdownRequestedEventArgs e)
        {
            try
            {
                Console.WriteLine("Application shutdown requested - cleaning up resources");
                _mainViewModel?.Cleanup();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error during shutdown cleanup: {ex.Message}");
            }
        }

        private void OnExit(object? sender, EventArgs e)
        {
            try
            {
                Console.WriteLine("Application exiting - performing final cleanup");
                _mainViewModel?.Cleanup();
                _mainViewModel = null;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error during exit cleanup: {ex.Message}");
            }
        }
    }
}