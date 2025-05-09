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
        public Transaction? Result { get; private set; }

        public TransactionDialog()
        {
            InitializeComponent();
        }

        public TransactionDialog(Transaction? transaction, bool isEdit)
        {
            InitializeComponent();
            DataContext = new TransactionDialogViewModel(transaction, isEdit);
        }

        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);
        }

        private void OnSaveClicked(object sender, RoutedEventArgs e)
        {
            if (DataContext is TransactionDialogViewModel viewModel)
            {
                Result = viewModel.GetTransaction();
                Close(true);
            }
        }

        private void OnCancelClicked(object sender, RoutedEventArgs e)
        {
            Close(false);
        }
    }
}