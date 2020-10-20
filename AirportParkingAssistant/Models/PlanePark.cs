using System.Collections.Generic;

namespace AirportParkingAssistant.Models
{
    public class PlanePark
    {
        public List<ParkingSpace> JumboJetSpaces { get; set; }
        public List<ParkingSpace> JetSpaces { get; set; }
        public List<ParkingSpace> PropSpaces { get; set; }

        public PlanePark()
        {

        }
    }
}
