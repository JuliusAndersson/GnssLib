
namespace GnssLibDL
{
    internal class BroadcastParserForRinex_v4
    {
        /// <summary>
        /// Takes a token (parameter from a broadcastmessage), manipulates it accordingly and adds it to the inputlist.
        /// </summary>
        /// <param name="token"></param>
        /// <param name="inputList"></param>
        public static void GalileoAndGPSParser(string token, List<string> inputList)
        {
            string temp;
            if (token.Length > 19)
            {
                if (token.Substring(0, 1) == "-")
                {
                    inputList.Add(token.Substring(0, 19));
                    temp = token.Substring(19);
                }
                else if (token.Substring(2, 1) == "-")
                {
                    inputList.Add(token.Substring(0, 2));
                    temp = token.Substring(2);
                }
                else
                {
                    inputList.Add(token.Substring(0, 18));
                    temp = token.Substring(18);
                }

                while (temp.Length > 19)
                {
                    inputList.Add(temp.Substring(0, 19));
                    temp = temp.Substring(19);
                }

                if (temp.Length > 1)
                {
                    inputList.Add(temp);
                }
            }
            else if (token != "\n")
            {
                inputList.Add(token);
            }
        }

        /// <summary>
        /// Takes a token (parameter from a broadcastmessage), manipulates it accordingly and adds it to the inputlist.
        /// </summary>
        /// <param name="token"></param>
        /// <param name="inputList"></param>
        public static void GlonassParser(string token, List<string> inputList)
        {
            string temp;

            if (token.Length > 19)
            {
                if (token.Substring(0, 1) == "-")
                {
                    inputList.Add(token.Substring(0, 19));
                    temp = token.Substring(19);
                }
                else if (token.Substring(2, 1) == "-")
                {
                    inputList.Add(token.Substring(0, 2));
                    temp = token.Substring(2);
                }
                else
                {
                    inputList.Add(token.Substring(0, 18));
                    temp = token.Substring(18);
                }

                while (temp.Length > 19)
                {
                    inputList.Add(temp.Substring(0, 19));
                    temp = temp.Substring(19);
                }

                if (temp.Length > 1)
                {
                    inputList.Add(temp);
                }
            }
            else if (token != "\n")
            {
                inputList.Add(token);
            }
        }
    }
}
