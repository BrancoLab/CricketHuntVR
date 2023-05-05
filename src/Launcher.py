# Launches CricketVR Bonsai workflow from the command line.
# Variables can be configured and multiple bonsai workflows can be
# simultaneously launched if needed

# NeuroGEARS

# "C:/Users/Cricket Team/anaconda3/python.exe" "c:/Users/Cricket Team/Documents/git/BrancoLab/CricketHuntVR/src/Launcher.py"
import subprocess
from pathlib import Path
from time import sleep
import os

## User Settings
SESSIOND_ID = r"221209_M6_h"
DATA_ROOT_PATH = r"C:\Users\Cricket Team\Desktop\data"

#ADD_FLAGS = "--no-editor" #launches Bonsai without the editor and starts the workflow automatically
#ADD_FLAGS = "--start" # launches Bonsai WITH the editor and starts automatically
ADD_FLAGS = "--no-editor" #launches Bonsai with the editor but waits for you to click Play
FORCE_LAYOUT = True
SAVE_LOG = True


#    //   ) )                                                  ||   / / //   ) )
#   //         __     ( )  ___     / ___      ___    __  ___   ||  / / //___/ /
#  //        //  ) ) / / //   ) ) //\ \     //___) )  / /      || / / / ___ (
# //        //      / / //       //  \ \   //        / /       ||/ / //   | |
#((____/ / //      / / ((____   //    \ \ ((____    / /        |  / //    | |

if (os.path.isdir( DATA_ROOT_PATH + "\\" + SESSIOND_ID)):
    raise FileExistsError("Folder {0} already exists. Aborting session start.".format(DATA_ROOT_PATH + "\\" + SESSIOND_ID))

Settings = {
"Logging.LoggingRootPath" : DATA_ROOT_PATH + "\\" + SESSIOND_ID,
"EnableTrialLogic.Enable" : str(True),
"EnableShelter.Enable" : str(True),
"ArmPositionControl.Enable" : str(True), 
#"ArmGainTrial.Value" : float(1), 
#"ArmGainEating.Value" : float(0.5), 
"TrialLogic.TrialListPath" : str(r"TrialLogicCsv\stage1.2.csv")
}

UDP_Settings = {}

## Launch Bonsai
cwd = Path(__file__).parent.resolve()

bonsai_path = cwd.parents[0] / "Bonsai" / "Bonsai.exe"

workflow_path = cwd / "CricketVRHuntingWorld.bonsai"
LayoutFile = cwd / "Layouts" / "CricketVRHuntingWorld.bonsai.layout"
udp_workflow_path = cwd / "DrawWorld.bonsai"
udp_LayoutFile = cwd / "Layouts" / "DrawWorld.bonsai.layout"

output_cmd = f'"{bonsai_path}" "{workflow_path}" {ADD_FLAGS}'

for sett in Settings.keys():
    output_cmd += f' -p:"{sett}"="{Settings[sett]}"'

_layout = LayoutFile
if not(_layout == "") and FORCE_LAYOUT:
    output_cmd += f' --visualizer-layout:"{_layout}"'

print(f"Starting... {output_cmd}")

if SAVE_LOG:
    os.mkdir(DATA_ROOT_PATH + "\\" + SESSIOND_ID)
    log = open(DATA_ROOT_PATH + "\\" + SESSIOND_ID + "\\" + 'log.txt', 'a')  
    bonsai_process = subprocess.Popen(output_cmd, cwd=cwd, creationflags=subprocess.CREATE_NEW_CONSOLE, stdout=log, stderr=log)
else:
    bonsai_process = subprocess.Popen(output_cmd, cwd=cwd, creationflags=subprocess.CREATE_NEW_CONSOLE)

sleep(10)
udp_draw_output_cmd = f'"{bonsai_path}" "{udp_workflow_path}" {ADD_FLAGS}'
for sett in UDP_Settings.keys():
    udp_draw_output_cmd += f' -p:"{sett}"="{UDP_Settings[sett]}"'

if not(udp_LayoutFile == "") and FORCE_LAYOUT:
    udp_draw_output_cmd += f' --visualizer-layout:"{udp_LayoutFile}"'

print(f"Starting... {udp_draw_output_cmd}")
draw_process = subprocess.Popen(udp_draw_output_cmd, cwd=cwd, creationflags=subprocess.CREATE_NEW_CONSOLE)

