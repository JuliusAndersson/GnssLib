using GeoTiffElevationReader;


namespace GnssLibGUI
{
    internal class AltitudeChecker
    {
        private string folderPath;

        public AltitudeChecker(string folderPath)
        {
            this.folderPath = folderPath;
        }
        /// <summary>
        /// Checks if there is altitude data in the folder containing DEM's for the specified coordinatets.
        /// </summary>
        /// <param name="latitude">Latitude in decimal degrees.</param>
        /// <param name="longitude">Longitude in decimal degrees.</param>
        /// <returns>Altitude found in DEM's, or 0 if none were found.</returns>
        public double GetAltitudeAtCoordinates(double latitude, double longitude)
        {
            double altitude = 0;
            string[] files = Directory.GetFiles(folderPath, "*.tif");
            foreach (string file in files)
            {             
                GeoTiff reader;
                try
                {
                    reader = new GeoTiff(file);
                    altitude = reader.GetElevationAtLatLon(latitude, longitude);        
                    if (altitude != 0)
                    {
                        break; 
                    }
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message);                   
                    continue;
                }
            }
            return altitude;
        }
    }
}
