using Microsoft.AspNet.Identity;
using ProjetParking.Models;
using System;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Web;
using System.Web.Mvc;
using System.Net.Http.Formatting;
using System.Collections.Generic;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Threading.Tasks;

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
                    UserParking parked = new UserParking {
                        UserId = userId,
                        ParkingName = parkingName,
                        ParkDate = DateTime.Now
                    };

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

        private const string URL = "https://data.rennesmetropole.fr/api/records/1.0/search";
        private string urlParameters = "?dataset=export-api-parking-citedia";

        public void GetStats()
        {
            HttpClient client = new HttpClient();
            client.BaseAddress = new Uri(URL);

            // Add an Accept header for JSON format.
            client.DefaultRequestHeaders.Accept.Add(
                new MediaTypeWithQualityHeaderValue("application/json"));
            
            // List data response.
            HttpResponseMessage response = client.GetAsync(urlParameters).Result;  // Blocking call! Program will wait here until a response is received or a timeout occurs.
            if (response.IsSuccessStatusCode)
            {
                var stats = response.Content.ReadAsStringAsync().Result;
                System.Diagnostics.Debug.WriteLine(stats);
                var data = (JObject)JsonConvert.DeserializeObject(stats);

                foreach (var r in data["records"])
                {
                    ParkingStat parkingStat = new ParkingStat
                    {
                        ParkingName = (string)r["fields"]["key"],
                        NbPlacesLibres = (int)r["fields"]["free"],
                        NbPlacesTotal = (int)r["fields"]["max"],
                        StatDate = DateTime.Now
                    };

                    db.ParkingStats.Add(parkingStat);

                    db.SaveChanges();
                }
            }
            else
            {
                //System.Diagnostics.Debug.WriteLine(r["fields"]["key"]);
                System.Diagnostics.Debug.WriteLine("{0} ({1})", (int)response.StatusCode, response.ReasonPhrase);
            }

            client.Dispose();
        }
        /*
        [HttpPost]
        public ActionResult Stats(ParkingStats ps)
        {
            List<UserParking> visitedParkings = new List<UserParking>();

            string userId = User.Identity.GetUserId();

            visitedParkings = db.UserParkings.Where(p => p.UserId == userId).ToList();

            return View(visitedParkings);
        }
        */
        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }

        public ActionResult Login()
        {
            return View();
        }

        public ActionResult Register()
        {
            return View();
        }

        public ActionResult Stats()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}