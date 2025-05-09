
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reactive;
using Avalonia.Controls;
using ReactiveUI;
using BudgetTrackerUI.Models;
using BudgetTrackerUI.Services;

namespace BudgetTrackerUI.ViewModels
{
    public class MainWindowViewModel : ViewModelBase
    {
        private readonly DataService _dataService;
        private string _selectedMonthYear = string.Empty;  // Initialize to avoid CS8618
        private double _totalIncome;
        private double _totalExpense;
        private double _balance;
        private ObservableCollection<Transaction> _recentTransactions;
        private ObservableCollection<string> _monthYears;
        private ReactiveCommand<Unit, Unit> _refreshDashboardCommand;

        public MainWindowViewModel()
        {
            // Initialize data service - point to the same data directory used by C++ backend
            _dataService = new DataService("../data");

            // Initialize properties
            _monthYears = new ObservableCollection<string>();
            _recentTransactions = new ObservableCollection<Transaction>();

            // Initialize command
            _refreshDashboardCommand = ReactiveCommand.Create(() => RefreshDashboard());

            InitializeMonthSelector();
            RefreshDashboard();
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

        public ReactiveCommand<Unit, Unit> RefreshDashboardCommand => _refreshDashboardCommand;

        private void InitializeMonthSelector()
        {
            var currentDate = DateTime.Now;

            // Add current month and 11 previous months
            for (int i = 0; i < 12; i++)
            {
                var date = currentDate.AddMonths(-i);
                var monthYear = date.ToString("yyyy-MM");
                _monthYears.Add(monthYear);
            }

            _selectedMonthYear = _monthYears.FirstOrDefault() ?? DateTime.Now.ToString("yyyy-MM");
        }

        public void RefreshDashboard()
        {
            // Update summary values
            TotalIncome = _dataService.GetTotalIncome(_selectedMonthYear);
            TotalExpense = _dataService.GetTotalExpense(_selectedMonthYear);
            Balance = TotalIncome - TotalExpense;

            // Update recent transactions list
            var transactions = _dataService.GetTransactionsByMonth(_selectedMonthYear)
                .OrderByDescending(t => t.Date)
                .Take(10)
                .ToList();

            RecentTransactions.Clear();
            foreach (var transaction in transactions)
            {
                RecentTransactions.Add(transaction);
            }
        }
    }
}