# Script launching CricketVR bonsai workflow from the command line.
# Variables can be configured and multiple bonsai workflows can be
# simultaneously launched if needed

# NeuroGEARS

import subprocess

from pathlib import Path
from datetime import datetime
from time import sleep

## User Settings
UDP_Mode = False

Settings = {
"Logging.LoggingRootPath" : r"C:\Users\Cricket Team\Desktop\data"
}

UDP_Settings = {}

## Launch Bonsai

#cwd = Path.cwd()
cwd = Path(__file__).parent.resolve()

bonsai_path = cwd.parents[2] / "Bonsai" / "Bonsai.exe"

if ~UDP_Mode:
    workflow_path = cwd / "CricketVRHuntingWorld.bonsai"
    LayoutFile = cwd / "DefaultCricketVRHuntingWorld.bonsai.layout"
else:
    workflow_path = cwd / "CricketVRHuntingWorld_NoFakeWorld.bonsai"
    LayoutFile = cwd / "DefaultCricketVRHuntingWorld_NoFakeWorld.bonsai.layout"
    udp_workflow_path = cwd / "DrawWorld.bonsai"
    udp_LayoutFile = cwd / "default_udp_drawworld.bonsai.layout"

today = datetime.now().strftime('%Y-%m-%d-%H%M%S')

output_cmd = f'"{bonsai_path}" "{workflow_path}" --no-editor'

for sett in Settings.keys():
    output_cmd += f' -p:"{sett}"="{Settings[sett]}"'

_layout = LayoutFile
if not(_layout == ""):
    output_cmd += f' --visualizer-layout:"{_layout}"'

print(f"Starting... {output_cmd}")

bonsai_process = subprocess.Popen(output_cmd, cwd=cwd, creationflags=subprocess.CREATE_NEW_CONSOLE)

if UDP_Mode:
    sleep(5)
    udp_draw_output_cmd = f'"{bonsai_path}" "{udp_workflow_path}" --no-editor'
    for sett in UDP_Settings.keys():
        udp_draw_output_cmd += f' -p:{sett}="{UDP_Settings[sett]}"'

    if not(udp_LayoutFile == ""):
        udp_draw_output_cmd += f' --visualizer-layout:"{udp_LayoutFile}"'

    print(f"Starting... {udp_draw_output_cmd}")
    draw_process = subprocess.Popen(udp_draw_output_cmd, cwd=cwd, creationflags=subprocess.CREATE_NEW_CONSOLE)
