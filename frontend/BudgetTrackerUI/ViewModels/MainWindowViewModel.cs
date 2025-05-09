using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using Avalonia.Threading;
using BudgetTrackerUI.Models;
using BudgetTrackerUI.Services;

namespace BudgetTrackerUI.ViewModels
{
    public class MainWindowViewModel : INotifyPropertyChanged
    {
        private readonly DataService _dataService;
        private string _selectedMonthYear = string.Empty;
        private double _totalIncome;
        private double _totalExpense;
        private double _balance;
        private ObservableCollection<Transaction> _recentTransactions = new();
        private ObservableCollection<string> _monthYears = new();
        private TransactionsViewModel _transactionsViewModel;

        public event PropertyChangedEventHandler? PropertyChanged;

        protected void OnPropertyChanged([CallerMemberName] string? propertyName = null)
        {
            Dispatcher.UIThread.Post(() =>
            {
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
            });
        }

        protected bool SetField<T>(ref T field, T value, [CallerMemberName] string? propertyName = null)
        {
            if (Equals(field, value)) return false;
            field = value;
            OnPropertyChanged(propertyName);
            return true;
        }

        public MainWindowViewModel()
        {
            // Initialize data service
            _dataService = new DataService("../data");

            // Create sample data if needed
            _dataService.CreateSampleData();

            // Initialize the TransactionsViewModel
            _transactionsViewModel = new TransactionsViewModel();

            InitializeMonthSelector();
            RefreshDashboard();
        }

        public TransactionsViewModel TransactionsViewModel
        {
            get => _transactionsViewModel;
            set => SetField(ref _transactionsViewModel, value);
        }

        public ObservableCollection<string> MonthYears
        {
            get => _monthYears;
            set => SetField(ref _monthYears, value);
        }

        public string SelectedMonthYear
        {
            get => _selectedMonthYear;
            set
            {
                if (SetField(ref _selectedMonthYear, value))
                {
                    RefreshDashboard();
                }
            }
        }

        public double TotalIncome
        {
            get => _totalIncome;
            set => SetField(ref _totalIncome, value);
        }

        public double TotalExpense
        {
            get => _totalExpense;
            set => SetField(ref _totalExpense, value);
        }

        public double Balance
        {
            get => _balance;
            set => SetField(ref _balance, value);
        }

        public ObservableCollection<Transaction> RecentTransactions
        {
            get => _recentTransactions;
            set => SetField(ref _recentTransactions, value);
        }

        private void InitializeMonthSelector()
        {
            Dispatcher.UIThread.Post(() =>
            {
                _monthYears.Clear();

                var currentDate = DateTime.Now;

                // Add current month and 11 previous months
                for (int i = 0; i < 12; i++)
                {
                    var date = currentDate.AddMonths(-i);
                    var monthYear = date.ToString("yyyy-MM");
                    _monthYears.Add(monthYear);
                }

                _selectedMonthYear = _monthYears.Count > 0 ? _monthYears[0] : DateTime.Now.ToString("yyyy-MM");
            });
        }

        public void RefreshDashboard()
        {
            if (string.IsNullOrEmpty(_selectedMonthYear)) return;

            try
            {
                // Update summary values
                var income = _dataService.GetTotalIncome(_selectedMonthYear);
                var expense = _dataService.GetTotalExpense(_selectedMonthYear);

                Dispatcher.UIThread.Post(() =>
                {
                    TotalIncome = income;
                    TotalExpense = expense;
                    Balance = income - expense;

                    Console.WriteLine($"Updated dashboard - Income: {TotalIncome}, Expense: {TotalExpense}, Balance: {Balance}");
                });

                // Update recent transactions list
                var transactions = _dataService.GetTransactionsByMonth(_selectedMonthYear)
                    .OrderByDescending(t => t.Date)
                    .Take(10)
                    .ToList();

                Dispatcher.UIThread.Post(() =>
                {
                    _recentTransactions.Clear();

                    foreach (var transaction in transactions)
                    {
                        _recentTransactions.Add(transaction);
                    }

                    Console.WriteLine($"Loaded {_recentTransactions.Count} recent transactions");
                });
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error refreshing dashboard: {ex.Message}");
            }
        }
    }
}