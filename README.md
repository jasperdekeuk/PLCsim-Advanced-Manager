# PLCsim-Advanced-Manager

A webserver manager for Siemens Simatic PLCsim Advanced.

Goals:

- Manage your PLCSIM Advanced instances remote in a webserver. This way you don't have to login to the target PC, but can start
  instances over the network
- Exploit the functionalities of the PLCSIM Advanced API to create a more user friendly interface
- Read and write variables from the PLC (DB/Inputs/Outputs)

## Overview
![](docs/img/Overview.png)

## Data monitoring and manipulation
Click the `show varibale in new tab' button on the line of the PLC.
![](docs/img/dataView.png)
# Quickstart

1. Download the latest version from the [releases](https://github.com/jasperdekeuk/PLCsim-Advanced-Manager/releases)
2. Extract the zip file
3. optional: setup the url and port in the `appsettings.json` file
4. Run the `PLCsimAdvancedManager.exe` file

> Make sure the port is not in use yet. The default port is 5000.
> 
> The `"http://*:5000"` in the appsettings.json file means that the server will listen to all incoming connections on port 5000. 
> Thus the server will be accessible from other devices in the network. This requires admin rights. You can delete this line of you only want to work locally
> 
> To reach the server from another device in the network, you have to know the IP address of the device running the server.
> You can access the server like <serverIP>:<port>