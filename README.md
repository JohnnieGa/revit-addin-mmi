#### Description:
This is an addin that search for elements (walls, doors, windows, floors, structural columns and columns) in a revitmodel with a value in the text-based parameter MMI over 250.
When these elements are found, they are pinned to lock the geometrychanges. 

Using the function OnDocumentChanges from revits api, the addin also notifies the user if changes are being made to elements with an MMI over 250.  

##### Examples: 

![Shows how to use the addin](https://github.com/JohnnieGa/revit-addin-mmi/blob/main/docs/1-step.jpg)

![Shows the result from clicking the addin](https://github.com/JohnnieGa/revit-addin-mmi/blob/main/docs/2-step.jpg)

![Shows the result from clicking the addin](https://github.com/JohnnieGa/revit-addin-mmi/blob/main/docs/3-step.jpg)

![Shows the pinned element with an MMI over 250](https://github.com/JohnnieGa/revit-addin-mmi/blob/main/docs/4-step.jpg)

![Shows the not pinned element with an MMI thats equal to 250](https://github.com/JohnnieGa/revit-addin-mmi/blob/main/docs/5-step.jpg)

#### How to build:
The addin is made for Revit 2025, using the framework .Net 8.0. Use the dependencies RevitAPI.dll and RevitAPIUI.dll. 
Add UIFrameworkServices.dll if you want to try and make use of QuickAccessToolBarService for the undofunction (now outcommented in the code in ChangedParameterHandler).

Change the manifest with your own path to the YourPlugin.dll - put the manifest under "C:\ProgramData\Autodesk\Revit\Addins\2025" (atleast the path should be something like this). 

![Shows the path to change in the manifest file](https://github.com/JohnnieGa/revit-addin-mmi/blob/main/docs/manifestpic.jpg)

#### How to install without installer:
Follow the step above where it shows how to place the manifest-file and change the path in the file. Change the path to the dll thats located under revit-addin-mmi/obj/Debug/net8.0/RevitPinElementsMMI.dll. As far as I know it doesnt matter where the dll is placed on your computer as long as the path to it is right in the manifest-file. 
