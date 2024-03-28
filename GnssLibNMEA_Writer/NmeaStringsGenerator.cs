using System;
using System.Globalization;
using System.Text;
using GnssLibDL.Models.SatModels;

namespace GnssLibNMEA_Writer
{

	public class NmeaStringsGenerator
	{
        private static Random random = new Random();
        
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
                sb.Append($",{satellite.SatId},{satellite.Elevation},{satellite.Azimuth},{GenerateRandomSNR()}");// fixed snr for now, to test
            }

            // Calculate and append checksum
            // added fixed for now
            string checksum = CalculateChecksum(sb.ToString());
            sb.Append($"*{checksum}");

            return sb.ToString();
        }

        public static string ConstructGPGGAMessage(string utcTime, string latitude, string latitudeDirection,
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

        private static int GenerateRandomSNR()
        {
            return random.Next(0, 51);
        }
    }
}

