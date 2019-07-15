using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ProjetParking.Models
{
    public class UserParking
    {
        private int userId;
        private string parkingName;
        private DateTime parkDate;

        public int UserId
        {
            get { return userId; }
            set { userId = value; }
        }

        public string ParkingName
        {
            get { return parkingName; }
            set { parkingName = value; }
        }

        public DateTime ParkDate
        {
            get { return parkDate; }
            set { parkDate = value; }
        }

        public UserParking()
        {

        }
    }
}