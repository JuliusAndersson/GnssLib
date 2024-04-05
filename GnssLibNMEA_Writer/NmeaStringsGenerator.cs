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
        private static Random _random = new Random();
        
        public static void NmeaGenerator(SerialPort serialPort, SimulationController sc)
        {


            List<string> activeSatellitesGPS = sc._visibleSatellitesPRN_GPS;
            List<string> activeSatellitesGL = sc._visibleSatellitesPRN_GL;
            double PDOP = sc._PDOP;
            double HDOP = sc._HDOP;
            double VDOP = sc._VDOP;
            List<SatelliteElevationAndAzimuthInfo> satListGPS = sc._satListGPS;
            List<SatelliteElevationAndAzimuthInfo> satListGL = sc._satListGL;
            DateTime utcTime = sc._rConfig.ReceiverStartDT.AddSeconds(sc._continousSecFromStart); ;
            double latitude = sc._rConfig.ReceiverLatitude;
            double longitude = sc._rConfig.ReceiverLongitude;
            double elevation = sc._rConfig.ReceiverElevetion;

              
            String latD = "N";
            String longD = "E";
            if(latitude < 0)
            {
                latD = "S";
            }
            if(longitude < 0)
            {
                longD = "W";
            }
            
            String latString = latitude.ToString(CultureInfo.InvariantCulture);
            if(latitude > 0 && latitude < 10)
            {
                latString = "0" + latString;
            }else if(latitude > -10 && latitude < 0)
            {
                latString = "0" + (latitude * -1).ToString(CultureInfo.InvariantCulture);
            }else if(latitude < -10)
            {
                latString = (latitude * -1).ToString(CultureInfo.InvariantCulture);
            }
            String longString = longitude.ToString(CultureInfo.InvariantCulture);
            if(longitude > 0 && longitude < 10)
            {
                longString = "00" + longString;
            }else if(longitude > 0 && longitude < 100)
            {
                longString = "0" + longString;
            }else if(longitude > -10 && longitude < 0)
            {
                longString = "00" +(longitude * -1).ToString(CultureInfo.InvariantCulture);
            }else if(longitude > -100 && longitude < 0 )
            {
                longString = "0" + (longitude * -1).ToString(CultureInfo.InvariantCulture);
            }else if(longitude < -100)
            {
                longString = (longitude * -1).ToString(CultureInfo.InvariantCulture);
            }

            List<string> NMEAString = new List<string>();
            NMEAString.Add(ConstructGPGGAString("GN",utcTime.ToString("hhmmss.ff"), latString, latD, longString, longD, 1, satListGPS.Count+satListGL.Count, HDOP, elevation, -15, 0, ""));
            if (satListGPS.Count != 0)
            {
                NMEAString.Add(ConstructGPGSAString("GP", activeSatellitesGPS, PDOP, HDOP, VDOP));

                foreach (string message in ConstructGPGSVString("GP", satListGPS))
                {
                    NMEAString.Add(message);
                }
            }


            //NMEAString.Add(ConstructGPGGAString("GL", utcTime.ToString("hhmmss.ff"), latString, latD, longString, longD, 1, satList.Count, HDOP, elevation, -15, 0, ""));

            if (satListGL.Count != 0)
            {
                NMEAString.Add(ConstructGPGSAString("GL", activeSatellitesGL, PDOP, HDOP, VDOP));

            foreach (string message in ConstructGPGSVString("GL", satListGL))
            {
                NMEAString.Add(message);
            }

            }
            
            foreach (string str in NMEAString)
            {
                serialPort.WriteLine(str);
            }

        }

        public static string ConstructGPGGAString(string satType, string utcTime, string latitude, string latitudeDirection,
                                      string longitude, string longitudeDirection, int qualityIndicator,
                                      int numberOfSatellites, double HDOP, double orthometricHeight,
                                      double geoidSeparation, double ageOfDGPSData, string referenceStationID)
        {
            // Construct GGA message string
            string ggaMessage = $"${satType}GGA,{utcTime},{latitude},{latitudeDirection},{longitude},{longitudeDirection}," +
                                $"{qualityIndicator:D1},{numberOfSatellites:D2},{HDOP.ToString(CultureInfo.InvariantCulture)}," +
                                $"{orthometricHeight.ToString(CultureInfo.InvariantCulture)},M," +
                                $"{geoidSeparation.ToString(CultureInfo.InvariantCulture)},M,{ageOfDGPSData},,{referenceStationID}";
            // Calculate and append checksum
            string checksum = CalculateChecksum(ggaMessage);
            ggaMessage += $"*{checksum}";
            return ggaMessage;
        }

        public static string ConstructGPGSAString(string satType,List<string> activeSatellites, double PDOP, double HDOP, double VDOP)
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
            string gsaMessage = $"${satType}GSA,A,3,{prnNumbers},";
            for (int i = 0; i < numberCommas-1; i++)
            {
                gsaMessage += ",";
            }

            gsaMessage += $"{PDOP.ToString(CultureInfo.InvariantCulture)},{HDOP.ToString(CultureInfo.InvariantCulture)},{VDOP.ToString(CultureInfo.InvariantCulture)}";
            string checksum = CalculateChecksum(gsaMessage);
            gsaMessage += "*" + checksum;
            return gsaMessage;
        }
        
        
        //=====================================================================================
        //This function constructs the GSV-String for the Nmea Message 
        //=====================================================================================
        public static List<string> ConstructGPGSVString(string satType, List<SatelliteElevationAndAzimuthInfo> satList)
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
                List<SatelliteElevationAndAzimuthInfo> subset = satList.GetRange(startIndex, endIndex - startIndex);
                string message = ConstructGSVMessage(satType, subset, i + 1, totalMessages, totalSatellites);
                messages.Add(message);
            }
            return messages;
        }

        //=====================================================================================
        //This is a helper-function for the GSV-String creator 
        //=====================================================================================
        public static string ConstructGSVMessage(string satType, List<SatelliteElevationAndAzimuthInfo> satellites, int messageNumber, int totalMessages, int totaltSats)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append($"${satType}GSV,{totalMessages},{messageNumber},{totaltSats}");

            foreach (SatelliteElevationAndAzimuthInfo satellite in satellites)
            {
                if (satType == "GL")
                {
                    sb.Append($",{int.Parse(satellite.SatId)+64},{satellite.Elevation},{satellite.Azimuth},{GenerateRandomSNR(satellite.Elevation)}");// fixed snr for now, to test
                }
                else
                {
                    sb.Append($",{satellite.SatId},{satellite.Elevation},{satellite.Azimuth},{GenerateRandomSNR(satellite.Elevation)}");// fixed snr for now, to test
                }

            }
            // Calculate and append checksum
            string checksum = CalculateChecksum(sb.ToString());
            sb.Append($"*{checksum}");
            return sb.ToString();
        }

        //=====================================================================================
        //Calculate the checksum for each NMEA-String
        //=====================================================================================
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
        }


        private static double SimulateSnrHelper(double minValue, double maxValue,double focusMin, double focusMax)
        {
            double mean = (focusMin + focusMax) / 2;
            double stdDev = (focusMax - focusMin) / 6;

            double u1 = 1.0 - _random.NextDouble();
            double u2 = 1.0 - _random.NextDouble();

            double randStdNormal = Math.Sqrt(-2.0 * Math.Log(u1)) * Math.Sin(2.0 * Math.PI * u2);
            double randNormal = mean + stdDev * randStdNormal;
            return Math.Max(minValue, Math.Min(maxValue, randNormal));
        }
    }
}

