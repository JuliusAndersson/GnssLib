using System;
using GnssLibCALC.Models.BroadCastDataModels;
namespace GnssLibCALC
{
	public class CoordinatesCalculator
    {
        private const double a = 6378137;             // semi-major axis in meters
        private const double b = 6356752.3142;        // semi-minor axis in meters
        private const double GM = 3.986005e14;        //WGS-84 value for the product of gravitational constant G and the mass of the Earth M
        private const double OMEGA_e = 7.292115e-5;   //WGS-84 value of the Earth’s rotation rate


        public static string DoubleToDegreesMinutesSeconds(double coordinate, bool isLatitude)
        {
            int degrees = (int)coordinate;
            double remaining = Math.Abs(coordinate - degrees) * 60;
            int minutes = (int)remaining;
            double seconds = (remaining - minutes) * 60;

            char direction = isLatitude ? (coordinate >= 0 ? 'N' : 'S') : (coordinate >= 0 ? 'E' : 'W');

            return $"{direction} {degrees:D3}º {minutes:D2}' {seconds:00.00}\"".Replace(',', '.');
        }

        public static double[] GeodeticToECEF(double latitude, double longitude, double altitude)
        {
            // Convert degrees to radians
            double phi = latitude * Math.PI / 180.0;   // geodetic latitude in radians
            double lambda = longitude * Math.PI / 180.0; // longitude in radians

            // Convert geodetic latitude to geocentric latitude
            double esquared = 1 - Math.Pow(a, 2) / Math.Pow(b, 2);
            double Nphi = a / Math.Sqrt(1 - esquared * Math.Pow(Math.Sin(phi), 2));

            // Calculate ECEF XYZ coordinates
            double x = (Nphi + altitude) * Math.Cos(phi) * Math.Cos(lambda);
            double y = (Nphi + altitude) * Math.Cos(phi) * Math.Sin(lambda);
            double z = ((1 - esquared) * Nphi + altitude) * Math.Sin(phi);

            return new double[] { x / 1000, y / 1000, z / 1000 };
        }
        

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

            double omega_k = bcd.OmegaAngle0 + (bcd.AngularVelocityPerSec - OMEGA_e) * tk - OMEGA_e * bcd.TimeOfEphemeris;

            double xk = xk_prim * Math.Cos(omega_k) - yk_prim * Math.Cos(ik) * Math.Sin(omega_k);
            double yk = xk_prim * Math.Sin(omega_k) + yk_prim * Math.Cos(ik) * Math.Cos(omega_k);
            double zk = yk_prim * Math.Sin(ik);
            return new double[] { xk / 1000, yk / 1000, zk / 1000 };
        }

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

            double omega_k = bcd.OmegaAngle0 + (bcd.AngularVelocityPerSec - OMEGA_e) * tk - OMEGA_e * bcd.TimeOfEphemeris;

            double xk = xk_prim * Math.Cos(omega_k) - yk_prim * Math.Cos(ik) * Math.Sin(omega_k);
            double yk = xk_prim * Math.Sin(omega_k) + yk_prim * Math.Cos(ik) * Math.Cos(omega_k);
            double zk = yk_prim * Math.Sin(ik);
            return new double[] { xk / 1000, yk / 1000, zk / 1000 };
        }

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

