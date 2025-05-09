using Avalonia;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;
using BudgetTrackerUI.Models;
using BudgetTrackerUI.ViewModels;
using System;

namespace BudgetTrackerUI.Views.Dialogs
{
    public partial class TransactionDialog : Window
    {
        private TransactionDialogViewModel _viewModel;

        public Transaction? Result { get; private set; }

        public TransactionDialog(Transaction? transaction, bool isEdit)
        {
            InitializeComponent();

            _viewModel = new TransactionDialogViewModel(transaction, isEdit);
            DataContext = _viewModel;
        }

        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);
        }

        private void OnSaveClicked(object? sender, RoutedEventArgs e)
        {
            try
            {
                Console.WriteLine("Save button clicked in TransactionDialog");
                Console.WriteLine($"Transaction details - Description: {_viewModel.Description}, Amount: {_viewModel.Amount}, CanSave: {_viewModel.CanSave}");

                Result = _viewModel.GetTransaction();
                Console.WriteLine($"Created transaction: {Result.Description}, Amount: {Result.Amount}");
                Close(true);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error in OnSaveClicked: {ex.Message}");
            }
        }

        private void OnCancelClicked(object? sender, RoutedEventArgs e)
        {
            Console.WriteLine("Cancel button clicked in TransactionDialog");
            Close(false);
        }
    }
}