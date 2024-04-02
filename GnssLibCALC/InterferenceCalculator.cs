using System;
namespace GnssLibCALC
{
	public class InterferenceCalculator
	{
        public static double Distance(double[] p1, double[] p2)
        {
            double dx = p1[0] - p2[0];
            double dy = p1[1] - p2[1];
            double dz = p1[2] - p2[2];
            return Math.Sqrt(dx * dx + dy * dy + dz * dz);
        }

        // Function to check if a line intersects with a sphere
        public static bool DoesLineSegmentIntersectSphere(double[] satellite, double[] receiver, double[] jammerCenter, double jamStr)
        {

            double jammerRadius = 395;
            // Vector from satellite to receiver
            double[] lineVector = { receiver[0] - satellite[0], receiver[1] - satellite[1], receiver[2] - satellite[2] };

            // Vector from satellite to jammer center
            double[] satelliteToJammer = { jammerCenter[0] - satellite[0], jammerCenter[1] - satellite[1], jammerCenter[2] - satellite[2] };

            // Length of the line segment
            double lineLength = Distance(satellite, receiver);

            // Normalized direction vector of the line segment
            double[] lineUnitVector = { lineVector[0] / lineLength, lineVector[1] / lineLength, lineVector[2] / lineLength };

            // Dot product of (line segment direction vector) . (satellite to jammer vector)
            double dotProduct = lineUnitVector[0] * satelliteToJammer[0] + lineUnitVector[1] * satelliteToJammer[1] + lineUnitVector[2] * satelliteToJammer[2];

            // Closest point on the line segment to the jammer
            double[] closestPoint;
            if (dotProduct <= 0)
                closestPoint = satellite;
            else if (dotProduct >= lineLength)
                closestPoint = receiver;
            else
                closestPoint = new double[] { satellite[0] + lineUnitVector[0] * dotProduct,
                                           satellite[1] + lineUnitVector[1] * dotProduct,
                                           satellite[2] + lineUnitVector[2] * dotProduct };

            // Check if the closest point is within the jammer's radius
            return Distance(closestPoint, jammerCenter) <= jamStr;
        }
    }
}

