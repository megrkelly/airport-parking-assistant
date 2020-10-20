using System;
using Xunit;
using Moq;
using AirportParkingAssistant;
using AirportParkingAssistant.Controllers;
using AirportParkingAssistant.Models;
using System.Collections.Generic;

namespace AirportParkingAssistantTests.ControllersTests
{
    public class PlaneControllerTests
    {
        private Mock<IAlertHandler> _mockAlertHandler;

        public PlaneControllerTests()
        {
            _mockAlertHandler = new Mock<IAlertHandler>();
        }

        [InlineData("A380", "A", 1)]
        [InlineData("B747", "A", 1)]
        [InlineData("A330", "B", 1)]
        [InlineData("B777", "B", 1)]
        [InlineData("E195", "C", 1)]
        [Theory]
        public void PlaneController_GivenPlanesForEmptyPlanePark_ReturnsCorrectParkingSpace(string planeType, string expectedZone, int expectedBay)
        {
            var mockSpaceRepository = new Mock<ICarParkSpaceRepository>();
            var emptyPlanePark = CreatePlanePark(25, 0, 50, 0, 25, 0);
            mockSpaceRepository.Setup(p => p.GetCarParkSpaces()).Returns(emptyPlanePark);

            var planeController = new PlaneController(mockSpaceRepository.Object, _mockAlertHandler.Object);

            var assignedSpace = planeController.AssignParkingSpace(new Plane(planeType, DateTime.Now));

            Assert.Equal(expectedBay, assignedSpace.ParkingBay);
            Assert.Equal(expectedZone, assignedSpace.ParkingZone);
        }


        [InlineData(25, 0, 0, "A330", "A", 1)]
        [InlineData(25, 0, 0, "B777", "A", 1)]
        [InlineData(25, 50, 0, "E195", "B", 1)]
        [Theory]
        public void PlaneController_GivenPlanesWithoutTheirSize_ReturnsNextLargestParkingSpace(int freeJumboSpaces, int freeJetSpaces, int freePropSpaces, string planeType, string expectedZone, int expectedBay)
        {
            var planePark = CreatePlanePark(freeJumboSpaces, 0, freeJetSpaces, 0, freePropSpaces, 0);
            var mockSpaceRepository = new Mock<ICarParkSpaceRepository>();
            mockSpaceRepository.Setup(p => p.GetCarParkSpaces()).Returns(planePark);

            var planeController = new PlaneController(mockSpaceRepository.Object, _mockAlertHandler.Object);

            var assignedSpace = planeController.AssignParkingSpace(new Plane(planeType, DateTime.Now));

            Assert.Equal(expectedBay, assignedSpace.ParkingBay);
            Assert.Equal(expectedZone, assignedSpace.ParkingZone);
        }

        [Fact]
        public void PlaneController_GivenJumboPlaneWithNoJumboSpaces_ReturnsUnavailableParkingSpaceAndRaisesAlert()
        {
            var planePark = CreatePlanePark(0, 25, 50, 0, 25, 0);
            var mockSpaceRepository = new Mock<ICarParkSpaceRepository>();
            mockSpaceRepository.Setup(p => p.GetCarParkSpaces()).Returns(planePark);
            
            var planeController = new PlaneController(mockSpaceRepository.Object, _mockAlertHandler.Object);

            var assignedSpace = planeController.AssignParkingSpace(new Plane("A380", DateTime.Now));

            _mockAlertHandler.Verify(p => p.RaiseAlert("No available spaces left"), Times.Once);
            Assert.Equal("0", assignedSpace.ParkingZone);
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
