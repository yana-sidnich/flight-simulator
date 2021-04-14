# Flight Simulation & Inspection App

This application is targeted for flight investigations following an accident and\or the track of certain anomalies mid-flight.

It's usage is for, but not restricted to, flight researchers and flight enthusiasts alike.

It uses csv file to register a proper flight parameters, and the purpose is to include another csv file representing a captured flight to test for any anomalies there might be.
(*Should add that the csv files provided is representing 10 lines in the table as a second in real-life flight, which means time differences between a line and it's former is only 100ms.*)

## Preview 



![alt text](https://github.com/eladoni1/pictures-for-ADP2/blob/main/Demo.png?raw=true)



## Installation & First run

Before using this application, we need the to install the following:
 - FlightGear Simulator:
   - Version 2020.3.x Download [here](https://www.flightgear.org/download/)
   - Version 2019.1.2 Download [here](https://sourceforge.net/projects/flightgear/files/release-2019.1/)
 - Microsoft Visual Studio, or another development environment for C# ( Download Visual Studio [here](https://visualstudio.microsoft.com/downloads/) )
 
### Setup

After the installations are done:
 - Open Visual Studio Installer > Modify > Under `Desktop & Mobile`, select `.NET desktop development` and click `Modity`.
 - In FlightGear, click on `Settings`, and under "Additional Settings", copy the following:
 
     ```
     --generic=socket,in,10,127.0.0.1,5400,tcp,playback_small
     --fdm=null
     ```     
Next, download the .ZIP file of the app, and extract.
Once that is done, get to the directory of the extraction, open the app folder and then open the solution file **FlightGearTestExec.sln**.

In the Solution Window, inside Dependencies > Packages, make sure you have the following packages installed:
  - *LiveChartsCore.1.0.4*
  - *LiveChartsCore.SkiaSharpView.1.0.4*
  - *LiveChartsCore.SkiaSharpView.WPF.1.0.4*
  - *MaterialDesignThemes.4.0.0*
  
If not, right-click the solution > Manage NuGet packages for the Solution > click the cogwheel > add a new path to the directory of the solution itself.
Then, select it in `Package source` next to the cogwheel, and install the packages. further explanation is provided under **local packages** below. 


And now, you are good to go! Just press the button up top!

## Special features

The design team have implemented a few features so the app will have it's looks:
1. *Live Charts* - this feature is including a clearer charts, emphasizes the changes of the different charts by their colors
2. *Dark mode* - adding this was also a requirement for our development team to reduce migraine during night-watch.
3. *GPS positioning* - a custom static map indicates the plane gps position. (More information # Views)

## Instructions

#### Connection Window - Path, IP & port selection



![alt text](https://raw.githubusercontent.com/eladoni1/pictures-for-ADP2/main/%D7%AA%D7%A4%D7%A8%D7%99%D7%98%20paths.PNG)


#### Path selection
(Provide flight csv files **without columns names** in first raw) 

In the next window we will see the app has several requests for paths:
1. CSV file's path for the flight in for testing.
2. CSV file's path for the standarized flight.
3. DLL's folder path that is included inside the extracted folder (This is for programmers to change the algorithm of the anomaly detection if needed be).
4. FlightGear's folder path.

If the user doesn't have any flight file that should represent a proper flight in-action, then the user may choose our own settings for a standarized flight, which is the CSV included *reg_flight*.

#### IP and port selection

![alt text](https://raw.githubusercontent.com/eladoni1/pictures-for-ADP2/main/%D7%AA%D7%A4%D7%A8%D7%99%D7%98%20paths%20%2B%20%D7%94%D7%92%D7%93%D7%A8%D7%AA%20IP%20%D7%95-Port.png)

After that the user may be able to change to a different IP address and port (**optional**), but there's the default settings of local host IP address and FlightGear's port already selected.

The user **must** press 'Launch Flight Gear' and wait for it to finish loading in order for the Simulator Controller to open up. if not, the user will be notified the connection failed.

![alt text](https://raw.githubusercontent.com/eladoni1/pictures-for-ADP2/main/%D7%AA%D7%A4%D7%A8%D7%99%D7%98%20paths%20%2B%20%D7%9C%D7%97%D7%99%D7%A6%D7%94%20%D7%A2%D7%9C%20launch%20flightgear.png)

Only When FlightGear's Window completed it's loading screen, the user may press 'Connect to Simulator Conntroller'.

![alt text](https://raw.githubusercontent.com/eladoni1/pictures-for-ADP2/main/%D7%AA%D7%A4%D7%A8%D7%99%D7%98%20paths%20%2B%20%D7%9C%D7%97%D7%99%D7%A6%D7%94%20%D7%A2%D7%9C%20connect%20to%20simulator.png)

After the Simulator Controller loaded up, the user may start by changing the speed, or the time requested to be queried, or just hold until anomalies will come up on screen as a red-dot on the correlation graph.

# Views

 - **Media Player**
 
 ![alt text](https://raw.githubusercontent.com/eladoni1/pictures-for-ADP2/main/MediaPlayer%20in%20MainWindow.PNG)
 
 The user can use the multimedia buttons to control or target a specific frame in the flight, or speed things up, or even start playing backwards (Indicated by negative speed).
 On each side of the slider there's a time indicator.
 The play button also indicates the current time.
 
 - **GPS Map**

![alt text](https://github.com/eladoni1/pictures-for-ADP2/blob/main/ani%20mapa.png?raw=true)


  Upon Initialization, we download a static map (using yandx maps - one of only free maps provider) which center is the avarage of the gps lat lon values.
  The lat lon values are converted to x,y pixels location on the image.
  This map is saved to byte[] (using async download method). With SkiSharp.WPF library we draw the map as background and draw circles representing all known GPS locations (up untill current time).
  The user can also control the zoom via slider (Which behind the scenes download a new image, recalculate the points x,y pixels position and redraw).
 
 - **Steers, rudder and throttle**
 
 ![alt text](https://raw.githubusercontent.com/eladoni1/pictures-for-ADP2/main/Joystick%20in%20mainWindow.PNG)
 
 The user can be notified with the steering joystick's direction in the current time of flight, and also the rudder and throttle values.
 
- **Graphs**
- 
  - **Variable graphs**
  
  
  ![alt text](https://raw.githubusercontent.com/eladoni1/pictures-for-ADP2/main/Graphs%20in%20MainWindow.PNG)
  
  
  The user can select each value, and check it's graph, or for it's most correlated feature, for any sudden changes mid-flight.
  
  
  - **Correlation graph**
  
  ![alt text](https://raw.githubusercontent.com/eladoni1/pictures-for-ADP2/main/Graph%20no2%20in%20MainWindow.PNG)
  
  After the user selected certain value and it's correlated feature, he will be notified by red dots whenever anomaly have been discovered by the app.
  
  To choose a different DLL - select dll from drop down menu (left to the graph) (the listed files are the ones that was avilable in the user dll selected plugins path)
  To choose a different Threshold - drag the slider to a desired value (left to the graph)
  
  
For further information, the app files includes the following folders -


- **DLL \ Plugins**
  - *LinearRegression.dll* - for linear based correlations between features - creates a line that represent the correlation between the 2 features. if a point a further away from the line, it means the correlation is smaller than usual. shows the last 30 values of both features.
  - *MinimumCircle.dll* - created for smaller correlations between the features - creates a circle that shows the last 30 values of both features - if a point is outside the circle, it means the correlation is smaller than usual.

Contains the DLLs' - the algorithms in C# language which is responsible for any anomaly detection in the app.

**Custom DLL Making**
To make your own custom dll, implement the C# class defined in the included CustomDLLImplement.txt file.
"<userControl Class Name> represents the name of the class which inherits from UserControl.
Implement the class inside the dll, and add it to the plugins Folder before running the app, and you are good to go!


- **local_packages**
  - *LiveChartsCore.1.0.4*
  - *LiveChartsCore.SkiaSharpView.1.0.4*
  - *LiveChartsCore.SkiaSharpView.WPF.1.0.4*
  - *MaterialDesignThemes.4.0.0*

 Some of this packages are not avilable in the Nuget Manager and therfor are attached in local_packages (in this git) folder.
 Visual studio should recognize nuget.config, but if not - 
 you can simply go to the Tools menu at the top, then select NuGet Package Manager and Package Manager Settings. In the screen that pops up you should add a reference to the local_packages folder.
 ![alt text](https://blog.verslu.is/wp-content/uploads/2018/02/Untitled.png)
 Afterward - you can install the packages:
 - 1. right click the procject
 - 2. Manage Nuget Packages...
 - 3. Set Package Source to Local Packages
 - 4. Install the 4 packages above
 - 5. (In some computers you should also install SkiaSharpView.WPF - which is avilable directly from Nuget packages)

Special features as specified above at the beginning.
External packages that were required to create the unique and vivid view of the Simulation Controller and it's graphs.
Added Dark mode so it will avert your eyes pain as quickly and have a slick, cool look to it.
Also added a GPS positioning of the plane, and map view of the area around to it's location; It also records your movement across the map!


## Directory Structure



- **Models**

Contains the models, continuation of the MVVM design pattern. responsible for reading the CSV files, and updating the view models from any changes in the values.

- **View Models**

Contains the view models, responsible for bridging between the 2 classes - the model, and the view. Finishing the MVVM design pattern.

- **Views**

Contains the rest of the views.

- **Controls**

Contains the controllers - which means, part of the views which manages the interactions with the user. Creating the MVVM design pattern.


[Introduction to the application](http://www.youtube.com/watch?v=hMtKQpLmde4)


## Documentation

[Here](https://github.com/yana-sidnich/flight-simulator/blob/main/UML.pdf) you can check out the UML that is representing the main classes of the project and the structure of each class and their relations with the other classes, fields, and which interfaces they are implementing.

The [flow charts](https://github.com/yana-sidnich/flight-simulator/blob/main/Flowchart.pdf) might give a better overall view on the project and how the app is running.


