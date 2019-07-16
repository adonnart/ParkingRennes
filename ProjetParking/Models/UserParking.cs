using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ProjetParking.Models
{
    public class UserParking
    {
        [Key]
        public int UserID { get; set; }

        public string ParkingName { get; set; }

        public DateTime ParkDate { get; set; }
    }
}