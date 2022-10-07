# Cricket Hunt VR
A closed-loop VR environment for the study of hunting behaviour in mice.

## Software dependencies

These should only need to be installed once on a fresh new system, and are not required if simply refreshing the install or deploying to a new folder.

 * Windows 10
 * [Visual Studio Code](https://code.visualstudio.com/) (recommended for editing code scripts and git commits)
 * [.NET Framework 4.7.2 Developer Pack](https://dotnet.microsoft.com/download/dotnet-framework/thank-you/net472-developer-pack-offline-installer) (required for intellisense when editing code scripts)
 * [Git for Windows](https://gitforwindows.org/) (recommended for cloning and manipulating this repository)
 * [Visual C++ Redistributable for Visual Studio 2012](https://www.microsoft.com/en-us/download/details.aspx?id=30679) (native dependency for OpenCV)
 * [FTDI CDM Driver 2.12.28](https://www.ftdichip.com/Drivers/CDM/CDM21228_Setup.zip) (serial port drivers for HARP devices)
 * [Spinnaker SDK 1.29.0.5](https://www.flir.co.uk/support/products/spinnaker-sdk/#Downloads) (device drivers for FLIR cameras)
   * On FLIR website: `Download > archive > 1.29.0.5 > SpinnakerSDK_FULL_1.29.0.5_x64.exe`
 * [pylon 7.1.0 Camera Software Suite](https://www.baslerweb.com/en/downloads/software-downloads/software-pylon-7-1-0-windows) (device drivers for Basler cameras)

## ARM documentation can be found here
https://github.com/SainsburyWellcomeCentre/virt-hunt-drv


## Default COM Ports

| Board      | COM port |
| ----------- | ----------- |
| Harp Behavior      | COM2       |
| Harp Expander   | COM3        |
| Arm Controller      | COM4       |
| Harp Clock Synchronizer   | COM5        |
