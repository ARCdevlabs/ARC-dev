using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Reflection;
using ADSK.JExtCom.Dnf;
using ADSK.JExtRAC.AutomaticFloor.Components;

namespace ADSK.JExtRAC.AutomaticFloor.Entities;

public class DtItems
{
	private ADSK.JExtRAC.AutomaticFloor.Components.Attribute _CmpAttribute;

	private string _FileItems;

	private DataTable _Default;

	public DataTable Default
	{
		get
		{
			if (_Default == null && _FileItems != null)
			{
				_Default = new DataTable();
				string className = "Default";
				string subClassName = "Item";
				IList<string> list = new List<string>();
				IList<Type> list2 = new List<Type>();
				list.Add("Name");
				list2.Add(typeof(string));
				list.Add("Value");
				list2.Add(typeof(string));
				_Default = UtilIO.GetXMLFile(_FileItems, className, subClassName, list, list2);
			}
			return _Default;
		}
	}

	public double ToleranceInter => double.Parse(UtilData.GetValueTableData(Default, "Name", "ToleranceInter", "Value"));

	public DtItems(ADSK.JExtRAC.AutomaticFloor.Components.Attribute cmpAttribute)
	{
		_CmpAttribute = cmpAttribute;
		string directoryName = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
		_FileItems = directoryName + "\\" + _CmpAttribute.ResourceText("IDS_FILE_ITEMS");
		if (!File.Exists(_FileItems))
		{
			_FileItems = null;
		}
	}
}
