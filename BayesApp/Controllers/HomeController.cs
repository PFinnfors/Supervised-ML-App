using BayesApp.Models;
using System.Web.Mvc;

namespace BayesApp.Controllers
{
    public class HomeController : Controller
    {
        TennisModel tennis = new TennisModel();
        
        // GET: Home
        public ActionResult Index()
        {
            return View();
        }

        // POST: Home
        [HttpPost]
        public ActionResult Index(TennisModel tennis, bool useless = true)
        {
            if (ModelState.IsValid)
            {
                using (var tennisCtrl = new TennisController())
                {
                    //
                    tennis.TennisAnswer = tennisCtrl.GetTennisAnswer(tennis);
                    return View(tennis);
                }
            }
            else
            {
                //Returns the page telling the user to input all the required parameters
                return View(tennis);
            }
        }
    }
}