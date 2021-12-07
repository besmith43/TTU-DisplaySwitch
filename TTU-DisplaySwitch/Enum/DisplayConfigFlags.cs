using System;

namespace TTU_DisplaySwitch.Enum
{
    [Flags]
    public enum DisplayConfigFlags : uint
    {
        Zero = 0x0,
        PathActive = 0x00000001
    }
}