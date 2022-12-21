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

## Currently implemented software events

### **General Notes**

- All payloads are of the type `Timestamped_<dataType>`. For the sake of brevity the `Timestamped` will be ommited from the payload's format.
- Unless otherwise stated, timestamping is achieved using the latest available photodiode event (~1kHz) as a timestamp source.
### TaskLogic

  * `TaskLogic\TaskLogic_address.bin [HARP data format]`
    | name       | address | payload   | attributes | description |
    | ---------- | ------- | --------- | ---------- | ----------- |
    | `trial_initiation` | `1` | `U8` | [trial_number] | Event timestamping the start of the trial. |
    | `perturbation_onset` | `2` | `U8` | [0] | Event timestamping the cricket arm perturbation onset. |
    | `trial_end` | `3` | `U8` | [0] | Event timestamping the end of the trial. |
    | `cricket_eat_end` | `4` | `U8` | [0] | Event timestamping the end of the period animals are allowed to eat the cricket for. |
    | `position_reset` | `11` | `float` [6] | [x,y,z,angle_x,angle_y,angle_z] | Whenever the position of the mouse is manually reset outputs an event containing the target position |
    | `play_audio_stim` | `12` | `U8` | [0] | Event that signals the triggering of a sound playback |
