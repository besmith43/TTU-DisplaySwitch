using System.Runtime.InteropServices;

namespace TTU_DisplaySwitch.Struct
{
    [StructLayout(LayoutKind.Sequential)]
    public struct DisplayConfigRational
    {
        public uint numerator;
        public uint denominator;
    }
}