using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace Microsoft.Office.Interop.Word;

[ComImport]
[CompilerGenerated]
[Guid("000209B0-0000-0000-C000-000000000046")]
[TypeIdentifier]
public interface Find
{
	void _VtblGap1_48();

	[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	[DispId(444)]
	bool Execute([Optional][In][MarshalAs(UnmanagedType.Struct)] ref object FindText, [Optional][In][MarshalAs(UnmanagedType.Struct)] ref object MatchCase, [Optional][In][MarshalAs(UnmanagedType.Struct)] ref object MatchWholeWord, [Optional][In][MarshalAs(UnmanagedType.Struct)] ref object MatchWildcards, [Optional][In][MarshalAs(UnmanagedType.Struct)] ref object MatchSoundsLike, [Optional][In][MarshalAs(UnmanagedType.Struct)] ref object MatchAllWordForms, [Optional][In][MarshalAs(UnmanagedType.Struct)] ref object Forward, [Optional][In][MarshalAs(UnmanagedType.Struct)] ref object Wrap, [Optional][In][MarshalAs(UnmanagedType.Struct)] ref object Format, [Optional][In][MarshalAs(UnmanagedType.Struct)] ref object ReplaceWith, [Optional][In][MarshalAs(UnmanagedType.Struct)] ref object Replace, [Optional][In][MarshalAs(UnmanagedType.Struct)] ref object MatchKashida, [Optional][In][MarshalAs(UnmanagedType.Struct)] ref object MatchDiacritics, [Optional][In][MarshalAs(UnmanagedType.Struct)] ref object MatchAlefHamza, [Optional][In][MarshalAs(UnmanagedType.Struct)] ref object MatchControl);
}
