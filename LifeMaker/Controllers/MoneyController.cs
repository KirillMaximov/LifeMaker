using LifeMaker.DataAccess;
using LifeMaker.Models.MoneySpendings;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query.Internal;
using System;
using System.Globalization;
using System.Net;
using System.Xml;
using Yandex.Checkout.V3;

namespace LifeMaker.Controllers
{
    public class MoneyController : Controller
    {
        MoneySpendingsDB db = new MoneySpendingsDB();

        [HttpGet]
        public ActionResult Index()
        {
            var model = new MoneySpendings();

            foreach (var category in db.Money_Spendings_Category)
            {
                model.CategoryDDL.Add(new SelectListItem { Text = category.Name, Value = category.Id.ToString() });
            }

            foreach (var person in db.Money_Spendings_Person)
            {
                model.PersonDDL.Add(new SelectListItem { Text = person.Name, Value = person.Id.ToString() });
            }

            return View(model);
        }

        string uniqid(string prefix, bool more_entropy)
        {
            if (string.IsNullOrEmpty(prefix))
                prefix = string.Empty;

            if (!more_entropy)
            {
                return (prefix + System.Guid.NewGuid().ToString()).Substring(0, 13);
            }
            else
            {
                return (prefix + System.Guid.NewGuid().ToString() + System.Guid.NewGuid().ToString()).Substring(0, 23);
            }
        }

        [HttpGet]
        public JsonResult Get(int monthNumber, int? pageSize, int? pageNumber, string searchText, string sortName, string sortOrder)
        {
            var minDate = GetMinDate(monthNumber);
            var maxDate = GetMaxDate(monthNumber);

            var query = db.Money_Spendings.Include(p => p.Person).Include(p => p.Category).Where(p => p.CreateDate > minDate && p.CreateDate < maxDate );

            query = query.OrderByDescending(p => p.CreateDate);

            if (!String.IsNullOrEmpty(sortName))
            {
                if (sortOrder == "asc")
                {
                    switch (sortName)
                    {
                        case "id": query = query.OrderBy(p => p.Id); break;
                        case "amount": query = query.OrderBy(p => p.Amount); break;
                        case "description": query = query.OrderBy(p => p.Description); break;
                        case "category.name": query = query.OrderBy(p => p.Category!.Name); break;
                        case "person.name": query = query.OrderBy(p => p.Person!.Name); break;
                        case "createDate": query = query.OrderBy(p => p.CreateDate); break;
                        default: break;
                    }
                }
                else if (sortOrder == "desc")
                {
                    switch (sortName)
                    {
                        case "id": query = query.OrderByDescending(p => p.Id); break;
                        case "amount": query = query.OrderByDescending(p => p.Amount); break;
                        case "description": query = query.OrderByDescending(p => p.Description); break;
                        case "category.name": query = query.OrderByDescending(p => p.Category!.Name); break;
                        case "person.name": query = query.OrderByDescending(p => p.Person!.Name); break;
                        case "createDate": query = query.OrderByDescending(p => p.CreateDate); break;
                        default: break;
                    }
                }
            }

            if (!String.IsNullOrEmpty(searchText))
            {
                query = query.Where(p =>
                    p.Amount!.ToString()!.ToLower().Contains(searchText.ToLower()) ||
                    p.Description!.ToLower().Contains(searchText.ToLower()) ||
                    p.Category!.Name!.ToLower().Contains(searchText.ToLower()) ||
                    p.Person!.Name!.ToLower().Contains(searchText.ToLower())
                );
            }

            var total = query.Count();
            int start = (pageNumber!.Value - 1) * pageSize!.Value;
            var rows = query.Skip(start).Take(pageSize.Value).ToList();

            return Json(new { rows, total });
        }

        [HttpPost]
        public JsonResult Create(MoneySpendings model)
        {
            db.Money_Spendings.Add(model);
            db.SaveChanges();

            return Json(true);
        }

        [HttpGet]
        public JsonResult GetLineChart(Int32 monthNumber)
        {
            var minDate = GetMinDate(monthNumber);
            var maxDate = GetMaxDate(monthNumber);

            var query = db.Money_Spendings.Where(p => p.CreateDate > minDate && p.CreateDate < maxDate).OrderBy(p => p.CreateDate).ToList();

            List<String> ChartDates= new List<String>();
            List<Decimal?> ChartAmounts = new List<Decimal?>();    
            List<Int32> ChartLimitations = new List<Int32>();

            DateTime? supportDate = null;

            var days = GetDatesForChart(minDate.AddDays(1), maxDate.AddDays(-1));

            foreach (var item in query)
            {
                if (supportDate == item.CreateDate!.Value.Date)
                {
                    ChartAmounts[ChartAmounts.Count - 1] = ChartAmounts[ChartAmounts.Count - 1] + item.Amount;
                }
                else
                {
                    var day = item.CreateDate.Value.Day;
                    var month = item.CreateDate.Value.Month.ToString().Length == 2 ? item.CreateDate.Value.Month.ToString() : "0" + item.CreateDate.Value.Month.ToString();
                    ChartDates.Add($"{day}.{month}");
                    ChartAmounts.Add(item.Amount);
                    ChartLimitations.Add(500);

                    supportDate = item.CreateDate.Value.Date;
                }
            }

            var result = new
            {
                category = ChartDates,
                amount = ChartAmounts,
                statics = ChartLimitations
            };

            return Json(result);
        }

        [HttpGet]
        public JsonResult GetDoughnutChart(Int32 monthNumber)
        {
            var minDate = GetMinDate(monthNumber);
            var maxDate = GetMaxDate(monthNumber);

            var query = db.Money_Spendings.Include(p => p.Person).Include(p => p.Category).Where(p => p.CreateDate > minDate && p.CreateDate < maxDate).OrderBy(p => p.CreateDate).ToList();
            var categories = db.Money_Spendings_Category;

            List<String> ChartDates = new List<String>();
            List<Decimal?> ChartAmounts = new List<Decimal?>();

            foreach (var cat in categories)
            {
                String currentCategory = cat.Name!;
                Decimal? carrentAmount = 0;
                foreach (var item in query)
                {
                    if (cat.Id == item.CategoryId)
                    {
                        carrentAmount += item.Amount;
                    }
                }
                ChartDates.Add($"{currentCategory}");
                ChartAmounts.Add(carrentAmount);
            }

            var result = new
            {
                category = ChartDates,
                amount = ChartAmounts
            };

            return Json(result);
        }

        [HttpGet]
        public JsonResult GetProgressBars(Int32 monthNumber)
        {
            var minDate = GetMinDate(monthNumber);
            var maxDate = GetMaxDate(monthNumber);

            var query = db.Money_Spendings.Where(p => p.CreateDate > minDate && p.CreateDate < maxDate).ToList();

            var progressBars = new List<ProgressBar>();

            var persons = db.Money_Spendings_Person;

            foreach (var person in persons)
            {
                var personQuery = query.Where(p => p.PersonId == person.Id);
                decimal? sum = 0;
                foreach (var item in personQuery)
                {
                    sum += item.Amount;
                }
                var progressBar = sum / 10000 * 100;

                progressBars.Add(new ProgressBar() { Person = person.Name, Percent = progressBar, Amount = (int)sum });
            }

            return Json(progressBars);
        }

        public List<DateTime> GetDatesForChart(DateTime startDate, DateTime finishDate)
        {
            List<DateTime> result = new List<DateTime>();

            while (startDate <= finishDate)
            {
                result.Add(startDate);
                startDate = startDate.AddDays(1);
            }

            return result;
        }

        public DateTime GetMinDate()
        {
            var currentDate = DateTime.Now; 
            var paymentDay = 8;

            return new DateTime(currentDate.Month == 1 ? currentDate.Year - 1 : currentDate.Year, currentDate.Month - 1, paymentDay - 1);

            if (currentDate.Day < paymentDay)
            {
                return new DateTime(currentDate.Month == 1 ? currentDate.Year - 1 : currentDate.Year, currentDate.Month - 1, paymentDay - 1); //8/23 день получения ЗП
            }
            else if (currentDate.Day >= paymentDay)
            {
                return new DateTime(currentDate.Year, currentDate.Month, paymentDay - 1); //8/23 день получения ЗП
            }
            else
            {
                return DateTime.Now;
            }
        }
        
        public DateTime GetMinDate(Int32 monthNumber)
        {
            var currentDate = new DateTime(2023, monthNumber, 1);
            var paymentDay = 8;

            return new DateTime(currentDate.Year, currentDate.Month, paymentDay);
        }
        
        public DateTime GetMaxDate(Int32 monthNumber)
        {
            var currentDate = new DateTime(2023, monthNumber, 1);
            var paymentDay = 8;

            return new DateTime(currentDate.Month == 12 ? currentDate.Year + 1 : currentDate.Year, currentDate.Month == 12 ? 1 : currentDate.Month + 1, paymentDay);
        }
    }
}
