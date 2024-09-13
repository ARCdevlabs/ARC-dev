using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Autodesk.Revit.UI;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI.Selection;

namespace ARC.Library
{
    public class ARCLibrary
    {
        public Element PickElement(UIDocument uidoc, Document doc)
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
            return null;
        }
    }
}
