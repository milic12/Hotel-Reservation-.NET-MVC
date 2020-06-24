﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HotelManagement.ViewModel
{
    public class RoomDetailViewModel
    {
        public int RoomId { get; set; }

        public string RoomNumber { get; set; }

        public string RoomImage { get; set; }

        public decimal RoomPrice { get; set; }

        public string BookingStatus { get; set; }

        public string RoomType { get; set; }

        public int RoomCapacity { get; set; }

        public string RoomDescription { get; set; }
    }


}