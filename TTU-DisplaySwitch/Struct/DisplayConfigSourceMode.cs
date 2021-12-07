using System.Runtime.InteropServices;
using TTU_DisplaySwitch.Enum;

namespace TTU_DisplaySwitch.Struct
{
    [StructLayout(LayoutKind.Sequential)]
    public struct DisplayConfigSourceMode
    {
        public int width;
        public int height;
        public DisplayConfigPixelFormat pixelFormat;
        public PointL position;
    }
}