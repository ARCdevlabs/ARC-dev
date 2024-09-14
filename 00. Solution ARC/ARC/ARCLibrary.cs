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
    public class ARCLibrary
    {
        public Element PickElement(UIDocument uidoc, Document doc)
        {

            try
            {
                // Create a reference to pick an object
                Selection choices = uidoc.Selection;

                Reference pickedObjectRef = choices.PickObject(ObjectType.Element);

                if (pickedObjectRef != null)
                {
                    // Get the element ID of the picked object
                    ElementId elementId = pickedObjectRef.ElementId;

                    // Get the element corresponding to the picked object
                    Element pickedElement = doc.GetElement(elementId);

                    return pickedElement;

                    // Return null if no element was picked

                }
            }
            catch
            {

            }

            return null;
        }

        public List<Element> PickElements(UIDocument uidoc, Document doc)
        {

            // Create a reference to pick an object
            Selection choices = uidoc.Selection;

            IList<Reference> pickedObjectsRef = choices.PickObjects(ObjectType.Element);

            List<Element> listElements = new List<Element>();

            if (pickedObjectsRef != null)
            {
                foreach (Reference reference in pickedObjectsRef)
                {
                    ElementId elementId = reference.ElementId;
                    Element pickedElement = doc.GetElement(elementId);
                    listElements.Add(pickedElement);
                }
                return listElements;

            }
            return null;
        }

        public List<Element> CurrentSelection(UIDocument uidoc, Document doc)
        {
            try
            {
                List<Element> listElements = new List<Element>();

                Selection selection = uidoc.Selection;

                ICollection<ElementId> selectedIds = uidoc.Selection.GetElementIds();

                if (0 != selectedIds.Count)
                {
                    foreach (ElementId id in selectedIds)
                    {
                        Element getElements = doc.GetElement(id);

                        listElements.Add(getElements);

                    }
                    return listElements;
                }
                else
                {
                    return null;
                }

            }
            catch
            {
                return null;
            }

        }
        public Face PickFace(UIDocument uidoc, Document doc)
        {
            try
            {
                // Create a reference to pick an object
                Selection choices = uidoc.Selection;

                Reference pickedObjectRef = choices.PickObject(ObjectType.Face);

                if (pickedObjectRef != null)
                {
                    // Get the element ID of the picked object
                    ElementId elementId = pickedObjectRef.ElementId;

                    // Get the element corresponding to the picked object
                    Element pickedElement = doc.GetElement(elementId);


                    // Lấy hình học của phần tử
                    GeometryObject geomObject = pickedElement.GetGeometryObjectFromReference(pickedObjectRef);

                    // Kiểm tra nếu đối tượng hình học là mặt (Face)
                    Face face = geomObject as Face;
                    if (face != null)
                    {
                        return face;
                    }
                }
            }
            catch
            {

            }
            return null;
        }

        public IList<Element> PickByRectangle(UIDocument uidoc)
        {
            IList<Element> elements = uidoc.Selection.PickElementsByRectangle(new SelectionFilter(), "Select Element By Drag Mouse Rectangle");

            return elements;
        }

    }











    //Class phụ

    public class SelectionFilter : ISelectionFilter
    {
        public bool AllowElement(Element elem)
        {
            if (elem != null) return true;
            return false;

        }
        public bool AllowReference(Reference refer, XYZ pos)
        {
            return false;

        }
    }

}