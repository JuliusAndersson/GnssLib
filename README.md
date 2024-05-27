# GnssLib

## Description
This is a prototype GNSS simulator developed during a thesis project. It calculates satellite positions based on broadcast files downloaded from NASA. From inputs such as position, time, GNSS-type, and ephemeris files you get satellite positions visualised via VisualGPSView, Quality indicators in the form of PDOP, VDOP, and HDOP, and the position. You also have the ability to "simulate" interference by configuring the jammer in the GUI. In the Current state of the program, two interference models are available. The first is called the "Dome Mode", and it works by representing the interference as a sphere with a set radius, or "jamming range", and if the vector between the receiver position and satellite intersects the sphere, it removes the satellite from the DOP calculation. The second model uses a more realistic and physiological approach but utilises a simple implementation. Electromagnetic waves as those in GPS communication do not bounce on the ionosphere, therefore, the receiver and the jammer need a line of sight of each other in order for the jammer to work. After calculations, an assumption was made that with reasonable and realistic values of jammer wattages, a jammer would always be able to jam a receiver if there is a line of sight. 

## Thesis Report
In the repository, the final thesis report can also be found where various parts of the projects are explained in more detail.


## Pre-requirements
Required third-party programs in order for the program to work as intended:

- VisualGPSView for visualisation. [VisualGPSView](https://www.visualgps.net/#visualgpsview-content)
- Virtual serial ports tool for communication with VisualGPSView. [Virtual COM port](https://freevirtualserialports.com/)

If you would want functionality outside the pre-added files. Make sure you place them in their corresponding folder within the Recourses folder.
- If you want to add more broadcast ephemeris files for different days they can be downloaded from NASA here: [NASA.GOV](https://cddis.nasa.gov/Data_and_Derived_Products/GNSS/broadcast_ephemeris_data.html) Be sure to download in the RINEX V.4 format. 
- If you would want to add different digital elevation models, they can also be downloaded from NASA here: [Earth Data](https://search.earthdata.nasa.gov/search/granules?p=C1711961296-LPCLOUD&pg[0][v]=f&pg[0][gsk]=-start_date&fi=ASTER&tl=1712687657!3!!)

## Instructions
1. Load the broadcast file from the desired day by adding it to the "Broadcast" folder.
2. Load elevation maps for the desired location by downloading DEM's from NASA and add them to the "NasaElevationMaps" folder.
3. Configure the receiver with a latitude and longitude coordinate, specify what time of the day the simulation should start, and what constellation you want to use. Currently, GPS and Galileo are available.
4. Configure the jammer with latitude and longitude coordinates, choose which model you want to use, and specify the range condition.
## For visualisation
5. Install VisualGPSView.
6. Install Virtual Serial Ports (VSP).
7. In VSP, set up a virtual COM port, e.g. COM1 -> COM2.
8. Then specify the VisualGPSView to take COM2 as input.
