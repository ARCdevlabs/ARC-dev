using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace Microsoft.Office.Interop.Excel;

[ComImport]
[CompilerGenerated]
[InterfaceType(2)]
[Guid("00020870-0000-0000-C000-000000000046")]
[TypeIdentifier]
public interface Interior
{
	void _VtblGap1_5();

	[DispId(97)]
	object ColorIndex
	{
		[MethodImpl(MethodImplOptions.PreserveSig | MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(97)]
		[return: MarshalAs(UnmanagedType.Struct)]
		get;
		[MethodImpl(MethodImplOptions.PreserveSig | MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(97)]
		[param: In]
		[param: MarshalAs(UnmanagedType.Struct)]
		set;
	}
}
