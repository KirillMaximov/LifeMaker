using Microsoft.AspNetCore.Mvc;

namespace LifeMaker.Controllers
{
    public class CreditCalculatorController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public JsonResult Calculate(Double creditAmount, Double yearPercent, Double monthAmount)
        {
            //creditAmount = 5 000 000
            //yearPercent = 11,4%
            //monthAmount = 150 000

            var monthCount = 0;

            while (creditAmount > 0)
            {
                creditAmount = creditAmount + (creditAmount * (yearPercent / 100 / 12)) - monthAmount;
                monthCount++;
            }

            var result = new
            {
                yearCount = monthCount / 12,
                monthCount = monthCount - ((monthCount / 12) * 12)
            };

            return Json(result);
        }
    }
}
