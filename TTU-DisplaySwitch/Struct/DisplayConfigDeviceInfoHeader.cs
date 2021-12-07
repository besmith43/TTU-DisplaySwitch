using System.Runtime.InteropServices;
using TTU_DisplaySwitch.Enum;

namespace TTU_DisplaySwitch.Struct
{
	[StructLayout(LayoutKind.Sequential)]
	public struct DisplayConfigDeviceInfoHeader
	{
		public DisplayConfigDeviceInfoType type;
		public int size;
		public LUID adapterId;
		public uint id;
	}
}