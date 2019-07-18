using Microsoft.AspNet.Identity;
using ProjetParking.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;

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

        public ActionResult Stats()
        {

            string userId = User.Identity.GetUserId();

            int nbParkingsVisitedUser = db.UserParkings.Where(p => p.UserId == userId).Count();
            int nbParkingsVisited = db.UserParkings.Count();
            int nbUsers = db.Users.Count();

            /**/
            var classementUser = (from p in db.UserParkings
                      group p by p.UserId into g
                      select new
                      {
                          Id = g.Key,
                          Count = g.Count()
                      }).Where(y => y.Id != null)
                      .OrderByDescending(z => z.Count);

            int positionUser = 1;
            foreach (var item in classementUser)
            {
                if (item.Id == User.Identity.GetUserId())
                {
                    break;
                }
                positionUser++;
            }
            /**/

            ViewBag.NbParkingsVisitedUser = nbParkingsVisitedUser;
            ViewBag.NbParkingsVisited = nbParkingsVisited;
            ViewBag.NbUsers = nbUsers;
            ViewBag.ClassementUser = positionUser + (positionUser == 1 ? "er" : "ème");

            return View();
        }

        public void GetFrequentations()
        {
            string userId = User.Identity.GetUserId();

            var classementParkingUser = (from p in db.UserParkings
                                         group p by p.ParkingName into g
                                         select new
                                         {
                                             Id = g.Key,
                                             Count = g.Where(t => t.UserId == userId).Count()
                                         }).Where(y => y.Count > 0).OrderBy(z => z.Count).ToList();

            System.Diagnostics.Debug.WriteLine(classementParkingUser);
            var jsonSerialiser = new JavaScriptSerializer();
            var json = jsonSerialiser.Serialize(classementParkingUser);


            Response.Clear();
            Response.ContentType = "application/json; charset=utf-8";
            Response.Write(json);
            Response.End();
        }
    }
}