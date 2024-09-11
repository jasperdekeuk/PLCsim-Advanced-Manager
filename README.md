# PLCsim-Advanced-Manager

A webserver manager for Siemens Simatic PLCsim Advanced.

Check the [**documentation**](https://jasperdekeuk.github.io/PLCsim-Advanced-Manager/)

Download from [**releases**](https://github.com/jasperdekeuk/PLCsim-Advanced-Manager/releases)

If you have a feature request or problem, create an [**issue**](https://github.com/jasperdekeuk/PLCsim-Advanced-Manager/issues/new)



> [!WARNING]
> The documentation is always behind. Download the latest version to see and play with all the features.

Features:

- [x] Manage your PLCSIM Advanced instances remote in a **webserver**. This way you don't have to log in to the target PC, but can start
  instances over the network
- [x] Exploit the functionalities of the PLCSIM Advanced API to create a more **advanced UI**
- [x] **Read and write variables** from the PLC (DB/Inputs/Outputs)
- [x] **Advanced network setting**. Being able to set every interface of the instance to an interface of the host machine
- [x] Create **snapshots** and restore them
- [x] Option for **auto start** instances on startup of PLCsim Advanced Manager. Either just register or completely start the instance when starting the manager application. (see instance settings)

Future feature ideas:
- [ ] Easy **virtual commissioning** by e.g. setting buttons and lights in the UI to the PLC variables
- [ ] **Traces** on the variables for analysis
- [ ] **Desktop version** so no webbrowser is needed for a local setup
- [ ] Connect to **remote PLCsim Advance APIs**
- [ ] Test **IO module events**

## Overview
![](docs/img/Overview.png)


# Quickstart

1. Download the latest version from the [releases](https://github.com/jasperdekeuk/PLCsim-Advanced-Manager/releases)
2. Extract the zip file
3. optional: set up the url and port in the `appsettings.json` file
4. Run the `PLCsimAdvancedManager.exe` file

> [!WARNING]
> REQUIREMENT: PLCsim Advanced v5 or higher installed and licensed. (Trial license might give unexpected behaviour)

> [!NOTE]  
> Make sure the port is not in use yet. The default port is 5000.
> 
> The `"http://*:5000"` in the appsettings.json file means that the server will listen to all incoming connections on port 5000. 
> Thus the server will be accessible from other devices in the network. This requires admin rights. You can delete this line if you only want to work locally
> 
> To reach the server from another device in the network, you have to know the IP address of the device running the server.
> You can access the server like `<serverIP>:<port>` in your webbrowser. For example `192.168.10.15:5000`.

