using System;
using AirportParkingAssistant.Models;

namespace AirportParkingAssistant
{
    public interface ICarParkSpaceRepository
    {
        public void UpdateCarPark(PlanePark updatedPlanePark);
        public PlanePark GetCarParkSpaces();
    }
}
