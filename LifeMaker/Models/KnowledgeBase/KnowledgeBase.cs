namespace LifeMaker.Models.KnowledgeBase
{
    public class KnowledgeBase
    {
        public Int32 Id { get; set; }
        public String? Name { get; set; }
        public String? Description { get; set; }
        public String? Link { get; set; }
        public Boolean IsActive { get; set; }
    }
}
