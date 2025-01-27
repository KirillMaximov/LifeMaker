using LifeMaker.DataAccess;
using LifeMaker.Models.NiceTime;
using LifeMaker.Models.ReferencePrices;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace LifeMaker.Controllers
{
    public class NicetimeController : Controller
    {
        NiceTimeDB db = new NiceTimeDB();
        public IActionResult Index()
        {
            var model = db.Nice_time.Where(p => p.IsActive ?? true).ToList();

            return View(model);
        }

        [HttpPost]
        public JsonResult Save(String newLink, String newName, String newDescription)
        {
            var insertRow = new NiceTime();

            insertRow.Link = newLink;
            insertRow.Name = newName;
            insertRow.Description = newDescription;
            insertRow.IsActive = true;

            db.Nice_time.Add(insertRow);
            db.SaveChanges();

            return Json(true);
        }

        [HttpPost]
        public JsonResult Delete(Int32 linkId)
        {
            var deleteLink = db.Nice_time.FirstOrDefault(p => p.Id == linkId);

            db.Nice_time.Remove(deleteLink);
            db.SaveChanges();  

            return Json(true);
        }
    }
}
