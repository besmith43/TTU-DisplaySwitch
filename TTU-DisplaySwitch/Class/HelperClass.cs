using System;
using System.Diagnostics;
using System.Xml.XPath;
using TTU_DisplaySwitch.Enum;
using TTU_DisplaySwitch.Struct;

namespace TTU_DisplaySwitch.Class;

public class HelperClass
{
    public void Run()
    {
        Console.WriteLine("testing");
        
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
            System.Diagnostics.ProcessStartInfo proc = new ProcessStartInfo();
            proc.FileName = "displayswitch";
            proc.Arguments = "/clone";

            System.Diagnostics.Process.Start(proc);
            /*
            var setDisplayStatus = CCDWrapper.SetDisplayConfig(
                numPathArrayElements, pathInfoArray,
                numModeInfoArrayElements, modeInfoArray,
                SdcFlags.TopologyClone);

            if (setDisplayStatus != StatusCode.Success)
            {
                var reason = string.Format("SetQueryConfig() failed. Status: {0}", setDisplayStatus);
                throw new Exception(reason);
            }
            
            */
        }
    }
       
}