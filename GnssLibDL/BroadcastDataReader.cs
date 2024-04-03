﻿using GnssLibCALC.Models;
using GnssLibCALC.Models.BroadCastDataModels;
using GnssLibCALC.Models.SatelliteSystemModels;
using GnssLibCALC.Models.SatModels;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GnssLibDL
{
    internal class BroadCastDataReader
    {
        public Satellites ReadBroadcastData(string filePath)
        {
            List<BroadCastDataLNAV> broadcastDataListGps = new List<BroadCastDataLNAV>();
            List<BroadCastDataINAV> broadcastDataListGAL = new List<BroadCastDataINAV>();
            List<BroadCastDataFDMA> broadcastDataListGLO = new List<BroadCastDataFDMA>();

            //Parser parser = new Parser();
            using (StreamReader reader = new StreamReader(filePath))
            {
                string? line;
                while ((line = reader.ReadLine()) != null)
                {
                    if (line.StartsWith("> EPH G") && line.Contains("LNAV"))
                    {
                        List<string> gpsBlock = new List<string>();
                        int type = 0;
                        if (line.Contains("CNAV"))
                        {
                            type = 9;
                        }
                        else if (line.Contains("LNAV"))
                        {
                            type = 8;
                        }

                        string block = line;
                        for (int i = 0; i < type; i++)
                        {
                            line = reader.ReadLine();
                            if (line == null)
                            {
                                break;
                            }
                            block += Environment.NewLine + line;
                        }
                        gpsBlock.Add(block);

                        foreach (string currentBlock in gpsBlock)
                        {
                            List<string> gpsTokens = new List<string>(currentBlock.Split(new char[] { ' ', '\n' }, StringSplitOptions.RemoveEmptyEntries));
                            List<string> tempList = new List<string>();

                            foreach (string token in gpsTokens)
                            {
                                Parser.GalileoAndGPSParser(token, tempList);
                            }
                            tempList.RemoveRange(0, 4);
                            broadcastDataListGps.Add(createBroadCastDataLNAV(tempList));
                        }
                    }

                    //----------------------------------------------------------------------------------

                    else if (line.StartsWith("> EPH E") && line.Contains("INAV"))
                    {
                        List<string> galileoBlock = new List<string>();
                        int type = 8;
                        string block = line;
                        for (int i = 0; i < type; i++)
                        {
                            line = reader.ReadLine();
                            if (line == null)
                            {
                                break;
                            }
                            block += Environment.NewLine + line;
                        }
                        galileoBlock.Add(block);

                        foreach (string currentBlock in galileoBlock)
                        {
                            List<string> galileoTokens = new List<string>(currentBlock.Split(new char[] { ' ', '\n' }, StringSplitOptions.RemoveEmptyEntries));
                            List<string> tempList = new List<string>();

                            foreach (string token in galileoTokens)
                            {
                                Parser.GalileoAndGPSParser(token, tempList);
                            }
                            tempList.RemoveRange(0, 4);
                            broadcastDataListGAL.Add(createBroadCastDataINAV(tempList));
                        }
                    }
                    else if (line.StartsWith("> EPH R") && line.Contains("FDMA"))
                    {
                        List<string> glonassBlock = new List<string>();
                        int type = 5;
                        string block = line;
                        for (int i = 0; i < type; i++)
                        {
                            line = reader.ReadLine();
                            if (line == null)
                            {
                                break;
                            }
                            block += Environment.NewLine + line;
                        }
                        glonassBlock.Add(block);

                        foreach (string currentBlock in glonassBlock)
                        {
                            List<string> GlonasTokens = new List<string>(currentBlock.Split(new char[] { ' ', '\n' }, StringSplitOptions.RemoveEmptyEntries));
                            List<string> tempList = new List<string>();

                            foreach (string token in GlonasTokens)
                            {
                                Parser.GlonassParser(token, tempList);
                            }
                            tempList.RemoveRange(0, 4);
                            broadcastDataListGLO.Add(createBroadCastDataFDMA(tempList));
                        }
                    }
                }

                var grouptedGPSData = broadcastDataListGps.GroupBy(data => data.SatId);
                List<GPS_Sat> gpsSatList = new List<GPS_Sat>();
                foreach (var group in grouptedGPSData)
                {
                    GPS_Sat gpsSat = new GPS_Sat
                    {
                        id = group.Key,
                        Data = group.ToList()
                    };
                    gpsSatList.Add(gpsSat);
                }
                Gps gps = new Gps()
                {
                    satList = gpsSatList
                };


                var grouptedGlonassData = broadcastDataListGLO.GroupBy(data => data.SatId);
                List<Glonass_Sat> gloSatList = new List<Glonass_Sat>();
                foreach (var group in grouptedGlonassData)
                {
                    Glonass_Sat GloSat = new Glonass_Sat
                    {
                        id = group.Key,
                        Data = group.ToList()
                    };
                    gloSatList.Add(GloSat);
                }
                Glonass glonass = new Glonass()
                {
                    satList = gloSatList
                };
                Satellites sats = new Satellites
                {
                    galileo = GroupAndConstructGalileoObject(broadcastDataListGAL), //Test to make it "prettier/easier to read"
                    gps = gps,
                    glonass = glonass
                };

                return sats;
            }
        }

        private BroadCastDataLNAV createBroadCastDataLNAV(List<string> broadCastDataAsList)
        {
            return new BroadCastDataLNAV()
            {
                SatId = broadCastDataAsList[0],
                DateTime = new DateTime(int.Parse(broadCastDataAsList[1]), int.Parse(broadCastDataAsList[2]), int.Parse(broadCastDataAsList[3]), int.Parse(broadCastDataAsList[4]), int.Parse(broadCastDataAsList[5]), int.Parse(broadCastDataAsList[6])).AddHours(-2),
                ClockBias = double.Parse(broadCastDataAsList[7], NumberStyles.Float, CultureInfo.InvariantCulture),
                ClockDrift = double.Parse(broadCastDataAsList[8], NumberStyles.Float, CultureInfo.InvariantCulture),
                ClockDriftRate = double.Parse(broadCastDataAsList[9], NumberStyles.Float, CultureInfo.InvariantCulture),
                //Orbit 1
                A_DOT = double.Parse(broadCastDataAsList[10], NumberStyles.Float, CultureInfo.InvariantCulture),
                C_rs = double.Parse(broadCastDataAsList[11], NumberStyles.Float, CultureInfo.InvariantCulture),
                Delta_n0 = double.Parse(broadCastDataAsList[12], NumberStyles.Float, CultureInfo.InvariantCulture),
                M0 = double.Parse(broadCastDataAsList[13], NumberStyles.Float, CultureInfo.InvariantCulture),
                //Orbit 2
                C_uc = double.Parse(broadCastDataAsList[14], NumberStyles.Float, CultureInfo.InvariantCulture),
                e_Ecentricity = double.Parse(broadCastDataAsList[15], NumberStyles.Float, CultureInfo.InvariantCulture),
                C_us = double.Parse(broadCastDataAsList[16], NumberStyles.Float, CultureInfo.InvariantCulture),
                sqrtA = double.Parse(broadCastDataAsList[17], NumberStyles.Float, CultureInfo.InvariantCulture),
                //Orbit 3
                t_oe = double.Parse(broadCastDataAsList[18], NumberStyles.Float, CultureInfo.InvariantCulture),
                C_ic = double.Parse(broadCastDataAsList[19], NumberStyles.Float, CultureInfo.InvariantCulture),
                OmegaA0 = double.Parse(broadCastDataAsList[20], NumberStyles.Float, CultureInfo.InvariantCulture),
                C_is = double.Parse(broadCastDataAsList[21], NumberStyles.Float, CultureInfo.InvariantCulture),
                //Orbit 4
                i0 = double.Parse(broadCastDataAsList[22], NumberStyles.Float, CultureInfo.InvariantCulture),
                C_rc = double.Parse(broadCastDataAsList[23], NumberStyles.Float, CultureInfo.InvariantCulture),
                omega = double.Parse(broadCastDataAsList[24], NumberStyles.Float, CultureInfo.InvariantCulture),
                OMEGA_DOT = double.Parse(broadCastDataAsList[25], NumberStyles.Float, CultureInfo.InvariantCulture),
                //Orbit 5
                IDOT = double.Parse(broadCastDataAsList[26], NumberStyles.Float, CultureInfo.InvariantCulture),
                L2_code = double.Parse(broadCastDataAsList[27], NumberStyles.Float, CultureInfo.InvariantCulture),
                GPS_WEEK = double.Parse(broadCastDataAsList[28], NumberStyles.Float, CultureInfo.InvariantCulture),
                L2_P = double.Parse(broadCastDataAsList[29], NumberStyles.Float, CultureInfo.InvariantCulture),
                //Orbit 6
                SV_acc = double.Parse(broadCastDataAsList[30], NumberStyles.Float, CultureInfo.InvariantCulture),
                SV_health = double.Parse(broadCastDataAsList[31], NumberStyles.Float, CultureInfo.InvariantCulture),
                TGD = double.Parse(broadCastDataAsList[32], NumberStyles.Float, CultureInfo.InvariantCulture),
                IODC = double.Parse(broadCastDataAsList[33], NumberStyles.Float, CultureInfo.InvariantCulture),
                //Orbit 7
                t_tm = double.Parse(broadCastDataAsList[34], NumberStyles.Float, CultureInfo.InvariantCulture)
            };
        }

        private BroadCastDataINAV createBroadCastDataINAV(List<string> broadCastDataAsList)
        {
            return new BroadCastDataINAV()
            {
                SatId = broadCastDataAsList[0],
                DateTime = new DateTime(int.Parse(broadCastDataAsList[1]), int.Parse(broadCastDataAsList[2]), int.Parse(broadCastDataAsList[3]), int.Parse(broadCastDataAsList[4]), int.Parse(broadCastDataAsList[5]), int.Parse(broadCastDataAsList[6])),
                ClockBias = double.Parse(broadCastDataAsList[7], NumberStyles.Float, CultureInfo.InvariantCulture),
                ClockDrift = double.Parse(broadCastDataAsList[8], NumberStyles.Float, CultureInfo.InvariantCulture),
                ClockDriftRate = double.Parse(broadCastDataAsList[9], NumberStyles.Float, CultureInfo.InvariantCulture),
                //Orbit 1
                IOD = double.Parse(broadCastDataAsList[10], NumberStyles.Float, CultureInfo.InvariantCulture),
                C_rs = double.Parse(broadCastDataAsList[11], NumberStyles.Float, CultureInfo.InvariantCulture),
                Delta_n = double.Parse(broadCastDataAsList[12], NumberStyles.Float, CultureInfo.InvariantCulture),
                M0 = double.Parse(broadCastDataAsList[13], NumberStyles.Float, CultureInfo.InvariantCulture),
                //Orbit 2
                C_uc = double.Parse(broadCastDataAsList[14], NumberStyles.Float, CultureInfo.InvariantCulture),
                e_Ecentricity = double.Parse(broadCastDataAsList[15], NumberStyles.Float, CultureInfo.InvariantCulture),
                C_us = double.Parse(broadCastDataAsList[16], NumberStyles.Float, CultureInfo.InvariantCulture),
                sqrtA = double.Parse(broadCastDataAsList[17], NumberStyles.Float, CultureInfo.InvariantCulture),
                //Orbit 3
                t_oe = double.Parse(broadCastDataAsList[18], NumberStyles.Float, CultureInfo.InvariantCulture),
                C_ic = double.Parse(broadCastDataAsList[19], NumberStyles.Float, CultureInfo.InvariantCulture),
                OmegaA0 = double.Parse(broadCastDataAsList[20], NumberStyles.Float, CultureInfo.InvariantCulture),
                C_is = double.Parse(broadCastDataAsList[21], NumberStyles.Float, CultureInfo.InvariantCulture),
                //Orbit 4     
                i0 = double.Parse(broadCastDataAsList[22], NumberStyles.Float, CultureInfo.InvariantCulture),
                C_rc = double.Parse(broadCastDataAsList[23], NumberStyles.Float, CultureInfo.InvariantCulture),
                omega = double.Parse(broadCastDataAsList[24], NumberStyles.Float, CultureInfo.InvariantCulture),
                OMEGA_DOT = double.Parse(broadCastDataAsList[25], NumberStyles.Float, CultureInfo.InvariantCulture),
                //Orbit 5
                IDOT = double.Parse(broadCastDataAsList[26], NumberStyles.Float, CultureInfo.InvariantCulture),
                Datasources = double.Parse(broadCastDataAsList[27], NumberStyles.Float, CultureInfo.InvariantCulture),
                GAL_WEEK = double.Parse(broadCastDataAsList[28], NumberStyles.Float, CultureInfo.InvariantCulture),
                //Orbit 6      
                //SISA_Signal = double.Parse(broadCastDataAsList[29], NumberStyles.Float, CultureInfo.InvariantCulture),
                SV_health = double.Parse(broadCastDataAsList[30], NumberStyles.Float, CultureInfo.InvariantCulture),
                BGD_a = double.Parse(broadCastDataAsList[31], NumberStyles.Float, CultureInfo.InvariantCulture),
                BGD_b = double.Parse(broadCastDataAsList[32], NumberStyles.Float, CultureInfo.InvariantCulture),
                //Orbit 7
                t_tm = double.Parse(broadCastDataAsList[33], NumberStyles.Float, CultureInfo.InvariantCulture)
            };
        }

        private BroadCastDataFDMA createBroadCastDataFDMA(List<string> broadCastDataAsList)
        {
            BroadCastDataFDMA broadcastData = new BroadCastDataFDMA()
            {
                SatId = broadCastDataAsList[0],
                DateTime = new DateTime(int.Parse(broadCastDataAsList[1]), int.Parse(broadCastDataAsList[2]), int.Parse(broadCastDataAsList[3]), int.Parse(broadCastDataAsList[4]), int.Parse(broadCastDataAsList[5]), int.Parse(broadCastDataAsList[6])),
                ClockBias = double.Parse(broadCastDataAsList[7], NumberStyles.Float, CultureInfo.InvariantCulture),
                ClockDrift = double.Parse(broadCastDataAsList[8], NumberStyles.Float, CultureInfo.InvariantCulture),
                Message_frame_time = double.Parse(broadCastDataAsList[9], NumberStyles.Float, CultureInfo.InvariantCulture),
                //Orbit 1
                Pos_X = double.Parse(broadCastDataAsList[10], NumberStyles.Float, CultureInfo.InvariantCulture),
                Velocity_X = double.Parse(broadCastDataAsList[11], NumberStyles.Float, CultureInfo.InvariantCulture),
                Acc_X = double.Parse(broadCastDataAsList[12], NumberStyles.Float, CultureInfo.InvariantCulture),
                Health = double.Parse(broadCastDataAsList[13], NumberStyles.Float, CultureInfo.InvariantCulture),
                //Orbit 2
                Pos_Y = double.Parse(broadCastDataAsList[14], NumberStyles.Float, CultureInfo.InvariantCulture),
                Velocity_Y = double.Parse(broadCastDataAsList[15], NumberStyles.Float, CultureInfo.InvariantCulture),
                Acc_Y = double.Parse(broadCastDataAsList[16], NumberStyles.Float, CultureInfo.InvariantCulture),
                Frequency_number = double.Parse(broadCastDataAsList[17], NumberStyles.Float, CultureInfo.InvariantCulture),
                //Orbit 3
                Pos_Z = double.Parse(broadCastDataAsList[18], NumberStyles.Float, CultureInfo.InvariantCulture),
                Velocity_Z = double.Parse(broadCastDataAsList[19], NumberStyles.Float, CultureInfo.InvariantCulture),
                Acc_Z = double.Parse(broadCastDataAsList[20], NumberStyles.Float, CultureInfo.InvariantCulture),
                Age_of_opereration = double.Parse(broadCastDataAsList[21], NumberStyles.Float, CultureInfo.InvariantCulture)
            };

            //Orbit 4
            // Two unused filed can be null if unkown from broadcast, discard both if one is missing, never used anyways
            if (broadCastDataAsList.Count < 26)
            {
                broadcastData.Status_Flags = 0;
                broadcastData.L1_L2_delay_diff = double.Parse(broadCastDataAsList[22], NumberStyles.Float, CultureInfo.InvariantCulture);
                broadcastData.URAI = double.Parse(broadCastDataAsList[23], NumberStyles.Float, CultureInfo.InvariantCulture);
                broadcastData.Health_Flags = 0;
            }
            else
            {
                broadcastData.Status_Flags = double.Parse(broadCastDataAsList[22], NumberStyles.Float, CultureInfo.InvariantCulture);
                broadcastData.L1_L2_delay_diff = double.Parse(broadCastDataAsList[23], NumberStyles.Float, CultureInfo.InvariantCulture);
                broadcastData.URAI = double.Parse(broadCastDataAsList[24], NumberStyles.Float, CultureInfo.InvariantCulture);
                broadcastData.Health_Flags = double.Parse(broadCastDataAsList[25], NumberStyles.Float, CultureInfo.InvariantCulture);
            }

            return broadcastData;
        }

        private static Galileo GroupAndConstructGalileoObject(List<BroadCastDataINAV> broadcastDataListGAL)
        {
            // Group broadcast data by satellite ID
            var groupedGalileoData = broadcastDataListGAL.GroupBy(data => data.SatId);

            // Construct GAL_sat objects and populate with grouped data
            List<GAL_sat> galSatList = new List<GAL_sat>();
            foreach (var group in groupedGalileoData)
            {
                GAL_sat galSat = new GAL_sat
                {
                    id = group.Key,
                    Data = group.ToList()
                };
                galSatList.Add(galSat);
            }

            // Construct Galileo object
            Galileo galileo = new Galileo()
            {
                satList = galSatList
            };

           
            return galileo;

           
        }
    }
}