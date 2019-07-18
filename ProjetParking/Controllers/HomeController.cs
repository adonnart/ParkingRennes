using Microsoft.AspNet.Identity;
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
        private ApplicationDbContext db = new ApplicationDbContext();

        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Index(string parkingName)
        {
            try
            {
                string userId = User.Identity.GetUserId();

                if (userId != "")
                {
                    UserParking parked = new UserParking { UserId = userId, ParkingName = parkingName, ParkDate = DateTime.Now };

                    db.UserParkings.Add(parked);

                    db.SaveChanges();
                }

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        public ActionResult Profil()
        {
            List<UserParking> visitedParkings = new List<UserParking>();

            string userId = User.Identity.GetUserId();

            visitedParkings = db.UserParkings.Where(p => p.UserId == userId).ToList();

            return View(visitedParkings);
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }

        public ActionResult Login()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }

        public ActionResult Register()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}