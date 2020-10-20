using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using AirportParkingAssistant.Models;

namespace AirportParkingAssistant
{
    public class CarParkSpaceRepository : ICarParkSpaceRepository
    {
        public PlanePark _planePark;
        public CarParkSpaceRepository()
        {
            _planePark = CreateEmptyPlanePark();
        }

        public PlanePark GetCarParkSpaces()
        {
            return _planePark;
        }

        public void UpdateCarPark(PlanePark updatedPlanePark) 
        {
            _planePark = updatedPlanePark;
        }

        private PlanePark CreateEmptyPlanePark()
        {
            return CreatePlanePark(25, 0, 50, 0, 25, 0);
        }

        private PlanePark CreatePlanePark(int freeJumboJetSpaces, int filledJumboJetSpaces, int freeJetSpaces, int filledJetSpaces, int freePropSpaces, int filledPropSpaces)
        { 
            return new PlanePark()
            {
                JumboJetSpaces = CreateParkingSpaces<JumboParkingSpace>(freeJumboJetSpaces, filledJumboJetSpaces),
                JetSpaces = CreateParkingSpaces<JetParkingSpace>(freeJetSpaces, filledJetSpaces),
                PropSpaces = CreateParkingSpaces<PropParkingSpace>(freePropSpaces, filledPropSpaces),
            };
        }

        private List<ParkingSpace> CreateParkingSpaces<T>(int numberOfEmptySpaces, int numberOfFullSpaces) where T : ParkingSpace, new()
        {
            var parkingSpaces = new List<ParkingSpace>();
            for (int i = 1; i <= numberOfFullSpaces; i++)
            {
                parkingSpaces.Add(new T
                {
                    IsEmpty = false,
                    ParkingBay = i + 1
                });
            }
            for (int i = numberOfFullSpaces + 1; i <= numberOfEmptySpaces + numberOfFullSpaces; i++)
            {
                parkingSpaces.Add(new T
                {
                    IsEmpty = true,
                    ParkingBay = i
                });
            }
            return parkingSpaces;
        }
    }
}
