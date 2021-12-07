using System.Runtime.InteropServices;

namespace TTU_DisplaySwitch.Struct
{
    [StructLayout(LayoutKind.Sequential)]
    public struct DisplayConfig2DRegion
    {
        public uint cx;
        public uint cy;
    }
}