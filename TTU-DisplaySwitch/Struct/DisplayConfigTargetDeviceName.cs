using System.Runtime.InteropServices;
using TTU_DisplaySwitch.Enum;
using TTU_DisplaySwitch.Class;

namespace TTU_DisplaySwitch.Struct
{
	[StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode, Pack=1)]
	public struct DisplayConfigTargetDeviceName : IDisplayConfigInfo
	{
		public DisplayConfigDeviceInfoHeader header;
		public DisplayConfigTargetDeviceNameFlags flags;
		public DisplayConfigVideoOutputTechnology outputTechnology;
		public ushort edidManufactureId;
		public ushort edidProductCodeId;
		public uint connectorInstance;

		[MarshalAs(UnmanagedType.ByValTStr, SizeConst = 64)] public string monitorFriendlyDeviceName;
		[MarshalAs(UnmanagedType.ByValTStr, SizeConst = 128)] public string monitorDevicePath;
	}
}