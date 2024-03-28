using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GnssLibCALC
{
    public class VisibleSatCalulator
    {
        public static void IsSatelliteVisible(double minElevation, double maxElevation, double[] locationXYZ, double[] satXYZ, out bool isVisible, out double angleOfView, out double azimuthDegrees)
        {
            // Constants
            double x1 = locationXYZ[0];
            double y1 = locationXYZ[1];
            double z1 = locationXYZ[2];
            double x2 = satXYZ[0];
            double y2 = satXYZ[1];
            double z2 = satXYZ[2];

            // Calculate the unit vector pointing from the observer to the satellite
            double dx = x2 - x1;
            double dy = y2 - y1;
            double dz = z2 - z1;

            //Calc elevation(angleOfView)
            double innerProduct = x1 * dx + y1 * dy + z1 * dz;
            double norm1 = Math.Sqrt(x1 * x1 + y1 * y1 + z1 * z1);
            double norm2 = Math.Sqrt(dx * dx + dy * dy + dz * dz);
            double cosElevation = innerProduct / (norm1 * norm2);
            double elevationRadians = Math.Acos(cosElevation);
            double elevationDegrees = elevationRadians * (180 / Math.PI);
            angleOfView = 90 - elevationDegrees;

            //Calc Azimuth
            double numeratorCos = (-z1 * x1 * dx) - (z1 * y1 * dy) + ((x1 * x1) + (y1 * y1)) * dz;
            double denominatorCos = Math.Sqrt((x1 * x1 + y1 * y1) * (x1 * x1 + y1 * y1 + z1 * z1) * (dx * dx + dy * dy + dz * dz));
            double cosAzimuth = numeratorCos / denominatorCos;
            double numeratorSin = (-y1 * dx) + (x1 * dy);
            double denominatorSin = Math.Sqrt((x1 * x1 + y1 * y1) * (dx * dx + dy * dy + dz * dz));
            double sinAzimuth = numeratorSin / denominatorSin;
            double azimuthRadians = Math.Atan2(sinAzimuth, cosAzimuth);
            azimuthDegrees = azimuthRadians * (180 / Math.PI);

            // Calculate the azimuth angle (angle clockwise from true north) in degrees
            if (azimuthDegrees < 0)
            {
                azimuthDegrees += 360; // Ensure azimuth is within [0, 360) degrees
            }
            // Check if the satellite is above the horizon and within the field of view
            if (angleOfView > minElevation && (angleOfView < maxElevation && angleOfView <= 90))
            {
                isVisible = true;
            }
            else
            {
                isVisible = false;
            }
        }
    }
}
