using System.Runtime.InteropServices;

namespace TTU_DisplaySwitch.Struct
{
    [StructLayout(LayoutKind.Sequential)]
    public struct LUID
    {
        public uint LowPart;
        public uint HighPart;
    }
}