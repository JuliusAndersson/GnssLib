using GnssLibCALC;

namespace CalculationsTest
{
    [TestClass]
    public class CalculationsTest
    {
        /// <summary>
        /// Test to verify distance calculation.  
        /// </summary>
        [TestMethod]
        public void CalculateDistanceTest()
        {
            double[] arr1 = new double[] { 5, 8, 3 };
            double[] arr2 = new double[] { 3, 6, 2 };
            var result = DOPCalulator.CalculateDistance(arr1, arr2);
            Assert.AreEqual(3, result);
        }

        /// <summary>
        /// Test to verify the convertion fron geodetic coordinates to EVEF. WGS84 reference elipsoid is used.  
        /// </summary>
        [TestMethod]
        public void ConvertGeodeticToECEF()
        {
            double latitude = 56.000;
            double longitude = 12.00;
            double altitude = 0;
            var result = CoordinatesCalculator.GeodeticToECEF(latitude, longitude, altitude);
            Assert.AreEqual(3496.724, Math.Round(result[0],3));
            Assert.AreEqual(743.252, Math.Round(result[1], 3));
            Assert.AreEqual(5264.442, Math.Round(result[2], 3));
        }

        /// <summary>
        /// Scenario tested can be seen in documentation folder.
        /// </summary>
        [TestMethod]
        public void IntersectTest()
        {
            double[] receiverPosition = new double[] { 0, 0, 10 };
            double[] satellitePosition = new double[] { 0, -12, 15 };
            double[] satellitePosition2 = new double[] { 0, 10, 15 };
            double[] jammerPosition = new double[] { 0, -1.96, 9.81 };
            double jammerRadius = 1.969;
            var result = InterferenceCalculator.DoesLineSegmentIntersectSphere(satellitePosition, receiverPosition, jammerPosition, jammerRadius);
            var result2 = InterferenceCalculator.DoesLineSegmentIntersectSphere(satellitePosition2,receiverPosition, jammerPosition, jammerRadius);
            Assert.AreEqual(true, result); //Line intersects
            Assert.AreEqual(false, result2);//Line does not intersect
        }

        /// <summary>
        /// Scenario tested can be seen in documentation folder.
        /// </summary>
        [TestMethod]
        public void SatelliteVisabilityTest()
        {
            double minimumElevation = 0;
            double maximumElevation = 90;
            double[] receiverPosition = new double[] { 0, 0, 10 };
            double[] satellitePosition = new double[] { 0, -2, 11 };
            double[] satellitePosition2 = new double[] { 0, 12, 9 };
            bool isVisible;
            bool isNotVisible;
            double elevation, azmituh;
            VisibleSatCalulator.IsSatelliteVisible(minimumElevation, maximumElevation,receiverPosition, satellitePosition,out isVisible,out elevation, out azmituh);
            VisibleSatCalulator.IsSatelliteVisible(minimumElevation, maximumElevation, receiverPosition, satellitePosition2, out isNotVisible, out elevation, out azmituh);
            Assert.AreEqual(true, isVisible);
            Assert.AreEqual(false, isNotVisible);
        
        }
    }
}