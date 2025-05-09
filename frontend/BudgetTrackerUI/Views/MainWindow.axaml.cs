using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;
using Avalonia.Threading;
using BudgetTrackerUI.ViewModels;
using System;

namespace BudgetTrackerUI.Views
{
    public partial class MainWindow : Window
    {
        private MainWindowViewModel? ViewModel => DataContext as MainWindowViewModel;

        public MainWindow()
        {
            InitializeComponent();

            try
            {
                if (DataContext is MainWindowViewModel vm)
                {
                    Console.WriteLine("Setting host window in MainWindow constructor");
                    vm.TransactionsViewModel.SetHostWindow(this);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error in MainWindow constructor: {ex.Message}");
            }

            // Add extra handler to ensure window is set
            this.Activated += (s, e) =>
            {
                Dispatcher.UIThread.Post(() =>
                {
                    try
                    {
                        if (DataContext is MainWindowViewModel vm)
                        {
                            Console.WriteLine("Setting host window in MainWindow.Activated");
                            vm.TransactionsViewModel.SetHostWindow(this);
                        }
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"Error in MainWindow.Activated: {ex.Message}");
                    }
                });
            };
        }

        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);
        }

        private void OnRefreshClick(object? sender, RoutedEventArgs e)
        {
            try
            {
                Console.WriteLine("Refresh button clicked");
                ViewModel?.RefreshDashboard();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error in OnRefreshClick: {ex.Message}");
            }
        }
    }
}