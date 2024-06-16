---
sidebar_position: 2
---

# Possible Setups


## Local setup
The most basic setup is running on the same machine as the PLCsim Advanced instance. This setup is the easiest to set up and requires no network configuration.
It is similar to how you would use PLCsim Advanced via the default UI. But with the added benefit like monitoring and manipulating variables.

![](/../static/img/setup_localhost.png)

### Use cases
- You want more feedback and options then with the default UI of PLCsim Advanced
- You want to monitor and manipulate variables from the PLC
- You want to use some of the feature this web app provides that are not in the default PLCsim Advanced UI
- You want to give this project a try

## Server setup
The webserver really shines when you run it on a server where PLCsim Advanced is installed. This way you can manage the PLC instances from any device in the network.

![](/../static/img/setup_server.png)

### Use cases
- You want to manage PLC instances from any device in the network
- You want to share PLC instances with other colleagues in the network
- You want to install PLCsim Advanced only on 1 machine and let multiple users use it


## Multiple servers setup
If you have multiple servers where PLCsim Advanced is installed, you can manage them all via the single webserver. This way you can manage all PLC instances from one location.
> This is technically possible, but not developed yet. Create an issue if you would need this

![](/../static/img/setup_multiple_server.png)

### Use cases
- You want to manage multiple instances of PLCsim Advanced from one location
- You want to work with more PLCsim Advanced instances than 1 server can handle (or more than the license allows)

## Advanced network setup
PLCsim Advanced has the option to map every network interfaces of the PLC instance to a different network interfaces of the host machine.
This way you can simulate a PLC that is connected to multiple networks.

![](/../static/img/setup_advanced_networking.png)

### Use cases
- You want the exact setup of the PLC in the real world where every PLC interface is connected to a different network
- You want to do advanced networking setups