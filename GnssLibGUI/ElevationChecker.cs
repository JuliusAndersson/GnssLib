using GeoTiffElevationReader;


namespace GnssLibGUI
{
    internal class ElevationChecker
    {
        private string folderPath;

        public ElevationChecker(string folderPath)
        {
            this.folderPath = folderPath;
        }

        public double GetElevationAtCoordinates(double latitude, double longitude)
        {
            double elevation = 0;
            string[] files = Directory.GetFiles(folderPath, "*.tif");
            foreach (string file in files)
            {             
                GeoTiff reader;
                try
                {
                    reader = new GeoTiff(file);
                    elevation = reader.GetElevationAtLatLon(latitude, longitude);        
                    if (elevation != 0)
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
            return elevation;
        }
    }
}
