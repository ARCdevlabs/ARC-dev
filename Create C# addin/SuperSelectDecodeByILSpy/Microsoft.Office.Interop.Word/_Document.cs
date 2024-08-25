using System.Reflection;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace Microsoft.Office.Interop.Word;

[ComImport]
[CompilerGenerated]
[DefaultMember("Name")]
[Guid("0002096B-0000-0000-C000-000000000046")]
[TypeIdentifier]
public interface _Document
{
	[DispId(0)]
	string Name
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(0)]
		[return: MarshalAs(UnmanagedType.BStr)]
		get;
	}

	void _VtblGap1_22();

	[DispId(17)]
	Words Words
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(17)]
		[return: MarshalAs(UnmanagedType.Interface)]
		get;
	}

	void _VtblGap2_136();

	[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	[DispId(1105)]
	void Close([Optional][In][MarshalAs(UnmanagedType.Struct)] ref object SaveChanges, [Optional][In][MarshalAs(UnmanagedType.Struct)] ref object OriginalFormat, [Optional][In][MarshalAs(UnmanagedType.Struct)] ref object RouteDocument);

	void _VtblGap3_12();

	[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	[DispId(113)]
	void Activate();
}
