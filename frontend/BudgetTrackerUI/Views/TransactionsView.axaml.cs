using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using BudgetTrackerUI.ViewModels;
using System;

namespace BudgetTrackerUI.Views
{
    public partial class TransactionsView : UserControl
    {
        public TransactionsView()
        {
            InitializeComponent();

            this.AttachedToVisualTree += (sender, e) =>
            {
                if (DataContext is TransactionsViewModel vm)
                {
                    var window = this.VisualRoot as Window;
                    vm.SetHostWindow(window);
                }
            };
        }

        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);
        }
    }
}