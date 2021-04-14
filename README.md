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
   - After the installation, we will require to add commands to the start menu.
     first open up FlightGear, in the menu select the 'Setting' tab, and from there go all the way down to 'Additional Settings'.

   once there just copy-paste the lines below:
   
   ```C++
   --generic=socket,in,10,127.0.0.1,5400,tcp,playback_small
   
   
   --fdm=null
   ```
 - .Net Framework ( Download the recommended version [here](https://dotnet.microsoft.com/download/dotnet-framework/) )
 - Microsoft Visual Studio, or another development environment for C# ( Download Visual Studio [here](https://visualstudio.microsoft.com/downloads/) )
 


Up next, we will have to download the .ZIP file of the app, and extract.
Once that is done, get to the directory of the extraction, open the app folder and then the solution file **FlightGearTestExec.sln**, and simply press the 'Debug' button.

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

After the Simulator Controller loaded up, the user may start by changing the speed, or the time requested to be queried, or just hold until anomalies will come up on screen as a red-dot on the correlation graph.

 - **Media Player**
 
 ![alt text](https://raw.githubusercontent.com/eladoni1/pictures-for-ADP2/main/MediaPlayer%20in%20MainWindow.PNG)
 
 The user can use the multimedia buttons to control or target a specific frame in the flight, or speed things up, or even start playing backwards.
 
 - **Steers, rudder and throttle**
 
 ![alt text](https://raw.githubusercontent.com/eladoni1/pictures-for-ADP2/main/Joystick%20in%20mainWindow.PNG)
 
 The user can be notified with the steering joystick's direction in the current time of flight, and also the rudder and throttle values.
 
- **Graphs**

  - **Variable graphs**
  
  
  ![alt text](https://raw.githubusercontent.com/eladoni1/pictures-for-ADP2/main/Graphs%20in%20MainWindow.PNG)
  
  
  The user can select each value, and check it's graph, or for it's most correlated feature, for any sudden changes mid-flight.
  
  
  - **Correlation graph**
  
  ![alt text](https://raw.githubusercontent.com/eladoni1/pictures-for-ADP2/main/Graph%20no2%20in%20MainWindow.PNG)
  
  After the user selected certain value and it's correlated feature, he will be notified by red dots whenever anomaly have been discovered by the app.
  
  (In order to change the threshold of correlation between features - just move the slider just left of the correlation graph. In order to change algorithm the user might need to add a DLL file accordingly, for the developer's needs).
  
  
For further information, the app files includes the following folders -

- **Controls**

Contains the controllers - which means, part of the views which manages the interactions with the user. Creating the MVVM design pattern.

- **Views**

Contains the rest of the views.


- **DLL \ Plugins**
  - *LinearRegression.dll* - for linear based correlations between features - creates a line that represent the correlation between the 2 features. if a point a further away from the line, it means the correlation is smaller than usual. shows the last 30 values of both features.
  - *MinimumCircle.dll* - created for smaller correlations between the features - creates a circle that shows the last 30 values of both features - if a point is outside the circle, it means the correlation is smaller than usual.

Contains the DLLs' - the algorithms in C# language which is responsible for any anomaly detection in the app.


- **local_packages**
  - *LiveChartsCore.1.0.4*
  - *LiveChartsCore.SkiaSharpView.1.0.4*
  - *LiveChartsCore.SkiaSharpView.WPF.1.0.4*

Special features, external packages that was required to create the unique and vivid view of the Simulation Controller and it's graphs.

- **Models**

Contains the models, continuation of the MVVM design pattern. responsible for reading the CSV files, and updating the view models from any changes in the values.

- **View Models**

Contains the view models, responsible for bridging between the 2 classes - the model, and the view. Finishing the MVVM design pattern.


[Here](https://www.youtube.com/watch?v=dQw4w9WgXcQ) you can check out the UML that is representing the main classes of the project and the structure of each class and their relations with the other classes, fields, and which interfaces they are implementing.


[![IMAGE ALT TEXT](http://img.youtube.com/vi/njRA-27rcQ4/0.jpg)]((http://www.youtube.com/watch?v=njRA-27rcQ4 "Introduction to the app"))
