using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GnssLibDL
{
    internal class BroadcastParserForRinex_v4
    {
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
