namespace LifeMaker.Models.NiceTime
{
    public class NiceTime
    {
        public Int32 Id { get; set; }
        public String Link { get; set; }
        public String Name { get; set; }
        public String Description { get; set; }
        public Boolean? IsActive { get; set; }
        public Int32? OrderBy { get; set; }
    }
}
