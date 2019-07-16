using ProjetParking.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ProjetParking.Controllers
{
    public class HomeController : Controller
    {
        private Context db = new Context();

        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Index(int userId = 0, string parkingName = "")
        {
            try
            {
                UserParking parked = new UserParking { UserID = userId, ParkingName = parkingName, ParkDate = DateTime.Now };

                db.UserParkings.Add(parked);

                db.SaveChanges();

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}