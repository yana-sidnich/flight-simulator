# Flight Simulation & Inspection App

This application is targeted for flight investigations following an accident and\or the track of certain anomalies mid-flight.

It's usage is for, but not restricted to, flight researchers and flight enthusiasts alike.

It uses csv file to register a proper flight parameters, and the purpose is to include another csv file representing a captured flight to test for any anomalies there might be.
(*Should add that the csv files provided is representing 10 lines in the table as a second in real-life flight, which means time differences between a line and it's former is only 100ms.*)

## Preview 



![alt text](https://raw.githubusercontent.com/eladoni1/pictures-for-ADP2/main/entireThing.png)



### Installation & First run

Before using this application, we need the to install the following:
 - FlightGear Simulator ( Download [here](https://www.flightgear.org/download/) )
   - After the installation, we will require to first open up FlightGear, and in the menu, select the 'Setting' tab, and from there go all the way down to 'Additional Settings'.

   once there just copy-paste the lines below:
   
   '
   --generic=socket,in,10,127.0.0.1,5400,tcp,playback_small
   
   
   --fdm=null
   '
 - .Net Framework ( Download the recommended version [here](https://dotnet.microsoft.com/download/dotnet-framework/) )
 - Microsoft Visual Studio, or another development environment for C# ( Download Visual Studio [here](https://visualstudio.microsoft.com/downloads/) )
 


Up next, we will have to download the .ZIP file of the app, and extract.
Once that is done, open the directory of the extraction, open the folder and the solution file **FlightGearTestExec.sln**, and simply press the 'Debug' button.

### Instructions

#### Connection Window - Path, IP & port selection



![alt text](https://raw.githubusercontent.com/eladoni1/pictures-for-ADP2/main/%D7%AA%D7%A4%D7%A8%D7%99%D7%98%20paths.PNG)


#### Path selection

In the next window we will see the app has several requests for paths:
1. CSV file's path for the flight in for testing.
2. CSV file's path for the standarized flight.
3. DLL's folder path that is included inside the extracted folder (This is for programmers to change the algorithm of the anomaly detection if needed be).
4. FlightGear's folder path.


#### IP and port selection

![alt text](https://raw.githubusercontent.com/eladoni1/pictures-for-ADP2/main/%D7%AA%D7%A4%D7%A8%D7%99%D7%98%20paths%20%2B%20%D7%94%D7%92%D7%93%D7%A8%D7%AA%20IP%20%D7%95-Port.png)

After that the user may be able to change to a different IP address and port (**optional**), but there's the default settings of local host IP address and FlightGear's port already selected.

The user **must** press 'Launch Flight Gear' and wait for it to finish loading in order for the Simulator Controller to open up. if not, the user will be notified the connection failed.

![alt text](https://raw.githubusercontent.com/eladoni1/pictures-for-ADP2/main/%D7%AA%D7%A4%D7%A8%D7%99%D7%98%20paths%20%2B%20%D7%9C%D7%97%D7%99%D7%A6%D7%94%20%D7%A2%D7%9C%20launch%20flightgear.png)

Only When FlightGear's Window completed it's loading screen, the user may press 'Connect to Simulator Conntroller'.

![alt text](https://raw.githubusercontent.com/eladoni1/pictures-for-ADP2/main/%D7%AA%D7%A4%D7%A8%D7%99%D7%98%20paths%20%2B%20%D7%9C%D7%97%D7%99%D7%A6%D7%94%20%D7%A2%D7%9C%20connect%20to%20simulator.png)

After the Simulator Controller opened, the user may start by changing the speed, or the time requested to be queried, or just hold until the anomaly come up on screen as a red-dot on the correlation graph.

 - **Media Player**
 
 ![alt text](https://raw.githubusercontent.com/eladoni1/pictures-for-ADP2/main/MediaPlayer%20in%20MainWindow.PNG)
 
 The user can use the multimedia buttons to control or target a specific frame in the flight, or speed things up, or even start playing backwards.
 
 - **Steers, rudder and throttle**
 
 ![alt text](https://raw.githubusercontent.com/eladoni1/pictures-for-ADP2/main/Joystick%20in%20mainWindow.PNG)
 
 The user can be notified with the steering joystick's direction in the current time of flight, and also the rudder and throttle values.
 



