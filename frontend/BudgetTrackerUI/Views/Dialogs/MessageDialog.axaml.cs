using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;
using System;

namespace BudgetTrackerUI.Views.Dialogs
{
    public partial class MessageDialog : Window
    {
        public string Message { get; set; } = string.Empty;
        public string PrimaryButtonText { get; set; } = "OK";
        public string SecondaryButtonText { get; set; } = "Cancel";

        public MessageDialog()
        {
            InitializeComponent();
            DataContext = this;
        }

        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);
        }

        private void OnPrimaryClicked(object sender, RoutedEventArgs e)
        {
            try
            {
                Console.WriteLine("Primary button clicked in MessageDialog");
                Close(true);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error in OnPrimaryClicked: {ex.Message}");
            }
        }

        private void OnSecondaryClicked(object sender, RoutedEventArgs e)
        {
            try
            {
                Console.WriteLine("Secondary button clicked in MessageDialog");
                Close(false);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error in OnSecondaryClicked: {ex.Message}");
            }
        }
    }
}