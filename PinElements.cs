using Autodesk.Revit.DB;
using Autodesk.Revit.Attributes;
using Autodesk.Revit.UI;


// Class that pins elements with an mmi-value over 250
namespace RevitPinElementsMMI
{
    [Transaction(TransactionMode.Manual)]
    public class PinElements : IExternalCommand
    {
        public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
        {

            UIApplication uiapp = commandData.Application;
            Document doc = uiapp.ActiveUIDocument.Document;

            // Value on MMI to compare with
            const int MMI = 250;

            FilteredElementCollector collector = new FilteredElementCollector(doc);

            List<ElementFilter> filters = new List<ElementFilter>
            {
                new ElementCategoryFilter(BuiltInCategory.OST_Walls),
                new ElementCategoryFilter(BuiltInCategory.OST_Windows),
                new ElementCategoryFilter(BuiltInCategory.OST_Doors),
                new ElementCategoryFilter(BuiltInCategory.OST_Columns),
                new ElementCategoryFilter(BuiltInCategory.OST_StructuralColumns),
                new ElementCategoryFilter(BuiltInCategory.OST_Floors)
            };

            LogicalOrFilter combinedFilters = new LogicalOrFilter(filters);

            // List with walls, windows, doors, columns, structural columns and floors from the filters above
            IList<Element> modelElements = collector.WherePasses(combinedFilters).WhereElementIsNotElementType().ToElements();

            // List to collect all the pinned elements
            List<ElementId> elementsToPin = new List<ElementId>();

            // Using TransactionGroup to make the entire operation a single transaction, no matter the number of elements, instead of changing each element one at a time
            using (TransactionGroup transGroup = new TransactionGroup(doc, "MMI"))
            {

                transGroup.Start();

                foreach (Element elem in modelElements)
                {
                    // Checks value in the parameter MMI
                    string mmiParam = elem.LookupParameter("MMI").AsValueString();

                    // Makes mmiParam to an int
                    int mmiValue = ParseMMIValue.GetParsedMMIValue(mmiParam);

                    // If MMI has a value higher than 250, we add the element to the list elementsToPin
                    if (mmiValue > MMI)
                    {
                        elementsToPin.Add(elem.Id);
                    }
                }

                // Creates the transaction inside the transactiongroup to pin all elements in the list elementsToPin
                using (Transaction trans = new Transaction(doc, "Pin Elements"))
                {
                    trans.Start();
                    foreach (ElementId id in elementsToPin)
                    {
                        Element elem = doc.GetElement(id);
                        if (elem != null)
                        {
                            elem.Pinned = true;
                        }
                    }

                    trans.Commit();
                }

                transGroup.Assimilate();
            }

            // Shows a log over numbers of elements that got pinned in the transaction
            TaskDialog.Show("Antall element", $"Antall pinnede element med MMI over 250 = {elementsToPin.Count}");

            return Result.Succeeded;
        }
    }
}


