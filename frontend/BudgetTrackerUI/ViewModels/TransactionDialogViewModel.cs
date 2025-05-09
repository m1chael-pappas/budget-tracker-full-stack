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
    public class TransactionDialogViewModel : INotifyPropertyChanged
    {
        private readonly DataService _dataService;
        private readonly bool _isEdit;
        private readonly int _transactionId;

        private DateTimeOffset _transactionDate;
        private double _amount;
        private string _description = string.Empty;
        private Category? _selectedCategory;
        private bool _isIncome;
        private ObservableCollection<Category> _categories = new();

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

        public TransactionDialogViewModel(Transaction? transaction, bool isEdit)
        {
            _dataService = new DataService("../data");
            _isEdit = isEdit;

            // Load categories
            LoadCategories();

            if (transaction != null)
            {
                _transactionId = transaction.Id;
                try
                {
                    // Convert string date to DateTimeOffset
                    _transactionDate = DateTimeOffset.Parse(transaction.Date);
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error parsing date {transaction.Date}: {ex.Message}");
                    _transactionDate = DateTimeOffset.Now;
                }

                _amount = transaction.Amount;
                _description = transaction.Description;
                _isIncome = transaction.IsIncome;
                _selectedCategory = _categories.FirstOrDefault(c => c.Id == transaction.CategoryId);
            }
            else
            {
                _transactionId = 0;
                _transactionDate = DateTimeOffset.Now;
                _amount = 0;
                _description = string.Empty;
                _isIncome = false;
                _selectedCategory = _categories.FirstOrDefault();
            }
        }

        private void LoadCategories()
        {
            var categoriesList = _dataService.GetAllCategories();

            Dispatcher.UIThread.Post(() =>
            {
                _categories.Clear();

                foreach (var category in categoriesList)
                {
                    _categories.Add(category);
                }

                // Re-assign selected category if it was set previously
                if (_selectedCategory != null)
                {
                    _selectedCategory = _categories.FirstOrDefault(c => c.Id == _selectedCategory.Id);
                }
                else
                {
                    _selectedCategory = _categories.FirstOrDefault();
                }
            });
        }

        public string WindowTitle => _isEdit ? "Edit Transaction" : "Add New Transaction";

        public DateTimeOffset TransactionDate
        {
            get => _transactionDate;
            set => SetField(ref _transactionDate, value);
        }

        public double Amount
        {
            get => _amount;
            set
            {
                if (SetField(ref _amount, value))
                {

                    OnPropertyChanged(nameof(CanSave));
                }
            }
        }

        public string Description
        {
            get => _description;
            set
            {
                if (SetField(ref _description, value))
                {

                    OnPropertyChanged(nameof(CanSave));
                }
            }
        }

        public Category? SelectedCategory
        {
            get => _selectedCategory;
            set
            {
                if (SetField(ref _selectedCategory, value))
                {

                    OnPropertyChanged(nameof(CanSave));
                }
            }
        }

        public bool IsIncome
        {
            get => _isIncome;
            set => SetField(ref _isIncome, value);
        }

        public ObservableCollection<Category> Categories
        {
            get => _categories;
            set => SetField(ref _categories, value);
        }

        public bool CanSave
        {
            get
            {
                bool hasAmount = Amount > 0;
                bool hasDescription = !string.IsNullOrWhiteSpace(Description);
                bool hasCategory = SelectedCategory != null;

                Console.WriteLine($"CanSave: Amount > 0: {hasAmount}, Has Description: {hasDescription}, Has Category: {hasCategory}");

                return hasAmount && hasDescription && hasCategory;
            }
        }

        public Transaction GetTransaction()
        {
            return new Transaction
            {
                Id = _transactionId,
                Date = _transactionDate.DateTime.ToString("yyyy-MM-dd"),
                Amount = _amount,
                Description = _description,
                CategoryId = _selectedCategory?.Id ?? 0,
                IsIncome = _isIncome,
                CategoryName = _selectedCategory?.Name ?? string.Empty
            };
        }
    }
}