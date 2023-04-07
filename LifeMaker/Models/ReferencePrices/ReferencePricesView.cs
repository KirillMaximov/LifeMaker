namespace LifeMaker.Models.ReferencePrices
{
    public class ReferencePricesView
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public decimal? Price { get; set; }
        public int? StoreId { get; set; }
        public int? CategoryId { get; set; }
        public string Store { get; set; }
        public string Category { get; set; }
        public DateTime? CreateDate { get; set; }
        public DateTime? UpdateDate { get; set; }
        public bool? IsActive { get; set; }
        public int? OrderBy { get; set; }
    }
}
