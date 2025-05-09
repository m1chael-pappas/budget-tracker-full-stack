
using Avalonia.Data.Converters;
using System;
using System.Globalization;

namespace BudgetTrackerUI.Converters
{
    public class BoolToTypeConverter : IValueConverter
    {
        public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
        {
            if (value is bool isIncome)
            {
                return isIncome ? "Income" : "Expense";
            }
            return "Unknown";
        }

        public object? ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
        {
            if (value is string str)
            {
                return str == "Income";
            }
            return false;
        }
    }
}