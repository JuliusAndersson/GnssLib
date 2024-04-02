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
        public static bool DoesLineIntersectSphere(double[] satellite, double[] receiver, double[] jammerCenter, double jamStr)
        {
            double jammerRadius = jamStr;
            // Vector from satellite to receiver
            double lineLength = Distance(satellite, receiver);
            double lineUnitX = (receiver[0] - satellite[0]) / lineLength;
            double lineUnitY = (receiver[1] - satellite[1]) / lineLength;
            double lineUnitZ = (receiver[2] - satellite[2]) / lineLength;

            // Vector from satellite to jammer center
            double jammerDistance = Distance(satellite, jammerCenter);

            // Projection of the jammer center onto the line
            double projection = ((jammerCenter[0] - satellite[0]) * lineUnitX) +
                                ((jammerCenter[1] - satellite[1]) * lineUnitY) +
                                ((jammerCenter[2] - satellite[2]) * lineUnitZ);

            // If the projection is negative, the closest point on the line to the jammer center is before the satellite
            if (projection < 0)
                return false;

            // Calculate the distance from the projected point to the jammer center
            double distanceToCenter = Distance(jammerCenter, new double[] {
                                                           satellite[0] + projection * lineUnitX,
                                                           satellite[1] + projection * lineUnitY,
                                                           satellite[2] + projection * lineUnitZ });

            // If the distance is less than or equal to the jammer radius, they intersect
            return distanceToCenter <= jammerRadius;
        }
    }
}

