using System.ComponentModel.DataAnnotations.Schema;

namespace LifeMaker.Models.ReferencePrices
{
    public class ReferencePricesStore
    {
        public Int32 Id { get; set; }
        public String Name { get; set; }
        public String Address { get; set; }

        //[InverseProperty(nameof(ReferencePrices.Store))]
        //public virtual ICollection<ReferencePrices> Prices { get; set; }
    }
}
