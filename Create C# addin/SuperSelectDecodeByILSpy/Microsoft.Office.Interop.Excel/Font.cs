using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace Microsoft.Office.Interop.Excel;

[ComImport]
[CompilerGenerated]
[InterfaceType(2)]
[Guid("0002084D-0000-0000-C000-000000000046")]
[TypeIdentifier]
public interface Font
{
	void _VtblGap1_5();

	[DispId(96)]
	object Bold
	{
		[MethodImpl(MethodImplOptions.PreserveSig | MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(96)]
		[return: MarshalAs(UnmanagedType.Struct)]
		get;
		[MethodImpl(MethodImplOptions.PreserveSig | MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(96)]
		[param: In]
		[param: MarshalAs(UnmanagedType.Struct)]
		set;
	}
}
