namespace BudgetTrackerUI.Models
{
    public class Category
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public string Color { get; set; } = "#000000";

        public override string ToString()
        {
            return $"Category [ID: {Id}, Name: {Name}, Description: {Description}, Color: {Color}]";
        }
    }
}