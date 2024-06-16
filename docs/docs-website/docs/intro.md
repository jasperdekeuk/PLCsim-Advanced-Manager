---
sidebar_position: 1
---

# Introduction

**PLCsim Advanced Manager** is a project that aims to modernise the way of working with simulated PLC's.
It is build around the Siemens PLCsim Advanced software and provides a web-based user interface to manage your PLCsim
Advanced instances.
In the backend, it uses the API of PLCsim Advanced to communicate with the instances.

![Overview.png](..%2F..%2Fimg%2FOverview.png)


## Goals
The goals of the project are:

- [x] Manage your PLCSIM Advanced instances remote in a **webserver**. This way you don't have to log in to the target
  PC, but can start
  instances over the network
- [x] Exploit the functionalities of the PLCSIM Advanced API to create a more **advanced UI**
- [x] **Read and write variables** from the PLC (DB/Inputs/Outputs)
- [x] **Advanced network setting**. Being able to set every interface of the instance to an interface of the host
  machine
- [x] Create **snapshots** and restore them
- [ ] Easy **virtual commissioning** by e.g. setting buttons and lights in the UI to the PLC variables
- [ ] **Traces** on the variables for analysis


:::info
The main focus is on Siemens PLCsim Advanced, but generally speaking, this could be extended to other PLC simulators as
well. (if they provide an API)
:::

## Quick Start

1. Download the last version of **PLCsim Advanced Manager** from the **[official website](https://github.com/jasperdekeuk/PLCsim-Advanced-Manager/releases)**.

*optional: set up the url and port in the appsettings.json file*

2. Run the **PLCsimAdvancedManager.exe** file to start the application. You will now be able to access the UI. On your
localhost, you can access the UI by navigating to **http://localhost:5000**.

### What you'll need

- PLCsim Advanced (>v5) installed and licensed.


