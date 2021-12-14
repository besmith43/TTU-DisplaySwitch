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