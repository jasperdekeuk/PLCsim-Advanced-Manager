# Changelog

All notable changes to this project will be documented in this file.

The format is based on [Keep a Changelog](https://keepachangelog.com/en/1.0.0/),
and this project adheres to [Semantic Versioning](https://semver.org/spec/v2.0.0.html).

## [Unreleased]

## [0.0.6] - 2024-05-08

This version is mainly a UI cleanup and small bugfix

### Changes
- UI improvement for network settings. This is now a table with interfaces and their settings.
- UI improvement for the storage dialog
- Add timestamp to snapshots overview and order by timestamp

### Fix
- Fix bug when snapshot folder does not exit yet
- Fix storage rename button
- Delete snapshots folder when instance storage is cleaned up

## [0.0.5] - 2024-05-01

### Features
- Add basic options for snapshots (Create/Delete/Rename)

### Changes
- Set colors to network buttons
- Change network option to chips

## [0.0.4] - 2024-04-27

### Changes
- Add icons to status

### Features
- Add basic options for storage (Rename/Delete/Duplicate/Register)

## [0.0.3] - 2024-04-25

### Fix
- Improve speed of Network config dialog
- No longer crashes on creating new instance when default path does not exist yet
- No longer crashes when trying to create more instances than PLCsim Advanced max allows

### Changes
- Use Chips for instance status
- Increase update frequency of dataview 

### Features
- Add instance status to dataview page
- Network mode now settable via network settings
- Switch binding settable in network settings

## [0.0.2] - 2024-04-14

### Features

- Button to check for updates
- Status of PLC in overview
- Interface mapping: every interface can be mapped to a network interface

### Fix

- Fixed the bug when an instance name was given of an instance that already exits

### Changes

- Moved notifications to bottom right

## [0.0.1] - 2024-01-08

### Added

- Manage plc's from a single page
- Monitor and manipulate variables from the PLC
