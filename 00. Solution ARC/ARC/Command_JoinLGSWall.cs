using System;
using Autodesk.Revit.UI;
using Autodesk.Revit.DB;
using System.Collections.Generic;
using Autodesk.Revit.Attributes;
using System.Linq.Expressions;
using System.Xml.Linq;

namespace ARC
{

    [Transaction(TransactionMode.Manual)] //Dòng này cần phải có trong Visual Studio thì build xong mới hiểu được
                                          //Nếu chạy trong pyRevit thì không cần
    public class JoinLGSWall : IExternalCommand
    {

        public Result Execute(ExternalCommandData revit,
                              ref string message, ElementSet elements)

        {
            try
            {
                UIDocument uidoc = revit.Application.ActiveUIDocument;
                Document doc = uidoc.Document;

                ARCLibrary lib = new ARCLibrary();

                ARCTransform transform = new ARCTransform();



                List<Element> listElement = lib.CurrentSelection(uidoc, doc);

                Element element = null;

                Face pickFace = null;

                if (listElement != null && listElement.Count == 1)
                {
                    Element selectedElement = listElement[0];
                    element = selectedElement;
                    Face face = lib.PickFace(uidoc, doc);
                    pickFace = face;
                }
                else
                {
                    Face face = lib.PickFace(uidoc, doc);
                    Element pickElement = lib.PickElement(uidoc, doc);
                    element = pickElement;
                    pickFace = face;
                }



                if (pickFace != null && element != null)
                {
                    int startOrEnd = 0;


                    if (element is Wall wall)

                    {
                        LocationCurve locationCurve = wall.Location as LocationCurve;

                        Line wallLine = locationCurve.Curve as Line;

                        Line newWallLine = null;

                        Surface getPlane = pickFace.GetSurface();

                        if (getPlane is Plane plane)
                        {
                            XYZ intersectPoint = transform.FindIntersection(wallLine, plane);

                            double pointX = intersectPoint.X;
                            double pointY = intersectPoint.Y;



                            XYZ startPoint = wallLine.GetEndPoint(0);

                            XYZ endPoint = wallLine.GetEndPoint(1);

                            double distanceStart = transform.DistancePointToSurface(startPoint, pickFace);

                            double distanceEnd = transform.DistancePointToSurface(endPoint, pickFace);


                            if (distanceStart < distanceEnd)
                            {
                                startOrEnd = 0;

                                XYZ newEndPoint = new XYZ(pointX, pointY, startPoint.Z);

                                Line newLine = Line.CreateBound(newEndPoint, endPoint);

                                newWallLine = newLine;

                                // Cập nhật chiều dài của bức tường
                            }
                            else
                            {
                                startOrEnd = 1;
                                XYZ newEndPoint = new XYZ(pointX, pointY, startPoint.Z);

                                Line newLine = Line.CreateBound(startPoint, newEndPoint);

                                newWallLine = newLine;
                            }
                            Transaction trans = new Transaction(doc, "Join LGS Wall");
                            {
                                trans.Start();
                                {
                                    WallUtils.DisallowWallJoinAtEnd(wall, startOrEnd);

                                    locationCurve.Curve = newWallLine;
                                    
                                }
                                    Parameter lengthParameter = wall.get_Parameter(BuiltInParameter.CURVE_ELEM_LENGTH);
                                    double wallLength = lengthParameter.AsDouble();
                                    if (wallLength > 1000)
                                    {
                                        trans.RollBack();
                                    }
                                    else
                                    {
                                        trans.Commit();
                                    }    
                                
                            }
                        }
                        else
                        {

                        }

                    }
                    else
                    {

                    }
                }
            }
            catch
            {

            }
            return Result.Succeeded;
        }
    }
}