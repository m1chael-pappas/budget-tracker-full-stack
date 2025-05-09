namespace BudgetTrackerUI.Models
{
    public class Budget
    {
        public int CategoryId { get; set; }
        public string MonthYear { get; set; } = string.Empty;
        public double AllocatedAmount { get; set; }

        // For display purposes
        public string CategoryName { get; set; } = string.Empty;

        public override string ToString()
        {
            return $"Budget [Category: {CategoryName}, Month/Year: {MonthYear}, " +
                   $"Allocated Amount: {AllocatedAmount:F2}]";
        }
    }
}