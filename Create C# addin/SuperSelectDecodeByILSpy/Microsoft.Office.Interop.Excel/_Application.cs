using System.Reflection;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace Microsoft.Office.Interop.Excel;

[ComImport]
[CompilerGenerated]
[DefaultMember("_Default")]
[Guid("000208D5-0000-0000-C000-000000000046")]
[TypeIdentifier]
public interface _Application
{
	void _VtblGap1_45();

	[DispId(572)]
	Workbooks Workbooks
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(572)]
		[return: MarshalAs(UnmanagedType.Interface)]
		get;
	}

	void _VtblGap2_1();

	[DispId(494)]
	Sheets Worksheets
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(494)]
		[return: MarshalAs(UnmanagedType.Interface)]
		get;
	}

	void _VtblGap3_58();

	[DispId(0)]
	string _Default
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(0)]
		[return: MarshalAs(UnmanagedType.BStr)]
		get;
	}

	void _VtblGap4_168();

	[DispId(558)]
	bool Visible
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[LCIDConversion(0)]
		[DispId(558)]
		get;
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[LCIDConversion(0)]
		[DispId(558)]
		[param: In]
		set;
	}
}
