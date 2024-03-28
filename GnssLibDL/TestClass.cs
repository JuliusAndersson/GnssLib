namespace GnssLibDL
{
    
    public class TestClass
    {  
        public string? testVar;
        public string hello(int h, int m, int s)
        {
            testVar = "testt";
            Console.WriteLine( "test");
            return h + " " + m + " " + s;
        }

        public string setValues(bool gps, bool galileo, bool glonass, DateTime dateTime, double latPos, double longPos, bool jammer, double jamRad, double jamLat, double jamLong)
        {
            return gps.ToString() + " " + galileo.ToString() + " " + glonass.ToString() + " " + dateTime.ToString() + " " + latPos.ToString() + " " + longPos.ToString() + " " + jammer.ToString() + " " + jamRad.ToString() + " " + jamLat.ToString() + " " + jamLong.ToString();
        }
        
        public string tetet(double latPos)
        {
            return latPos.ToString();
        }

        public void SetInter()
        {

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