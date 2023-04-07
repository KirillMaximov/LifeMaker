using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.ComponentModel.DataAnnotations.Schema;
using System.Security.Policy;
using System.Xml.Linq;

namespace LifeMaker.Models.ReferencePrices
{
    public class ReferencePrices
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public decimal? Price { get; set; }
        public int? StoreId { get; set; }
        public int? CategoryId { get; set; }
        public DateTime? CreateDate { get; set; }
        public DateTime? UpdateDate { get; set; }
        public bool? IsActive { get; set; }
        public int? OrderBy { get; set; }

        [ForeignKey(nameof(StoreId))]
        public virtual ReferencePricesStore? Store { get; set; }

        [ForeignKey(nameof(CategoryId))]
        public virtual ReferencePricesCategory? Category { get; set; }

        public List<SelectListItem> CategoryDDL { get; set; }
        public List<SelectListItem> StoreDDL { get; set; }

        public ReferencePrices()
        {
            CategoryDDL = new List<SelectListItem>();
            StoreDDL = new List<SelectListItem>();
        }
    }
}
