from ctypes import *
from enum import IntEnum
import sys
import os
import time
from ctypes import cdll, c_void_p, c_int, c_float


# Rotary Joint USB port #
ROTARY_JOINT_USB_PORT_NUMBER = 24

# Load the Doric DLL
mDLL = cdll.LoadLibrary(r'C:\Users\Cricket Team\Documents\Doric\Test_Scripts\DoricSystem.dll')
# Define motor direction constants (assumed to be in the DLL)
# duty of 80 for 1s is about 1/3 turn
# duty of 80 for 0.75s is about 1/4 turn
DUTY_CYCLE = 80

class RotaryJointSettings(Structure):
    _fields_ = [
        ("samplingRate", c_int),    # kFreq_120Hz = 120
        ("motorSpeed", c_int),      # kSPEED_FACTOR_NORMAL = 1
    ]

class RotaryJointMotorMode(IntEnum):
    kMode_Manual = 0
    kMode_Continuous = 1
    kMode_TurnPerSide = 2
    kMode_Random = 3

class RotaryJointMotorDirection(IntEnum):
    kDirection_NoDirection = 0
    kDirection_Clockwise = 1
    kDirection_CounterClockwise = 2

def move_rotary_joint_to_angle(mDLL, port_number, direction_instruction, movement_duration):
    """
    Move the rotary joint to a specific angle (in degrees) using manual control.
    Assumes we know the current position and the target position.
    """
    

    
    print("Enter")
    if direction_instruction > 0:
        mDLL.rotary_joint_send_manual_control_direction(port_number, RotaryJointMotorDirection.kDirection_Clockwise)
    else:
        mDLL.rotary_joint_send_manual_control_direction(port_number, RotaryJointMotorDirection.kDirection_CounterClockwise)
        #angle_diff = abs(angle_diff)  # Make angle_diff positive for further calculations
    
    mDLL.wait(movement_duration)

    mDLL.rotary_joint_send_manual_control_direction(port_number, RotaryJointMotorDirection.kDirection_NoDirection)
    print("Exit")
    
def open():

    # Initialize communication with Doric devices
    mDLL.init(True)
    mDLL.wait(3000)

    mDLL.available_devices_with_ports()

    mDLL.open_device(ROTARY_JOINT_USB_PORT_NUMBER)
    mDLL.wait(3000)
    # Activate manual control
    mDLL.rotary_joint_send_manual_control_activated(ROTARY_JOINT_USB_PORT_NUMBER)

    mDLL.rotary_joint_send_manual_control_duty_cycle(24, c_double(80))

    mDLL.rotary_joint_send_manual_control_mode(ROTARY_JOINT_USB_PORT_NUMBER, RotaryJointMotorMode.kMode_Manual)

    # Send 'no direction' direction 
    mDLL.rotary_joint_send_manual_control_direction(ROTARY_JOINT_USB_PORT_NUMBER, RotaryJointMotorDirection.kDirection_NoDirection)


def move(direction_instruction,movement_duration):
    move_rotary_joint_to_angle(mDLL, ROTARY_JOINT_USB_PORT_NUMBER, direction_instruction, movement_duration)

def stop_control():
    mDLL.rotary_joint_send_manual_control_deactivated(24) # need to change this to be 'port_number' in bonsai
    mDLL.wait(1000)
    mDLL.close_device(ROTARY_JOINT_USB_PORT_NUMBER)

def close():
    mDLL.rotary_joint_send_manual_control_deactivated(24) # need to change this to be 'port_number' in bonsai
    mDLL.wait(1000)
    mDLL.close_device(ROTARY_JOINT_USB_PORT_NUMBER)

    mDLL.wait(1000)
    mDLL.quit()
    # try:
    #     sys.exit(0)
    # except SystemExit:
    #     os._exit(0)
    

