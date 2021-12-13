using System;
using System.Diagnostics;
using System.Xml.XPath;
using System.Runtime.InteropServices;
using TTU_DisplaySwitch.Enum;
using TTU_DisplaySwitch.Struct;

namespace TTU_DisplaySwitch.Class;

public class HelperClass
{
    public void Run()
    {
        // setup
        QueryDisplayFlags pathType = QueryDisplayFlags.OnlyActivePaths;
        int numPathArrayElements;
        int numModeInfoArrayElements;

        var status = CCDWrapper.GetDisplayConfigBufferSizes(
            pathType,
            out numPathArrayElements,
            out numModeInfoArrayElements);

        if (status != StatusCode.Success)
        {
            var reason = string.Format("GetDisplayConfigBufferSizes() failed. Status: {0}", status);
            throw new Exception(reason);
        }

        var pathInfoArray = new DisplayConfigPathInfo[numPathArrayElements];
        var modeInfoArray = new DisplayConfigModeInfo[numModeInfoArrayElements];
        DisplayConfigTopologyId topologyId = DisplayConfigTopologyId.Zero;
        
        // check if in clone mode
        /*var queryDisplayStatus = pathType == QueryDisplayFlags.DatabaseCurrent
            ? CCDWrapper.QueryDisplayConfig(
                pathType,
                ref numPathArrayElements, pathInfoArray,
                ref numModeInfoArrayElements, modeInfoArray, out topologyId)
            : CCDWrapper.QueryDisplayConfig(
                pathType,
                ref numPathArrayElements, pathInfoArray,
                ref numModeInfoArrayElements, modeInfoArray);
        */

        var queryDisplayStatus = CCDWrapper.QueryDisplayConfig(QueryDisplayFlags.DatabaseCurrent,
            ref numPathArrayElements, pathInfoArray,
            ref numModeInfoArrayElements, modeInfoArray,
            out topologyId);
        
        if (queryDisplayStatus != StatusCode.Success)
        {
            var reason = string.Format("QueryDisplayconfig() failed. Status: {0}", queryDisplayStatus);
            throw new Exception(reason);
        }
        
        Console.WriteLine(topologyId);
        // swap to clone if not

        if (topologyId != DisplayConfigTopologyId.Clone)
        {

            /*
            System.Diagnostics.ProcessStartInfo proc = new ProcessStartInfo();
            proc.FileName = "displayswitch";
            proc.Arguments = "/clone";

            System.Diagnostics.Process.Start(proc);
            */

            // need something here to sort and select the proper config

            /*
            // set pathinfoarray.sourceinfo.id and modeinfoidx to the display you want to duplicate
            pathInfoArray[1].sourceInfo.id = pathInfoArray[0].sourceInfo.id;
            pathInfoArray[1].sourceInfo.modeInfoIdx = pathInfoArray[0].sourceInfo.modeInfoIdx;
            */

            /*
            // manually set objects with clone query info
            numPathArrayElements = 2;

            // first element
            pathInfoArray[0].sourceInfo.adapterId.LowPart = 1772913031;
            pathInfoArray[0].sourceInfo.adapterId.HighPart = 0;
            pathInfoArray[0].sourceInfo.id = 0;
            pathInfoArray[0].sourceInfo.modeInfoIdx = 1;
            pathInfoArray[0].sourceInfo.statusFlags = DisplayConfigSourceStatus.InUse;
            pathInfoArray[0].targetInfo.adapterId.LowPart = 1772913031;
            pathInfoArray[0].targetInfo.adapterId.HighPart = 0;
            pathInfoArray[0].targetInfo.id = 256;
            pathInfoArray[0].targetInfo.modeInfoIdx = 0;
            pathInfoArray[0].targetInfo.outputTechnology = DisplayConfigVideoOutputTechnology.DisplayportExternal;
            pathInfoArray[0].targetInfo.rotation = DisplayConfigRotation.Identity;
            pathInfoArray[0].targetInfo.scaling = DisplayConfigScaling.Aspectratiocenteredmax;
            pathInfoArray[0].targetInfo.refreshRate.numerator = 60;
            pathInfoArray[0].targetInfo.refreshRate.denominator = 1;
            pathInfoArray[0].targetInfo.scanLineOrdering = DisplayConfigScanLineOrdering.Progressive;
            pathInfoArray[0].targetInfo.targetAvailable = true;
            pathInfoArray[0].targetInfo.statusFlags = DisplayConfigTargetStatus.InUse;
            pathInfoArray[0].flags = DisplayConfigFlags.PathActive;

            // second element
            pathInfoArray[1].sourceInfo.adapterId.LowPart = 1772913031;
            pathInfoArray[1].sourceInfo.adapterId.HighPart = 0;
            pathInfoArray[1].sourceInfo.id = 0;
            pathInfoArray[1].sourceInfo.modeInfoIdx = 1;
            pathInfoArray[1].sourceInfo.statusFlags = DisplayConfigSourceStatus.InUse;
            pathInfoArray[1].targetInfo.adapterId.LowPart = 1772913031;
            pathInfoArray[1].targetInfo.adapterId.HighPart = 0;
            pathInfoArray[1].targetInfo.id = 256;
            pathInfoArray[1].targetInfo.modeInfoIdx = 0;
            pathInfoArray[1].targetInfo.outputTechnology = DisplayConfigVideoOutputTechnology.DisplayportExternal;
            pathInfoArray[1].targetInfo.rotation = DisplayConfigRotation.Identity;
            pathInfoArray[1].targetInfo.scaling = DisplayConfigScaling.Aspectratiocenteredmax;
            pathInfoArray[1].targetInfo.refreshRate.numerator = 60;
            pathInfoArray[1].targetInfo.refreshRate.denominator = 1;
            pathInfoArray[1].targetInfo.scanLineOrdering = DisplayConfigScanLineOrdering.Progressive;
            pathInfoArray[1].targetInfo.targetAvailable = true;
            pathInfoArray[1].targetInfo.statusFlags = DisplayConfigTargetStatus.InUse;
            pathInfoArray[1].flags = DisplayConfigFlags.PathActive;

            numModeInfoArrayElements = 3;

            // first element
            modeInfoArray[0].infoType = DisplayConfigModeInfoType.Target;
            modeInfoArray[0].id = 256;
            modeInfoArray[0].adapterId.LowPart = 1772913031;
            modeInfoArray[0].adapterId.HighPart = 0;
            modeInfoArray[0].targetMode.targetVideoSignalInfo.pixelRate = 497664000;
            modeInfoArray[0].targetMode.targetVideoSignalInfo.hSyncFreq.numerator = 129600;
            modeInfoArray[0].targetMode.targetVideoSignalInfo.hSyncFreq.denominator = 1;
            modeInfoArray[0].targetMode.targetVideoSignalInfo.vSyncFreq.numerator = 60;
            modeInfoArray[0].targetMode.targetVideoSignalInfo.vSyncFreq.denominator = 1;
            modeInfoArray[0].targetMode.targetVideoSignalInfo.activeSize.cx = 3840;
            modeInfoArray[0].targetMode.targetVideoSignalInfo.activeSize.cy = 2160;
            modeInfoArray[0].targetMode.targetVideoSignalInfo.totalSize.cx = 0;
            modeInfoArray[0].targetMode.targetVideoSignalInfo.totalSize.cy = 0;
            modeInfoArray[0].targetMode.targetVideoSignalInfo.videoStandard = D3DmdtVideoSignalStandard.NoIdea;
            modeInfoArray[0].targetMode.targetVideoSignalInfo.scanLineOrdering = DisplayConfigScanLineOrdering.Progressive;
            modeInfoArray[0].sourceMode.width = 497664000;
            modeInfoArray[0].sourceMode.height = 0;
            modeInfoArray[0].sourceMode.pixelFormat = DisplayConfigPixelFormat.NoIdea;
            modeInfoArray[0].sourceMode.position.x = 1;
            modeInfoArray[0].sourceMode.position.y = 60;

            // second element
            modeInfoArray[1].infoType = DisplayConfigModeInfoType.Source;
            modeInfoArray[1].id = 0;
            modeInfoArray[1].adapterId.LowPart = 1772913031;
            modeInfoArray[1].adapterId.HighPart = 0;
            modeInfoArray[1].targetMode.targetVideoSignalInfo.pixelRate = 4638564681600;
            modeInfoArray[1].targetMode.targetVideoSignalInfo.hSyncFreq.numerator = 4;
            modeInfoArray[1].targetMode.targetVideoSignalInfo.hSyncFreq.denominator = 0;
            modeInfoArray[1].targetMode.targetVideoSignalInfo.vSyncFreq.numerator = 0;
            modeInfoArray[1].targetMode.targetVideoSignalInfo.vSyncFreq.denominator = 0;
            modeInfoArray[1].targetMode.targetVideoSignalInfo.activeSize.cx = 0;
            modeInfoArray[1].targetMode.targetVideoSignalInfo.activeSize.cy = 0;
            modeInfoArray[1].targetMode.targetVideoSignalInfo.totalSize.cx = 0;
            modeInfoArray[1].targetMode.targetVideoSignalInfo.totalSize.cy = 0;
            modeInfoArray[1].targetMode.targetVideoSignalInfo.videoStandard = D3DmdtVideoSignalStandard.Uninitialized;
            modeInfoArray[1].targetMode.targetVideoSignalInfo.scanLineOrdering = DisplayConfigScanLineOrdering.Unspecified;
            modeInfoArray[1].sourceMode.width = 1920;
            modeInfoArray[1].sourceMode.height = 1080;
            modeInfoArray[1].sourceMode.pixelFormat = DisplayConfigPixelFormat.Pixelformat32Bpp;
            modeInfoArray[1].sourceMode.position.x = 0;
            modeInfoArray[1].sourceMode.position.y = 0;

            // third element
            modeInfoArray[2].infoType = DisplayConfigModeInfoType.Target;
            modeInfoArray[2].id = 251658497;
            modeInfoArray[2].adapterId.LowPart = 1772913031;
            modeInfoArray[2].adapterId.HighPart = 0;
            modeInfoArray[2].targetMode.targetVideoSignalInfo.pixelRate = 124416000;
            modeInfoArray[2].targetMode.targetVideoSignalInfo.hSyncFreq.numerator = 64800;
            modeInfoArray[2].targetMode.targetVideoSignalInfo.hSyncFreq.denominator = 1;
            modeInfoArray[2].targetMode.targetVideoSignalInfo.vSyncFreq.numerator = 60;
            modeInfoArray[2].targetMode.targetVideoSignalInfo.vSyncFreq.denominator = 1;
            modeInfoArray[2].targetMode.targetVideoSignalInfo.activeSize.cx = 1920;
            modeInfoArray[2].targetMode.targetVideoSignalInfo.activeSize.cy = 1080;
            modeInfoArray[2].targetMode.targetVideoSignalInfo.totalSize.cx = 0;
            modeInfoArray[2].targetMode.targetVideoSignalInfo.totalSize.cy = 0;
            modeInfoArray[2].targetMode.targetVideoSignalInfo.videoStandard = D3DmdtVideoSignalStandard.NoIdea;
            modeInfoArray[2].targetMode.targetVideoSignalInfo.scanLineOrdering = DisplayConfigScanLineOrdering.Progressive;
            modeInfoArray[2].sourceMode.width = 124416000;
            modeInfoArray[2].sourceMode.height = 0;
            modeInfoArray[2].sourceMode.pixelFormat = DisplayConfigPixelFormat.NoIdea2;
            modeInfoArray[2].sourceMode.position.x = 1;
            modeInfoArray[2].sourceMode.position.y = 60;

            // fourth element
            modeInfoArray[3].infoType = DisplayConfigModeInfoType.Zero;
            modeInfoArray[3].id = 0;
            modeInfoArray[3].adapterId.LowPart = 0;
            modeInfoArray[3].adapterId.HighPart = 0;
            modeInfoArray[3].targetMode.targetVideoSignalInfo.pixelRate = 0;
            modeInfoArray[3].targetMode.targetVideoSignalInfo.hSyncFreq.numerator = 0;
            modeInfoArray[3].targetMode.targetVideoSignalInfo.hSyncFreq.denominator = 0;
            modeInfoArray[3].targetMode.targetVideoSignalInfo.vSyncFreq.numerator = 0;
            modeInfoArray[3].targetMode.targetVideoSignalInfo.vSyncFreq.denominator = 0;
            modeInfoArray[3].targetMode.targetVideoSignalInfo.activeSize.cx = 0;
            modeInfoArray[3].targetMode.targetVideoSignalInfo.activeSize.cy = 0;
            modeInfoArray[3].targetMode.targetVideoSignalInfo.totalSize.cx = 0;
            modeInfoArray[3].targetMode.targetVideoSignalInfo.totalSize.cy = 0;
            modeInfoArray[3].targetMode.targetVideoSignalInfo.videoStandard = D3DmdtVideoSignalStandard.Uninitialized;
            modeInfoArray[3].targetMode.targetVideoSignalInfo.scanLineOrdering = DisplayConfigScanLineOrdering.Unspecified;
            modeInfoArray[3].sourceMode.width = 0;
            modeInfoArray[3].sourceMode.height = 0;
            modeInfoArray[3].sourceMode.pixelFormat = DisplayConfigPixelFormat.Zero;
            modeInfoArray[3].sourceMode.position.x = 0;
            modeInfoArray[3].sourceMode.position.y = 0;
            */

            /*
            // attempting to follow MS sample psuedocode

            DisplayConfigModeInfo primaryModeInfo = modeInfoArray[0];

            foreach (var modeInfo in modeInfoArray)
            {
                int temp_x = modeInfo.sourceMode.position.x;
                int temp_y = modeInfo.sourceMode.position.y;

                if (temp_x == 0 && temp_y == 0)
                {
                    primaryModeInfo = modeInfo;
                    break;
                }
            }

            DisplayConfigSourceDeviceName displayConfigSourceDeviceName = new DisplayConfigSourceDeviceName
            {
                header = new DisplayConfigDeviceInfoHeader
                {
                    adapterId = primaryModeInfo.adapterId,
                    id = primaryModeInfo.id,
                    size =
                        Marshal.SizeOf(
                            typeof(DisplayConfigSourceDeviceName)),
                    type = DisplayConfigDeviceInfoType.GetSourceName,
                }
            };

            var getDisplayConfigDeviceInfoStatus = CCDWrapper.DisplayConfigGetDeviceInfo(ref displayConfigSourceDeviceName);

            if (getDisplayConfigDeviceInfoStatus != StatusCode.Success)
            {
                var reason = string.Format("DisplayConfigGetDeviceInfo() failed. Status: {0}", getDisplayConfigDeviceInfoStatus);
                throw new Exception(reason);
            }

            Console.WriteLine($"Primary Mode Info Adapter Device Path: { displayConfigSourceDeviceName.viewGdiDeviceName }");

            pathInfoArray[0].flags |= DisplayConfigFlags.PathActive;
            pathInfoArray[0].sourceInfo.modeInfoIdx = 0; // need to find enum definition for invalid

            var setDisplayStatus = CCDWrapper.SetDisplayConfig(
                numPathArrayElements, pathInfoArray,
                numModeInfoArrayElements, modeInfoArray,
                (SdcFlags.TopologyClone | SdcFlags.Apply));
            */

            // https://stackoverflow.com/questions/67332814/how-to-properly-clone-and-extend-two-specific-monitors-on-windows

            var setDisplayStatus = CCDWrapper.SetDisplayConfig(
                0, null, 0, null, (SdcFlags.TopologyClone | SdcFlags.Apply)
            );

            if (setDisplayStatus != StatusCode.Success)
            {
                ListInfo(numPathArrayElements, pathInfoArray, numModeInfoArrayElements, modeInfoArray);
                var reason = string.Format("SetQueryConfig() failed. Status: {0}", setDisplayStatus);
                throw new Exception(reason);
            }
            
            //*/
        }
        else
        {
            ListInfo(numPathArrayElements, pathInfoArray, numModeInfoArrayElements, modeInfoArray);
        }
    }

    private void ListInfo(int numPathArrayElements, DisplayConfigPathInfo[] pathInfoArray, int numModeInfoArrayElements, DisplayConfigModeInfo[] modeInfoArray)
    {
        Console.WriteLine($"Number of Path Array Elements: { numPathArrayElements }");

        foreach (var pathInfo in pathInfoArray)
        {
            Console.WriteLine("------------------------------------------------------------------------------------------");
            Console.WriteLine($"Source Info Adpater ID Low Part: { pathInfo.sourceInfo.adapterId.LowPart }");
            Console.WriteLine($"Source Info Adapter ID High Part: { pathInfo.sourceInfo.adapterId.HighPart }");
            Console.WriteLine($"Source Info ID: { pathInfo.sourceInfo.id }");
            Console.WriteLine($"Source Info Mode Info IDX: { pathInfo.sourceInfo.modeInfoIdx }");
            Console.WriteLine($"Source Info Status Flags: { pathInfo.sourceInfo.statusFlags }");
            Console.WriteLine($"Target Info Adapter ID Low Part: { pathInfo.targetInfo.adapterId.LowPart }");
            Console.WriteLine($"Target Info Adapter ID High Part: { pathInfo.targetInfo.adapterId.HighPart }");
            Console.WriteLine($"Target Info ID: { pathInfo.targetInfo.id }");
            Console.WriteLine($"Target Info Mode Info IDX: { pathInfo.targetInfo.modeInfoIdx }");
            Console.WriteLine($"Target Info Output Technology: { pathInfo.targetInfo.outputTechnology }");
            Console.WriteLine($"Target Info Rotation: { pathInfo.targetInfo.rotation }");
            Console.WriteLine($"Target Info Scaling: { pathInfo.targetInfo.scaling }");
            Console.WriteLine($"Target Info Refresh Rate Numerator: { pathInfo.targetInfo.refreshRate.numerator }");
            Console.WriteLine($"Target Info Refresh Rate Denominator: { pathInfo.targetInfo.refreshRate.denominator }");
            Console.WriteLine($"Target Info Scan Line Ordering: { pathInfo.targetInfo.scanLineOrdering }");
            Console.WriteLine($"Target Info Target Available: { pathInfo.targetInfo.targetAvailable }");
            Console.WriteLine($"Target Info Status Flags: { pathInfo.targetInfo.statusFlags }");
            Console.WriteLine($"Flags: { pathInfo.flags }");
            Console.WriteLine("------------------------------------------------------------------------------------------");
        }

        Console.WriteLine($"Number of Mode Info Array Elements: { numModeInfoArrayElements }");

        foreach (var modeInfo in modeInfoArray)
        {
            Console.WriteLine("------------------------------------------------------------------------------------------");
            Console.WriteLine($"Info Type: { modeInfo.infoType }");
            Console.WriteLine($"ID: { modeInfo.id }");
            Console.WriteLine($"Adpater ID Low Part: { modeInfo.adapterId.LowPart }");
            Console.WriteLine($"Adapter ID High Part: { modeInfo.adapterId.HighPart }");
            Console.WriteLine($"Pixel Rate: { modeInfo.targetMode.targetVideoSignalInfo.pixelRate }");
            Console.WriteLine($"H Sync Freq Numerator: { modeInfo.targetMode.targetVideoSignalInfo.hSyncFreq.numerator }");
            Console.WriteLine($"H Sync Freq Denominator: { modeInfo.targetMode.targetVideoSignalInfo.hSyncFreq.denominator }");
            Console.WriteLine($"V Sync Freq Numerator: { modeInfo.targetMode.targetVideoSignalInfo.vSyncFreq.numerator }");
            Console.WriteLine($"V Sync Freq Denominator: { modeInfo.targetMode.targetVideoSignalInfo.vSyncFreq.denominator }");
            Console.WriteLine($"Active Size CX: { modeInfo.targetMode.targetVideoSignalInfo.activeSize.cx }");
            Console.WriteLine($"Active Size CY: { modeInfo.targetMode.targetVideoSignalInfo.activeSize.cy }");
            Console.WriteLine($"Total Size CX: { modeInfo.targetMode.targetVideoSignalInfo.totalSize.cx }");
            Console.WriteLine($"Total Size CY: { modeInfo.targetMode.targetVideoSignalInfo.totalSize.cy }");
            Console.WriteLine($"Video Standard: { modeInfo.targetMode.targetVideoSignalInfo.videoStandard }");
            Console.WriteLine($"Scan Line Ordering: { modeInfo.targetMode.targetVideoSignalInfo.scanLineOrdering }");
            Console.WriteLine($"Wdith: { modeInfo.sourceMode.width }");
            Console.WriteLine($"Height: { modeInfo.sourceMode.height }");
            Console.WriteLine($"Pixel Format: { modeInfo.sourceMode.pixelFormat }");
            Console.WriteLine($"X: { modeInfo.sourceMode.position.x }");
            Console.WriteLine($"Y: { modeInfo.sourceMode.position.y }");
            Console.WriteLine("------------------------------------------------------------------------------------------");
        }
    }
}