using System;
using GnssLibCALC.Models.BroadCastDataModels;
namespace GnssLibCALC
{
	public class CoordinatesCalculator
    {
        private const double WGS84_SEMI_MAJOR_AXIS = 6378137;              //semi-major axis in meters
        private const double WGS84_SEMI_MINOR_AXIS = 6356752.3142;         //semi-minor axis in meters
        private const double GM = 3.986005e14;                             //WGS-84 value for the product of gravitational constant G and the mass of the Earth M
        private const double WGS84_ROTATION_RATE_OF_EARTH = 7.292115e-5;   //WGS-84 value of the Earth’s rotation rate


        /// <summary>
        /// Converts a DecimalDegree coordinate to a DegreeMinutesSecond 
        /// </summary>
        /// <param name="coordinate">Coordinate (latitude or longitude) in decimal degree's.</param>
        /// <param name="isLatitude">To specify if it's latitude or longitude (true for latitude, false for longitude)</param>
        /// <returns>Coordinate in DecimalMinutesSeconds format, either N 59º 58' 55.23" or E 017º 50' 06.12".</returns>
        public static string DoubleToDegreesMinutesSeconds(double coordinate, bool isLatitude)
        {
            int degrees = (int)coordinate;
            double remaining = Math.Abs(coordinate - degrees) * 60;
            int minutes = (int)remaining;
            double seconds = (remaining - minutes) * 60;

            char direction = isLatitude ? (coordinate >= 0 ? 'N' : 'S') : (coordinate >= 0 ? 'E' : 'W');

            return $"{direction} {degrees:D3}º {minutes:D2}' {seconds:00.00}\"".Replace(',', '.');
        }


        /// <summary>
        /// Converts from longitude, latitude and altitude in decimalDegrees to ECEF xyz-coordinates.
        /// </summary>
        /// <param name="latitude">Latitude in decimal degrees.</param>
        /// <param name="longitude">Longitude in decimal degrees.</param>
        /// <param name="altitude">Altitude in meters.</param>
        /// <returns>
        /// An array containing the ECEF coordinates (X, Y, Z) in kilometers.
        /// Index 0: X-coordinate in kilometers.
        /// Index 1: Y-coordinate in kilometers.
        /// Index 2: Z-coordinate in kilometers.
        /// </returns>
        public static double[] GeodeticToECEF(double latitude, double longitude, double altitude)
        {
            // Convert degrees to radians
            double phi = latitude * Math.PI / 180.0;   // geodetic latitude in radians
            double lambda = longitude * Math.PI / 180.0; // longitude in radians
            double f = 1 - (WGS84_SEMI_MINOR_AXIS / WGS84_SEMI_MAJOR_AXIS);
            // Convert geodetic latitude to geocentric latitude
            double esquared = f*(2 - f);
            double Nphi = WGS84_SEMI_MAJOR_AXIS / Math.Sqrt(1 - esquared * Math.Pow(Math.Sin(phi), 2));

            // Calculate ECEF XYZ coordinates
            double x = (Nphi + altitude) * Math.Cos(phi) * Math.Cos(lambda);
            double y = (Nphi + altitude) * Math.Cos(phi) * Math.Sin(lambda);
            double z = ((1 - esquared) * Nphi + altitude) * Math.Sin(phi);
            
            //Convert from meters to km
            return new double[] { x / 1000, y / 1000, z / 1000 };
        }


        /// <summary>
        /// Calculation of GPS satelliteposition from broadcast ephemeris data. Calculation based on instructions in "Satellite-position-from-ephemeris". 
        /// </summary>
        /// <param name="broadcastData">A BroadCastDataLNAV object containing all paramaters parsed from a LNAV-block in the broadcast ephemeris file.</param>
        /// <param name="seconds">Time for simulation, adding one seconds get satellite position one second later etc.</param>
        /// <returns>
        /// An array containing the ECEF coordinates (X, Y, Z) of the satellite in kilometers.
        /// Index 0: X-coordinate in kilometers.
        /// Index 1: Y-coordinate in kilometers.
        /// Index 2: Z-coordinate in kilometers.
        /// </returns>
        public static double[] CalculatePosition(BroadCastDataLNAV broadcastData, double seconds)
        {
            double A = broadcastData.SqrtOfMajorAxis * broadcastData.SqrtOfMajorAxis;
            double n_0 = Math.Sqrt(GM / Math.Pow(A, 3));
            double n = n_0 + broadcastData.Delta_n0;
            double e = broadcastData.OrbitEcentricity;
            double tk = CalculateTk(broadcastData.TransmissionTime + seconds, broadcastData.TimeOfEphemeris);
            double Mk = broadcastData.M0_Angle + n * tk;
            double Ek = CalculateEk(Mk, e);

            double vk = Math.Atan2(Math.Sqrt(1 - e * e) * Math.Sin(Ek), Math.Cos(Ek) - e);
            double phi_k = vk + broadcastData.OmegaAngle;

            double d_uk = broadcastData.LatitudeCorrectionCosinusComponent * Math.Cos(2 * phi_k) + broadcastData.LatitudeCorrectionSinusComponent * Math.Sin(2 * phi_k);
            double d_rk = broadcastData.RadiusCorrectionCosinusComponent * Math.Cos(2 * phi_k) + broadcastData.RadiusCorrectionSinusComponent * Math.Sin(2 * phi_k);
            double d_ik = broadcastData.InclinationCorrectionCosinusComponent * Math.Cos(2 * phi_k) + broadcastData.AngularVelocity * Math.Sin(2 * phi_k);

            double uk = phi_k + d_uk;
            double rk = A * (1 - e * Math.Cos(Ek)) + d_rk;
            double ik = broadcastData.InitialInclination + d_ik + broadcastData.InclinationRate * tk;

            double xk_prim = rk * Math.Cos(uk);
            double yk_prim = rk * Math.Sin(uk);

            double omega_k = broadcastData.OmegaAngle0 + (broadcastData.AngularVelocityPerSec - WGS84_ROTATION_RATE_OF_EARTH) * tk - WGS84_ROTATION_RATE_OF_EARTH * broadcastData.TimeOfEphemeris;

            double xk = xk_prim * Math.Cos(omega_k) - yk_prim * Math.Cos(ik) * Math.Sin(omega_k);
            double yk = xk_prim * Math.Sin(omega_k) + yk_prim * Math.Cos(ik) * Math.Cos(omega_k);
            double zk = yk_prim * Math.Sin(ik);
            return new double[] { xk / 1000, yk / 1000, zk / 1000 };
        }


        /// <summary>
        /// Calculation of GPS satelliteposition from broadcast ephemeris data. Calculation based on instructions in "Satellite-position-from-ephemeris". 
        /// </summary>
        /// <param name="broadcastData">A BroadCastDataLNAV object containing all paramaters parsed from a INAV-block in the broadcast ephemeris file.</param>
        /// <param name="seconds">Time for simulation, adding one seconds get satellite position one second later etc.</param>
        /// <returns>
        /// An array containing the ECEF coordinates (X, Y, Z) of the satellite in kilometers.
        /// Index 0: X-coordinate in kilometers.
        /// Index 1: Y-coordinate in kilometers.
        /// Index 2: Z-coordinate in kilometers.
        /// </returns>
        public static double[] CalculatePosition(BroadCastDataINAV broadcastData, double seconds)
        {
            double A = broadcastData.SqtOfMajorAxis * broadcastData.SqtOfMajorAxis;
            double n_0 = Math.Sqrt(GM / Math.Pow(A, 3));
            double n = n_0 + broadcastData.Delta_n;
            double e = broadcastData.OrbitEcentricity;
            double tk = CalculateTk(broadcastData.TransmissionTime + seconds, broadcastData.TimeOfEphemeris);
            double Mk = broadcastData.M0_Angle + n * tk;
            double Ek = CalculateEk(Mk, e);

            double vk = Math.Atan2(Math.Sqrt(1 - e * e) * Math.Sin(Ek), Math.Cos(Ek) - e); 
            double phi_k = vk + broadcastData.OmegaAngle;

            double d_uk = broadcastData.LatitudeCorrectionCosinusComponent * Math.Cos(2 * phi_k) + broadcastData.LatitudeCorrectionSinusComponent * Math.Sin(2 * phi_k);
            double d_rk = broadcastData.RadiusCorrectionCosinusComponent * Math.Cos(2 * phi_k) + broadcastData.RadiusCorrectionSinusComponent * Math.Sin(2 * phi_k);
            double d_ik = broadcastData.InclinationCorrectionCosinusComponent * Math.Cos(2 * phi_k) + broadcastData.AngularVelocity * Math.Sin(2 * phi_k);

            double uk = phi_k + d_uk;
            double rk = A * (1 - e * Math.Cos(Ek)) + d_rk;
            double ik = broadcastData.InitialInclination + d_ik + broadcastData.InclinationRate * tk;

            double xk_prim = rk * Math.Cos(uk);
            double yk_prim = rk * Math.Sin(uk);

            double omega_k = broadcastData.OmegaAngle0 + (broadcastData.AngularVelocityPerSec - WGS84_ROTATION_RATE_OF_EARTH) * tk - WGS84_ROTATION_RATE_OF_EARTH * broadcastData.TimeOfEphemeris;

            double xk = xk_prim * Math.Cos(omega_k) - yk_prim * Math.Cos(ik) * Math.Sin(omega_k);
            double yk = xk_prim * Math.Sin(omega_k) + yk_prim * Math.Cos(ik) * Math.Cos(omega_k);
            double zk = yk_prim * Math.Sin(ik);
            return new double[] { xk / 1000, yk / 1000, zk / 1000 };
        }

        /// <summary>
        ///  
        /// </summary>
        private static double CalculateTk(double t, double toe)
        {
            double tk = t - toe;
            if (tk > 302400.0)
            {
                tk -= 604800.0;
            }
            else if (tk < -302400.0)
            {
                tk += 604800.0;
            }
            return tk;
        }
        /// <summary>
        /// 
        /// </summary>
     
        private static double CalculateEk(double Mk, double e)
        {
            double Ek = Mk;
            double temp = Ek;
            while (Math.Abs(Ek - temp) >= 1e-14)
            {
                temp = Ek;
                Ek = Mk + e * Math.Sin(Ek);
            }
            return Ek;
        }
    }

}

