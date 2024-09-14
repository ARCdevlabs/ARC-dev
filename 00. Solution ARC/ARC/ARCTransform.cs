using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Autodesk.Revit.UI;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI.Selection;
using Nice3point.Revit.Extensions;

namespace ARC
{
    public class ARCTransform
    {
        public double DistancePointToSurface(XYZ point, Face surface)
        {
            // Kiểm tra nếu surface là một mặt phẳng (PlanarFace)
            if (surface is PlanarFace planarFace)
            {
                // Vector pháp tuyến của mặt phẳng
                XYZ normal = planarFace.FaceNormal;

                // Tính điểm gốc của mặt phẳng
                XYZ origin = planarFace.Origin;

                // Vector từ gốc mặt phẳng tới điểm
                XYZ vectorFromSurfaceToPoint = point - origin;

                // Tính khoảng cách từ điểm đến mặt phẳng
                double distance = Math.Abs(vectorFromSurfaceToPoint.DotProduct(normal));

                return distance;
            }
            else
            {
                throw new InvalidOperationException("The provided face is not planar.");
            }
        }
        public XYZ FindIntersection(Line line, Plane plane)
        {
            // Lấy điểm bắt đầu và điểm kết thúc của đường thẳng
            XYZ lineStart = line.GetEndPoint(0);
            XYZ lineEnd = line.GetEndPoint(1);

            // Lấy điểm và vector pháp tuyến của mặt phẳng
            XYZ planePoint = plane.Origin;
            XYZ planeNormal = plane.Normal;

            // Tính toán vector hướng của đường thẳng
            XYZ lineDirection = (lineEnd - lineStart).Normalize();

            // Tính toán hệ số của phương trình mặt phẳng và đường thẳng
            double d = -planeNormal.DotProduct(planePoint);
            double t = -(planeNormal.DotProduct(lineStart) + d) / planeNormal.DotProduct(lineDirection);

            // Tính toán điểm giao nhau
            XYZ intersectionPoint = lineStart + t * lineDirection;

            return intersectionPoint;
        }
    }
}
