using System;
using System.Linq;
using System.Reflection;
using AirportParkingAssistant.Controllers;
using AirportParkingAssistant.Models;

namespace AirportParkingAssistant
{
    class Program
    {
        private static PlaneController _planeController = new PlaneController(new CarParkSpaceRepository(), new AlertHandler());

        public Program()
        {
            
        }
        static void Main(string[] args)
        {
            RunApplication();
        }

        public static void RunApplication()
        {
            Console.WriteLine("Welcome to the airport! We'll find you somewhere to park. Please type your plane model below:");

            var planeModel = Console.ReadLine();

            var space = _planeController.AssignParkingSpace(new Models.Plane(planeModel, DateTime.Now));

            if (space.ParkingZone == "0")
            {
                Console.WriteLine("Sorry, no spaces are currently available for you to park in. An alert has been raised.");
                Console.ReadLine();
            }
            else
            {
                Console.WriteLine($"Please park in zone {space.ParkingZone} in bay number {space.ParkingBay}");
                RunApplication();
            }
        }
    }
}
