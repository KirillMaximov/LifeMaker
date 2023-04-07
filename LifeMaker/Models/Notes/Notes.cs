namespace LifeMaker.Models.Notes
{
    public class Notes
    {
        public Int32 Id { get; set; }
        public String Text { get; set; }
        public String Period { get; set; }
        public String Sender { get; set; }
        public DateTime CreateDate { get; set; }
        public Boolean IsActive { get; set; }
    }
}
