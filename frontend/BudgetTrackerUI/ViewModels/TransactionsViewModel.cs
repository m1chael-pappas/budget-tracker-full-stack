using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reactive;
using System.Threading.Tasks;
using Avalonia.Controls;
using Avalonia.Threading;
using BudgetTrackerUI.Models;
using BudgetTrackerUI.Services;
using BudgetTrackerUI.Views.Dialogs;
using ReactiveUI;

namespace BudgetTrackerUI.ViewModels
{
    public class TransactionsViewModel : ReactiveViewModelBase
    {
        private readonly DataService _dataService;
        private ObservableCollection<Transaction> _allTransactions = new();
        private ObservableCollection<Transaction> _filteredTransactions = new();
        private ObservableCollection<string> _monthYears = new();
        private ObservableCollection<Category> _filterCategories = new();
        private string _selectedMonthYear = string.Empty;
        private Category? _selectedFilterCategory;
        private Transaction? _selectedTransaction;
        private Window? _hostWindow;

        public TransactionsViewModel(Window? hostWindow = null)
        {
            _hostWindow = hostWindow;
            _dataService = new DataService("../data");

            // Initialize commands using our thread-safe wrappers
            AddTransactionCommand = CreateAsyncCommand(AddTransactionAsync);
            EditTransactionCommand = CreateAsyncCommand(EditTransactionAsync);
            DeleteTransactionCommand = CreateAsyncCommand(DeleteTransactionAsync);
            ClearFiltersCommand = CreateCommand(ClearFilters);

            // Load data
            InitializeMonthSelector();
            LoadCategories();
            LoadTransactions();
        }

        public ObservableCollection<Transaction> FilteredTransactions
        {
            get => _filteredTransactions;
            set => SetField(ref _filteredTransactions, value);
        }

        public ObservableCollection<string> MonthYears
        {
            get => _monthYears;
            set => SetField(ref _monthYears, value);
        }

        public ObservableCollection<Category> FilterCategories
        {
            get => _filterCategories;
            set => SetField(ref _filterCategories, value);
        }

        public string SelectedMonthYear
        {
            get => _selectedMonthYear;
            set
            {
                if (SetField(ref _selectedMonthYear, value))
                {
                    ApplyFilters();
                }
            }
        }

        public Category? SelectedFilterCategory
        {
            get => _selectedFilterCategory;
            set
            {
                if (SetField(ref _selectedFilterCategory, value))
                {
                    ApplyFilters();
                }
            }
        }

        public Transaction? SelectedTransaction
        {
            get => _selectedTransaction;
            set => SetField(ref _selectedTransaction, value);
        }

        public ReactiveCommand<Unit, Unit> AddTransactionCommand { get; }
        public ReactiveCommand<Unit, Unit> EditTransactionCommand { get; }
        public ReactiveCommand<Unit, Unit> DeleteTransactionCommand { get; }
        public ReactiveCommand<Unit, Unit> ClearFiltersCommand { get; }

        public void SetHostWindow(Window? window)
        {
            _hostWindow = window;
        }

        private void InitializeMonthSelector()
        {
            Dispatcher.UIThread.Post(() =>
            {
                _monthYears.Clear();

                var currentDate = DateTime.Now;

                // First add "All Months" option
                _monthYears.Add("All");

                // Add current month and 11 previous months
                for (int i = 0; i < 12; i++)
                {
                    var date = currentDate.AddMonths(-i);
                    var monthYear = date.ToString("yyyy-MM");
                    _monthYears.Add(monthYear);
                }

                _selectedMonthYear = _monthYears.Count > 1 ? _monthYears[1] : "All"; // Default to current month
            });
        }

        private void LoadCategories()
        {
            Dispatcher.UIThread.Post(() =>
            {
                _filterCategories.Clear();

                // First add "All Categories" option
                var allCategory = new Category { Id = 0, Name = "All Categories" };
                _filterCategories.Add(allCategory);

                // Add all categories from database
                foreach (var category in _dataService.GetAllCategories())
                {
                    _filterCategories.Add(category);
                }

                _selectedFilterCategory = _filterCategories.Count > 0 ? _filterCategories[0] : null; // Default to "All Categories"
            });
        }

        private void LoadTransactions()
        {
            var transactions = _dataService.GetAllTransactions();

            Dispatcher.UIThread.Post(() =>
            {
                _allTransactions.Clear();

                foreach (var transaction in transactions)
                {
                    _allTransactions.Add(transaction);
                }

                ApplyFilters();
            });
        }

        private void ApplyFilters()
        {
            Dispatcher.UIThread.Post(() =>
            {
                _filteredTransactions.Clear();

                var filtered = _allTransactions.AsEnumerable();

                // Apply month filter if not "All"
                if (!string.IsNullOrEmpty(_selectedMonthYear) && _selectedMonthYear != "All")
                {
                    filtered = filtered.Where(t => t.Date.StartsWith(_selectedMonthYear));
                }

                // Apply category filter if not "All Categories"
                if (_selectedFilterCategory != null && _selectedFilterCategory.Id != 0)
                {
                    filtered = filtered.Where(t => t.CategoryId == _selectedFilterCategory.Id);
                }

                // Sort by date descending
                filtered = filtered.OrderByDescending(t => t.Date);

                // Update filtered collection
                foreach (var transaction in filtered)
                {
                    _filteredTransactions.Add(transaction);
                }
            });
        }

        private void ClearFilters()
        {
            if (_monthYears.Count > 0)
                SelectedMonthYear = _monthYears[0]; // "All"

            if (_filterCategories.Count > 0)
                SelectedFilterCategory = _filterCategories[0]; // "All Categories"
        }

        private async Task AddTransactionAsync()
        {
            if (_hostWindow == null) return;

            var dialog = new TransactionDialog(null, false);

            var result = await dialog.ShowDialog<bool?>(_hostWindow);
            if (result == true && dialog.Result != null)
            {
                var transaction = dialog.Result;
                _dataService.AddTransaction(transaction);
                LoadTransactions();
            }
        }

        private async Task EditTransactionAsync()
        {
            if (_hostWindow == null || _selectedTransaction == null) return;

            var dialog = new TransactionDialog(_selectedTransaction, true);

            var result = await dialog.ShowDialog<bool?>(_hostWindow);
            if (result == true && dialog.Result != null)
            {
                var transaction = dialog.Result;
                _dataService.UpdateTransaction(transaction);
                LoadTransactions();
            }
        }

        private async Task DeleteTransactionAsync()
        {
            if (_hostWindow == null || _selectedTransaction == null) return;

            var messageBox = new MessageDialog
            {
                Title = "Confirm Delete",
                Message = $"Are you sure you want to delete the transaction '{_selectedTransaction.Description}'?",
                PrimaryButtonText = "Delete",
                SecondaryButtonText = "Cancel"
            };

            var result = await messageBox.ShowDialog<bool?>(_hostWindow);
            if (result == true)
            {
                _dataService.DeleteTransaction(_selectedTransaction.Id);
                LoadTransactions();
            }
        }
    }
}