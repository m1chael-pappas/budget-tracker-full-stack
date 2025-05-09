using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;
using BudgetTrackerUI.ViewModels;

namespace BudgetTrackerUI.Views
{
    public partial class MainWindow : Window
    {
        private MainWindowViewModel? ViewModel => DataContext as MainWindowViewModel;

        public MainWindow()
        {
            InitializeComponent();

            if (DataContext is MainWindowViewModel vm)
            {
                vm.TransactionsViewModel.SetHostWindow(this);
            }
        }

        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);
        }

        private void OnRefreshClick(object? sender, RoutedEventArgs e)
        {
            ViewModel?.RefreshDashboard();
        }
    }
}