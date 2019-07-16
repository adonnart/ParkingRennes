using ProjetParking.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ProjetParking.Controllers
{
    public class UserParkingController : Controller
    {
        private Context db = new Context();

        // GET: UserParking
        public ActionResult Index()
        {
            return View();
        }

        // GET: UserParking/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: UserParking/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: UserParking/Create
        [HttpPost]
        public ActionResult Create(int userId, string parkingName, DateTime parkDate)
        {
            try
            {
                UserParking parked = new UserParking { UserID = userId, ParkingName = parkingName, ParkDate = parkDate };

                db.UserParkings.Add(parked);

                db.SaveChanges();

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        // GET: UserParking/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: UserParking/Edit/5
        [HttpPost]
        public ActionResult Edit(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add update logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        // GET: UserParking/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: UserParking/Delete/5
        [HttpPost]
        public ActionResult Delete(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add delete logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }
    }
}
