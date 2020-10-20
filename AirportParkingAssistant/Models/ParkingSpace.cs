using System;
using System.Linq;

namespace AirportParkingAssistant.Models
{
    public abstract class ParkingSpace
    {
        public abstract string ParkingZone { get;}
        public int ParkingBay;
        public bool IsEmpty;
   }

    public class JumboParkingSpace : ParkingSpace
    {
        public override string ParkingZone { get => "A"; }

    }

    public class JetParkingSpace : ParkingSpace
    {
        public override string ParkingZone { get => "B"; }

    }

    public class PropParkingSpace : ParkingSpace
    {
        public override string ParkingZone { get => "C"; }
    }

    public class UnavailableParkingSpace : ParkingSpace
    {
        public override string ParkingZone { get => "0"; }
    }
}
