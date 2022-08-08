# Script launching CricketVR bonsai workflow from the command line.
# Variables can be configured and multiple bonsai workflows can be
# simultaneously launched if needed

# NeuroGEARS

import subprocess

from pathlib import Path
from datetime import datetime

from CricketVRSettings import Settings, LayoutFile

## User Settings

## Launch Bonsai

#cwd = Path.cwd()
cwd = Path(__file__).parent.resolve()

bonsai_path = cwd.parents[2] / "Bonsai" / "Bonsai.exe"
workflow_path = cwd / "CricketVRHuntingWorld.bonsai"

today = datetime.now().strftime('%Y-%m-%d-%H%M%S')

output_cmd = f'"{bonsai_path}" "{workflow_path}" --no-editor'

for sett in Settings.keys():
    output_cmd += f' -p:{sett}="{Settings[sett]}"'

_layout = LayoutFile
if not(_layout == ""):
    output_cmd += f' --visualizer-layout:"{_layout}"'

print(output_cmd)

bonsai_process = subprocess.Popen(output_cmd, cwd=cwd, creationflags=subprocess.CREATE_NEW_CONSOLE)
