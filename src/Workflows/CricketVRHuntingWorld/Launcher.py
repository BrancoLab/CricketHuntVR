# Launches CricketVR Bonsai workflow from the command line.
# Variables can be configured and multiple bonsai workflows can be
# simultaneously launched if needed

# NeuroGEARS

import subprocess
from pathlib import Path
from time import sleep
import os

## User Settings
SESSIOND_ID = r"Animal001"
DATA_ROOT_PATH = r"C:\Users\Cricket Team\Desktop\data"
ADD_FLAGS = "--no-editor"
FORCE_LAYOUT = True


#    //   ) )                                                  ||   / / //   ) )
#   //         __     ( )  ___     / ___      ___    __  ___   ||  / / //___/ /
#  //        //  ) ) / / //   ) ) //\ \     //___) )  / /      || / / / ___ (
# //        //      / / //       //  \ \   //        / /       ||/ / //   | |
#((____/ / //      / / ((____   //    \ \ ((____    / /        |  / //    | |

if not(os.path.isdir( DATA_ROOT_PATH + "\\" + SESSIOND_ID)):
    raise FileExistsError("Folder {0} already exists. Aborting session start.".format(DATA_ROOT_PATH + "\\" + SESSIOND_ID))

Settings = {
"Logging.LoggingRootPath" : DATA_ROOT_PATH + "\\" + SESSIOND_ID,
"RenderLoop.LocalSimulatedEnviornment.Enable" : str(True),
"EnableTrialLogic.Enable" : str(False),
"EnableShelter.Enable" : str(False),
}

UDP_Settings = {}

## Launch Bonsai
cwd = Path(__file__).parent.resolve()

bonsai_path = cwd.parents[2] / "Bonsai" / "Bonsai.exe"

workflow_path = cwd / "CricketVRHuntingWorld.bonsai"
LayoutFile = cwd / "Layouts" / "Default_CricketVRHuntingWorld_NoFakeWorld.bonsai.layout"
udp_workflow_path = cwd / "DrawWorld.bonsai"
udp_LayoutFile = cwd / "Layouts" / "Default_DrawWorld.bonsai.layout"

output_cmd = f'"{bonsai_path}" "{workflow_path}" {ADD_FLAGS}'

for sett in Settings.keys():
    output_cmd += f' -p:"{sett}"="{Settings[sett]}"'

_layout = LayoutFile
if not(_layout == "") and FORCE_LAYOUT:
    output_cmd += f' --visualizer-layout:"{_layout}"'

print(f"Starting... {output_cmd}")

bonsai_process = subprocess.Popen(output_cmd, cwd=cwd, creationflags=subprocess.CREATE_NEW_CONSOLE)

sleep(5)
udp_draw_output_cmd = f'"{bonsai_path}" "{udp_workflow_path}" {ADD_FLAGS}'
for sett in UDP_Settings.keys():
    udp_draw_output_cmd += f' -p:"{sett}"="{UDP_Settings[sett]}"'

if not(udp_LayoutFile == "") and FORCE_LAYOUT:
    udp_draw_output_cmd += f' --visualizer-layout:"{udp_LayoutFile}"'

print(f"Starting... {udp_draw_output_cmd}")
draw_process = subprocess.Popen(udp_draw_output_cmd, cwd=cwd, creationflags=subprocess.CREATE_NEW_CONSOLE)
