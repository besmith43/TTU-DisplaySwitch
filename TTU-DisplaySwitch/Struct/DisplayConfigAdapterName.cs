using System.Runtime.InteropServices;
using TTU_DisplaySwitch.Class;

namespace TTU_DisplaySwitch.Struct
{
	[StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
	public struct DisplayConfigAdapterName : IDisplayConfigInfo
	{
		public DisplayConfigDeviceInfoHeader header;
		[MarshalAs(UnmanagedType.ByValTStr, SizeConst = 128)]public string adapterDevicePath;
	}
}