# GnssLib

This is a prototype GNSS simulator developed during a thesis project. It calculates satellite positions based on broadcast files downloaded from NASA. From inputs such as position, time, GNSS type and ephemeris files. You get satellite positions visualised via VisualGPSView, Qualityindicators in the form of PDOP, VDOP and HDOP, and the position. You also have the ability to "simulate" interference by configuring the jammer in the GUI. 

Required third party programs in order for the program to work as intended:

- VisualGPSView for visualisation. [VisualGPSView](https://www.visualgps.net/#visualgpsview-content)
- Virutal serial ports tool for communication with VisualGPSView. [Virtual COM port](https://freevirtualserialports.com/)

If you want to add more ehpemeris files for different days they can be downloaded from NASA here: [NASA.GOV](https://cddis.nasa.gov/Data_and_Derived_Products/GNSS/broadcast_ephemeris_data.html) Be sure to download in the RINEX V.4 format. 
