using System.Runtime.InteropServices;
using TTU_DisplaySwitch.Enum;

namespace TTU_DisplaySwitch.Struct
{
    [StructLayout(LayoutKind.Sequential)]
    public struct DisplayConfigPathInfo
    {
        public DisplayConfigPathSourceInfo sourceInfo;
        public DisplayConfigPathTargetInfo targetInfo;
        public DisplayConfigFlags flags;
    }
}