#region Namespaces
using System;
using System.Collections.Generic;
using Autodesk.Revit.ApplicationServices;
using Autodesk.Revit.Attributes;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using System.Reflection;
using System.Windows.Media.Imaging;
#endregion

namespace ndBIM
{
    class App : IExternalApplication
    {
        // define a method that will create our tab and button
        static void AddRibbonPanel(UIControlledApplication application)
        {
            // Create a custom ribbon tab
            String tabName = "ndBIM";
            application.CreateRibbonTab(tabName);

            // Add a new ribbon panel
            RibbonPanel ribbonPanel_01 = application.CreateRibbonPanel(tabName, "Information");
            RibbonPanel ribbonPanel_02 = application.CreateRibbonPanel(tabName, "Import/Export");
            RibbonPanel ribbonPanel_03 = application.CreateRibbonPanel(tabName, "Software VICO");
            RibbonPanel ribbonPanel_04 = application.CreateRibbonPanel(tabName, "ndBIM Information");

            // Get dll assembly path
            string thisAssemblyPath = Assembly.GetExecutingAssembly().Location;
             
            // Create the buttons
            CreatePushButton(ribbonPanel_01, String.Format("Create Budget {0} Parameters", Environment.NewLine), thisAssemblyPath, "ndBIM.cmdBudgetParameters", 
                "Creates and populates current project with predefined Shared Parameters.", "btn01.png");
            CreatePushButton(ribbonPanel_01, "Automatic Fill", thisAssemblyPath, "ndBIM.cmdToolsAndHosts",
                "Automatically populate Budget parameters. Only use after using Budget Parameters first.", "btn02.png");
            CreatePushButton(ribbonPanel_02, "Export to Excel", thisAssemblyPath, "ndBIM.cmdExportExcel",
                "Exports Budget Parameters to Excel.", "btn03.png");
            CreatePushButton(ribbonPanel_02, "Import from Excel", thisAssemblyPath, "ndBIM.cmdImportExcel",
                "Import Budget Parameters from Excel.", "btn04.png");
            CreatePushButton(ribbonPanel_03, "Export to VICO", thisAssemblyPath, "ndBIM.cmdExportVICO",
                "Export to VICO.", "btn05.png");
            CreatePushButton(ribbonPanel_04, "About us", thisAssemblyPath, "ndBIM.cmdAboutUs", "About us.", "btn06.png");
        }
        private static void CreatePushButton(RibbonPanel ribbonPanel, string name, string path, string command, string tooltip, string icon)
        {
            PushButtonData pbData = new PushButtonData(
                name,
                name,
                path,
                command);

            PushButton pb = ribbonPanel.AddItem(pbData) as PushButton;
            pb.ToolTip = tooltip;
            BitmapImage pb2Image = new BitmapImage(new Uri(String.Format("pack://application:,,,/ndBIM;component/Resources/{0}", icon)));
            pb.LargeImage = pb2Image;
        }
        public Result OnStartup(UIControlledApplication a)
        {
            AddRibbonPanel(a);
            return Result.Succeeded;
        }

        public Result OnShutdown(UIControlledApplication a)
        {
            return Result.Succeeded;
        }
    }
}
