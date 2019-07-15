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

        //POST : UserParking
        [HttpPost]
        public ActionResult Index(int userId, string parkingName, DateTime parkDate)
        {
            UserParking parked = new UserParking { UserId = userId, ParkingName = parkingName, ParkDate = parkDate };

            db.UserParkings.Add(parked);

            db.SaveChanges();

            return RedirectToAction("Index");
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