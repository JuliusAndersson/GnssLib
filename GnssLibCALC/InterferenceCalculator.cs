namespace GnssLibCALC
{
	public class InterferenceCalculator
	{


        private static readonly double _Xsquare = Math.Pow(CoordinatesCalculator.WGS84_SEMI_MAJOR_AXIS, 2);
        private static readonly double _Ysquare = Math.Pow(CoordinatesCalculator.WGS84_SEMI_MAJOR_AXIS, 2);
        private static readonly double _Csquare = Math.Pow(CoordinatesCalculator.WGS84_SEMI_MINOR_AXIS, 2);


        /// <summary>
        /// Calculates distance between two points in ECEF coordinates
        /// </summary>
        private static double Distance(double[] fromPosition, double[] toPosition)
        {
            double dx = fromPosition[0] - toPosition[0];
            double dy = fromPosition[1] - toPosition[1];
            double dz = fromPosition[2] - toPosition[2];
            return Math.Sqrt(dx * dx + dy * dy + dz * dz);
        }

        /// <summary>
        /// Checks if the line between satellite and reciever intersects the sphere.
        /// </summary>
        /// <param name="satellitePosition">The coordinates (X ,Y ,Z) of the satellite.</param>
        /// <param name="receieverPosition">The coordinates (X ,Y ,Z) of the receiver.</param>
        /// <param name="jammerCenter">The coordinates (X ,Y ,Z) of the jammers centre.</param>
        /// <param name="jammerRadius">The radius (in kilometers) of the sphere representing a jammer.</param>
        /// <returns>True if the line from the receiver to the satelliet intersects the sphere.</returns>
        public static bool DoesLineSegmentIntersectSphere(double[] satellitePosition, double[] receieverPosition, double[] jammerCenter, double jammerRadius)
        {

            // Vector from satellite to receiver
            double[] receiverToSatellieVector = { receieverPosition[0] - satellitePosition[0], receieverPosition[1] - satellitePosition[1], receieverPosition[2] - satellitePosition[2] };

            // Vector from satellite to jammer center
            double[] satelliteToJammerVector = { jammerCenter[0] - satellitePosition[0], jammerCenter[1] - satellitePosition[1], jammerCenter[2] - satellitePosition[2] };

            // Length of the line segment
            double lineLength = Distance(satellitePosition, receieverPosition);

            // Normalized direction vector of the line segment
            double[] lineUnitVector = { receiverToSatellieVector[0] / lineLength, receiverToSatellieVector[1] / lineLength, receiverToSatellieVector[2] / lineLength };

            // Dot product of (line segment direction vector) . (satellite to jammer vector)
            double dotProduct = lineUnitVector[0] * satelliteToJammerVector[0] + lineUnitVector[1] * satelliteToJammerVector[1] + lineUnitVector[2] * satelliteToJammerVector[2];

            // Closest point on the line segment to the jammer
            double[] closestPoint;
            if (dotProduct <= 0)
                closestPoint = satellitePosition;
            else if (dotProduct >= lineLength)
                closestPoint = receieverPosition;
            else
                closestPoint = new double[] { satellitePosition[0] + lineUnitVector[0] * dotProduct,
                                           satellitePosition[1] + lineUnitVector[1] * dotProduct,
                                           satellitePosition[2] + lineUnitVector[2] * dotProduct };

            // Check if the closest point is within the jammer's radius
            return Distance(closestPoint, jammerCenter) <= jammerRadius;
        }

        /// <summary>
        /// Checks if the line from p1 to p2 intersect the wgs84 elipsoid at any value of t.
        /// </summary>
        /// <param name="fromPosition">From-position.</param>
        /// <param name="toPosition">To-position.</param>
        /// <returns>True if it intersects, else false.</returns>
        public static bool CheckIntersection(double[] fromPosition, double[] toPosition)
        {
            double x, y, z;
            for (double t = 0; t <= 1; t += 0.02)
            {
                x = fromPosition[0] + t * (toPosition[0] - fromPosition[0]);
                y = fromPosition[1] + t * (toPosition[1] - fromPosition[1]);
                z = fromPosition[2] + t * (toPosition[2] - fromPosition[2]);

                if (CalculateIntersect(x, y, z))
                {
                    return true;
                }
            }
            return false;
        }

        /// <summary>
        /// Checks if a position is on or inside the elipsoid. If result if less than or equal to 1 returns true.
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <param name="z"></param>
        /// <returns>True if point is inside elipsoid.</returns>
        public static bool CalculateIntersect(double x, double y, double z)
        {
            double result = (x * x) / _Xsquare + (y * y) / _Ysquare + (z * z) / _Csquare;
            return result <= 1;
        }


    }
}

