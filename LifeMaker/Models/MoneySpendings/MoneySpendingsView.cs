namespace LifeMaker.Models.MoneySpendings
{
    public class MoneySpendingsView
    {
        public int Id { get; set; }
        public decimal? Amount { get; set; }
        public string Description { get; set; }
        public string Person { get; set; }
        public string Category { get; set; }
        public DateTime? CreateDate { get; set; }
        public string CreateDateString { get; set; }
        public int? PersonId { get; set; }
        public int? CategoryId { get; set; }
    }
}
