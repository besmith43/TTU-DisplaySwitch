using System.Runtime.InteropServices;

namespace TTU_DisplaySwitch.Struct
{
    [StructLayout(LayoutKind.Sequential)]
    public struct DisplayConfigTargetMode
    {
        public DisplayConfigVideoSignalInfo targetVideoSignalInfo;
    }
}