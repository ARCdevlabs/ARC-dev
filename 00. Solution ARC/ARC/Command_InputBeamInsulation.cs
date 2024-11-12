using Autodesk.Revit.Attributes;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Xml.Linq;
using static Autodesk.Revit.DB.SpecTypeId;

namespace ARC
{
    [Transaction(TransactionMode.Manual)] //Dòng này cần phải có trong Visual Studio thì build xong mới hiểu được
                                          //Nếu chạy trong pyRevit thì không cần
    public class BeamInsulation : IExternalCommand
    {
        public Result Execute(ExternalCommandData revit, ref string message, ElementSet elements)

        {
            FamilyInstance newBeam = null;

            UIDocument uidoc = revit.Application.ActiveUIDocument;

            Document doc = uidoc.Document;

            ARCLibrary lib = new ARCLibrary();

            List<Element> listElement = null;

            List<Element> selectedElement = lib.CurrentSelection(uidoc, doc);

            if (selectedElement == null)
            {
                List<Element> pickElements = lib.PickElements(uidoc, doc);
                listElement = pickElements;
            }
            else
            {
                listElement = selectedElement;
            }
            TransactionGroup trangroup = new TransactionGroup(doc, "Input Beam Insulation");
            {
                trangroup.Start();
                foreach (Element ele in listElement)
                {
                    double lastStartLevelOffset = 0;
                    double lastEndLevelOffset = 0;

                    double startLevelOffset = ele.get_Parameter(BuiltInParameter.STRUCTURAL_BEAM_END0_ELEVATION).AsDouble();
                    double endLevelOffset = ele.get_Parameter(BuiltInParameter.STRUCTURAL_BEAM_END1_ELEVATION).AsDouble();

                    int yz_jus = ele.get_Parameter(BuiltInParameter.YZ_JUSTIFICATION).AsInteger();
                    if (yz_jus == 0)
                    {
                        double zOffset = ele.get_Parameter(BuiltInParameter.Z_OFFSET_VALUE).AsDouble();
                        lastStartLevelOffset = startLevelOffset + zOffset;
                        lastEndLevelOffset = endLevelOffset + zOffset;
                    }
                    if (yz_jus == 1)
                    {
                        double startZOffset = ele.get_Parameter(BuiltInParameter.START_Z_OFFSET_VALUE).AsDouble();
                        lastStartLevelOffset = startLevelOffset + startZOffset;
                        double endZOffset = ele.get_Parameter(BuiltInParameter.END_Z_OFFSET_VALUE).AsDouble();
                        lastEndLevelOffset = endLevelOffset + endZOffset;
                    }

                    Line location = lib.GetBeamLocation(ele);

                    Element beamType = doc.GetElement(new ElementId(13038672)); //Cần điều chỉnh lại beamTypeSymbol dựa vào form WPF

                    FamilySymbol beamTypeSymbol = beamType as FamilySymbol;

                    lib.ActivateSymbol(doc, beamTypeSymbol);

                    ElementId levelId = ele.get_Parameter(BuiltInParameter.INSTANCE_REFERENCE_LEVEL_PARAM).AsElementId();

                    Level level = doc.GetElement(levelId) as Level;

                    Transaction trans = new Transaction(doc, "Input Beam Insulation");
                    {
                        trans.Start();

                        newBeam = lib.CreateBeam(doc, location, beamTypeSymbol, level);  //Cần điều chỉnh lại beamTypeSymbol dựa vào form WPF

                        trans.Commit();

                    }
                    Transaction trans2 = new Transaction(doc, "Setting Level Insulation");
                    {
                        trans2.Start();
                        Parameter paramStartLevelOffset = newBeam.get_Parameter(BuiltInParameter.STRUCTURAL_BEAM_END0_ELEVATION);

                        paramStartLevelOffset.Set(lastStartLevelOffset);

                        Parameter paramEndLevelOffset = newBeam.get_Parameter(BuiltInParameter.STRUCTURAL_BEAM_END1_ELEVATION);

                        paramEndLevelOffset.Set(lastEndLevelOffset);

                        //TaskDialog.Show("Beam Information", "Last Start Level Offset: " + lastStartLevelOffset.ToString());

                        trans2.Commit();
                    }                    
                }
                trangroup.Commit();

            }
            
            return Result.Succeeded;
        }
    }
}
