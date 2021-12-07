using System;

namespace TTU_DisplaySwitch.Enum
{
    [Flags]
    public enum DisplayConfigModeInfoType : uint
    {
        Zero = 0,

        Source = 1,
        Target = 2,
    }
}