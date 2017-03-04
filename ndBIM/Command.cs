#region Namespaces
using System;
using System.Collections.Generic;
using System.Diagnostics;
using Autodesk.Revit.ApplicationServices;
using Autodesk.Revit.Attributes;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using Autodesk.Revit.UI.Selection;
#endregion

namespace ndBIM
{
    [Transaction(TransactionMode.Manual)]
    public class Command : IExternalCommand
    {
        public Result Execute(
          ExternalCommandData commandData,
          ref string message,
          ElementSet elements)
        {
            UIApplication uiapp = commandData.Application;
            UIDocument uidoc = uiapp.ActiveUIDocument;
            Application app = uiapp.Application;
            Document doc = uidoc.Document;

            using (ndBIMInterface m_form = new ndBIMInterface(uiapp))
            {
                var result = m_form.Show();
            }


            // Access current selection

            //Selection sel = uidoc.Selection;

            // Retrieve elements from database

            //FilteredElementCollector col
            //  = new FilteredElementCollector(doc)
            //    .WhereElementIsNotElementType()
            //    .OfCategory(BuiltInCategory.INVALID)
            //    .OfClass(typeof(Wall));

            // Filtered element collector is iterable

            //foreach (Element e in col)
            //{
            //    Debug.Print(e.Name);
            //}

            // Modify document within a transaction

            using (Transaction tx = new Transaction(doc))
            {
                tx.Start("Transaction Name");
                tx.Commit();
            }

            return Result.Succeeded;
        }
    }
}
