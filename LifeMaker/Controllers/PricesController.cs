using LifeMaker.DataAccess;
using LifeMaker.Models.ReferencePrices;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace LifeMaker.Controllers;

public class PricesController : Controller
{
    ReferencePricesDB db = new ReferencePricesDB();

    [HttpGet]
    public ActionResult Index() 
    {
        var model = new ReferencePrices();

        foreach (var category in db.Reference_Prices_Category)
        {
            model.CategoryDDL.Add(new SelectListItem { Text = category.Name, Value = category.Id.ToString() });
        }

        foreach (var store in db.Reference_Prices_Store)
        {
            model.StoreDDL.Add(new SelectListItem { Text = store.Name, Value = store.Id.ToString() });
        }

        return View(model);
    }

    [HttpGet]
    public JsonResult Get(String searchText, Int32 searchCategory, Int32 searchStore)
    {
        List<ReferencePrices> model = new List<ReferencePrices>();

        using (var db = new ReferencePricesDB())
        {
            var query = db.Reference_Prices.Include(p => p.Category).Include(p => p.Store).Where(p => p.IsActive!.Value);

            if (!String.IsNullOrEmpty(searchText))
            {
                query = query.Where(p => 
                    p.Name.ToLower().Contains(searchText) ||
                    p.Price.ToString()!.ToLower().Contains(searchText) ||
                    p.Store!.Name.ToLower().Contains(searchText) ||
                    p.Category!.Name.ToLower().Contains(searchText)
                );
            }
            if (searchCategory != 0)
            {
                query = query.Where(p => p.Category!.Id == searchCategory);
            }
            if (searchStore != 0)
            {
                query = query.Where(p => p.Store!.Id == searchStore);
            }

            var total = query.Count();
            model = query.Skip(0).Take(total).ToList();
        }

        return Json(model);
    }

    [HttpPost]
    public JsonResult Create(ReferencePrices model, String newStore)
    {
        if (!String.IsNullOrEmpty(newStore))
        {
            db.Reference_Prices_Store.Add(new ReferencePricesStore() { Name = newStore });
            db.SaveChanges();

            model.StoreId = db.Reference_Prices_Store.FirstOrDefault(p => p.Name == newStore)!.Id;
        }

        model.CategoryId = model.CategoryId ?? 0;
        model.StoreId = model.StoreId ?? 0;

        model.CreateDate = DateTime.Now;
        model.UpdateDate = DateTime.Now;
        model.IsActive = true;

        db.Reference_Prices.Add(model);
        db.SaveChanges();

        return Json(true);
    }

    [HttpPost]
    public JsonResult Delete(Int32 Id)
    {
        var price = db.Reference_Prices.FirstOrDefault(p => p.Id == Id);
        price!.IsActive = false;
        db.SaveChanges();

        return Json(true);
    }

    [HttpPost]
    public JsonResult Save(Int32 Id)
    { 
        return Json(true); 
    }
}

#region Methods for DataTable

//[HttpPost]
//public JsonResult Get(int? page, int? limit, string sortBy, string direction)
//{
//    page = 1; limit = 10;

//    var query = db.Reference_Prices.Include(p => p.Store).Include(p => p.Category).OrderBy(p => p.CreateDate);

//    if (!String.IsNullOrEmpty(sortBy))
//    {
//        if (direction.Trim().ToLower() == "asc")
//        {
//            switch (sortBy.Trim().ToLower())
//            {
//                case "price": query = query.OrderBy(p => p.Price); break;
//                case "1": break;
//                case "2": break;
//            }
//        }
//        else 
//        {
//            switch (sortBy.Trim().ToLower())
//            {
//                case "price": query = query.OrderByDescending(p => p.Price); break;
//                case "1": break;
//                case "2": break;
//            }
//        }
//    }

//    int total = query.Count();

//    int start = (page.Value - 1) * limit.Value;

//    List<ReferencePrices> model = query.Skip(start).Take(limit.Value).ToList();

//    var records = new List<ReferencePricesView>();

//    foreach (var item in model)
//    {
//        var price = new ReferencePricesView()
//        {
//            Id = item.Id,
//            Name = item.Name,
//            Price = item.Price,
//            StoreId = item.StoreId,
//            CategoryId = item.CategoryId,
//            Store = item.Store.Name,
//            Category = item.Category.Name,
//            CreateDate = item.CreateDate,
//            UpdateDate = item.UpdateDate,
//            IsActive = item.IsActive,
//            OrderBy = item.OrderBy
//        };
//        records.Add(price);
//    }

//    var returnObj = new
//    {
//        draw = page,
//        recordsTotal = total,
//        recordsFiltered = total,
//        data = records
//    };

//    return this.Json(returnObj);
//}

//public JsonResult GetStores()
//{
//    var stores = db.Reference_Prices_Store.Select(p => new ReferencePricesStore()
//    {
//        Id = p.Id,
//        Name = p.Name
//    }).ToList();

//    return Json(stores);
//}

//public JsonResult GetCategories()
//{
//    var categories = db.Reference_Prices_Category.Select(p => new ReferencePricesCategory()
//    {
//        Id = p.Id,
//        Name = p.Name
//    }).ToList();
//    return Json(categories);
//}

#endregion
