using Avalonia;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;
using Avalonia.Threading;
using BudgetTrackerUI.ViewModels;
using System;

namespace BudgetTrackerUI.Views
{
    public partial class TransactionsView : UserControl
    {
        private TransactionsViewModel? ViewModel => DataContext as TransactionsViewModel;
        private Window? _parentWindow;

        public TransactionsView()
        {
            InitializeComponent();

            this.AttachedToVisualTree += (sender, e) =>
            {
                _parentWindow = this.VisualRoot as Window;
                if (ViewModel != null && _parentWindow != null)
                {
                    ViewModel.SetHostWindow(_parentWindow);
                }
            };
        }

        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);
        }

        private void OnClearFiltersClick(object? sender, RoutedEventArgs e)
        {
            if (ViewModel != null)
            {
                ViewModel.ClearFilters();
            }
        }

        private async void OnAddClick(object? sender, RoutedEventArgs e)
        {
            if (ViewModel != null)
            {
                try
                {
                    // Make sure we have the parent window
                    if (_parentWindow == null)
                    {
                        _parentWindow = this.VisualRoot as Window;
                        ViewModel.SetHostWindow(_parentWindow);
                    }

                    await Dispatcher.UIThread.InvokeAsync(async () =>
                    {
                        Console.WriteLine("Add button clicked, calling AddTransactionAsync");
                        await ViewModel.AddTransactionAsync();
                    });
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error in OnAddClick: {ex.Message}\n{ex.StackTrace}");
                }
            }
        }

        private async void OnEditClick(object? sender, RoutedEventArgs e)
        {
            if (ViewModel != null)
            {
                try
                {
                    // Make sure we have the parent window
                    if (_parentWindow == null)
                    {
                        _parentWindow = this.VisualRoot as Window;
                        ViewModel.SetHostWindow(_parentWindow);
                    }

                    await Dispatcher.UIThread.InvokeAsync(async () =>
                    {
                        Console.WriteLine("Edit button clicked, calling EditTransactionAsync");
                        await ViewModel.EditTransactionAsync();
                    });
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error in OnEditClick: {ex.Message}\n{ex.StackTrace}");
                }
            }
        }

        private async void OnDeleteClick(object? sender, RoutedEventArgs e)
        {
            if (ViewModel != null)
            {
                try
                {
                    // Make sure we have the parent window
                    if (_parentWindow == null)
                    {
                        _parentWindow = this.VisualRoot as Window;
                        ViewModel.SetHostWindow(_parentWindow);
                    }

                    await Dispatcher.UIThread.InvokeAsync(async () =>
                    {
                        Console.WriteLine("Delete button clicked, calling DeleteTransactionAsync");
                        await ViewModel.DeleteTransactionAsync();
                    });
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error in OnDeleteClick: {ex.Message}\n{ex.StackTrace}");
                }
            }
        }
    }
}