using Autodesk.Revit.UI;

namespace Autodesk.Revit.DB
{
    internal class Elements
    {
        private UIDocument activeUIDocument;

        public Elements(UIDocument activeUIDocument)
        {
            this.activeUIDocument = activeUIDocument;
        }
    }
}