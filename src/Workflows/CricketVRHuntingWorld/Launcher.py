# Script launching CricketVR bonsai workflow from the command line.
# Variables can be configured and multiple bonsai workflows can be 
# simultaneously launched if needed

# NeuroGEARS

from pathlib import Path
from datetime import datetime
from subprocess import Popen

## User Settings

settingsDict = {
    "Logging.LoggingRootPath" : r"C:\Users\neurogears\Desktop"
}

## Launch Bonsai

cwd = Path.cwd()
bonsai_path = cwd.parents[2] / "Bonsai" / "Bonsai.exe"
workflow_path = cwd / "CricketVRHuntingWorld.bonsai"

today = datetime.now().strftime('%Y-%m-%d-%H%M%S')
output_cmd = bonsai_path + ' ' +  workflow_path  + ' --start'

# # Append subject specific info to Bonsai launch command.

# for box in camera_IDs.keys():
#     command += ' -p:Box{}.Index={}'.format(box, camera_IDs[box])
#     if box in subjects.keys(): # Box is being used.
#         pinstate_file_path = os.path.join(data_dir, subjects[box] + '_pinstate_' + datetime_str + '.csv')
#         command += ' -p:Box{}.Enable=True'.format(box) + \
#                    ' -p:Box{}.PinFileName={}'.format(box, pinstate_file_path)
#     else: # Box not being used.
#         command += ' -p:Box{}.Enable=False'.format(box) + \
#                    ' -p:Box{}.PinFileName=temp\\empty{}.csv'.format(box, box)

# # Launch Bonsai

# bonsai_process = Popen(command)