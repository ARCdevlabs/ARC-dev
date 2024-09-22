using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;

namespace NS.WrapARC
{
    public class WrapARC
    {
        public static Parameter GetBuiltInParameterByName(Element element, BuiltInParameter builtInParameter)
        {
            Parameter param = element.get_Parameter(builtInParameter);
            return param;
        }


        public static string GetParameterValueByName(Element element, string name)
        {
            // Lấy danh sách các tham số có tên tương ứng
            IList<Parameter> listParam = element.GetParameters(name);

            // Nếu không tìm thấy tham số, trả về null hoặc xử lý lỗi tùy ý
            if (listParam == null || listParam.Count == 0)
            {
                return null;
            }

            // Lấy tham số đầu tiên từ danh sách
            Parameter param = listParam[0];

            // Trả về giá trị của tham số dưới dạng chuỗi
            return param.AsValueString();
        }

        public static void OverrideGraphicsInView(Document doc, View view, List<ElementId> listElementId, Color color, Color colorCut)
        {
            string[] namePatterns = { "<Solid fill>", "<塗り潰し>" };

            // Tìm FillPatternElement có tên phù hợp
            ElementId solidPatternId = null;
            FilteredElementCollector patterns = new FilteredElementCollector(doc).OfClass(typeof(FillPatternElement));
            foreach (FillPatternElement pattern in patterns)
            {
                foreach (string patternName in namePatterns)
                {
                    if (pattern.Name == patternName)
                    {
                        solidPatternId = pattern.Id;
                        break;
                    }
                }
                if (solidPatternId != null) break;
            }

            // Nếu không tìm thấy Solid Pattern, thoát khỏi phương thức
            if (solidPatternId == null) return;

            // Tạo đối tượng OverrideGraphicSettings
            OverrideGraphicSettings overrideSettings = new OverrideGraphicSettings();

            // Thiết lập màu nền và mẫu cho các phần tử
            foreach (ElementId elementId in listElementId)
            {
                overrideSettings.SetSurfaceForegroundPatternColor(color);
                overrideSettings.SetSurfaceForegroundPatternId(solidPatternId);
                overrideSettings.SetCutForegroundPatternColor(colorCut);
                overrideSettings.SetCutForegroundPatternId(solidPatternId);

                // Áp dụng OverrideGraphicSettings cho phần tử trong View
                view.SetElementOverrides(elementId, overrideSettings);
            }
        }


    }
}
