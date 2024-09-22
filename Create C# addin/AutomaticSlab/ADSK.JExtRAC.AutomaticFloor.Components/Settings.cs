using System;
using System.Collections.Generic;
using ADSK.JExtCom.Rvt;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;

namespace ADSK.JExtRAC.AutomaticFloor.Components
{
    // Token: 0x02000014 RID: 20
    public class Settings : ADSK.JExtCom.Rvt.Settings
    {
        // Token: 0x06000093 RID: 147 RVA: 0x00006044 File Offset: 0x00004244
        public Settings(UIDocument rvtUIDoc) : base(rvtUIDoc)
        {
        }

        // Token: 0x17000049 RID: 73
        // (get) Token: 0x06000094 RID: 148 RVA: 0x0000604D File Offset: 0x0000424D
        public IList<Category> CategorySlab
        {
            get
            {
                return new List<Category>
                {
                    base.GetCategory(BuiltInCategory.OST_Floors)
                };
            }
        }
    }
}
