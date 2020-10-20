using AirportParkingAssistant.Models;
using AirportParkingAssistant.Enums;
using System.Linq;

namespace AirportParkingAssistant.Controllers
{
    public class PlaneController
    {
        private ICarParkSpaceRepository _spaceRepository;
        private readonly IAlertHandler _alertHandler; 

        public PlaneController(ICarParkSpaceRepository spaceRepository, IAlertHandler alertHandler)
        {
            _spaceRepository = spaceRepository;
            _alertHandler = alertHandler;
        }

        public ParkingSpace AssignParkingSpace(Plane plane)
        {
            var planeParkSpaces = _spaceRepository.GetCarParkSpaces();
            ParkingSpace assignedParkingSpace;

            if (plane.PlaneModel == PlaneType.E195)
            {
                assignedParkingSpace = GetPropParkingSpace(planeParkSpaces);
            }
            else if (plane.PlaneModel == PlaneType.A380 || plane.PlaneModel == PlaneType.B747)
            {
                assignedParkingSpace = GetJumboParkingSpace(planeParkSpaces);
            }
            else if (plane.PlaneModel == PlaneType.A330 || plane.PlaneModel == PlaneType.B777)
            {
                assignedParkingSpace = GetJetParkingSpace(planeParkSpaces);
            }
            else
            {
                assignedParkingSpace = HandleNoSpacesAvailable();
            }

            _spaceRepository.UpdateCarPark(planeParkSpaces);
            return assignedParkingSpace;
        }

        private ParkingSpace HandleNoSpacesAvailable()
        {
            _alertHandler.RaiseAlert("No available spaces left");
            return new UnavailableParkingSpace();
        }

        private ParkingSpace GetPropParkingSpace(PlanePark planeParkSpaces)
        {
            var availablePropSpace = (PropParkingSpace)planeParkSpaces.PropSpaces.Where(x => x.IsEmpty == true).FirstOrDefault();

            if (availablePropSpace != null)
            {
                availablePropSpace.IsEmpty = false;
                return availablePropSpace;
            }
            else return GetJetParkingSpace(planeParkSpaces);
        }

        private ParkingSpace GetJetParkingSpace(PlanePark planeParkSpaces)
        {
            var availableJetSpace = (JetParkingSpace)planeParkSpaces.JetSpaces.Where(x => x.IsEmpty == true).FirstOrDefault();
            if (availableJetSpace != null)
            {
                availableJetSpace.IsEmpty = false;
                return availableJetSpace;
            }
            else return GetJumboParkingSpace(planeParkSpaces);
        }

        private ParkingSpace GetJumboParkingSpace(PlanePark planeParkSpaces)
        {
            var availableJumboSpace = (JumboParkingSpace)planeParkSpaces.JumboJetSpaces.Where(x => x.IsEmpty == true).FirstOrDefault();

            if (availableJumboSpace != null)
            {
                availableJumboSpace.IsEmpty = false;
                return availableJumboSpace;
            }
            else return HandleNoSpacesAvailable();

        }
    }
}
