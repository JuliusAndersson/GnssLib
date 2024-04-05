using GnssLibCALC;

namespace CalculationsTest
{
    [TestClass]
    public class CalculationsTest
    {

        [TestMethod]
        public void CalculateDistanceTest()
        {
            double[] arr1 = new double[] { 5, 8, 3 };
            double[] arr2 = new double[] { 3, 6, 2 };
            var result = DOPCalulator.CalculateDistance(arr1, arr2);
            Assert.AreEqual(3, result);
        }

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
        /// Scenario tested can be seen in documentation folder
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

    }
}