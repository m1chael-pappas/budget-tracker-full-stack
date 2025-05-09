namespace BudgetTrackerUI.Models
{
    public class Transaction
    {
        public int Id { get; set; }
        public string Date { get; set; } = string.Empty;
        public double Amount { get; set; }
        public string Description { get; set; } = string.Empty;
        public int CategoryId { get; set; }
        public bool IsIncome { get; set; }

        // For display purposes
        public string CategoryName { get; set; } = string.Empty;

        public override string ToString()
        {
            return $"Transaction [ID: {Id}, Date: {Date}, Amount: {Amount:F2}, " +
                   $"Description: {Description}, Category: {CategoryName}, " +
                   $"Type: {(IsIncome ? "Income" : "Expense")}]";
        }
    }
}