using System.ComponentModel.DataAnnotations.Schema;

namespace LifeMaker.Models.ReferencePrices
{
    public class ReferencePricesCategory
    {
        public Int32 Id { get; set; }
        public String Name { get; set; }
        
        //[InverseProperty(nameof(ReferencePrices.Category))]
        //public virtual ICollection<ReferencePrices> Prices { get; set; }
    }
}
