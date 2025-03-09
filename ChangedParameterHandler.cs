using Autodesk.Revit.DB.Events;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using UIFrameworkServices;

// Class that checks if an element with an MMI-value over 250 gets modified using OnDocumentChanged
namespace RevitPinElementsMMI
{
    public class ChangedParameterHandler : IExternalApplication
    {
        public Result OnStartup(UIControlledApplication application)
        {
            // Register DocumentChanged on ControlledApplication 
            application.ControlledApplication.DocumentChanged += OnDocumentChanged;
            return Result.Succeeded;
        }

        public Result OnShutdown(UIControlledApplication application)
        {
            // Deregister on shutdown
            application.ControlledApplication.DocumentChanged -= OnDocumentChanged;
            return Result.Succeeded;
        }

        private void OnDocumentChanged(object sender, DocumentChangedEventArgs e)
        {
            Document doc = e.GetDocument();

            // Value on MMI to compare with
            const int MMI = 250;

            // Create a TaskDialog to make user notice the change 
            TaskDialog taskDialog = new TaskDialog("Bekreft Endring");
            taskDialog.MainContent = "Advarsel, du endrer nå en parameter på et element med en MMI-verdi over 250";
            taskDialog.CommonButtons = TaskDialogCommonButtons.Ok;

            // List over changed elementids
            var changedElementIds = e.GetModifiedElementIds().ToList();

            if (changedElementIds.Count > 0)
            {
                List<string> changedElementsList = new List<string>();

                foreach (ElementId id in changedElementIds)
                {
                    // Get the element, not just the id
                    Element element = doc.GetElement(id);
                    if (element != null && element.Category != null)
                    {

                        // Gets the value from the parameter MMI and makes to an int with ParseMMIValue
                        string mmiParam = element.LookupParameter("MMI").AsValueString();
                        int mmiValue = ParseMMIValue.GetParsedMMIValue(mmiParam);

                        if (mmiValue > MMI)
                        {
                            // Add the element category and mmi-value in a list
                            changedElementsList.Add($"{element.Category.Name} med en MMI-verdi på {mmiValue}");

                            // -------------- Code that gets to be here because it might be useful with more work -------------- // 

                            //TaskDialogResult result = taskDialog.Show();

                            //if (result == TaskDialogResult.Ok)
                            //{
                            //    TaskDialog.Show("Modify Element", $"{element.Category.Name} med en MMI-verdi på {element.LookupParameter("MMI").AsValueString()} er nå endret");
                            //} //else

                            // The QuickAccessToolBarService is not working properly. It skips undos or jumps during certain transactions, so im commenting it out
                            // It would otherwise have been an option for a cancel button on the task dialog
                            // It should be possible to work around the jumps in undos with some additional effort by using collectUndoRedoItems to filter out which transaction was made
                            //{
                            // QuickAccessToolBarService.performMultipleUndoRedoOperations(true, 1);
                            //}
                        }
                    }
                }

                // Prints the list with the elementcategory that has changed and the mmi-value to make the user even more aware of the change
                if (changedElementsList.Count > 0)
                {
                    string message = "Følgende element har blitt endret med en MMI-verdi over 250:\n\n";
                    message += string.Join("\n", changedElementsList);

                    // Shows the dialog with the list of changes
                    TaskDialog.Show("Endringar med MMI-verdi over 250", message);
                }
            }
        }
    }
}
