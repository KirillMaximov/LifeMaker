using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Security.Policy;
using System.Xml.Linq;

namespace LifeMaker.Models.MoneySpendings
{
    public class MoneySpendings
    {
        public Int32 Id { get; set; }
        public Decimal? Amount { get; set; }
        public String? Description { get; set; }
        public Int32? PersonId { get; set; }
        public Int32? CategoryId { get; set; }
        public DateTime? CreateDate { get; set; }
        public virtual MoneySpendingsPerson? Person { get; set; }
        public virtual MoneySpendingsCategory? Category { get; set; }
        public List<SelectListItem> CategoryDDL { get; set; }
        public List<SelectListItem> PersonDDL { get; set; }

        public MoneySpendings()
        {
            CategoryDDL = new List<SelectListItem>();
            PersonDDL = new List<SelectListItem>();
        }
    }
}
