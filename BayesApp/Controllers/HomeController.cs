using BayesApp.Models;
using System.Web.Mvc;

namespace BayesApp.Controllers
{
    public class HomeController : Controller
    {
        TennisModel tennis = new TennisModel();

        // GET: Home
        public ActionResult Index(TennisModel tennis)
        {
            return View(tennis);
        }

        // POST: Home
        [HttpPost]
        public ActionResult Index(TennisModel tennis, bool posted = false)
        {
            if (ModelState.IsValid)
            {
                using (var tennisCtrl = new TennisController())
                {
                    tennisCtrl.GetTennisAnswer(tennis);
                    return View(tennis);
                }
            }
            else
            {
                return View(tennis);
            }
        }
    }
}