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
using System.Web.Script.Serialization;
using System.Data.Entity;

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
                ViewBag.Message = "Vous avez choisi le parking " + parkingName;

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

                return View();
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

            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            
            HttpResponseMessage response = client.GetAsync(urlParameters).Result;
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
                System.Diagnostics.Debug.WriteLine("{0} ({1})", (int)response.StatusCode, response.ReasonPhrase);
            }

            client.Dispose();
        }

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

            var parkingList = new List<string>();
            var parkings = (from p in db.ParkingStats
                           select new
                           {
                               Name = p.ParkingName
                           }).Distinct().ToList();

            foreach (var item in parkings)
            {
                parkingList.Add(item.Name);
            }

            ViewBag.NbParkingsVisitedUser = nbParkingsVisitedUser;
            ViewBag.NbParkingsVisited = nbParkingsVisited;
            ViewBag.NbUsers = nbUsers;
            ViewBag.ClassementUser = positionUser + (positionUser == 1 ? "er" : "ème");
            ViewBag.Parkings = parkingList;

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

        public void GetFrequentationParking(string parking, string date)
        {

            DateTime datetime = Convert.ToDateTime(date);

            var parkingList = (from p in db.ParkingStats
                               where p.ParkingName == parking
                               && DbFunctions.TruncateTime(p.StatDate) == DbFunctions.TruncateTime(datetime)
                               group p by p.StatDate.Hour into g
                            select new
                            {
                                Hour = g.Key,
                                Value = g.Average(s => (s.NbPlacesTotal - s.NbPlacesLibres) / (float)s.NbPlacesTotal * 100)
                            });


            var jsonSerialiser = new JavaScriptSerializer();
            var json = jsonSerialiser.Serialize(parkingList);


            Response.Clear();
            Response.ContentType = "application/json; charset=utf-8";
            Response.Write(json);
            Response.End();
        }
    }
}