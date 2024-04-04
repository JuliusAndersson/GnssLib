using System;
using GnssLibCALC.Models.BroadCastDataModels;
namespace GnssLibCALC
{
	public class CoordinatesCalculator
    {
        private const double WGS84_SEMI_MAJOR_AXIS = 6378137;             // semi-major axis in meters
        private const double b = 6356752.3142;        // semi-minor axis in meters
        private const double GM = 3.986005e14;        //WGS-84 value for the product of gravitational constant G and the mass of the Earth M
        private const double WGS84_ROTATION_RATE_OF_EARTH = 7.292115e-5;   //WGS-84 value of the Earth’s rotation rate


        /// <summary>
        /// Converts a DecimalDegree coordinate to a DegreeMinutesSecond to format N 59º 58' 55.23" or E 017º 50' 06.12".
        /// </summary>
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
        /// Converts from longitude, latitude and altitude in decimalDegrees to ECEF xyz-coordinates. Returns a double array with three values, representing xyz.
        /// </summary>
        public static double[] GeodeticToECEF(double latitude, double longitude, double altitude)
        {
            // Convert degrees to radians
            double phi = latitude * Math.PI / 180.0;   // geodetic latitude in radians
            double lambda = longitude * Math.PI / 180.0; // longitude in radians

            // Convert geodetic latitude to geocentric latitude
            double esquared = 1 - Math.Pow(WGS84_SEMI_MAJOR_AXIS, 2) / Math.Pow(b, 2);
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
        /// Returns a double array with three values representing the satellites position in xyz. Returnvalues are in km.
        /// </summary>
        public static double[] CalculatePosition(BroadCastDataLNAV bcd, double seconds)
        {
            double A = bcd.SqrtOfMajorAxis * bcd.SqrtOfMajorAxis;
            double n_0 = Math.Sqrt(GM / Math.Pow(A, 3));
            double n = n_0 + bcd.Delta_n0;
            double e = bcd.OrbitEcentricity;
            double tk = CalculateTk(bcd.TransmissionTime + seconds, bcd.TimeOfEphemeris);
            double Mk = bcd.M0_Angle + n * tk;
            double Ek = CalculateEk(Mk, e);

            double vk = Math.Atan2(Math.Sqrt(1 - e * e) * Math.Sin(Ek), Math.Cos(Ek) - e);
            double phi_k = vk + bcd.OmegaAngle;

            double d_uk = bcd.LatitudeCorrectionCosinusComponent * Math.Cos(2 * phi_k) + bcd.LatitudeCorrectionSinusComponent * Math.Sin(2 * phi_k);
            double d_rk = bcd.RadiusCorrectionCosinusComponent * Math.Cos(2 * phi_k) + bcd.RadiusCorrectionSinusComponent * Math.Sin(2 * phi_k);
            double d_ik = bcd.InclinationCorrectionCosinusComponent * Math.Cos(2 * phi_k) + bcd.AngularVelocity * Math.Sin(2 * phi_k);

            double uk = phi_k + d_uk;
            double rk = A * (1 - e * Math.Cos(Ek)) + d_rk;
            double ik = bcd.InitialInclination + d_ik + bcd.InclinationRate * tk;

            double xk_prim = rk * Math.Cos(uk);
            double yk_prim = rk * Math.Sin(uk);

            double omega_k = bcd.OmegaAngle0 + (bcd.AngularVelocityPerSec - WGS84_ROTATION_RATE_OF_EARTH) * tk - WGS84_ROTATION_RATE_OF_EARTH * bcd.TimeOfEphemeris;

            double xk = xk_prim * Math.Cos(omega_k) - yk_prim * Math.Cos(ik) * Math.Sin(omega_k);
            double yk = xk_prim * Math.Sin(omega_k) + yk_prim * Math.Cos(ik) * Math.Cos(omega_k);
            double zk = yk_prim * Math.Sin(ik);
            return new double[] { xk / 1000, yk / 1000, zk / 1000 };
        }
        /// <summary>
        /// Calculation of Galileo satelliteposition from broadcast ephemeris data. Calculation based on instructions in "Satellite-position-from-ephemeris".
        /// Returns a double array with three values representing the satellites position in xyz. Returnvalues are in km.
        /// </summary>
        public static double[] CalculatePosition(BroadCastDataINAV bcd, double seconds)
        {
            double A = bcd.SrqtOfMajorAxis * bcd.SrqtOfMajorAxis;
            double n_0 = Math.Sqrt(GM / Math.Pow(A, 3));
            double n = n_0 + bcd.Delta_n;
            double e = bcd.OrbitEcentricity;
            double tk = CalculateTk(bcd.TransmissionTime + seconds, bcd.TimeOfEphemeris);
            double Mk = bcd.M0_Angle + n * tk;
            double Ek = CalculateEk(Mk, e);

            double vk = Math.Atan2(Math.Sqrt(1 - e * e) * Math.Sin(Ek), Math.Cos(Ek) - e); 
            double phi_k = vk + bcd.OmegaAngle;

            double d_uk = bcd.LatitudeCorrectionCosinusComponent * Math.Cos(2 * phi_k) + bcd.LatitudeCorrectionSinusComponent * Math.Sin(2 * phi_k);
            double d_rk = bcd.RandiusCorrectionCosinusComponent * Math.Cos(2 * phi_k) + bcd.RadiusCorrectionSinusComponent * Math.Sin(2 * phi_k);
            double d_ik = bcd.nclinationCorrectionCosinusComponent * Math.Cos(2 * phi_k) + bcd.AngularVelocity * Math.Sin(2 * phi_k);

            double uk = phi_k + d_uk;
            double rk = A * (1 - e * Math.Cos(Ek)) + d_rk;
            double ik = bcd.InitialInclination + d_ik + bcd.InclinationRate * tk;

            double xk_prim = rk * Math.Cos(uk);
            double yk_prim = rk * Math.Sin(uk);

            double omega_k = bcd.OmegaAngle0 + (bcd.AngularVelocityPerSec - WGS84_ROTATION_RATE_OF_EARTH) * tk - WGS84_ROTATION_RATE_OF_EARTH * bcd.TimeOfEphemeris;

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

