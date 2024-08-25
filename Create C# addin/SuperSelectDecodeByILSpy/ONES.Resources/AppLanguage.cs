using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;
using System.Globalization;
using System.Resources;
using System.Runtime.CompilerServices;

namespace ONES.Resources;

[GeneratedCode("System.Resources.Tools.StronglyTypedResourceBuilder", "17.0.0.0")]
[DebuggerNonUserCode]
[CompilerGenerated]
internal class AppLanguage
{
	private static ResourceManager resourceMan;

	private static CultureInfo resourceCulture;

	[EditorBrowsable(EditorBrowsableState.Advanced)]
	internal static ResourceManager ResourceManager
	{
		get
		{
			if (resourceMan == null)
			{
				ResourceManager resourceManager = new ResourceManager("ONES.Resources.AppLanguage", typeof(AppLanguage).Assembly);
				resourceMan = resourceManager;
			}
			return resourceMan;
		}
	}

	[EditorBrowsable(EditorBrowsableState.Advanced)]
	internal static CultureInfo Culture
	{
		get
		{
			return resourceCulture;
		}
		set
		{
			resourceCulture = value;
		}
	}

	internal static string ColorFilter_EN => ResourceManager.GetString("ColorFilter_EN", resourceCulture);

	internal static string ColorFilter_EN_Tooltip => ResourceManager.GetString("ColorFilter_EN_Tooltip", resourceCulture);

	internal static string ColorFilter_JA => ResourceManager.GetString("ColorFilter_JA", resourceCulture);

	internal static string ColorFilter_JA_Toolip => ResourceManager.GetString("ColorFilter_JA_Toolip", resourceCulture);

	internal static string PanelBonus => ResourceManager.GetString("PanelBonus", resourceCulture);

	internal static string PanelCollaboration => ResourceManager.GetString("PanelCollaboration", resourceCulture);

	internal static string PanelExport => ResourceManager.GetString("PanelExport", resourceCulture);

	internal static string PanelGenerate => ResourceManager.GetString("PanelGenerate", resourceCulture);

	internal static string PanelModify => ResourceManager.GetString("PanelModify", resourceCulture);

	internal static string PanelONES => ResourceManager.GetString("PanelONES", resourceCulture);

	internal static string PanelReview => ResourceManager.GetString("PanelReview", resourceCulture);

	internal static string PanelSelect => ResourceManager.GetString("PanelSelect", resourceCulture);

	internal AppLanguage()
	{
	}
}
