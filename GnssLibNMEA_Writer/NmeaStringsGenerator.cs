using System;
using System.Globalization;
using System.IO.Ports;
using System.Text;
using GnssLibCALC.Models.SatModels;
using GnssLibDL;

namespace GnssLibNMEA_Writer
{

	public class NmeaStringsGenerator
	{
        private static Random random = new Random();
        
        public static void NmeaGenerator(SerialPort serialPort, SimulationController sc)
        {

            sc.NmeaValues(out List<string> activeSatellites, out double PDOP, out double HDOP, out double VDOP,
                                out List<Satellite> satList, out DateTime utcTime, out double latitude, out double longitude);

            List<string> NMEAString = new List<string>();
            NMEAString.Add(ConstructGPGGAString(utcTime.ToString("hhmmss.ff"), latitude.ToString(CultureInfo.InvariantCulture), "N", 
                longitude.ToString(CultureInfo.InvariantCulture), "E", 1, satList.Count, HDOP, 10, -15, 0, ""));
            NMEAString.Add(ConstructGPGSAString(activeSatellites, PDOP, HDOP, VDOP));

            foreach (string message in ConstructGPGSVString(satList))
            {
                NMEAString.Add(message);
            }


            foreach (string str in NMEAString)
            {
                serialPort.WriteLine(str);
            }

        }

        public static string ConstructGPGGAString(string utcTime, string latitude, string latitudeDirection,
                                      string longitude, string longitudeDirection, int qualityIndicator,
                                      int numberOfSatellites, double HDOP, double orthometricHeight,
                                      double geoidSeparation, double ageOfDGPSData, string referenceStationID)
        {
            // Construct GGA message string
            string ggaMessage = $"$GPGGA,{utcTime},{latitude},{latitudeDirection},0{longitude},{longitudeDirection}," +
                                $"{qualityIndicator:D1},{numberOfSatellites:D2},{HDOP.ToString(CultureInfo.InvariantCulture)},{orthometricHeight.ToString(CultureInfo.InvariantCulture)},M," +
                                $"{geoidSeparation.ToString(CultureInfo.InvariantCulture)},M,{ageOfDGPSData},,{referenceStationID}";

            // Calculate and append checksum
            string checksum = CalculateChecksum(ggaMessage);
            ggaMessage += $"*{checksum}";

            return ggaMessage;
        }

        public static string ConstructGPGSAString(List<string> activeSatellites, double PDOP, double HDOP, double VDOP)
        {

            int numberCommas = 0;
            if (activeSatellites.Count > 12)
            {
                int removeAmount = activeSatellites.Count - 12;
                activeSatellites.RemoveRange(12, removeAmount);
            }
            else
            {
                numberCommas = 12 - activeSatellites.Count;
            }


            string prnNumbers = string.Join(",", activeSatellites.Select(prn => prn.ToString().Substring(1)));
            string gsaMessage = $"$GPGSA,A,3,{prnNumbers},";
            for (int i = 0; i < numberCommas; i++)
            {
                gsaMessage += ",";
            }


            gsaMessage += $"{PDOP.ToString(CultureInfo.InvariantCulture)},{HDOP.ToString(CultureInfo.InvariantCulture)},{VDOP.ToString(CultureInfo.InvariantCulture)}";

            string checksum = CalculateChecksum(gsaMessage);
            gsaMessage += "*" + checksum;
            return gsaMessage;
        }

        public static List<string> ConstructGPGSVString(List<Satellite> satList)
        {
            List<string> messages = new List<string>();

            // Calculate the number of messages needed
            int totalSatellites = satList.Count;
            int totalMessages = (int)Math.Ceiling((double)totalSatellites / 4);

            // Construct and add GSV messages
            for (int i = 0; i < totalMessages; i++)
            {
                int startIndex = i * 4;
                int endIndex = Math.Min(startIndex + 4, totalSatellites);
                List<Satellite> subset = satList.GetRange(startIndex, endIndex - startIndex);
                string message = ConstructGSVMessage(subset, i + 1, totalMessages, totalSatellites);
                messages.Add(message);
            }

            return messages;
        }

        public static string ConstructGSVMessage(List<Satellite> satellites, int messageNumber, int totalMessages, int totaltSats)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append($"$GPGSV,{totalMessages},{messageNumber},{totaltSats}");

            foreach (Satellite satellite in satellites)
            {
                sb.Append($",{satellite.SatId},{satellite.Elevation},{satellite.Azimuth},{GenerateRandomSNR(satellite.Elevation)}");// fixed snr for now, to test
            }

            // Calculate and append checksum
            // added fixed for now
            string checksum = CalculateChecksum(sb.ToString());
            sb.Append($"*{checksum}");

            return sb.ToString();
        }

        
        private static string CalculateChecksum(string message)
        {
            int checksum = 0;
            foreach (char c in message)
            {
                if (c == '$')
                    continue;
                if (c == '*')
                    break;
                checksum ^= (int)c;
            }
            return checksum.ToString("X2");
        }

        private static int GenerateRandomSNR(double elevation)
        {

            if (elevation < 10)
            {
                return (int) SimulateSnrHelper(0, 25, 10, 20);
            }
            else
            {
                return (int)SimulateSnrHelper(10, 45, 22, 33);
            }

            //return random.Next(0, 40);


        }
        private static double SimulateSnrHelper(double minValue, double maxValue,double focusMin, double focusMax)
        {
            double mean = (focusMin + focusMax) / 2;
            double stdDev = (focusMax - focusMin) / 6;




            double u1 = 1.0 - random.NextDouble();
            double u2 = 1.0 - random.NextDouble();

            double randStdNormal = Math.Sqrt(-2.0 * Math.Log(u1)) * Math.Sin(2.0 * Math.PI * u2);
            double randNormal = mean + stdDev * randStdNormal;
            return Math.Max(minValue, Math.Min(maxValue, randNormal));




        }
    }
}

