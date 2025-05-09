using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using BudgetTrackerUI.ViewModels;

namespace BudgetTrackerUI.Views
{
    public partial class MainWindow : Window
    {
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
    }
}