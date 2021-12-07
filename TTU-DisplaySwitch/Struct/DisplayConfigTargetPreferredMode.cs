using System.Runtime.InteropServices;
using TTU_DisplaySwitch.Class;

namespace TTU_DisplaySwitch.Struct
{
	[StructLayout(LayoutKind.Sequential)]
	public struct DisplayConfigTargetPreferredMode : IDisplayConfigInfo
	{
		public DisplayConfigDeviceInfoHeader header;
		public uint width;
		public uint height;
		public DisplayConfigTargetMode targetMode;
	}
}