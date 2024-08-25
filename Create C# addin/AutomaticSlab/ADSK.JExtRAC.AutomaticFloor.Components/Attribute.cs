using System.IO;
using System.Reflection;
using ADSK.JExtCom.Dnf;

namespace ADSK.JExtRAC.AutomaticFloor.Components;

public class Attribute : UtilAttrib
{
	public Attribute()
	{
		SetAssembly(Assembly.GetExecutingAssembly(), "ADSK.JExtRAC.AutomaticFloor.Resources.Text", "ADSK.JExtRAC.AutomaticFloor.Resources.Image", Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location));
	}
}
