using System;
using System.Globalization;
using System.IO.Ports;
using System.Runtime.InteropServices;
using System.Text;
using GnssLibCALC.Models.SatModels;
using GnssLibDL;

namespace GnssLibNMEA_Writer
{

    public class NmeaStringsGenerator
    {
        private static Random _random = new Random();
        private static SerialPort _SerialPort;

        /// <summary>
        /// Creates all NMEA-messages from data in the SimulationController and whrites it to the serialPort.
        /// </summary>
        /// <param name="serialPort">The output serialPort. I.e. "COM1".</param>
        /// <param name="simulationController">The SimulationController</param>
        public static void NmeaGenerator(SerialPort serialPort, SimulationController simulationController)
        {
            _SerialPort = serialPort;
            List<string> activeSatellitesGPS = simulationController.visibleSatellitesPRN_GPS;
            List<string> activeSatellitesGL = simulationController.visibleSatellitesPRN_GL;
            double PDOP = simulationController.PDOP;
            double HDOP = simulationController.HDOP;
            double VDOP = simulationController.VDOP;
            List<SatelliteElevationAndAzimuthInfo> satListGPS = simulationController.satListGPS;
            List<SatelliteElevationAndAzimuthInfo> satListGL = simulationController.satListGL;
            DateTime utcTime = simulationController.simulationStartDateTime.AddSeconds(simulationController.continousSecFromStart); ;
            double latitude = simulationController.rConfig.ReceiverLatitude;
            double longitude = simulationController.rConfig.ReceiverLongitude;
            double elevation = simulationController.rConfig.ReceiverAltitude;

            string latD = "N";
            string longD = "E";
            if (latitude < 0)
            {
                latD = "S";
            }
            if (longitude < 0)
            {
                longD = "W";
            }


            int latDegrees = Math.Abs((int)latitude);
            double latDecimalMinutes = (Math.Abs(latitude) - Math.Abs(latDegrees)) * 60;

            int longDegrees = Math.Abs((int)longitude);
            double longDecimalMinutes = (Math.Abs(longitude) - Math.Abs(longDegrees)) * 60;


            string latString = $"{latDegrees}{latDecimalMinutes}".Replace(',', '.');
            if (latitude > 0 && latitude < 10)
            {
                latString = $"0{latDegrees}{latDecimalMinutes}".Replace(',', '.');
            }
            else if (latitude > -10 && latitude < 0)
            {
                latString = $"0{latDegrees}{latDecimalMinutes}".Replace(',', '.'); ;
            }

            string longString = $"{longDegrees}{longDecimalMinutes}".Replace(',', '.');
            if (longitude > 0 && longitude < 10)
            {
                longString = $"00{longDegrees}{longDecimalMinutes}".Replace(',', '.');
            }
            else if (longitude > 0 && longitude < 100)
            {
                longString = $"0{longDegrees}{longDecimalMinutes}".Replace(',', '.');
            }
            else if (longitude > -10 && longitude < 0)
            {
                longString = $"00{longDegrees}{longDecimalMinutes}".Replace(',', '.');
            }
            else if (longitude > -100 && longitude < 0)
            {
                longString = $"0{longDegrees}{longDecimalMinutes}".Replace(',', '.');
            }

            List<string> NMEAString = new List<string>();
            NMEAString.Add(ConstructGPGGAString("GN", utcTime.ToString("hhmmss.ff"), latString, latD, longString, longD, 1, satListGPS.Count + satListGL.Count, HDOP, Math.Round(elevation, 3), -15, 0, ""));
            if (satListGPS.Count != 0)
            {
                NMEAString.Add(ConstructGPGSAString("GP", activeSatellitesGPS, PDOP, HDOP, VDOP));

                foreach (string message in ConstructGPGSVString("GP", satListGPS))
                {
                    NMEAString.Add(message);
                }
            }
            if (satListGL.Count != 0)
            {
                NMEAString.Add(ConstructGPGSAString("GL", activeSatellitesGL, PDOP, HDOP, VDOP));
                foreach (string message in ConstructGPGSVString("GL", satListGL))
                {
                    NMEAString.Add(message);
                }
            }

            WriteToCOM(NMEAString);

        }

        public static void ClearGPSView()
        {
            List<string> NMEAString = new List<string>();
            NMEAString.Add("$GPGSA,A,3,,,,,,,,,,,,0,0,0*" + CalculateChecksum("$GPGSA,A,1,,,,,,,,,,,,0,0,0"));
            NMEAString.Add("$GPGSV,1,0*" + CalculateChecksum("$GPGSV,1,0"));
            NMEAString.Add("$GLGSA,A,3,0.0,0.0,0.0*" + CalculateChecksum("$GLGSA,A,3,0.0,0.0,0.0"));
            NMEAString.Add("$GLGSV,1,0*" + CalculateChecksum("$GLGSV,1,0"));
            string tempnone = "$GNGGA, 120001.00, 000.00, N, 00000.000, E, 1, 0, 0.0, 0.0, M, 0, M, 0,,";
            NMEAString.Add(tempnone + "*" + CalculateChecksum(tempnone));

            WriteToCOM(NMEAString);
        }


        public static void WriteToCOM(List<string> NMEAString)
        {
            foreach (string str in NMEAString)
            {
                _SerialPort.WriteLine(str);
            }
        }

        /// <summary>
        /// Constructs the GNGGA-string for the NMEA-message.
        /// </summary>
        /// <param name="satType">GP for Gps, GL for Glonass (used for Galileo in our case) and GN (for GNSS) for combined messages.</param>
        /// <param name="utcTime"></param>
        /// <param name="latitude"></param>
        /// <param name="latitudeDirection"></param>
        /// <param name="longitude"></param>
        /// <param name="longitudeDirection"></param>
        /// <param name="qualityIndicator"></param>
        /// <param name="numberOfSatellites"></param>
        /// <param name="HDOP"></param>
        /// <param name="orthometricHeight"></param>
        /// <param name="geoidSeparation"></param>
        /// <param name="ageOfDGPSData"></param>
        /// <param name="referenceStationID"></param>
        /// <returns>The GGA-string.</returns>
        public static string ConstructGPGGAString(string satType, string utcTime, string latitude, string latitudeDirection,
                                      string longitude, string longitudeDirection, int qualityIndicator,
                                      int numberOfSatellites, double HDOP, double orthometricHeight,
                                      double geoidSeparation, double ageOfDGPSData, string referenceStationID)
        {
            // Construct GGA message string
            string ggaMessage = $"${satType}GGA,{utcTime},{latitude}000,{latitudeDirection},{longitude}000,{longitudeDirection}," +
                                $"{qualityIndicator:D1},{numberOfSatellites:D2},{HDOP.ToString(CultureInfo.InvariantCulture)}," +
                                $"{orthometricHeight.ToString(CultureInfo.InvariantCulture)},M," +
                                $"{geoidSeparation.ToString(CultureInfo.InvariantCulture)},M,{ageOfDGPSData},,{referenceStationID}";
            // Calculate and append checksum
            string checksum = CalculateChecksum(ggaMessage);
            ggaMessage += $"*{checksum}";
            return ggaMessage;
        }

        /// <summary>
        /// Construct the GSA-string for the NMEA-message.
        /// </summary>
        /// <param name="satType">GP for Gps, GL for Glonas but in our case used for Galileo.</param>
        /// <param name="activeSatellites">List of all active satellites.</param>
        /// <param name="PDOP">Position dilution of precision.</param>
        /// <param name="HDOP">Horizontal dilution of precision.</param>
        /// <param name="VDOP">Vertical dilution of precision.</param>
        /// <returns></returns>
        public static string ConstructGPGSAString(string satType, List<string> activeSatellites, double PDOP, double HDOP, double VDOP)
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
            for (int i = 0; i < numberCommas; i++)
            {
                gsaMessage += ",";
            }

            gsaMessage += $"{PDOP.ToString(CultureInfo.InvariantCulture)},{HDOP.ToString(CultureInfo.InvariantCulture)},{VDOP.ToString(CultureInfo.InvariantCulture)}";
            string checksum = CalculateChecksum(gsaMessage);
            gsaMessage += "*" + checksum;
            return gsaMessage;
        }


        /// <summary>
        /// Constructs the GSV-string for the NEMA-messeges.
        /// </summary>
        /// <param name="satType"></param>
        /// <param name="satList"></param>
        /// <returns></returns>
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

        /// <summary>
        /// Helper function for creating the GSV-string in the NMEA-message's.
        /// </summary>
        /// <param name="satType"></param>
        /// <param name="satellites"></param>
        /// <param name="messageNumber"></param>
        /// <param name="totalMessages"></param>
        /// <param name="totaltSats"></param>
        /// <returns></returns>
        public static string ConstructGSVMessage(string satType, List<SatelliteElevationAndAzimuthInfo> satellites, int messageNumber, int totalMessages, int totaltSats)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append($"${satType}GSV,{totalMessages},{messageNumber},{totaltSats}");

            foreach (SatelliteElevationAndAzimuthInfo satellite in satellites)
            {
                if (satType == "GL")
                {
                    sb.Append($",{int.Parse(satellite.SatId) + 64},{satellite.Elevation},{satellite.Azimuth},{GenerateRandomSNR(satellite.Elevation)}");// fixed snr for now, to test
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

        /// <summary>
        /// Calculate the checksum for the NMEA-Strings. 
        /// For each character, it performs a bitwise XOR operation with the current checksum.
        /// </summary>
        /// <param name="message">The string to be calculated.</param>
        /// <returns>The checksum for this message string.</returns>
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
        /// <summary>
        /// Checks if elevation is higher or lower than 10 and returns SNR-number accordingly.
        /// </summary>
        /// <param name="elevation">Elevation above horizon (in degrees).</param>
        /// <returns>Simulated SNR-number.</returns>
        private static int GenerateRandomSNR(double elevation)
        {
            if (elevation < 10)
            {
                return (int)SimulateSnrHelper(0, 25, 12, 16, 4);
            }
            else
            {
                return (int)SimulateSnrHelper(10, 45, 20, 30, 5);
            }
        }

        /// <summary>
        /// Creates a simulated SNR-number for the NMEA-message using box-muller method. Generates random numbers with a normaldistrubution using a median and standard deviation values. 
        /// </summary>
        /// <param name="minValue">Minimum SNR-number.</param>
        /// <param name="maxValue">Maximum SNR-number.</param>
        /// <param name="focusMin">Focuses on generating number's from this value.</param>
        /// <param name="focusMax">Focuses on generating numbers's to this value.</param>
        /// <returns>A number from minValue to maxValue but with a focus in the range from focusMin to focusMax.</returns>
        
        private static double SimulateSnrHelper(double minValue, double maxValue, double focusMin, double focusMax, double stdDev)
        {
            double mean = (focusMin + focusMax) / 2;
            double firstRandom = _random.NextDouble();
            double secondRandom = _random.NextDouble();

            double randStdNormal = Math.Sqrt(-2.0 * Math.Log(firstRandom)) * Math.Sin(2.0 * Math.PI * secondRandom);
            double randNormal = mean + stdDev * randStdNormal;
            return Math.Max(minValue, Math.Min(maxValue, randNormal));
        }
    }
}

