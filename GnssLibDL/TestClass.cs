namespace GnssLibDL
{
    
    public class TestClass
    {  
        public string? testVar;
        public String hello(int h, int m, int s)
        {
            testVar = "testt";
            Console.WriteLine( "test");
            return h + " " + m + " " + s;
        }

        public string setValues(Boolean gps, Boolean galileo, Boolean glonass, DateTime dateTime, double latPos, double longPos, Boolean jammer, double jamRad, double jamLat, double jamLong)
        {
            return gps + " " + galileo + " " + glonass + " " + dateTime + " " + latPos + " " + longPos + " " + jammer + " " + jamRad + " " + jamLat + " " + jamLong;
        }
        

        /*
         * 
         * simulator controller
         * sim inizilize
         * sim top
         * time-ticker 
         * 
         */
    }
}