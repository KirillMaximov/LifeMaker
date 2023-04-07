using LifeMaker.DataAccess;
using Microsoft.AspNetCore.Mvc;

namespace LifeMaker.Controllers
{
    public class KnowledgeController : Controller
    {
        KnowledgeBaseDB db = new KnowledgeBaseDB();

        [HttpGet]
        public ActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public JsonResult Get(String searchText)
        {
            var model = db.Knowledge_Base.Where(p => p.IsActive);

            if (!String.IsNullOrEmpty(searchText))
            {
                model = model.Where(p => 
                    p.Name!.ToLower().Contains(searchText) ||
                    p.Description!.ToLower().Contains(searchText) ||
                    p.Link!.ToLower().Contains(searchText)
                );
            }

            return Json(model);
        }
    }
}
