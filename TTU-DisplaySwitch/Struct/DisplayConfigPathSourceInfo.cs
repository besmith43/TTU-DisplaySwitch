using System.Runtime.InteropServices;
using TTU_DisplaySwitch.Enum;

namespace TTU_DisplaySwitch.Struct
{
    [StructLayout(LayoutKind.Sequential)]
    public struct DisplayConfigPathSourceInfo
    {
        public LUID adapterId;
        public uint id;
        public uint modeInfoIdx;

        public DisplayConfigSourceStatus statusFlags;
    }
}