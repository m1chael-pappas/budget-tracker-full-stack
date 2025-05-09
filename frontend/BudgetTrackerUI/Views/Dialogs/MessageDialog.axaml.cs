using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;

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
            Close(true);
        }

        private void OnSecondaryClicked(object sender, RoutedEventArgs e)
        {
            Close(false);
        }
    }
}