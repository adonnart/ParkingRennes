using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ProjetParking.Models
{
    public class ParkingStat
    {
        [Key]
        public int ParkingStatId { get; set; }

        public string ParkingName { get; set; }

        public int NbPlacesLibres { get; set; }

        public int NbPlacesTotal { get; set; }

        public DateTime StatDate { get; set; }
    }
}