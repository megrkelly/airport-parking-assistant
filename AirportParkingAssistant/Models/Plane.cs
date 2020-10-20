using System;
using AirportParkingAssistant.Enums;

namespace AirportParkingAssistant.Models
{
    public class Plane
    {
        public PlaneType PlaneModel { get; set;  }
        public DateTime PlaneArrivalTime { get; set; }
        public DateTime Duration { get; set; }
        public int ParkingSlot { get; set; }

        public Plane(string planeType, DateTime arrivalTime)
        {
            PlaneModel = (PlaneType)Enum.Parse(typeof(PlaneType), planeType);
            PlaneArrivalTime = arrivalTime; 
        }
    }
}
