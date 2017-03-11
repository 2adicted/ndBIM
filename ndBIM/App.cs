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
            RibbonPanel ribbonPanel_04 = application.CreateRibbonPanel(tabName, "ndBIM Information");

            // Get dll assembly path
            string thisAssemblyPath = Assembly.GetExecutingAssembly().Location;

            // create push button for Budget Parameters
            PushButtonData b1Data = new PushButtonData(
                String.Format("Create Budget {0} Parameters", Environment.NewLine),
                String.Format("Create Budget {0} Parameters", Environment.NewLine),
                thisAssemblyPath,
                "ndBIM.cmdBudgetParameters");

            PushButton pb1 = ribbonPanel_01.AddItem(b1Data) as PushButton;
            pb1.ToolTip = "Creates and populates current project with predefined Shared Parameters.";
            BitmapImage pb1Image = new BitmapImage(new Uri("pack://application:,,,/ndBIM;component/Resources/btn01.png"));
            pb1.LargeImage = pb1Image;
            // create push button for Tools and Hosts
            PushButtonData b2Data = new PushButtonData(
                "Automatic Fill",
                "Automatic Fill",
                thisAssemblyPath,
                "ndBIM.cmdToolsAndHosts");

            PushButton pb2 = ribbonPanel_01.AddItem(b2Data) as PushButton;
            pb2.ToolTip = "Automatically populate Budget parameters. Only use after using Budget Parameters first.";
            BitmapImage pb2Image = new BitmapImage(new Uri("pack://application:,,,/ndBIM;component/Resources/btn02.png"));
            pb2.LargeImage = pb2Image;

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
