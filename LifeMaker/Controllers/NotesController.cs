using LifeMaker.DataAccess;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace LifeMaker.Controllers
{
    public class NotesController : Controller
    {
        NotesDB db = new NotesDB();
        public IActionResult Index()
        {
            var model = db.Notes.Where(p => p.IsActive);

            var users = db.Notes.Select(p => p.Sender).Distinct().ToList();

            var SelectUsers = new List<SelectListItem>();

            foreach (var user in users)
            {
                SelectUsers.Add(new SelectListItem() { Text = user, Value = user });
            }

            ViewBag.Users = SelectUsers;

            return View(model);
        }

        [HttpGet]
        public JsonResult Get(String searchText, String sortText, String userText)
        {
            var model = db.Notes.Where(p => p.IsActive);

            if (!String.IsNullOrEmpty(userText))
            {
                model = model.Where(p => p.Sender == userText);
            }

            if (!String.IsNullOrEmpty(searchText))
            {
                model = model.Where(p => p.Text.ToLower().Contains(searchText));
            }

            if (!String.IsNullOrEmpty(sortText))
            {
                switch (sortText)
                {
                    case "1": model = model.Where(p => p.Period == "Срочная"); break; 
                    case "2": model = model.Where(p => p.Period == "Средняя"); break; 
                    case "3": model = model.Where(p => p.Period == "Подождет"); break; 
                    default: break;
                }
            }

            return Json(model);
        }

        [HttpPost]
        public JsonResult Delete(Int32 Id)
        {
            var note = db.Notes.FirstOrDefault(p => p.Id== Id);
            db.Notes.Remove(note!);
            db.SaveChanges();
            return Json(true);
        }
    }
}
