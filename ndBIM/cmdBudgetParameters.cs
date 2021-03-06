#region Namespaces
using System;
using System.Collections.Generic;
using System.Diagnostics;
using Autodesk.Revit.ApplicationServices;
using Autodesk.Revit.Attributes;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using Autodesk.Revit.UI.Selection;
using System.Linq;
using AdnRme;
#endregion

namespace ndBIM
{
    [Transaction(TransactionMode.Manual)]
    public class cmdBudgetParameters : IExternalCommand
    {
        public string msg;

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
                if (Utils.IsAlreadyBound(doc, "Modelling Tool"))
                {
                    TaskDialog.Show("Budget Parameters failed to execute.", "Budget Parameters already exist. This command will not be executed.");
                    return Result.Failed;
                }

                System.Windows.Forms.DialogResult result = m_form.ShowDialog();   
                
                if(result == System.Windows.Forms.DialogResult.OK)
                {
                    return Result.Succeeded;
                }
                else
                {
                    return Result.Failed;
                }
            }
        }
    }
    [Transaction(TransactionMode.Manual)]
    public class cmdAboutUs : IExternalCommand
    {
        public Result Execute(
          ExternalCommandData commandData,
          ref string message,
          ElementSet elements)
        {
            try
            {
                System.Diagnostics.Process.Start("http://ndbim.com");
                return Result.Succeeded;
            }
            catch(Exception ex)
            {
                message = ex.ToString();
                return Result.Failed;
            }
        }
    }
    [Transaction(TransactionMode.Manual)]
    public class cmdImportExcel : IExternalCommand
    {
        public string msg;

        public Result Execute(
          ExternalCommandData commandData,
          ref string message,
          ElementSet elements)
        {
            UIApplication uiapp = commandData.Application;
            UIDocument uidoc = uiapp.ActiveUIDocument;
            Application app = uiapp.Application;
            Document doc = uidoc.Document;

            if (!Utils.IsAlreadyBound(doc, "Modelling Tool"))
            {
                TaskDialog.Show("Import from Excel will not execute.", "No Budget Parameters exist to be populated.");
                return Result.Failed;
            }

            ParameterManager manager = new ParameterManager(uiapp, doc);
            manager.ImportExcel();
            
            return Result.Succeeded;
        }
    }
    [Transaction(TransactionMode.Manual)]
    public class cmdExportExcel : IExternalCommand
    {
        public string msg;

        public Result Execute(
          ExternalCommandData commandData,
          ref string message,
          ElementSet elements)
        {
            UIApplication uiapp = commandData.Application;
            UIDocument uidoc = uiapp.ActiveUIDocument;
            Application app = uiapp.Application;
            Document doc = uidoc.Document;

            ParameterManager manager = new ParameterManager(uiapp, doc);
            manager.ExportExcel();


            return Result.Succeeded;
        }
    }
    [Transaction(TransactionMode.Manual)]
    public class cmdExportVICO : IExternalCommand
    {
        public string msg;

        public Result Execute(
          ExternalCommandData commandData,
          ref string message,
          ElementSet elements)
        {
            UIApplication uiapp = commandData.Application;
            UIDocument uidoc = uiapp.ActiveUIDocument;
            Application app = uiapp.Application;
            Document doc = uidoc.Document;

            ParameterManager manager = new ParameterManager(uiapp, doc);
            manager.ExportVICO();

            return Result.Succeeded;
        }
    }
    [Transaction(TransactionMode.Manual)]
    public class cmdToolsAndHosts : IExternalCommand
    {
        public string msg;

        public Result Execute(
          ExternalCommandData commandData,
          ref string message,
          ElementSet elements)
        {
            UIApplication uiapp = commandData.Application;
            UIDocument uidoc = uiapp.ActiveUIDocument;
            Application app = uiapp.Application;
            Document doc = uidoc.Document;

            Tools tools = new Tools(uiapp);

            if (!tools.go)
            {
                TaskDialog.Show("Failed", "Please, run 'Budget Parameters' first.");
                return Result.Failed;
            }

            using (TransactionGroup tgx = new TransactionGroup(doc))
            {
                tgx.Start();
                tools.ModellingTool();
                tools.WindowsAndDoorsHosts();
                tools.BoQ();
                tgx.Commit();
            }

            TaskDialog.Show("Status", "Tools and Hosts successfully finished.");
            return Result.Succeeded;
        }
    }
    internal class Tools
    {
        private UIApplication uiapp;
        private UIDocument uidoc;
        private Application app;
        private Document doc;
        internal bool go;

        internal Tools(UIApplication uiapp)
        {
            this.uiapp = uiapp;
            this.uidoc = uiapp.ActiveUIDocument;
            this.app = uiapp.Application;
            this.doc = uidoc.Document;

            Initialise();
        }

        private void Initialise()
        {
            if (Utils.IsAlreadyBound(doc, "Modelling Tool"))
            {
                go = true;
            }
            else
            {
                go = false;
            }
        }
        // All Categories
        internal void ModellingTool()
        {
            CategorySet catSet = Utils.AssignableCategories(uiapp, doc);

            using (Transaction t = new Transaction(doc, "Modelling Tool Populate"))
            {
                int n = catSet.Size;

                string s = "{0} of " + n.ToString() + " categories processed...";
                string caption = "Modelling Tools";

                t.Start();
                int count = 0;
                using (ProgressForm pf = new ProgressForm(caption, s, n))
                { 
                    foreach (Category cat in catSet)
                    {
                        if (pf.getAbortFlag())
                        {
                            t.RollBack();
                            return;
                        }

                        pf.Increment();
                        try
                        {
                            IList<Element> collector = new FilteredElementCollector(doc).OfCategoryId(cat.Id).WhereElementIsElementType().ToElements();
                            if (collector.Count < 1) continue;
                            foreach (Element el in collector)
                            {
                                Parameter p = el.LookupParameter("Modelling Tool");

                                if (p != null && !p.IsReadOnly) p.Set(cat.Name);
                            }
                            count++;
                        }
                        catch (Exception)
                        {
                            //TaskDialog.Show("Failed", cat.Name + " : " + ex);
                        }
                    }
                }
                if (count == 0)
                {
                    TaskDialog.Show("Modelling Tool failed to execute.", "No elements with Modelling Tool parameter found in this document.");
                    t.RollBack();
                }
                t.Commit();
            }
        }

        internal void WindowsAndDoorsHosts()
        {
            CategorySet catSet = new CategorySet();
            Categories categories = doc.Settings.Categories;

            Category doors = categories.get_Item("Doors");
            Category windows = categories.get_Item("Windows");

            catSet.Insert(doors);
            catSet.Insert(windows);

            List<HostObject> hostObjects = new FilteredElementCollector(doc)
                .OfClass(typeof(Wall))
                .WhereElementIsNotElementType()
                .Cast<HostObject>()
                //.Where(x => x.FindInserts(false, false, false, false).Count > 0)
                .ToList();

            using (Transaction t = new Transaction(doc, "Window and Door Hosts Populate"))
            {
                t.Start();
                int count = 0;
                foreach (Category cat in catSet)
                {
                    try
                    {
                        IList<Element> collector = new FilteredElementCollector(doc).OfCategoryId(cat.Id).WhereElementIsNotElementType().ToElements();
                        if (collector.Count < 1) continue;
                        int n = collector.Count;

                        string s = "{0} of " + n.ToString() + " elements processed...";
                        string caption = cat.Name;

                        using (ProgressForm pf = new ProgressForm(caption, s, n))
                        {
                            foreach (Element el in collector)
                            {
                                if (pf.getAbortFlag())
                                {
                                    t.RollBack();
                                    return;
                                }
                                pf.Increment();
                                Parameter p = el.LookupParameter("Windows and Doors Host");

                                string hostName = "";

                                foreach (HostObject hostObject in hostObjects)
                                {
                                    IList<ElementId> ids = hostObject.FindInserts(false, false, false, false);
                                    if (ids.Contains(el.Id))
                                    {
                                        hostName = String.Format("{0}", (hostObject as Wall).WallType.Name);
                                        break;
                                    }
                                }
                                if (p != null && !p.IsReadOnly) p.Set(hostName);
                            }
                            count++;
                        }
                    }
                    catch (Exception)
                    {
                        //TaskDialog.Show("Failed", cat.Name + " : " + ex);
                    }
                }

                
                if (count == 0)
                {
                    TaskDialog.Show("Windows And Doors Hosts failed to execute.", "No Windows or Door elements with Modelling Tool parameter found in this document.");
                    t.RollBack();
                    return;
                }
                t.Commit();
            }
        }
        internal void BoQ()
        {
            CategorySet catSet = Utils.AssignableCategories(uiapp, doc);

            using (Transaction t = new Transaction(doc, "Modelling Tool Populate"))
            {
                int n = catSet.Size;

                string s = "{0} of " + n.ToString() + " categories processed...";
                string caption = "BoQ";

                t.Start();
                int count = 0;
                using (ProgressForm pf = new ProgressForm(caption, s, n))
                {
                    foreach (Category cat in catSet)
                    {
                        if (pf.getAbortFlag()) break;
                        pf.Increment();
                        try
                        {
                            IList<Element> collector = new FilteredElementCollector(doc).OfCategoryId(cat.Id).WhereElementIsNotElementType().ToElements();
                            //IList<Element> collector = new FilteredElementCollector(doc).OfCategory(BuiltInCategory.OST_Walls).WhereElementIsNotElementType().ToElements();
                            if (collector.Count < 1) continue;
                            foreach (Element el in collector)
                            {
                                Parameter p = el.LookupParameter("BoQ Codes, Units and Factors");
                                int i = 1;
                                string name = "";
                                string second = "";
                                //Element type = doc.GetElement(el.GetTypeId());
                                while (el.LookupParameter(String.Format("Budget Code_{0}", i.ToString("00"))) != null)
                                {
                                    string a = el.LookupParameter(String.Format("Budget Code_{0}", i.ToString("00"))).AsString();
                                    string b = el.LookupParameter(String.Format("Unit_{0}", i.ToString("00"))).AsString();
                                    string c = el.LookupParameter(String.Format("Factor_{0}", i.ToString("00"))).AsString();
                                    if (!String.IsNullOrEmpty(c)) c = String.Format("({0})", c);
                                    if (string.IsNullOrEmpty(a + b + c))
                                    {
                                        i++;
                                        continue;
                                    }
                                    name += second;
                                    name += String.Format("{0}_{1}{2}", a, b, c);
                                    second = "#";
                                    i++;
                                }

                                if (p != null && !p.IsReadOnly) p.Set(name);
                            }
                            count++;
                        }
                        catch (Exception)
                        {
                            //TaskDialog.Show("Failed", cat.Name + " : " + ex);
                        }
                    }

                }
                if (count == 0)
                {
                    TaskDialog.Show("Failed", "No elements with Modelling Tool parameter found in this document.");
                    t.RollBack();
                }
                t.Commit();
            }

        }
    }
}
